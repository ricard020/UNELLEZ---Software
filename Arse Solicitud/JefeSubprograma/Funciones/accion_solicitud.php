<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
verificarRol('JP');

$solicitudId = $_POST['solicitud_id'] ?? null;
$accion = $_POST['accion'] ?? null;
$nota = $_POST['nota'] ?? null;
$numero_caso = $_POST['numero_caso'] ?? null;
$numero_resolucion = $_POST['numero_resolucion'] ?? null;

if (!$solicitudId || !$accion || !$nota || !$numero_caso || !$numero_resolucion) {
    die("Error: Datos incompletos.");
}

try {
    // Habilitar el modo de depuración en PDO
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

    // Verificar si el número de caso ya existe
    $stmt = $pdo->prepare("SELECT COUNT(*) FROM solicitudes WHERE numero_caso = :numero_caso AND id != :solicitud_id");
    $stmt->execute(['numero_caso' => $numero_caso, 'solicitud_id' => $solicitudId]);
    if ($stmt->fetchColumn() > 0) {
        die("Ya existe una solicitud con ese número de caso.");
    }

    // Verificar si el número de resolución ya existe
    $stmt = $pdo->prepare("SELECT COUNT(*) FROM solicitudes WHERE numero_resolucion = :numero_resolucion AND id != :solicitud_id");
    $stmt->execute(['numero_resolucion' => $numero_resolucion, 'solicitud_id' => $solicitudId]);
    if ($stmt->fetchColumn() > 0) {
        die("Ya existe una solicitud con ese número de resolución.");
    }

    $pdo->beginTransaction();

    $estado = '';
    switch ($accion) {
        case 'aceptar':
            $estado = 'Aceptada';
            break;
        case 'rechazar':
            $estado = 'Rechazada';
            break;
        case 'diferir':
            $estado = 'Diferida';
            break;
        case 'elevar':
            $estado = 'Elevada';
            break;
        default:
            die("Error: Acción no válida.");
    }

    $stmt = $pdo->prepare("UPDATE solicitudes SET estado = :estado, nota = :nota, numero_caso = :numero_caso, numero_resolucion = :numero_resolucion WHERE id = :solicitud_id");
    $stmt->execute(['estado' => $estado, 'nota' => $nota, 'numero_caso' => $numero_caso, 'numero_resolucion' => $numero_resolucion, 'solicitud_id' => $solicitudId]);

    if ($accion === 'aceptar') {
        $stmt = $pdo->prepare("SELECT usuario_id, nuevo_subprograma_id, subprogramas_estudiantes_id FROM solicitudes WHERE id = :solicitud_id");
        $stmt->execute(['solicitud_id' => $solicitudId]);
        $solicitud = $stmt->fetch(PDO::FETCH_ASSOC);

        if ($solicitud) {
            $usuarioId = $solicitud['usuario_id'];
            $nuevoSubprogramaId = $solicitud['nuevo_subprograma_id'];
            $subprogramasEstudiantesId = $solicitud['subprogramas_estudiantes_id'];

            $updates = [];
            $params = ['subprogramas_estudiantes_id' => $subprogramasEstudiantesId, 'usuario_id' => $usuarioId];

            if (!empty($nuevoSubprogramaId)) {
                // Verificar si nuevo_subprograma_id existe en subprogramas
                $stmt = $pdo->prepare("SELECT id FROM subprogramas WHERE id = :nuevo_subprograma_id");
                $stmt->execute(['nuevo_subprograma_id' => $nuevoSubprogramaId]);
                if ($stmt->fetch()) {
                    $updates[] = "subprograma_id = :nuevo_subprograma_id";
                    $params['nuevo_subprograma_id'] = $nuevoSubprogramaId;
                } else {
                    throw new PDOException("El nuevo subprograma no existe.");
                }
            }

            if (!empty($updates)) {
                $updateQuery = "UPDATE subprogramas_estudiantes SET " . implode(', ', $updates) . " WHERE id = :subprogramas_estudiantes_id AND usuario_id = :usuario_id";
                $stmt = $pdo->prepare($updateQuery);
                $stmt->execute($params);
            }
        }
    }

    $pdo->commit();
    echo "Solicitud " . ($accion === 'aceptar' ? "aceptada" : ($accion === 'rechazar' ? "rechazada" : ($accion === 'diferir' ? "diferida" : "elevada"))) . " con éxito.";
} catch (PDOException $e) {
    $pdo->rollBack();
    // Mostrar el mensaje de error completo
    die("Error al actualizar la solicitud: " . $e->getMessage() . " en la línea " . $e->getLine());
}
?>

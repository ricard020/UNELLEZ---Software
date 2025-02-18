<?php
session_start(); // Iniciar sesión
require_once '../../PHP/bd_conexion.php';
require_once '../../fpdf/fpdf.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $pdfBase64 = $_POST['pdfBase64'];
    $detalles = json_decode($_POST['detalles'], true);
    $subprograma_id_anterior = $_POST['subprograma_id_anterior'];
    $sede_id_anterior = $_POST['sede_id_anterior'];
    $tipo_solicitud_nombres = explode(',', $_POST['tipo_solicitud']); // Separar los nombres de tipo de solicitud
    $nueva_sede_nombre = $_POST['nueva_sede_nombre']; // Obtener el nombre de la nueva sede

    // Obtener el usuario_id de la sesión
    $usuario_id = $_SESSION['user_id'];

    // Decodificar el contenido del PDF
    $pdfContent = base64_decode($pdfBase64);

    // Generar un nombre de archivo único para el PDF
    $pdfFilename = uniqid('solicitud_', true) . '.pdf';
    $pdfPath = '../../solicitudes/pdf/' . $pdfFilename;

    // Guardar el PDF en el servidor
    file_put_contents($pdfPath, $pdfContent);

    // Verificar que el usuario_id existe en la tabla usuarios
    $stmt = $pdo->prepare("SELECT COUNT(*) FROM usuarios WHERE id = :usuario_id");
    $stmt->execute(['usuario_id' => $usuario_id]);
    $userExists = $stmt->fetchColumn();

    if ($userExists) {
        // Obtener las IDs de los tipos de solicitud basados en sus nombres
        $stmt = $pdo->prepare("SELECT id, nombre_tipo FROM tipos_solicitudes WHERE nombre_tipo IN (" . str_repeat('?,', count($tipo_solicitud_nombres) - 1) . "?)");
        $stmt->execute($tipo_solicitud_nombres);
        $tipos_solicitud = $stmt->fetchAll(PDO::FETCH_KEY_PAIR);

        if (count($tipos_solicitud) === count($tipo_solicitud_nombres)) {
            // Obtener la ID del nuevo subprograma basado en el nombre del tipo de solicitud
            $nuevo_subprograma_id = null;
            foreach ($tipo_solicitud_nombres as $tipo_solicitud_nombre) {
                if ($tipo_solicitud_nombre === 'Cambio de carrera') {
                    $stmt = $pdo->prepare("SELECT id FROM subprogramas WHERE nombre_subprograma = :nombre_subprograma");
                    $stmt->execute(['nombre_subprograma' => $detalles['carrera-equivalencia-cambio']]);
                    $nuevo_subprograma_id = $stmt->fetchColumn();
                    if (!$nuevo_subprograma_id) {
                        echo "<script>alert('El subprograma especificado no se encontró.'); window.history.back();</script>";
                        exit;
                    }
                    break;
                }
            }

            // Obtener la ID de la nueva sede basada en su nombre
            $nueva_sede_id = null;
            if ($nueva_sede_nombre) {
                $stmt = $pdo->prepare("SELECT id FROM sedes WHERE nombre_sede = :nombre_sede");
                $stmt->execute(['nombre_sede' => $nueva_sede_nombre]);
                $nueva_sede_id = $stmt->fetchColumn();
                if (!$nueva_sede_id) {
                    echo "<script>alert('La sede especificada no se encontró.'); window.history.back();</script>";
                    exit;
                }
            }

            // Obtener el ID de la relación en la tabla subprogramas_estudiantes
            $stmt = $pdo->prepare("SELECT id FROM subprogramas_estudiantes WHERE usuario_id = :usuario_id AND subprograma_id = :subprograma_id");
            $stmt->execute([
                'usuario_id' => $usuario_id,
                'subprograma_id' => $subprograma_id_anterior
            ]);
            $subprogramas_estudiantes_id = $stmt->fetchColumn();

            if (!$subprogramas_estudiantes_id) {
                echo "<script>alert('No se encontró la relación entre el usuario y el subprograma.'); window.history.back();</script>";
                exit;
            }

            // Insertar datos en la tabla `solicitudes`
            try {
                $pdo->beginTransaction();

                $stmt = $pdo->prepare("
                    INSERT INTO solicitudes (usuario_id, subprograma_id_anterior, sede_id_anterior, nuevo_subprograma_id, nueva_sede_id, estado, fecha_solicitud, nota, archivo_pdf, subprogramas_estudiantes_id)
                    VALUES (:usuario_id, :subprograma_id_anterior, :sede_id_anterior, :nuevo_subprograma_id, :nueva_sede_id, 'Pendiente', NOW(), NULL, :archivo_pdf, :subprogramas_estudiantes_id)
                ");
                $stmt->execute([
                    'usuario_id' => $usuario_id,
                    'subprograma_id_anterior' => $subprograma_id_anterior,
                    'sede_id_anterior' => $sede_id_anterior,
                    'nuevo_subprograma_id' => $nuevo_subprograma_id,
                    'nueva_sede_id' => $nueva_sede_id,
                    'archivo_pdf' => $pdfPath,
                    'subprogramas_estudiantes_id' => $subprogramas_estudiantes_id
                ]);

                // Obtener el ID de la solicitud recién creada
                $solicitud_id = $pdo->lastInsertId();

                // Insertar detalles en la tabla `detalles_solicitudes` para cada tipo de solicitud
                $stmt = $pdo->prepare("
                    INSERT INTO detalles_solicitudes (solicitud_id, tipo_solicitud_id, archivo_pdf)
                    VALUES (:solicitud_id, :tipo_solicitud_id, :archivo_pdf)
                ");
                foreach ($tipos_solicitud as $tipo_solicitud_id => $nombre_tipo) {
                    $stmt->execute([
                        'solicitud_id' => $solicitud_id,
                        'tipo_solicitud_id' => $tipo_solicitud_id,
                        'archivo_pdf' => $pdfPath
                    ]);
                }

                $pdo->commit();
                echo "<script>alert('Solicitud enviada con éxito.'); window.location.href='../perfil_estudiante.php';</script>";
            } catch (PDOException $e) {
                $pdo->rollBack();
                echo "<script>alert('Error al enviar la solicitud: " . $e->getMessage() . "'); window.history.back();</script>";
            }
        } else {
            echo "<script>alert('Tipo de solicitud no válido.'); window.history.back();</script>";
        }
    } else {
        echo "<script>alert('Usuario no encontrado.'); window.history.back();</script>";
    }
} else {
    echo "<script>alert('Método no permitido.'); window.history.back();</script>";
}
?>
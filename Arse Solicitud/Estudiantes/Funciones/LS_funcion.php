<?php
// Importar la conexión a la base de datos
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
verificarRol('Estudiante');

// Verificar si el usuario ha iniciado sesión
if (!isset($_SESSION['user_id'])) {
    header('Location: ../../index.php');
    exit();
}

$userId = $_SESSION['user_id'];

try {
    // Consulta para obtener las solicitudes del estudiante incluyendo el programa y tipo de solicitud
    $stmt = $pdo->prepare("
SELECT 
    s.id AS solicitud_id,
    s.estado,
    s.fecha_solicitud,
    s.nota,
    s.archivo_pdf,
    s.numero_caso,
    s.numero_resolucion,
    sp.nombre_subprograma AS subprograma,
    p.nombre_programa AS programa,
    GROUP_CONCAT(ts.nombre_tipo SEPARATOR ', ') AS tipo_solicitud
FROM solicitudes s
LEFT JOIN subprogramas sp ON s.subprograma_id_anterior = sp.id
LEFT JOIN programas p ON sp.programa_id = p.id
LEFT JOIN detalles_solicitudes ds ON s.id = ds.solicitud_id
LEFT JOIN tipos_solicitudes ts ON ds.tipo_solicitud_id = ts.id
WHERE s.usuario_id = :userId
GROUP BY s.id, s.estado, s.fecha_solicitud, s.nota, s.archivo_pdf, s.numero_caso, s.numero_resolucion, sp.nombre_subprograma, p.nombre_programa
    ");
    $stmt->execute(['userId' => $userId]);
    $solicitudes = $stmt->fetchAll(PDO::FETCH_ASSOC);
} catch (PDOException $e) {
    die("Error al obtener las solicitudes: " . $e->getMessage());
}
?>
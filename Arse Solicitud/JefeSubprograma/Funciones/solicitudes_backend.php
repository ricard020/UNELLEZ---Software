<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
verificarRol('JP');

$subprogramaId = $_GET['subprograma_id'] ?? null;
$estado = $_GET['estado'] ?? 'Pendiente';

if (!$subprogramaId) {
    die("Error: No se especificó el subprograma.");
}

try {
    // Consulta para obtener las solicitudes según el estado seleccionado
    $stmt = $pdo->prepare("
        SELECT 
            s.id AS solicitud_id,
            s.fecha_solicitud,
            s.archivo_pdf,
            s.nota,
            s.numero_caso,
            s.numero_resolucion,
            CONCAT_WS(' ', u.primer_nombre, u.primer_apellido) AS nombre_estudiante,
            GROUP_CONCAT(ts.nombre_tipo SEPARATOR ', ') AS tipo_solicitud
        FROM solicitudes s
        LEFT JOIN subprogramas sp ON s.subprograma_id_anterior = sp.id
        LEFT JOIN programas p ON sp.programa_id = p.id
        LEFT JOIN detalles_solicitudes ds ON s.id = ds.solicitud_id
        LEFT JOIN tipos_solicitudes ts ON ds.tipo_solicitud_id = ts.id
        LEFT JOIN usuarios u ON s.usuario_id = u.id
        WHERE s.subprograma_id_anterior = :subprogramaId AND s.estado = :estado
        GROUP BY s.id, s.fecha_solicitud, s.archivo_pdf, s.nota, s.numero_caso, s.numero_resolucion, nombre_estudiante
    ");
    $stmt->execute(['subprogramaId' => $subprogramaId, 'estado' => $estado]);
    $solicitudes = $stmt->fetchAll(PDO::FETCH_ASSOC);
} catch (PDOException $e) {
    die("Error al obtener las solicitudes: " . $e->getMessage());
}
?>
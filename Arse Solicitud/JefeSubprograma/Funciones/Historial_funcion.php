<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/Admin_seguridad.php';
verificarRol('JP');

// Obtener el subprograma del jefe de subprograma
$jefe_id = $_SESSION['user_id'];
$stmt = $pdo->prepare("SELECT subprograma_id FROM jefe_subprogramas WHERE jefe_id = :jefe_id");
$stmt->execute(['jefe_id' => $jefe_id]);
$jefe_subprograma = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$jefe_subprograma) {
    die("Error: No se encontró el subprograma del jefe.");
}

$subprograma_id = $jefe_subprograma['subprograma_id'];

// Obtener las solicitudes del subprograma
$stmt = $pdo->prepare("
    SELECT hs.*, 
           CONCAT(u.primer_nombre, ' ', u.primer_apellido) AS nombre_usuario,
           GROUP_CONCAT(ts.nombre_tipo SEPARATOR ', ') AS tipos_solicitud,
           s.numero_caso,
           s.numero_resolucion
    FROM historial_solicitudes hs
    LEFT JOIN detalles_solicitudes ds ON hs.solicitud_id = ds.solicitud_id
    LEFT JOIN tipos_solicitudes ts ON ds.tipo_solicitud_id = ts.id
    LEFT JOIN usuarios u ON hs.usuario_id = u.id
    LEFT JOIN solicitudes s ON hs.solicitud_id = s.id
    WHERE hs.subprograma_id_anterior = :subprograma_id
    GROUP BY hs.id
");
$stmt->execute(['subprograma_id' => $subprograma_id]);
$solicitudes = $stmt->fetchAll(PDO::FETCH_ASSOC);
?>
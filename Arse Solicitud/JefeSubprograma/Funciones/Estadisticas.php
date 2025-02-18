<?php
header('Content-Type: application/json');
require_once '../../PHP/bd_conexion.php';
require_once '../../PHP/Admin_seguridad.php';
verificarRol('JP');

try {
    $jefe_id = $_SESSION['user_id'];
    
    // Obtener el subprograma del jefe de subprograma
    $stmt = $pdo->prepare("SELECT subprograma_id FROM jefe_subprogramas WHERE jefe_id = :jefe_id");
    $stmt->execute(['jefe_id' => $jefe_id]);
    $jefe_subprograma = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$jefe_subprograma) {
        throw new Exception("Error: No se encontró el subprograma del jefe.");
    }

    $subprograma_id = $jefe_subprograma['subprograma_id'];

    if (isset($_GET['meses_disponibles'])) {
        // Obtener los meses disponibles con datos
        $sql = "
            SELECT DISTINCT
                YEAR(fecha_solicitud) AS anio,
                MONTH(fecha_solicitud) AS mes
            FROM historial_solicitudes
            WHERE subprograma_id_anterior = :subprograma_id
            ORDER BY anio, mes
        ";
        $stmt = $pdo->prepare($sql);
        $stmt->execute(['subprograma_id' => $subprograma_id]);
        echo json_encode($stmt->fetchAll(PDO::FETCH_ASSOC));
        exit;
    }

    if (isset($_GET['mes'])) {
        // Filtrar solicitudes por el mes seleccionado
        $fecha = $_GET['mes'];
        [$anio, $mes] = explode('-', $fecha);
        $sql = "
            SELECT 
                ts.nombre_tipo AS tipo_solicitud,
                COUNT(hs.id) AS total
            FROM historial_solicitudes hs
            LEFT JOIN detalles_solicitudes ds ON hs.solicitud_id = ds.solicitud_id
            LEFT JOIN tipos_solicitudes ts ON ds.tipo_solicitud_id = ts.id
            WHERE YEAR(hs.fecha_solicitud) = :anio 
              AND MONTH(hs.fecha_solicitud) = :mes
              AND hs.subprograma_id_anterior = :subprograma_id
            GROUP BY ts.nombre_tipo
            ORDER BY total DESC
        ";
        $stmt = $pdo->prepare($sql);
        $stmt->execute(['anio' => $anio, 'mes' => $mes, 'subprograma_id' => $subprograma_id]);
        echo json_encode($stmt->fetchAll(PDO::FETCH_ASSOC));
        exit;
    }

    echo json_encode(['error' => 'Solicitud no válida.']);
} catch (Exception $e) {
    echo json_encode(['error' => 'Error del servidor: ' . $e->getMessage()]);
}
?>

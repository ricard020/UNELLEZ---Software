<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
verificarRol('JP');

if (!isset($_SESSION['user_id'])) {
    die("Acceso denegado.");
}

$solicitudId = $_GET['id'] ?? null;

if (!$solicitudId) {
    die("Solicitud no especificada.");
}

try {
    $stmt = $pdo->prepare("
        SELECT archivo_pdf 
        FROM solicitudes 
        WHERE id = :solicitud_id
    ");
    $stmt->execute(['solicitud_id' => $solicitudId]);
    $solicitud = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$solicitud) {
        die("No se encontró la solicitud.");
    }

    $filePath = $solicitud['archivo_pdf'];

    if (!file_exists($filePath)) {
        die("El archivo no existe.");
    }

    header('Content-Type: application/pdf');
    header('Content-Disposition: inline; filename="' . basename($filePath) . '"');
    header('Content-Transfer-Encoding: binary');
    header('Accept-Ranges: bytes');
    readfile($filePath);
} catch (PDOException $e) {
    die("Error al obtener el archivo: " . $e->getMessage());
}
?>
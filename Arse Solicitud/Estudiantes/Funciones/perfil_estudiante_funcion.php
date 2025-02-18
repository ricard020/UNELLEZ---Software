<?php
require_once '../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    die("Error: Usuario no autenticado.");
}

$idUsuario = $_SESSION['user_id'];

try {
    // Consulta para obtener los subprogramas inscritos del usuario
    $stmt = $pdo->prepare("
        SELECT sp.id, sp.nombre_subprograma 
        FROM subprogramas_estudiantes se
        JOIN subprogramas sp ON se.subprograma_id = sp.id
        WHERE se.usuario_id = :idUsuario
    ");
    $stmt->execute(['idUsuario' => $idUsuario]);

    $subprogramas = $stmt->fetchAll(PDO::FETCH_ASSOC);

    if (empty($subprogramas)) {
        $subprogramas = [];
    }

    // Consulta para obtener las notificaciones no leídas del usuario
    $stmt = $pdo->prepare("
        SELECT id, mensaje, fecha_envio 
        FROM notificaciones 
        WHERE usuario_id = :idUsuario AND leido = 0
    ");
    $stmt->execute(['idUsuario' => $idUsuario]);

    $notificaciones = $stmt->fetchAll(PDO::FETCH_ASSOC);

    // Actualizar el estado de las notificaciones a "leído"
    $stmt = $pdo->prepare("UPDATE notificaciones SET leido = 1 WHERE usuario_id = :idUsuario AND leido = 0");
    $stmt->execute(['idUsuario' => $idUsuario]);

} catch (PDOException $e) {
    die("Error al consultar los datos: " . $e->getMessage());
}
?>
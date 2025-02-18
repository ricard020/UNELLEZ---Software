<?php
require_once '../PHP/bd_conexion.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['id'])) {
    $id = $_POST['id'];

    try {
        $stmt = $pdo->prepare("UPDATE notificaciones SET leido = 1 WHERE id = :id");
        $stmt->execute(['id' => $id]);
        echo 'Notificación marcada como leída';
    } catch (PDOException $e) {
        echo 'Error al marcar la notificación como leída: ' . $e->getMessage();
    }
} else {
    echo 'Solicitud no válida';
}
?>
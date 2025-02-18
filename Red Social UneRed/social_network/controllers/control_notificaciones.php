<?php

require_once '../includes/config/database.php'; // Asegúrate de tener la conexión a la base de datos configurada aquí.

session_start();

// Verificar si el usuario está autenticado
if (!isset($_SESSION['id'])) {
    header("Location: ../login.php");
    exit();
}

$usuario_id = $_SESSION['id'];

// Obtener las notificaciones del usuario
$query = "SELECT n.*, u.nombre AS origen_nombre, u.apellido AS origen_apellido, u.foto_perfil AS origen_foto, p.id AS proyecto_id
          FROM notificaciones n
          JOIN usuarios u ON n.origen_usuario_id = u.id
          LEFT JOIN proyectos p ON n.proyecto_id = p.id  -- Si la notificación está asociada a un proyecto
          WHERE n.usuario_id = ? 
          ORDER BY n.fecha_notificacion DESC";


$stmt = $conn->prepare($query);
$stmt->bind_param("i", $usuario_id);
$stmt->execute();
$result = $stmt->get_result();
$notificaciones = $result->fetch_all(MYSQLI_ASSOC);

// Marcar todas las notificaciones como leídas
$update_query = "UPDATE notificaciones SET leido = 1 WHERE usuario_id = ?";
$update_stmt = $conn->prepare($update_query);
$update_stmt->bind_param("i", $usuario_id);
$update_stmt->execute();
?>



<?php
session_start();  // Iniciar sesión
include('../includes/config/database.php');  // Conexión a la base de datos

if (isset($_SESSION['id'])) {
    $user_id = $_SESSION['id'];
    $post_id = $_POST['post_id'];
    $comment_text = $_POST['comment_text'];

    // Verificar que el comentario no esté vacío
    if (!empty($comment_text)) {
        // Insertar el comentario en la base de datos
        $sql = "INSERT INTO comentarios (proyecto_id, usuario_id, comentario, fecha_comentario) VALUES (?, ?, ?, NOW())";
        $stmt = $conn->prepare($sql);
        $stmt->bind_param("iis", $post_id, $user_id, $comment_text);
        $stmt->execute();

        // Responder con éxito
        echo json_encode(['success' => true]);
    } else {
        // Responder con error si el comentario está vacío
        echo json_encode(['success' => false, 'error' => 'Comentario vacío']);
    }

    $stmt->close();
} else {
    echo json_encode(['success' => false, 'error' => 'Usuario no logueado']);
}

$conn->close();
?>

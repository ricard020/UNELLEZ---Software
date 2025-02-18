<?php
session_start(); // Iniciar sesión
include('../includes/config/database.php');

if (isset($_SESSION['id'])) {
    $user_id = $_SESSION['id'];
    $post_id = $_POST['post_id'];

    // Verificar si el usuario ya retweeteó este post
    $checkQuery = "SELECT * FROM retweets WHERE proyecto_id = ? AND usuario_id = ?";
    $stmt = $conn->prepare($checkQuery);
    $stmt->bind_param("ii", $post_id, $user_id);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        // Si ya existe un retweet, eliminarlo
        $deleteQuery = "DELETE FROM retweets WHERE proyecto_id = ? AND usuario_id = ?";
        $stmt = $conn->prepare($deleteQuery);
        $stmt->bind_param("ii", $post_id, $user_id);
        $stmt->execute();

        echo json_encode(['success' => true, 'action' => 'removed']); // Acción: eliminado
    } else {
        // Si no existe, agregar un nuevo retweet
        $insertQuery = "INSERT INTO retweets (proyecto_id, usuario_id) VALUES (?, ?)";
        $stmt = $conn->prepare($insertQuery);
        $stmt->bind_param("ii", $post_id, $user_id);
        $stmt->execute();

        echo json_encode(['success' => true, 'action' => 'added']); // Acción: añadido
    }

    $stmt->close();
} else {
    echo json_encode(['success' => false, 'error' => 'Usuario no logueado']);
}

$conn->close();
?>

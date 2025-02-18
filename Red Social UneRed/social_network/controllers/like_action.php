<?php
session_start(); // Iniciar sesión
include('../includes/config/database.php');

if (isset($_SESSION['id'])) {
    $user_id = $_SESSION['id'];
    $post_id = $_POST['post_id'];

    // Verificar si el usuario ya dio "Me gusta" a este post
    $checkQuery = "SELECT * FROM valoraciones WHERE proyecto_id = ? AND usuario_id = ? AND valoracion = 'Me gusta'";
    if ($stmt = $conn->prepare($checkQuery)) {
        $stmt->bind_param("ii", $post_id, $user_id);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($result->num_rows > 0) {
            // Si ya existe un "Me gusta", eliminarlo
            $deleteQuery = "DELETE FROM valoraciones WHERE proyecto_id = ? AND usuario_id = ? AND valoracion = 'Me gusta'";
            if ($stmt = $conn->prepare($deleteQuery)) {
                $stmt->bind_param("ii", $post_id, $user_id);
                $stmt->execute();
                echo json_encode(['success' => true, 'action' => 'removed']); // Acción: eliminado
            } else {
                echo json_encode(['success' => false, 'error' => 'Error al eliminar "Me gusta".']);
            }
        } else {
            // Si no existe, agregar un nuevo "Me gusta"
            $insertQuery = "INSERT INTO valoraciones (proyecto_id, usuario_id, valoracion) VALUES (?, ?, 'Me gusta')";
            if ($stmt = $conn->prepare($insertQuery)) {
                $stmt->bind_param("ii", $post_id, $user_id);
                $stmt->execute();
                echo json_encode(['success' => true, 'action' => 'added']); // Acción: añadido
            } else {
                echo json_encode(['success' => false, 'error' => 'Error al agregar "Me gusta".']);
            }
        }

        $stmt->close();
    } else {
        echo json_encode(['success' => false, 'error' => 'Error al verificar la valoración.']);
    }
} else {
    echo json_encode(['success' => false, 'error' => 'Usuario no logueado']);
}

$conn->close();
?>

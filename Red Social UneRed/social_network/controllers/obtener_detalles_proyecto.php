<?php
include('../includes/config/database.php');

if (isset($_GET['post_id'])) {
    $post_id = $_GET['post_id'];

    // Consulta para obtener el título y descripción del proyecto
    $sql = "SELECT titulo, descripcion FROM proyectos WHERE id = ?";
    $stmt = $conn->prepare($sql);
    $stmt->bind_param("i", $post_id);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        $project = $result->fetch_assoc();
        echo json_encode(['success' => true, 'project' => $project]);
    } else {
        echo json_encode(['success' => false, 'error' => 'Proyecto no encontrado']);
    }

    $stmt->close();
} else {
    echo json_encode(['success' => false, 'error' => 'ID de proyecto no proporcionado']);
}

$conn->close();
?>

<?php
include('../includes/config/database.php');

if (isset($_GET['post_id'])) {
    $post_id = $_GET['post_id'];

    $query_comentarios = "SELECT c.comentario, c.fecha_comentario, u.nombre, u.apellido 
                          FROM comentarios c
                          JOIN usuarios u ON c.usuario_id = u.id
                          WHERE c.proyecto_id = ?
                          ORDER BY c.fecha_comentario ASC";

    $stmt_comentarios = $conn->prepare($query_comentarios);
    $stmt_comentarios->bind_param("i", $post_id);
    $stmt_comentarios->execute();
    $resultado_comentarios = $stmt_comentarios->get_result();

    if ($resultado_comentarios->num_rows > 0) {
        while ($comentario = $resultado_comentarios->fetch_assoc()) {
            echo "<div class='comentario card mb-3 p-3'>";
            echo "<div class='align-items-center mb-2'>";
            echo "<i class='bi bi-person-circle me-2 text-primary'></i>";
            echo "<strong>" . htmlspecialchars($comentario['nombre']) . " " . htmlspecialchars($comentario['apellido']) . "</strong>";
            echo "</div>";
            echo "<p class='mb-1'>" . nl2br(htmlspecialchars($comentario['comentario'])) . "</p>";
            echo "<p class='text-muted small'><i class='bi bi-clock me-1'></i>" . $comentario['fecha_comentario'] . "</p>";
            echo "</div>";
        }
    } else {
        echo "<p class='text-muted'><i class='bi bi-info-circle me-2'></i>No hay comentarios aún.</p>";
    }

    $stmt_comentarios->close();
    $conn->close();
}
?>

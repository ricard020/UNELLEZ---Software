<link rel="stylesheet" href="../public/css/comments.css">
<?php
    if (!isset($_SESSION)) {
    session_start();
    }

    if (!isset($_SESSION['id'])) {
    header("Location: ../index.html");
    exit;
    }

    include('../includes/config/database.php');
        
        // Obtener el usuario al que se está accediendo
        if (!isset($_GET['usuario_id'])) {
            echo "<p>Error: No se especificó un usuario.</p>";
            exit;
        }
        
        $user_id = intval($_GET['usuario_id']);

    // archivo de conexión
    include('../includes/config/database.php');

    echo "<br><div style='text-align: left;' id='comentarios-container' class='comentarios'>"; // Añade margen superior para separación
    // Consulta para obtener los comentarios asociados a esta publicación
    $query_comentarios = "SELECT c.comentario, c.fecha_comentario, c.usuario_id, u.nombre, u.apellido, u.foto_perfil, c.proyecto_id
                        FROM comentarios c
                        INNER JOIN usuarios u ON c.usuario_id = u.id
                        WHERE c.usuario_id = ?
                        ORDER BY c.fecha_comentario DESC"; // Orden descendente para comentarios más recientes

    $stmt_comentarios = $conn->prepare($query_comentarios);
    $stmt_comentarios->bind_param("i", $user_id);
    $stmt_comentarios->execute();
    $resultado_comentarios = $stmt_comentarios->get_result();

    if ($resultado_comentarios->num_rows > 0) {
        while ($comentario = $resultado_comentarios->fetch_assoc()) {
            // Mostrar el comentario con la foto de perfil del usuario
        echo "<div style='text-align: left;' class='comentario card mb-3 p-3' onclick=\"window.location.href='../controllers/publicacion_detalle.php?post_id=" . $comentario['proyecto_id'] . "'\" style='cursor: pointer;'>"; // Uso de tarjeta de Bootstrap
        echo "<div class='align-items-center mb-2'>"; // Flexbox para alinear icono y nombre
        
        // Envolvemos la foto de perfil y el nombre en un enlace al perfil del usuario
        $foto_perfil = $comentario['foto_perfil'] ? $comentario['foto_perfil'] : '../images/profile-default.png'; // Foto por defecto si no tiene foto
        echo "<span href='../pages/usuarios_perfil.php?usuario_id=" . $comentario['usuario_id'] . "'>";
        echo "<img src='" . htmlspecialchars($foto_perfil) . "' alt='Foto de perfil' class='rounded-circle me-2' style='width: 40px; height: 40px;'>"; // Foto redonda de perfil
        echo "<strong>" . htmlspecialchars($comentario['nombre']) . " " . htmlspecialchars($comentario['apellido']) . "</strong>";
        echo "</span>";
        
        echo "</div>";
        echo "<p class='mb-1'><i class='bi bi-text-left'></i>" . nl2br(htmlspecialchars($comentario['comentario'])) . "</p>";
        echo "<p class='commentDate small'><i class='bi bi-clock me-1'></i>" . $comentario['fecha_comentario'] . "</p>"; // Icono de reloj
        echo "</div>";
        }
    } else {
        echo "<p><i class='bi bi-info-circle me-2'></i>No has hecho comentarios aún.</p>"; // Mensaje con icono
    }

    echo "</div>"; // Cierra los comentarios
?>
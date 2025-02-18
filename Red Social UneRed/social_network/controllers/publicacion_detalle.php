<link rel="stylesheet" href="../public/css/post_users.css">
<link rel="stylesheet" href="../public/css/post_detail.css">
<link rel="stylesheet" href="../public/css/comments.css">
<meta name="viewport" content="width=device-width, initial-scale=1">
<!-- Bootstrap CSS -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
<!-- Bootstrap Icons -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">
<link rel="stylesheet" href="../pages/home.php">

<style>

.justify-content-center > * {
    flex-grow: 1;
    width: 100%;
}
.p-4  {

padding-bottom: 3rem !important;

}

</style>

<!-- Modal de comentario -->
<div style="margin-top: 10%;" class="modal" id="commentModal" tabindex="-1" aria-labelledby="commentModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="commentModalLabel">Agregar Comentario</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <!-- Título y Descripción del Proyecto -->
        <h5 class="titulodeproyecto" id="projectTitle"></h5>
        <h6 class="descripciondeproyecto" id="projectDescription"></h6>

        <!-- Cuadro de Texto para Comentarios -->
        <textarea id="commentText" class="form-control" rows="3" placeholder="Escribe tu comentario..."></textarea>
      </div>
      <div class="modal-footer">
        
        <button type="button" id="submitComment" class="btn btn-primary">Comentar</button>
      </div>
    </div>
  </div>
</div>

<div class='container d-flex align-items-start justify-content-center py-4 w-100'>

<!-- Modal Imagen-->
<div class="modal fade" id="imageModal" tabindex="-1" aria-labelledby="imageModalLabel" aria-hidden="true">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <img id="modalImage" src="" class="img-fluid" alt="Imagen en Grande">
      </div>
    </div>
  </div>
</div>


<script>
function openImageModal(imageSrc) {
    document.getElementById('modalImage').src = imageSrc;
    var imageModal = new bootstrap.Modal(document.getElementById('imageModal'));
    imageModal.show();
}
</script>


<?php

echo"<div style='max-width: 800px' class='postDetailCard card p-4'>";

include '../includes/partials/navbar.php';

    if (!isset($_SESSION)) {
        session_start();
    }

    if (!isset($_SESSION['id'])) {
        header("Location: ../index.html");
        exit;
    }
    $user_id = $_SESSION['id'];

    // archivo de conexión
    include('../includes/config/database.php');

    // Verificar si se ha recibido el ID de la publicación (post_id)
    if (isset($_GET['post_id'])) {
        $post_id = $_GET['post_id'];

        // Consulta para obtener los detalles del proyecto (publicación)
        $query = "SELECT p.id, p.titulo, p.descripcion, p.fecha_publicacion, u.nombre, u.apellido, u.foto_perfil, 
                 (SELECT GROUP_CONCAT(DISTINCT a.archivo_url) FROM archivos_proyectos a WHERE a.proyecto_id = p.id) AS archivos, 
                 i.imagen_url AS imagen,  -- Seleccionar solo la primera imagen
                 AVG(v.valoracion) AS valoracion_promedio,
                 (SELECT GROUP_CONCAT(DISTINCT c.nombre) 
                  FROM proyectos_categorias pc 
                  JOIN categorias c ON pc.categoria_id = c.id 
                  WHERE pc.proyecto_id = p.id) AS categorias,
                 (SELECT COUNT(*) FROM retweets r WHERE r.proyecto_id = p.id) AS retweet_count,
                 (SELECT COUNT(*) FROM retweets r WHERE r.proyecto_id = p.id AND r.usuario_id = ?) AS user_retweeted,
                 (SELECT COUNT(*) FROM valoraciones v2 WHERE v2.proyecto_id = p.id AND v2.valoracion = 'Me gusta') AS like_count,
                 (SELECT COUNT(*) FROM comentarios c2 WHERE c2.proyecto_id = p.id) AS comment_count,
                 u.id AS usuario_id
          FROM proyectos p
          JOIN usuarios u ON p.usuario_id = u.id
          LEFT JOIN imagenes_proyectos i ON p.id = i.proyecto_id
          LEFT JOIN valoraciones v ON p.id = v.proyecto_id
          WHERE p.id = ?
          GROUP BY p.id";

        $stmt = $conn->prepare($query);
        $stmt->bind_param("ii", $user_id, $post_id);  // Se pasan ambos parámetros: el ID del usuario y el ID del proyecto
        $stmt->execute();
        $resultado = $stmt->get_result();

        if ($resultado->num_rows > 0) {
        $row = $resultado->fetch_assoc();
        // Detalles del proyecto
        $titulo = $row['titulo'];
        $descripcion = $row['descripcion'];
        $fecha_publicacion = $row['fecha_publicacion'];
        $nombre_usuario = $row['nombre'];
        $apellido_usuario = $row['apellido'];
        $foto_perfil = $row['foto_perfil'];
        $archivos = explode(",", $row['archivos']);
        $imagen = $row['imagen'];  // Solo una imagen
        $categorias = $row['categorias'];  // Lista de categorías separadas por coma

        // Eliminar duplicados en las categorías
        if ($categorias) {
        $categorias_array = explode(",", $categorias);
        $categorias_array = array_unique($categorias_array); // Elimina duplicados
        $categorias = implode(", ", $categorias_array);  // Vuelve a convertir en cadena
        }

        // Mostrar los detalles del proyecto
        echo "<div class='post card mb-3'>";
        echo "<div class='card-body'>";

        // Sección del usuario (ahora clicable)
        echo "<div class='align-items-center mb-2 postProfileCard'>";

        // Envolvemos la imagen y nombre en un enlace a la página de perfil del usuario
        echo "<a href='../pages/usuarios_perfil.php?usuario_id=" . $row['usuario_id'] . "'>";

        echo "<img src='" . ($row['foto_perfil'] ? $row['foto_perfil'] : 'default_profile.jpg') . "' alt='Foto de perfil' class='rounded-circle me-2' style='width: 40px; height: 40px;'>";
        echo "<span><strong>" . htmlspecialchars($row['nombre']) . " " . htmlspecialchars($row['apellido']) . "</strong></span>";
        echo "</a>";

        echo "</div>";

    // Título y descripción
    echo "<h5 class='card-title'><i class='bi bi-card-heading'></i> " . htmlspecialchars($titulo) . "</h5>";
    echo "<p class='card-text'><i class='bi bi-text-left'></i> " . nl2br(htmlspecialchars($descripcion)) . "</p>";
    echo "<p><i class='bi bi-calendar'></i> Publicado el " . htmlspecialchars($fecha_publicacion) . "</p>";


    // Categorías (sin duplicados)
    if ($categorias) {
        echo "<p><i class='bi bi-tags'></i> <strong>Categorías:</strong> " . htmlspecialchars($categorias) . "</p>";
    }

    // Mostrar archivos relacionados (si existen)
    if ($row['archivos']) {
      echo "<h6><i class='bi bi-file-earmark'></i> Archivos:</h6>";
      $archivos = explode(",", $row['archivos']);
      foreach ($archivos as $archivo) {
          echo "<a href='$archivo' target='_blank' onclick='event.stopPropagation();' class='postFileButton btn btn-link'><i class='bi bi-download'></i> Ver archivo</a><br>";
      }
    }

    // Mostrar solo una imagen relacionada
    if ($imagen) {
        echo "<h6><i class='bi bi-image'></i> Imágenes:</h6>";
        echo "<div class='text-center'>"; // Agregar este contenedor
        echo "<img src='$imagen' class='img-fluid mb-2' alt='Imagen del proyecto' onclick='event.stopPropagation(); openImageModal(\"$imagen\");'><br>";
        echo "</div>"; // Cerrar el contenedor
    }

            // Botones de interacción (retweet, me gusta, comentarios)
            echo "<div class='d-flex align-items-center postActionButtonsContainer'>";

            // Verifica si el usuario ya ha retweeteado el post
            $isRetweeted = $row['user_retweeted'] > 0 ? 'retweeted' : '';  // La clase 'retweeted' es para el color verde
            $btnText = $row['user_retweeted'] > 0 ? '' : '';  // Texto dinámico

            // Botón de retweet
            echo "<button class='btn2 retweet-btn d-flex align-items-center $isRetweeted' data-user-id='$user_id' data-post-id='" . $row['id'] . "' onclick='event.stopPropagation()'>
                <i class='bi bi-arrow-repeat me-1'></i> 
                <span class='retweet-count'>" . $row['retweet_count'] . "</span>
                <span class='retweet-text ms-1'>$btnText</span>
            </button>";

            // Lógica para verificar si el usuario ha dado "Me gusta"
$projectId = $row['id'];  // ID del proyecto
$userId = $user_id;  // ID del usuario (debería ser proporcionado dinámicamente)

// Obtener el número total de likes del proyecto
$sql = "SELECT COUNT(*) AS like_count FROM valoraciones WHERE proyecto_id = ? AND valoracion = 'Me gusta'";
$stmt = $conn->prepare($sql);
$stmt->bind_param("i", $projectId);
$stmt->execute();
$result = $stmt->get_result();
$rowLike = $result->fetch_assoc();
$likeCount = $rowLike['like_count'];  // Número total de "Me gusta" del proyecto

// Verificar si el usuario actual ha dado "Me gusta"
$sql = "SELECT COUNT(*) AS user_like_count FROM valoraciones WHERE proyecto_id = ? AND usuario_id = ? AND valoracion = 'Me gusta'";
$stmt = $conn->prepare($sql);
$stmt->bind_param("ii", $projectId, $userId);
$stmt->execute();
$result = $stmt->get_result();
$rowUserLike = $result->fetch_assoc();
$hasLiked = $rowUserLike['user_like_count'] > 0;  // Si el usuario ya ha dado "Me gusta"

$likeButtonClass = $hasLiked ? 'liked' : '';  // Agregar clase 'liked' si el usuario ya dio "Me gusta"
$likeButtonText = $hasLiked ? '' : '';  // Texto dinámico para "Me gusta"

// Mostrar el botón de "Me gusta"
echo "<button class='btn2 like-btn d-flex align-items-center $likeButtonClass' data-user-id='$userId' data-post-id='$projectId' onclick='event.stopPropagation()'>
    <i class='bi bi-heart me-1'></i> 
    <span class='like-count'>" . $likeCount . "</span>
    <span class='like-text ms-1'>$likeButtonText</span>
</button>";

            // Lógica para obtener la cantidad de comentarios
            $sql = "SELECT COUNT(*) AS comment_count FROM comentarios WHERE proyecto_id = ?";
            $stmt = $conn->prepare($sql);
            $stmt->bind_param("i", $projectId);
            $stmt->execute();
            $result = $stmt->get_result();
            $rowComment = $result->fetch_assoc();
            $commentCount = $rowComment['comment_count'];  // Número de comentarios del proyecto

            // Mostrar el botón de comentarios
            echo "<button class='btn2 comment-btn2 d-flex align-items-center' data-post-id='$projectId' onclick='event.stopPropagation()'>
                <i class='bi bi-chat-left-text me-1'></i> 
                <span class='comment-count'>" . $commentCount . "</span>
                <span class='comment-text ms-1'></span>
            </button>";

            

            echo "</div>"; // Cierra la fila de botones

            echo "</div>"; // Cierra card-body
            echo "</div>"; // Cierra post card

            // Contenedor de comentarios
echo "<div id='comentarios-container' class='comentarios mt-4'>"; // Añade margen superior para separación
echo "<h5 class='mb-3'><i class='bi bi-chat-dots me-2'></i>Comentarios:</h5>"; // Icono de comentarios

// Consulta para obtener los comentarios asociados a esta publicación
$query_comentarios = "SELECT c.comentario, c.fecha_comentario, c.usuario_id, u.nombre, u.apellido, u.foto_perfil
                      FROM comentarios c
                      JOIN usuarios u ON c.usuario_id = u.id
                      WHERE c.proyecto_id = ?
                      ORDER BY c.fecha_comentario DESC"; // Orden descendente para comentarios más recientes

$stmt_comentarios = $conn->prepare($query_comentarios);
$stmt_comentarios->bind_param("i", $post_id);
$stmt_comentarios->execute();
$resultado_comentarios = $stmt_comentarios->get_result();

if ($resultado_comentarios->num_rows > 0) {
    while ($comentario = $resultado_comentarios->fetch_assoc()) {
        // Mostrar el comentario con la foto de perfil del usuario
        echo "<div class='comentario card mb-3 p-3'>"; // Uso de tarjeta de Bootstrap
        echo "<div class='postProfileCard align-items-center mb-2'>"; // Flexbox para alinear icono y nombre
        
        // Envolvemos la foto de perfil y el nombre en un enlace al perfil del usuario
        $foto_perfil = $comentario['foto_perfil'] ? $comentario['foto_perfil'] : '../images/profile-default.png'; // Foto por defecto si no tiene foto
        echo "<a href='../pages/usuarios_perfil.php?usuario_id=" . $comentario['usuario_id'] . "'>";
        echo "<img src='" . htmlspecialchars($foto_perfil) . "' alt='Foto de perfil' class='rounded-circle me-2' style='width: 40px; height: 40px;'>"; // Foto redonda de perfil
        echo "<strong>" . htmlspecialchars($comentario['nombre']) . " " . htmlspecialchars($comentario['apellido']) . "</strong>";
        echo "</a>";
        
        echo "</div>";
        echo "<p class='mb-1'><i class='bi bi-text-left'></i>" . nl2br(htmlspecialchars($comentario['comentario'])) . "</p>";
        echo "<p class='commentDate small'><i class='bi bi-clock me-1'></i>" . $comentario['fecha_comentario'] . "</p>"; // Icono de reloj
        echo "</div>";
    }
} else {
    echo "<p><i class='bi bi-info-circle me-2'></i>No hay comentarios aún.</p>"; // Mensaje con icono
}

echo "</div>"; // Cierra los comentarios

        } else {
            echo "<p>Proyecto no encontrado.</p>";
        }
    } else {
        echo "<p>ID de proyecto no proporcionado.</p>";
    }

    $conn->close();

echo"</div>";

?>

<!--En este archivo se maneja el proceso de hacer retweet-->
<script src="../public/js/retweet.js"></script>
<script src="../public/js/like.js"></script>
<script src="../public/js/comentario.js"></script>
<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

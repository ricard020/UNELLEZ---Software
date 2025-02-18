<link rel="stylesheet" href="../public/css/post_users.css">

<!-- Modal de comentario -->
<div class="modal" id="commentModal" tabindex="-1" aria-labelledby="commentModalLabel" aria-hidden="true">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="commentModalLabel">Agregar Comentario</h5>
        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
      </div>
      <div class="modal-body">
        <!-- Título y Descripción del Proyecto -->
        <h5 class="titulodeproyecto" id="projectTitle"></h5>
        <p class="descripciondeproyecto" id="projectDescription"></p>

        <!-- Cuadro de Texto para Comentarios -->
        <textarea id="commentText" class="form-control" rows="3" placeholder="Escribe tu comentario..."></textarea>
      </div>
      <div class="modal-footer">
        
        <button type="button" id="submitComment" class="btn btn-primary">Comentar</button>
      </div>
    </div>
  </div>
</div>

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
    
    $usuario_id = intval($_GET['usuario_id']);

    // Consulta para obtener los proyectos (posts) junto con sus archivos, imágenes, valoraciones y categorías
    $query = "SELECT p.id, p.titulo, p.descripcion, p.fecha_publicacion, u.nombre, u.apellido, u.foto_perfil, 
                 (SELECT GROUP_CONCAT(DISTINCT a.archivo_url) FROM archivos_proyectos a WHERE a.proyecto_id = p.id) AS archivos, 
                 (SELECT GROUP_CONCAT(DISTINCT i.imagen_url) FROM imagenes_proyectos i WHERE i.proyecto_id = p.id) AS imagenes, 
                 AVG(v.valoracion) AS valoracion_promedio,
                 (SELECT GROUP_CONCAT(DISTINCT c.nombre) 
                  FROM proyectos_categorias pc 
                  JOIN categorias c ON pc.categoria_id = c.id 
                  WHERE pc.proyecto_id = p.id) AS categorias,
                 (SELECT COUNT(*) FROM retweets r WHERE r.proyecto_id = p.id) AS retweet_count,
                 (SELECT COUNT(*) FROM retweets r WHERE r.proyecto_id = p.id AND r.usuario_id = $usuario_id) AS user_retweeted,
                 (SELECT COUNT(*) FROM valoraciones v2 WHERE v2.proyecto_id = p.id AND v2.valoracion = 'Me gusta') AS like_count,
                 (SELECT COUNT(*) FROM comentarios c2 WHERE c2.proyecto_id = p.id) AS comment_count
          FROM retweets r
          LEFT JOIN proyectos p ON p.id = r.proyecto_id
          JOIN usuarios u ON p.usuario_id = u.id
          LEFT JOIN valoraciones v ON p.id = v.proyecto_id
          WHERE r.usuario_id = $usuario_id AND r.proyecto_id = p.id
          GROUP BY p.id
          ORDER BY p.fecha_publicacion DESC";


    $resultado = $conn->query($query); 

    // Verificar si hay resultados
    if ($resultado->num_rows > 0) {
        while ($row = $resultado->fetch_assoc()) {
            
            // Generación dinámica de publicaciones
            echo "<br><div style='max-width: 800px; text-align: left;' class='post card mb-3' onclick=\"window.location.href='../controllers/publicacion_detalle.php?post_id=" . $row['id'] . "'\" style='cursor: pointer;'>";

echo "<div class='card-body'>";

// Sección del usuario
echo "<div class=' align-items-center mb-2 postProfileCard'>";
echo "<img src='" . ($row['foto_perfil'] ? $row['foto_perfil'] : 'default_profile.jpg') . "' alt='Foto de perfil' class='rounded-circle me-2' style='width: 40px; height: 40px;'>";
echo "<span><strong>" . htmlspecialchars($row['nombre']) . " " . htmlspecialchars($row['apellido']) . "</strong></span>";
echo "</div>";

// Título y descripción
echo "<h5 class='card-title'><i class='bi bi-card-heading'></i> " . htmlspecialchars($row['titulo']) . "</h5>";
echo "<p class='card-text'><i class='bi bi-text-left'></i> " . nl2br(htmlspecialchars($row['descripcion'])) . "</p>";
echo "<p><i class='bi bi-calendar'></i> Publicado el " . $row['fecha_publicacion'] . "</p>";

// Categorías
if ($row['categorias']) {
    echo "<p><i class='bi bi-tags'></i> <strong>Categorías:</strong> " . htmlspecialchars($row['categorias']) . "</p>";
}

// Mostrar archivos relacionados (si existen)
if ($row['archivos']) {
    echo "<h6><i class='bi bi-file-earmark'></i> Archivos:</h6>";
    $archivos = explode(",", $row['archivos']);
    foreach ($archivos as $archivo) {
        echo "<a href='$archivo' target='_blank' class='postFileButton'><i class='bi bi-download'></i> Ver archivo</a><br>";
    }
}

// Mostrar imágenes relacionadas (si existen)
if ($row['imagenes']) {
    echo "<h6><i class='bi bi-image'></i> Imágenes:</h6>";
    $imagenes = explode(",", $row['imagenes']);
    foreach ($imagenes as $imagen) {
        echo "<div class='text-center'>"; // Agregar este contenedor
        echo "<img src='$imagen' class='img-fluid mb-2' alt='Imagen del proyecto' onclick='event.stopPropagation(); openImageModal(\"$imagen\");'><br>";
        echo "</div>"; // Cerrar el contenedor
    }
}
// Botones de interacción (retweet, me gusta, comentarios)
echo "<div class='d-flex align-items-center postActionButtonsContainer' onclick='event.stopPropagation();'>";

// Verifica si el usuario ya ha retweeteado el post
$isRetweeted = $row['user_retweeted'] > 0 ? 'retweeted' : '';  // La clase 'retweeted' es para el color verde
$btnText = $row['user_retweeted'] > 0 ? '' : '';  // Texto dinámico

// Botón de retweet
echo "<button class='btn retweet-btn d-flex align-items-center $isRetweeted' data-user-id='$usuario_id' data-post-id='" . $row['id'] . "'>
    <i class='bi bi-arrow-repeat me-1'></i> 
    <span class='retweet-count'>" . $row['retweet_count'] . "</span>
    <span class='retweet-text ms-1'>$btnText</span>
</button>";

// Lógica para verificar si el usuario ha dado "Me gusta"
$projectId = $row['id'];  // ID del proyecto
$userId = $usuario_id;  // ID del usuario (debería ser proporcionado dinámicamente)

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
echo "<button class='btn like-btn d-flex align-items-center $likeButtonClass' data-user-id='$userId' data-post-id='$projectId'>
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
echo "<button class='btn comment-btn2 d-flex align-items-center' data-post-id='$projectId'>
    <i class='bi bi-chat-left-text me-1'></i> 
    <span class='comment-count'>" . $commentCount . "</span>
    <span class='comment-text ms-1'></span>
</button>";

echo "</div>"; // Cierra la fila de botones
echo "</div>"; // Cierra card-body
echo "</div>"; // Cierra post card

        }
    } else {
        echo "<p>No hay publicaciones compartidas por ti.</p>";
    }

    $conn->close(); // Cierra la conexión
?>

<!--En este archivo se maneja el proceso de hacer retweet-->
<script src="../public/js/retweet_for_shared.js"></script>
<script src="../public/js/like.js"></script>
<script src="../public/js/comentario.js"></script>

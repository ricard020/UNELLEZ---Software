<?php
require '../includes/config/database.php';

// Consulta para obtener los proyectos (posts) junto con sus archivos, imágenes y valoraciones
$query = "SELECT p.id, p.titulo, p.descripcion, p.fecha_publicacion, u.nombre, u.apellido, 
                 GROUP_CONCAT(a.archivo_url) AS archivos, 
                 GROUP_CONCAT(i.imagen_url) AS imagenes, 
                 AVG(v.valoracion) AS valoracion_promedio
          FROM proyectos p
          JOIN usuarios u ON p.usuario_id = u.id
          LEFT JOIN archivos_proyectos a ON p.id = a.proyecto_id
          LEFT JOIN imagenes_proyectos i ON p.id = i.proyecto_id
          LEFT JOIN valoraciones v ON p.id = v.proyecto_id
          GROUP BY p.id
          ORDER BY p.fecha_publicacion DESC";  // Ordena los posts de más recientes a más antiguos

$resultado = $conexion->query($query);

// Verificar si hay resultados
if ($resultado->num_rows > 0) {
    while ($row = $resultado->fetch_assoc()) {
        echo "<div class='post card mb-3'>";
        echo "<div class='card-body'>";
        echo "<h5 class='card-title'>" . htmlspecialchars($row['titulo']) . "</h5>";
        echo "<p class='card-text'>" . nl2br(htmlspecialchars($row['descripcion'])) . "</p>";
        echo "<p class='text-muted'>Publicado por " . htmlspecialchars($row['nombre']) . " " . htmlspecialchars($row['apellido']) . " el " . $row['fecha_publicacion'] . "</p>";
        
        // Mostrar archivos relacionados (si existen)
        if ($row['archivos']) {
            echo "<h6>Archivos:</h6>";
            $archivos = explode(",", $row['archivos']);
            foreach ($archivos as $archivo) {
                echo "<a href='$archivo' target='_blank' class='btn btn-link'>Ver archivo</a><br>";
            }
        }

        // Mostrar imágenes relacionadas (si existen)
        if ($row['imagenes']) {
            echo "<h6>Imágenes:</h6>";
            $imagenes = explode(",", $row['imagenes']);
            foreach ($imagenes as $imagen) {
                echo "<img src='$imagen' class='img-fluid mb-2' alt='Imagen del proyecto'><br>";
            }
        }

        // Mostrar la valoración promedio
        if ($row['valoracion_promedio']) {
            echo "<p><strong>Valoración Promedio: </strong>" . round($row['valoracion_promedio'], 2) . "/5</p>";
        } else {
            echo "<p><strong>No hay valoraciones aún.</strong></p>";
        }

        echo "</div>";  // Cierra card-body
        echo "</div>";  // Cierra post card
    }
} else {
    echo "<p>No hay publicaciones.</p>";
}

$conexion->close();
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Publicaciones de Proyectos</title>
    <!-- Enlazar Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">

    <div class="container mt-5">
        <h1 class="text-center mb-4">Publicaciones de Proyectos</h1>

        <!-- Incluir los posts -->
        <?php include('ver_post.php'); ?>

    </div>

    <!-- Enlazar scripts de Bootstrap -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>

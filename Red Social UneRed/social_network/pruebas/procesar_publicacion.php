<?php
if (!isset($_SESSION)) {
    session_start();
}

if (!isset($_SESSION['id'])) {
    header("Location: login.php");
    exit;
}

include '../includes/config/database.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $usuario_id = $_SESSION['id'];
    $titulo = $conn->real_escape_string($_POST['titulo']);
    $descripcion = $conn->real_escape_string($_POST['descripcion']);
    $categorias = $_POST['categorias'];

    // Insertar proyecto en la tabla 'proyectos'
    $query = "INSERT INTO proyectos (usuario_id, titulo, descripcion, fecha_publicacion) 
              VALUES ('$usuario_id', '$titulo', '$descripcion', NOW())";
    if ($conn->query($query)) {
        $proyecto_id = $conn->insert_id;

        // Insertar categorías en la tabla 'proyectos_categorias'
        foreach ($categorias as $categoria_id) {
            $query_categoria = "INSERT INTO proyectos_categorias (proyecto_id, categoria_id) 
                                VALUES ('$proyecto_id', '$categoria_id')";
            $conn->query($query_categoria);
        }

        // Manejar imágenes
if (!empty($_FILES['imagenes']['name'][0])) {
    foreach ($_FILES['imagenes']['tmp_name'] as $index => $tmpName) {
        $nombreImagen = basename($_FILES['imagenes']['name'][$index]);
        $rutaImagen = "../uploads/threads/images/images" . $nombreImagen; // ubicación donde se guarda el archivo

        
        if (move_uploaded_file($tmpName, $rutaImagen)) {
            $query_imagen = "INSERT INTO imagenes_proyectos (proyecto_id, imagen_url) 
                             VALUES ('$proyecto_id', '$rutaImagen')";
            $conn->query($query_imagen);
        } else {
            echo "Error al mover la imagen: $nombreImagen.";
        }
    }
}

// Manejar archivos adjuntos
if (!empty($_FILES['archivos']['name'][0])) {
    foreach ($_FILES['archivos']['tmp_name'] as $index => $tmpName) {
        $nombreArchivo = basename($_FILES['archivos']['name'][$index]);
        $rutaArchivo = "../uploads/threads/docs/docs" . $nombreArchivo; // ubicación donde se guarda el archivo

        
        if (move_uploaded_file($tmpName, $rutaArchivo)) {
            $query_archivo = "INSERT INTO archivos_proyectos (proyecto_id, archivo_url) 
                              VALUES ('$proyecto_id', '$rutaArchivo')";
            $conn->query($query_archivo);
        } else {
            echo "Error al mover el archivo: $nombreArchivo.";
        }
    }
}


        echo "Proyecto publicado exitosamente.";
    } else {
        echo "Error al publicar el proyecto: " . $conn->error;
    }

header("location: ../pages/home.php");

}
?>

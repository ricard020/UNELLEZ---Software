<?php
session_start();
require_once '../includes/config/database.php';

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Recibir datos del formulario
    $titulo = isset($_POST['titulo']) ? trim($_POST['titulo']) : null;
    $descripcion = isset($_POST['descripcion']) ? trim($_POST['descripcion']) : null;
    $categorias = isset($_POST['categorias']) ? $_POST['categorias'] : null;
    $usuario_id = $_SESSION['id']; // ID del usuario autenticado

    // Validación de campos obligatorios (sin mostrar errores)
    if (!$titulo || !$descripcion || !$categorias || !$usuario_id) {
        header("Location: ../pages/home.php");
        exit;
    }

    // Insertar proyecto en la base de datos
    $stmt = $conn->prepare("INSERT INTO proyectos (usuario_id, titulo, descripcion, fecha_publicacion) VALUES (?, ?, ?, NOW())");
    if (!$stmt) {
        die("Error en la preparación de la consulta: " . $conn->error);
    }
    $stmt->bind_param("iss", $usuario_id, $titulo, $descripcion);

    if ($stmt->execute()) {
        $proyecto_id = $stmt->insert_id;

        // Guardar la categoría
        $stmt_categoria = $conn->prepare("INSERT INTO proyectos_categorias (proyecto_id, categoria_id) VALUES (?, ?)");
        if (!$stmt_categoria) {
            die("Error preparando la consulta de categorías: " . $conn->error);
        }
        $stmt_categoria->bind_param("ii", $proyecto_id, $categorias);
        $stmt_categoria->execute();
        $stmt_categoria->close();
    }
    $stmt->close();

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


    // Redirigir al archivo home.php después de completar el proceso
    header("Location: ../pages/home.php");
    exit;
} else {
    header("Location: ../pages/home.php");
    exit;
}
?>

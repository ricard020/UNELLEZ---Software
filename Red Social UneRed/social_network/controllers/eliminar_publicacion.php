<?php
// Iniciar sesión si no está iniciada
if (!isset($_SESSION)) {
    session_start();
}

// Verificar que el usuario está autenticado
if (!isset($_SESSION['id'])) {
    echo json_encode(['success' => false, 'error' => 'No autorizado']);
    exit;
}

$user_id = $_SESSION['id'];

// Incluir la conexión a la base de datos
include('../includes/config/database.php');

// Verificar si se recibió un `post_id`
if (!isset($_GET['post_id']) || empty($_GET['post_id'])) {
    echo json_encode(['success' => false, 'error' => 'ID de publicación no válido']);
    exit;
}

$post_id = intval($_GET['post_id']); // Convertir a entero por seguridad

try {
    // Iniciar transacción
    $conn->begin_transaction();

    // Verificar si la publicación pertenece al usuario autenticado
    $query_verificar = "SELECT usuario_id FROM proyectos WHERE id = ?";
    $stmt_verificar = $conn->prepare($query_verificar);
    $stmt_verificar->bind_param("i", $post_id);
    $stmt_verificar->execute();
    $resultado_verificar = $stmt_verificar->get_result();

    if ($resultado_verificar->num_rows === 0) {
        throw new Exception("Publicación no encontrada");
    }

    $row = $resultado_verificar->fetch_assoc();
    if ($row['usuario_id'] !== $user_id) {
        throw new Exception("No tienes permisos para eliminar esta publicación");
    }

    // **1. Eliminar archivos asociados**
    $query_archivos = "SELECT archivo_url FROM archivos_proyectos WHERE proyecto_id = ?";
    $stmt_archivos = $conn->prepare($query_archivos);
    $stmt_archivos->bind_param("i", $post_id);
    $stmt_archivos->execute();
    $resultado_archivos = $stmt_archivos->get_result();

    while ($archivo = $resultado_archivos->fetch_assoc()) {
        $archivo_path = "../uploads/" . $archivo['archivo_url'];
        if (file_exists($archivo_path)) {
            unlink($archivo_path); // Eliminar archivo del servidor
        }
    }

    // **2. Eliminar imágenes asociadas**
    $query_imagenes = "SELECT imagen_url FROM imagenes_proyectos WHERE proyecto_id = ?";
    $stmt_imagenes = $conn->prepare($query_imagenes);
    $stmt_imagenes->bind_param("i", $post_id);
    $stmt_imagenes->execute();
    $resultado_imagenes = $stmt_imagenes->get_result();

    while ($imagen = $resultado_imagenes->fetch_assoc()) {
        $imagen_path = "../uploads/" . $imagen['imagen_url'];
        if (file_exists($imagen_path)) {
            unlink($imagen_path); // Eliminar imagen del servidor
        }
    }

    // **3. Eliminar registros en tablas relacionadas**
    $tablas_relacionadas = ['notificaciones', 'archivos_proyectos', 'imagenes_proyectos', 'valoraciones', 'comentarios', 'retweets', 'proyectos_categorias'];
    foreach ($tablas_relacionadas as $tabla) {
        $query_delete = "DELETE FROM $tabla WHERE proyecto_id = ?";
        $stmt_delete = $conn->prepare($query_delete);
        $stmt_delete->bind_param("i", $post_id);
        $stmt_delete->execute();
    }

    // **4. Eliminar la publicación**
    $query_eliminar = "DELETE FROM proyectos WHERE id = ?";
    $stmt_eliminar = $conn->prepare($query_eliminar);
    $stmt_eliminar->bind_param("i", $post_id);
    $stmt_eliminar->execute();

    // Confirmar la transacción
    $conn->commit();

    echo json_encode(['success' => true]);

} catch (Exception $e) {
    $conn->rollback(); // Deshacer la transacción en caso de error
    echo json_encode(['success' => false, 'error' => $e->getMessage()]);
}

// Cerrar conexiones
$stmt_verificar->close();
$stmt_archivos->close();
$stmt_imagenes->close();
$stmt_eliminar->close();
$conn->close();
?>



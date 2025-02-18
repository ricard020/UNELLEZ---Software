<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['error' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];

if (!isset($_FILES['nuevaFoto']) || $_FILES['nuevaFoto']['error'] !== UPLOAD_ERR_OK) {
    echo json_encode(['error' => 'Error al subir la foto.']);
    exit();
}

$foto = $_FILES['nuevaFoto'];
$ext = pathinfo($foto['name'], PATHINFO_EXTENSION);
$nombreArchivo = uniqid('foto_perfil_') . '.' . $ext;
$rutaDestino = '../../imagen/foto_perfil/' . $nombreArchivo;

// Verificar si el directorio de destino existe, si no, crearlo
if (!is_dir('../../imagen/foto_perfil/')) {
    mkdir('../../imagen/foto_perfil/', 0777, true);
}

try {
    // Obtener la ruta de la foto actual
    $stmt = $pdo->prepare("SELECT foto_perfil FROM usuarios WHERE id = :userId");
    $stmt->execute(['userId' => $userId]);
    $user = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($user && $user['foto_perfil']) {
        $rutaActual = "../../" . $user['foto_perfil'];
        if (file_exists($rutaActual)) {
            unlink($rutaActual); // Eliminar la foto actual
        }
    }

    // Mover el archivo subido a la ubicación deseada
    if (!move_uploaded_file($foto['tmp_name'], $rutaDestino)) {
        echo json_encode(['error' => 'Error al mover la foto subida.']);
        exit();
    }

    // Actualizar la ruta de la foto en la base de datos
    $stmt = $pdo->prepare("UPDATE usuarios SET foto_perfil = :fotoPerfil WHERE id = :userId");
    $stmt->execute([
        'fotoPerfil' => "imagen/foto_perfil/$nombreArchivo",
        'userId' => $userId
    ]);

    echo json_encode(['success' => true]);
} catch (Exception $e) {
    echo json_encode(['error' => 'Error al actualizar la foto de perfil. Por favor, intente de nuevo más tarde.']);
}
?>
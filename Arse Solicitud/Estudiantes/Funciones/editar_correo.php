<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['error' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];
$data = json_decode(file_get_contents('php://input'), true);

if (!$data['contrasenaActual'] || !$data['nuevoCorreo']) {
    echo json_encode(['error' => 'Todos los campos son obligatorios.']);
    exit();
}

if (!filter_var($data['nuevoCorreo'], FILTER_VALIDATE_EMAIL)) {
    echo json_encode(['error' => 'Correo electrónico inválido.']);
    exit();
}

try {
    // Verificar la contraseña actual
    $stmt = $pdo->prepare("SELECT contrasena FROM usuarios WHERE id = :userId");
    $stmt->execute(['userId' => $userId]);
    $user = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$user || $user['contrasena'] !== hash('sha256', $data['contrasenaActual'])) {
        echo json_encode(['error' => 'La contraseña actual es incorrecta.']);
        exit();
    }

    // Actualizar el correo
    $stmt = $pdo->prepare("UPDATE usuarios SET correo = :nuevoCorreo WHERE id = :userId");
    $stmt->execute([
        'nuevoCorreo' => $data['nuevoCorreo'],
        'userId' => $userId
    ]);

    echo json_encode(['success' => true]);
} catch (Exception $e) {
    echo json_encode(['error' => 'Error al actualizar el correo. Por favor, intente de nuevo más tarde.']);
}
?>

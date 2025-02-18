<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['error' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];
$data = json_decode(file_get_contents('php://input'), true);

if (!$data['contrasenaActual'] || !$data['nuevaContrasena']) {
    echo json_encode(['error' => 'Todos los campos son obligatorios.']);
    exit();
}

if (!preg_match('/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/', $data['nuevaContrasena'])) {
    echo json_encode(['error' => 'La nueva contraseña no cumple con los requisitos.']);
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

    // Generar el hash de la nueva contraseña
    $nuevaContrasenaHashed = hash('sha256', $data['nuevaContrasena']);

    // Actualizar con la nueva contraseña
    $updateStmt = $pdo->prepare("UPDATE usuarios SET contrasena = :nuevaContrasena WHERE id = :userId");
    $updateStmt->execute([
        'nuevaContrasena' => $nuevaContrasenaHashed,
        'userId' => $userId
    ]);

    session_unset();
    session_destroy();

    echo json_encode(['success' => true, 'redirect' => '../../index.html']);
} catch (Exception $e) {
    echo json_encode(['error' => 'Error al actualizar la contraseña. Por favor, intente de nuevo más tarde.']);
}
?>
<?php
require_once '../includes/config/database.php';

session_start();

if (!isset($_SESSION['email_verificacion'])) {
    echo json_encode(['success' => false, 'message' => 'Sesión no válida.']);
    exit;
}

$data = json_decode(file_get_contents('php://input'), true);
$password = $data['password'];
$email = $_SESSION['email_verificacion'];

// Encripta la nueva contraseña
$contrasena_hashed = password_hash($password, PASSWORD_BCRYPT);

try {
    $stmt = $conn->prepare("UPDATE usuarios SET contrasena = ?, codigo_recuperacion = NULL, fecha_expiracion_codigo = NULL WHERE email = ?");
    $stmt->bind_param("ss", $contrasena_hashed, $email);
    $stmt->execute();

    if ($stmt->affected_rows > 0) {
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Error al restablecer la contraseña.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>

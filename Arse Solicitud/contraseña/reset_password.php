<?php
require_once '../PHP/bd_conexion.php';

session_start();

if (!isset($_SESSION['email_verificacion'])) {
    echo json_encode(['success' => false, 'message' => 'Sesión no válida.']);
    exit;
}

$data = json_decode(file_get_contents('php://input'), true);
$password = hash('sha256', $data['password']);
$email = $_SESSION['email_verificacion'];

try {
    $stmt = $pdo->prepare("UPDATE usuarios SET contrasena = ?, codigo_recuperacion = NULL, fecha_expiracion_codigo = NULL WHERE correo = ?");
    $stmt->execute([$password, $email]);

    if ($stmt->rowCount() > 0) {
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Error al restablecer la contraseña.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>
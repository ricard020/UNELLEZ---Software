<?php
require_once '../includes/config/database.php';

$data = json_decode(file_get_contents('php://input'), true);
$code = $data['code'];

try {
    $stmt = $conn->prepare("SELECT * FROM usuarios WHERE codigo_recuperacion = ? AND fecha_expiracion_codigo > NOW()");
    $stmt->bind_param("s", $code);
    $stmt->execute();
    $result = $stmt->get_result();
    $usuario = $result->fetch_assoc();

    if ($usuario) {
        session_start();
        $_SESSION['email_verificacion'] = $usuario['email'];
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Código incorrecto o expirado.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>

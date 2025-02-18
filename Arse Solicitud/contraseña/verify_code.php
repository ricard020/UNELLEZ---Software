<?php
require_once '../PHP/bd_conexion.php';

$data = json_decode(file_get_contents('php://input'), true);
$code = $data['code'];

try {
    $stmt = $pdo->prepare("SELECT * FROM usuarios WHERE codigo_recuperacion = ? AND fecha_expiracion_codigo > NOW()");
    $stmt->execute([$code]);
    $result = $stmt->fetchAll();

    if (count($result) > 0) {
        session_start();
        $_SESSION['email_verificacion'] = $result[0]['correo'];
        echo json_encode(['success' => true]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Código incorrecto o expirado.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => $e->getMessage()]);
}
?>
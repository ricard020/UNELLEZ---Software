<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['success' => false, 'message' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];
$data = json_decode(file_get_contents('php://input'), true);

if (!$data['primerNombre'] || !$data['primerApellido'] || !$data['correo']) {
    echo json_encode(['success' => false, 'message' => 'Todos los campos son obligatorios.']);
    exit();
}

try {
    $stmt = $pdo->prepare("
        UPDATE usuarios 
        SET primer_nombre = :primerNombre, primer_apellido = :primerApellido, correo = :correo 
        WHERE id = :userId
    ");
    $stmt->execute([
        'primerNombre' => $data['primerNombre'],
        'primerApellido' => $data['primerApellido'],
        'correo' => $data['correo'],
        'userId' => $userId
    ]);

    if ($stmt->rowCount() > 0) {
        echo json_encode(['success' => true, 'message' => 'Datos actualizados']);
    } else {
        echo json_encode(['success' => false, 'message' => 'No se realizaron cambios en los datos.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => 'Error al actualizar los datos del usuario.', 'error' => $e->getMessage()]);
}
?>

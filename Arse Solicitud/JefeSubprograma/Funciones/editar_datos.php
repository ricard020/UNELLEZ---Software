<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['error' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];
$data = json_decode(file_get_contents('php://input'), true);

error_log(print_r($data, true)); // Agrega esta línea para depuración

if (empty($data['primerNombre']) || empty($data['primerApellido'])) {
    echo json_encode(['error' => 'Todos los campos son obligatorios.']);
    exit();
}

try {
    // Actualizar los datos del usuario
    $updateStmt = $pdo->prepare("UPDATE usuarios SET primer_nombre = :primerNombre, primer_apellido = :primerApellido WHERE id = :userId");
    $updateStmt->execute([
        'primerNombre' => $data['primerNombre'],
        'primerApellido' => $data['primerApellido'],
        'userId' => $userId
    ]);

    echo json_encode(['success' => true]);
} catch (Exception $e) {
    echo json_encode(['error' => 'Error al actualizar los datos. Por favor, intente de nuevo más tarde.']);
}
?>

<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['success' => false, 'message' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];

try {
    $stmt = $pdo->prepare("
        SELECT 
            u.primer_nombre,
            u.primer_apellido,
            u.correo,
            sp.nombre_subprograma AS subprograma
        FROM usuarios u
        JOIN jefe_subprogramas js ON u.id = js.jefe_id
        JOIN subprogramas sp ON js.subprograma_id = sp.id
        WHERE u.id = :userId
    ");
    $stmt->execute(['userId' => $userId]);
    $data = $stmt->fetch(PDO::FETCH_ASSOC);

    if ($data) {
        echo json_encode(['success' => true, 'data' => $data]);
    } else {
        echo json_encode(['success' => false, 'message' => 'No se encontraron datos.']);
    }
} catch (Exception $e) {
    echo json_encode(['success' => false, 'message' => 'Error al obtener los datos del usuario.']);
}
?>
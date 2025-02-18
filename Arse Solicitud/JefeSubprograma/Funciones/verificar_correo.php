<?php
require_once '../../PHP/bd_conexion.php';

$data = json_decode(file_get_contents('php://input'), true);
$correo = $data['correo'];

$stmt = $pdo->prepare("SELECT COUNT(*) FROM usuarios WHERE correo = :correo");
$stmt->execute(['correo' => $correo]);
$count = $stmt->fetchColumn();

echo json_encode(['exists' => $count > 0]);
?>
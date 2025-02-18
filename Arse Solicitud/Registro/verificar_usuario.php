<?php
require_once '../php/bd_conexion.php';

$response = ['exists' => false];

if (isset($_GET['username'])) {
    $username = $_GET['username'];
    $sql = "SELECT id FROM usuarios WHERE Nombre_usuario = ?";
    $stmt = $pdo->prepare($sql);
    $stmt->execute([$username]);
    if ($stmt->rowCount() > 0) {
        $response['exists'] = true;
    }
}

if (isset($_GET['email'])) {
    $email = $_GET['email'];
    $sql = "SELECT id FROM usuarios WHERE correo = ?";
    $stmt = $pdo->prepare($sql);
    $stmt->execute([$email]);
    if ($stmt->rowCount() > 0) {
        $response['exists'] = true;
    }
}

if (isset($_GET['cedula'])) {
    $cedula = $_GET['cedula'];
    $sql = "SELECT id FROM usuarios WHERE ci = ?";
    $stmt = $pdo->prepare($sql);
    $stmt->execute([$cedula]);
    if ($stmt->rowCount() > 0) {
        $response['exists'] = true;
    }
}

header('Content-Type: application/json');
echo json_encode($response);
?>
<?php
require_once '../../PHP/bd_conexion.php';

function obtenerEstudiantes(PDO $pdo) {
    $query = "
        SELECT 
            u.id, 
            u.nombre_usuario, 
            u.primer_nombre, 
            u.segundo_nombre, 
            u.primer_apellido, 
            u.segundo_apellido, 
            u.correo, 
            u.fecha_creacion,
            GROUP_CONCAT(s.nombre_subprograma SEPARATOR ', ') AS subprogramas
        FROM usuarios u
        LEFT JOIN subprogramas_estudiantes se ON u.id = se.usuario_id
        LEFT JOIN subprogramas s ON se.subprograma_id = s.id
        WHERE u.rol = 'Estudiante'
        GROUP BY u.id
    ";
    return $pdo->query($query)->fetchAll(PDO::FETCH_ASSOC);
}

function obtenerTodosSubprogramas(PDO $pdo) {
    $query = "SELECT id, nombre_subprograma FROM subprogramas";
    return $pdo->query($query)->fetchAll(PDO::FETCH_ASSOC);
}

function eliminarEstudiante(PDO $pdo, $id) {
    $pdo->beginTransaction();
    $stmt1 = $pdo->prepare("DELETE FROM subprogramas_estudiantes WHERE usuario_id = :id");
    $stmt1->execute(['id' => $id]);
    $stmt2 = $pdo->prepare("DELETE FROM usuarios WHERE id = :id");
    $stmt2->execute(['id' => $id]);
    $pdo->commit();
    return true;
}

function actualizarEstudiante(PDO $pdo, $id, $nombre_usuario, $contrasena) {
    // Verificar si ya existe un nombre de usuario
    $sql_verificar_usuario = "
        SELECT COUNT(*) FROM usuarios
        WHERE nombre_usuario = :nombre_usuario AND id != :id
    ";
    $stmt_verificar_usuario = $pdo->prepare($sql_verificar_usuario);
    $stmt_verificar_usuario->execute([':nombre_usuario' => $nombre_usuario, ':id' => $id]);
    $existe_usuario = $stmt_verificar_usuario->fetchColumn();

    if ($existe_usuario > 0) {
        return "El nombre de usuario ya está en uso. Por favor, elija otro nombre de usuario.";
    }

    $pdo->beginTransaction();
    $stmt = $pdo->prepare("UPDATE usuarios SET nombre_usuario = :nombre_usuario, contrasena = :contrasena WHERE id = :id");
    $stmt->execute([
        'nombre_usuario' => $nombre_usuario, 
        'contrasena' => hash('sha256', $contrasena), 
        'id' => $id
    ]);
    $pdo->commit();
    return true;
}

// Manejar la solicitud POST
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    header('Content-Type: application/json');
    $input = json_decode(file_get_contents('php://input'), true);

    if (isset($input['action'])) {
        try {
            if ($input['action'] === 'delete') {
                eliminarEstudiante($pdo, $input['id']);
                echo json_encode(['success' => true]);
            } elseif ($input['action'] === 'update') {
                $id = $input['id'];
                $nombre_usuario = $input['nombre_usuario'];
                $contrasena = $input['contrasena'];
                $result = actualizarEstudiante($pdo, $id, $nombre_usuario, $contrasena);
                if ($result === true) {
                    echo json_encode(['success' => true]);
                } else {
                    echo json_encode(['success' => false, 'message' => $result]);
                }
            } else {
                echo json_encode(['success' => false, 'message' => 'Acción no válida.']);
            }
        } catch (Exception $e) {
            echo json_encode(['success' => false, 'message' => $e->getMessage()]);
        }
    }
    exit;
}
?>
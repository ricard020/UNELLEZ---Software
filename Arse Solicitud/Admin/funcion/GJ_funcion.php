<?php
require_once '../../PHP/bd_conexion.php';

/**
 * Obtiene la lista de jefes de subprograma y sus subprogramas asignados.
 * @param PDO $pdo Conexión a la base de datos.
 * @return array Lista de jefes y sus subprogramas.
 */
function obtenerJefesSubprograma($pdo) {
    try {
        $sql = "
            SELECT u.id, u.nombre_usuario, u.primer_nombre, u.segundo_nombre, 
                   u.primer_apellido, u.segundo_apellido,
                   u.correo, u.fecha_creacion,
                   GROUP_CONCAT(s.nombre_subprograma SEPARATOR ', ') AS subprogramas
            FROM usuarios u
            LEFT JOIN jefe_subprogramas js ON u.id = js.jefe_id
            LEFT JOIN subprogramas s ON js.subprograma_id = s.id
            WHERE u.rol = 'JP'
            GROUP BY u.id
            ORDER BY u.id;
        ";
        $stmt = $pdo->query($sql);
        return $stmt->fetchAll(PDO::FETCH_ASSOC);
    } catch (PDOException $e) {
        die("Error al obtener jefes de subprograma: " . $e->getMessage());
    }
}

/**
 * Obtiene la lista de todos los subprogramas disponibles.
 * @param PDO $pdo Conexión a la base de datos.
 * @return array Lista de subprogramas.
 */
function obtenerTodosSubprogramas($pdo) {
    try {
        $sql = "SELECT id, nombre_subprograma FROM subprogramas ORDER BY nombre_subprograma";
        $stmt = $pdo->query($sql);
        return $stmt->fetchAll(PDO::FETCH_ASSOC);
    } catch (PDOException $e) {
        die("Error al obtener subprogramas: " . $e->getMessage());
    }
}

/**
 * Elimina un jefe de subprograma.
 * @param PDO $pdo Conexión a la base de datos.
 * @param int $id ID del jefe.
 * @return bool True si la eliminación fue exitosa, false en caso contrario.
 */
function eliminarJefe(PDO $pdo, $id) {
    try {
        $pdo->beginTransaction();
        $stmt1 = $pdo->prepare("DELETE FROM jefe_subprogramas WHERE jefe_id = :id");
        $stmt1->execute(['id' => $id]);
        $stmt2 = $pdo->prepare("DELETE FROM usuarios WHERE id = :id");
        $stmt2->execute(['id' => $id]);
        $pdo->commit();
        return true;
    } catch (PDOException $e) {
        $pdo->rollBack();
        throw new Exception("Error al eliminar jefe de subprograma: " . $e->getMessage());
    }
}

/**
 * Actualiza los datos de un jefe de subprograma.
 * @param PDO $pdo Conexión a la base de datos.
 * @param int $id ID del jefe.
 * @param string $nombre_usuario Nombre de usuario del jefe.
 * @param string $nombre Nombre del jefe.
 * @param string $apellido Apellido del jefe.
 * @param string $correo Correo del jefe.
 * @return bool|string True si la actualización fue exitosa, mensaje de error en caso contrario.
 */
function actualizarJefeSubprograma($pdo, $id, $nombre_usuario, $nombre, $apellido, $correo, $password = null) {
    try {
        if (nombreUsuarioExiste($pdo, $nombre_usuario, $id)) {
            return "El nombre de usuario ya existe.";
        }

        $pdo->beginTransaction();
        $query = "
            UPDATE usuarios
            SET nombre_usuario = :nombre_usuario, primer_nombre = :nombre, primer_apellido = :apellido, correo = :correo
        ";
        $params = [
            ':nombre_usuario' => $nombre_usuario,
            ':nombre' => $nombre,
            ':apellido' => $apellido,
            ':correo' => $correo,
            ':id' => $id
        ];

        if (!empty($password)) {
            $passwordHash = hash('sha256', $password);
            $query .= ", contrasena = :contrasena";
            $params[':contrasena'] = $passwordHash;
        }

        $query .= " WHERE id = :id AND rol = 'JP'";

        $stmt = $pdo->prepare($query);
        $stmt->execute($params);
        $pdo->commit();
        return true;
    } catch (PDOException $e) {
        $pdo->rollBack();
        return "Error al actualizar el jefe de subprograma: " . $e->getMessage();
    }
}

/**
 * Verifica si un nombre de usuario ya existe en la base de datos.
 * @param PDO $pdo Conexión a la base de datos.
 * @param string $nombre_usuario Nombre de usuario a verificar.
 * @param int|null $id ID del usuario (opcional).
 * @return bool True si el nombre de usuario ya existe, false en caso contrario.
 */
function nombreUsuarioExiste($pdo, $nombre_usuario, $id = null) {
    try {
        $query = "SELECT COUNT(*) FROM usuarios WHERE nombre_usuario = :nombre_usuario";
        $params = [':nombre_usuario' => $nombre_usuario];

        if ($id !== null) {
            $query .= " AND id != :id";
            $params[':id'] = $id;
        }

        $stmt = $pdo->prepare($query);
        $stmt->execute($params);
        return $stmt->fetchColumn() > 0;
    } catch (PDOException $e) {
        throw new Exception("Error al verificar el nombre de usuario: " . $e->getMessage());
    }
}

// Manejar la solicitud POST
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    header('Content-Type: application/json');
    $data = json_decode(file_get_contents('php://input'), true);
    $action = $data['action'];

    if ($action === 'update') {
        $id = $data['id'];
        $nombre_usuario = $data['nombre_usuario'];
        $nombre = $data['nombre'];
        $apellido = $data['apellido'];
        $correo = $data['correo'];
        $password = $data['password'];
        $result = actualizarJefeSubprograma($pdo, $id, $nombre_usuario, $nombre, $apellido, $correo, $password);
        if ($result === true) {
            echo json_encode(['success' => true]);
        } else {
            echo json_encode(['success' => false, 'message' => $result]);
        }
    } elseif ($action === 'delete') {
        $id = $data['id'];
        try {
            $result = eliminarJefe($pdo, $id);
            echo json_encode(['success' => true]);
        } catch (Exception $e) {
            echo json_encode(['success' => false, 'message' => $e->getMessage()]);
        }
    } else {
        echo json_encode(['success' => false, 'message' => 'Acción no válida']);
    }
}
?>

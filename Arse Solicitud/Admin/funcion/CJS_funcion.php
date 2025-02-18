<?php
require_once '../../PHP/bd_conexion.php';

/**
 * Crea un nuevo jefe de subprograma y asigna un subprograma.
 * @param PDO $pdo Conexión a la base de datos.
 * @param array $datos Datos del jefe.
 * @param int $subprograma_id ID del subprograma seleccionado.
 * @return string Mensaje de estado.
 */
function crearJefeSubprograma($pdo, $datos, $subprograma_id) {
    try {
        // Verificar si ya existe un jefe para el subprograma
        $sql_verificar_subprograma = "
            SELECT COUNT(*) FROM jefe_subprogramas
            WHERE subprograma_id = :subprograma_id
        ";
        $stmt_verificar_subprograma = $pdo->prepare($sql_verificar_subprograma);
        $stmt_verificar_subprograma->execute([':subprograma_id' => $subprograma_id]);
        $existe_subprograma = $stmt_verificar_subprograma->fetchColumn();

        if ($existe_subprograma > 0) {
            return "No se puede crear otra cuenta para este jefe de subprograma, ya existe.";
        }

        // Verificar si ya existe un nombre de usuario
        $sql_verificar_usuario = "
            SELECT COUNT(*) FROM usuarios
            WHERE nombre_usuario = :nombre_usuario
        ";
        $stmt_verificar_usuario = $pdo->prepare($sql_verificar_usuario);
        $stmt_verificar_usuario->execute([':nombre_usuario' => $datos['nombre_usuario']]);
        $existe_usuario = $stmt_verificar_usuario->fetchColumn();

        if ($existe_usuario > 0) {
            return "El nombre de usuario ya está en uso. Por favor, elija otro nombre de usuario.";
        }

        // Comenzar transacción
        $pdo->beginTransaction();

        // Encriptar la contraseña
        $contrasena_hash = hash('sha256', $datos['contrasena']);

        // Generar valores aleatorios para CI y correo
        $ci_aleatorio = 'temporal-' . bin2hex(random_bytes(5));
        $correo_aleatorio = 'temporal_' . bin2hex(random_bytes(5)) . '@temporal.com';

        // Insertar datos del jefe de subprograma
        $sql = "
            INSERT INTO usuarios (
                nombre_usuario, contrasena, rol, ci, correo
            ) VALUES (
                :nombre_usuario, :contrasena, 'JP', :ci, :correo
            );
        ";
        $stmt = $pdo->prepare($sql);
        $stmt->execute([
            ':nombre_usuario' => $datos['nombre_usuario'],
            ':contrasena' => $contrasena_hash,
            ':ci' => $ci_aleatorio,
            ':correo' => $correo_aleatorio,
        ]);

        // Obtener el ID del jefe creado
        $jefe_id = $pdo->lastInsertId();

        // Asignar el subprograma seleccionado al jefe
        $sql_relacion = "
            INSERT INTO jefe_subprogramas (jefe_id, subprograma_id)
            VALUES (:jefe_id, :subprograma_id);
        ";
        $stmt_relacion = $pdo->prepare($sql_relacion);
        $stmt_relacion->execute([
            ':jefe_id' => $jefe_id,
            ':subprograma_id' => $subprograma_id,
        ]);

        // Confirmar la transacción
        $pdo->commit();
        return "Cuenta de Jefe de Subprograma creada exitosamente.";
    } catch (PDOException $e) {
        // Revertir la transacción en caso de error
        $pdo->rollBack();
        return "Error al crear el Jefe de Subprograma: " . $e->getMessage();
    }
}

// Manejar la solicitud POST
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $datos = [
        'nombre_usuario' => $_POST['nombre_usuario'],
        'contrasena' => $_POST['contrasena'],
    ];
    $subprograma_id = $_POST['subprograma'];

    $mensaje = crearJefeSubprograma($pdo, $datos, $subprograma_id);
    echo "<script>alert(" . json_encode($mensaje) . "); window.location.href='../Super_usuario/creacion_jefe_subprograma.php';</script>";
}
?>
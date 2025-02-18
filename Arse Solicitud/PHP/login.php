<?php
// Archivo: /php/login.php
session_start(); // Iniciar sesión

// Importar la conexión a la base de datos
require_once './bd_conexion.php';

// Verificar que los datos fueron enviados
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $nombreUsuario = $_POST['nombre_usuario'];
    $password = $_POST['password'];

    try {
        // Consulta para obtener el usuario con el nombre de usuario proporcionado
        $stmt = $pdo->prepare("SELECT * FROM usuarios WHERE nombre_usuario = :nombre_usuario");
        $stmt->execute(['nombre_usuario' => $nombreUsuario]);
        $user = $stmt->fetch();

        // Validar contraseña con SHA2
        if ($user && hash('sha256', $password) === $user['contrasena']) {
            // Contraseña correcta, establecer sesión
            $_SESSION['user_id'] = $user['id'];
            $_SESSION['user_name'] = $user['nombre_usuario'];
            $_SESSION['user_role'] = $user['rol'];

            // Redirigir según el rol del usuario
            if ($user['rol'] === 'SA') {
                header("Location: ../Admin/menu_super_usuario.php");
            } elseif ($user['rol'] === 'JP') {
                header("Location: ../JefeSubprograma/perfil_jefesubprograma.php");
            } elseif ($user['rol'] === 'Estudiante') {
                header("Location: ../Estudiantes/perfil_estudiante.php");
            } else {
                echo "Rol no reconocido.";
            }
            exit();
        } else {
            // Credenciales incorrectas
            echo "<script>alert('Nombre de usuario o contraseña incorrecta.'); window.history.back();</script>";
        }
    } catch (PDOException $e) {
        die("Error en el login: " . $e->getMessage());
    }
} else {
    // Si no es una solicitud POST, redirigir al login
    header("Location: ../index.html");
    exit();
}
?>

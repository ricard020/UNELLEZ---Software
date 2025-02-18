<?php
session_start();
require '../includes/config/database.php';

// Verifica que el usuario ha iniciado sesión correctamente
if (!isset($_SESSION["nombre_usuario"])) {
    die("Error: No se ha iniciado sesión. Por favor, inicie sesión para continuar.");
}

// Obtiene el correo electrónico del usuario desde la sesión
$email_usuario = $_SESSION["nombre_usuario"]; // Usamos el correo almacenado en la sesión como identificador

// Consulta para obtener los datos del usuario
$query = "SELECT nombre, apellido, carrera, semestre, foto_perfil, email, contrasena FROM usuarios WHERE email = ?";
$stmt = $conn->prepare($query);
$stmt->bind_param("s", $email_usuario);
$stmt->execute();
$result = $stmt->get_result();
$user = $result->fetch_assoc();

// Verifica si el usuario existe
if (!$user) {
    die("Error: No se encontraron datos de usuario. Intente nuevamente.");
}

// Asignar los valores del usuario a las variables
$nombre_usuario = $user['nombre'];
$apellido_usuario = $user['apellido'];
$carrera_usuario = $user['carrera'];
$semestre_usuario = $user['semestre'];
$correo_usuario = $user['email'];
$contrasena_usuario = $user['contrasena'];
$foto_perfil = !empty($user['foto_perfil']) ? $user['foto_perfil'] : 'profile-default.png';
$foto_perfil_path = '../uploads/profiles/' . $email_usuario . '.jpg';

// Verifica si la imagen de perfil existe, si no, usa la imagen por defecto
if (!file_exists($foto_perfil_path)) {
    $foto_perfil_path = '../public/images/profile-default.png';
}

// Manejo de la actualización de datos
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Manejo de actualización de la foto de perfil
    if (isset($_FILES['new_profile_picture']) && $_FILES['new_profile_picture']['error'] == UPLOAD_ERR_OK) {
        $file_tmp_path = $_FILES['new_profile_picture']['tmp_name'];
        $file_name = $_FILES['new_profile_picture']['name'];
        $file_extension = pathinfo($file_name, PATHINFO_EXTENSION);

        // Define una lista de extensiones permitidas
        $allowed_extensions = ['jpg', 'jpeg', 'png'];

        if (in_array(strtolower($file_extension), $allowed_extensions)) {
            $new_file_path = '../uploads/profiles/' . $email_usuario . '.' . $file_extension;

            // Mover el archivo a la carpeta de destino
            if (move_uploaded_file($file_tmp_path, $new_file_path)) {
                // Actualizar la ruta en la base de datos
                $update_photo_query = "UPDATE usuarios SET foto_perfil = ? WHERE email = ?";
                $stmt = $conn->prepare($update_photo_query);

                if ($stmt) {
                    $stmt->bind_param("ss", $new_file_path, $email_usuario);
                    $stmt->execute();
                    $stmt->close();

                    echo "<script>alert('Foto de perfil actualizada correctamente.'); window.location.href = 'perfil.php';</script>";
                } else {
                    echo "<script>alert('Error al actualizar la foto en la base de datos.');</script>";
                }
            } else {
                echo "<script>alert('Error al subir la imagen. Intente nuevamente.');</script>";
            }
        } else {
            echo "<script>alert('Formato de archivo no permitido. Solo se permiten JPG, JPEG y PNG.');</script>";
        }
    }

    // Manejo de actualización de los demás datos del perfil
    $nuevo_nombre = isset($_POST["nombre"]) ? $_POST["nombre"] : null;
    $nuevo_apellido = isset($_POST["apellido"]) ? $_POST["apellido"] : null;
    $nueva_carrera = isset($_POST["carrera"]) ? $_POST["carrera"] : null;
    $nuevo_semestre = isset($_POST["semestre"]) ? $_POST["semestre"] : null;
    $nuevo_correo = isset($_POST["email"]) ? $_POST["email"] : null;
    $nueva_contrasena = isset($_POST["contrasena"]) ? $_POST["contrasena"] : null;

    if (isset($nuevo_nombre) && isset($nuevo_apellido) && isset($nueva_carrera) && isset($nuevo_semestre) && isset($nuevo_correo) && isset($nueva_contrasena)) {
        // Validar que la conexión a la base de datos está definida
        if (isset($conn)) {
            // Consulta preparada para actualizar datos
            $update_query = "UPDATE usuarios SET nombre=?, apellido=?, carrera=?, semestre=?, email=?, contrasena=? WHERE email=?";
            $stmt = $conn->prepare($update_query);

            if ($stmt) {
                // Asignar valores a los parámetros
                $stmt->bind_param("sssssss", $nuevo_nombre, $nuevo_apellido, $nueva_carrera, $nuevo_semestre, $nuevo_correo, $nueva_contrasena, $email_usuario);

                // Ejecutar la consulta y verificar el resultado
                if ($stmt->execute()) {
                    // Actualizar datos en la sesión
                    $_SESSION["nombre_usuario"] = $nuevo_correo; // Actualizar el correo en la sesión
                    $_SESSION["nombre"] = $nuevo_nombre;
                    $_SESSION["apellido"] = $nuevo_apellido;

                    echo "<script>alert('Datos actualizados correctamente.'); window.location.href = 'perfil.php';</script>";
                } else {
                    echo "<script>alert('Error al actualizar los datos. Intente nuevamente.');</script>";
                }

                $stmt->close(); 
            } else {
                echo "<script>alert('Error al preparar la consulta.');</script>";
            }
        } else {
            echo "<script>alert('Error: No se pudo establecer la conexión con la base de datos.');</script>";
        }
    }
}
?>



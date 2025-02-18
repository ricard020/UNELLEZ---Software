<?php
require_once './bd_conexion.php';

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $nombre_usuario = $_POST['nombre_usuario'];
    $correo = $_POST['correo'];
    $password = hash('sha256', $_POST['password']);
    $primer_nombre = $_POST['primer_nombre'];
    $segundo_nombre = $_POST['segundo_nombre'];
    $primer_apellido = $_POST['primer_apellido'];
    $segundo_apellido = $_POST['segundo_apellido'];
    $cedula = $_POST['cedula'];
    $cantidadCarreras = $_POST['cantidadCarreras'];

    // Manejo de la foto de perfil
    $foto_perfil = $_FILES['foto_perfil'];
    $extension = pathinfo($foto_perfil['name'], PATHINFO_EXTENSION);
    $nombreArchivo = uniqid('foto_perfil_') . '.' . $extension;
    $foto_perfil_path = '../imagen/foto_perfil/' . $nombreArchivo;

    // Verificar si el directorio de destino existe, si no, crearlo
    if (!is_dir('../imagen/foto_perfil/')) {
        mkdir('../imagen/foto_perfil/', 0777, true);
    }

    move_uploaded_file($foto_perfil['tmp_name'], $foto_perfil_path);

    try {
        $pdo->beginTransaction();

        // Insertar usuario
        $stmt = $pdo->prepare("INSERT INTO usuarios (nombre_usuario, contrasena, primer_nombre, segundo_nombre, primer_apellido, segundo_apellido, ci, correo, foto_perfil, rol) VALUES (:nombre_usuario, :contrasena, :primer_nombre, :segundo_nombre, :primer_apellido, :segundo_apellido, :ci, :correo, :foto_perfil, 'Estudiante')");
        $stmt->execute([
            ':nombre_usuario' => $nombre_usuario,
            ':contrasena' => $password,
            ':primer_nombre' => $primer_nombre,
            ':segundo_nombre' => $segundo_nombre,
            ':primer_apellido' => $primer_apellido,
            ':segundo_apellido' => $segundo_apellido,
            ':ci' => $cedula,
            ':correo' => $correo,
            ':foto_perfil' => 'imagen/foto_perfil/' . $nombreArchivo
        ]);

        $usuario_id = $pdo->lastInsertId();

        // Insertar subprogramas y sedes
        for ($i = 1; $i <= $cantidadCarreras; $i++) {
            $programa_id = $_POST["programa$i"];
            $subprograma_id = $_POST["subprograma$i"];
            $sede_id = $_POST["sede$i"];

            $stmt = $pdo->prepare("INSERT INTO subprogramas_estudiantes (usuario_id, subprograma_id, sede_id) VALUES (:usuario_id, :subprograma_id, :sede_id)");
            $stmt->execute([
                ':usuario_id' => $usuario_id,
                ':subprograma_id' => $subprograma_id,
                ':sede_id' => $sede_id
            ]);
        }

        $pdo->commit();
        echo "<script>alert('Registro exitoso.'); window.location.href='../index.html';</script>";
    } catch (PDOException $e) {
        $pdo->rollBack();
        echo "<script>alert('Error al registrar: " . $e->getMessage() . "'); window.history.back();</script>";
    }
}
?>
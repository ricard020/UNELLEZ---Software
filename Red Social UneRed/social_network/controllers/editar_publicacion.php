

<?php
// Iniciar sesión
if (!isset($_SESSION)) {
    session_start();
}

// Verificar autenticación
if (!isset($_SESSION['id'])) {
    header("Location: ../index.html"); // Redirigir si no está autenticado
    exit;
}

$user_id = $_SESSION['id'];

// Incluir la conexión a la base de datos
include('../includes/config/database.php');

// Verificar si se recibió un `post_id`
if (!isset($_GET['post_id']) || empty($_GET['post_id'])) {
    die("ID de publicación no válido.");
}

$post_id = intval($_GET['post_id']); // Seguridad

// Verificar si la publicación pertenece al usuario autenticado
$query_verificar = "SELECT * FROM proyectos WHERE id = ? AND usuario_id = ?";
$stmt_verificar = $conn->prepare($query_verificar);
$stmt_verificar->bind_param("ii", $post_id, $user_id);
$stmt_verificar->execute();
$resultado_verificar = $stmt_verificar->get_result();

if ($resultado_verificar->num_rows === 0) {
    die("No tienes permisos para editar esta publicación.");
}

// Obtener datos actuales
$publicacion = $resultado_verificar->fetch_assoc();

// Obtener categorías del proyecto
$query_categorias = "SELECT categoria_id FROM proyectos_categorias WHERE proyecto_id = ?";
$stmt_categorias = $conn->prepare($query_categorias);
$stmt_categorias->bind_param("i", $post_id);
$stmt_categorias->execute();
$resultado_categorias = $stmt_categorias->get_result();
$categorias_seleccionadas = [];
while ($fila = $resultado_categorias->fetch_assoc()) {
    $categorias_seleccionadas[] = $fila['categoria_id'];
}

// Obtener todas las categorías disponibles
$query_todas_categorias = "SELECT * FROM categorias";
$resultado_todas_categorias = $conn->query($query_todas_categorias);

// Obtener imágenes del proyecto
$query_imagenes = "SELECT * FROM imagenes_proyectos WHERE proyecto_id = ?";
$stmt_imagenes = $conn->prepare($query_imagenes);
$stmt_imagenes->bind_param("i", $post_id);
$stmt_imagenes->execute();
$resultado_imagenes = $stmt_imagenes->get_result();

// Obtener archivos del proyecto
$query_archivos = "SELECT * FROM archivos_proyectos WHERE proyecto_id = ?";
$stmt_archivos = $conn->prepare($query_archivos);
$stmt_archivos->bind_param("i", $post_id);
$stmt_archivos->execute();
$resultado_archivos = $stmt_archivos->get_result();

if ($_SERVER["REQUEST_METHOD"] === "POST") {
    $titulo = filter_input(INPUT_POST, 'titulo', FILTER_SANITIZE_STRING);
    $descripcion = filter_input(INPUT_POST, 'descripcion', FILTER_SANITIZE_STRING);
    $categorias = $_POST['categorias'] ?? [];

    if (empty($titulo) || empty($descripcion)) {
        die("Todos los campos son obligatorios.");
    }

    // Actualizar proyecto
    $query_actualizar = "UPDATE proyectos SET titulo = ?, descripcion = ? WHERE id = ? AND usuario_id = ?";
    $stmt_actualizar = $conn->prepare($query_actualizar);
    $stmt_actualizar->bind_param("ssii", $titulo, $descripcion, $post_id, $user_id);

    if (!$stmt_actualizar->execute()) {
        die("Error al actualizar la publicación.");
    }

    // Actualizar categorías
    $query_borrar_categorias = "DELETE FROM proyectos_categorias WHERE proyecto_id = ?";
    $stmt_borrar_categorias = $conn->prepare($query_borrar_categorias);
    $stmt_borrar_categorias->bind_param("i", $post_id);
    $stmt_borrar_categorias->execute();

    foreach ($categorias as $categoria_id) {
        $query_insertar_categoria = "INSERT INTO proyectos_categorias (proyecto_id, categoria_id) VALUES (?, ?)";
        $stmt_insertar_categoria = $conn->prepare($query_insertar_categoria);
        $stmt_insertar_categoria->bind_param("ii", $post_id, $categoria_id);
        $stmt_insertar_categoria->execute();
    }

    // Verificar si se suben nuevas imágenes
if (!empty($_FILES['imagenes']['name'][0])) {
    // Eliminar todas las imágenes existentes
    $query_eliminar_imagenes = "DELETE FROM imagenes_proyectos WHERE proyecto_id = ?";
    $stmt_eliminar_imagenes = $conn->prepare($query_eliminar_imagenes);
    $stmt_eliminar_imagenes->bind_param("i", $post_id);
    $stmt_eliminar_imagenes->execute();

    // Subir nuevas imágenes
    foreach ($_FILES['imagenes']['tmp_name'] as $key => $tmp_name) {
        $nombre_imagen = basename($_FILES['imagenes']['name'][$key]);
        $ruta_imagen = "../uploads/threads/images/images" . $nombre_imagen; // Ruta donde se guarda la imagen
        if (move_uploaded_file($tmp_name, $ruta_imagen)) {
            $query_insertar_imagen = "INSERT INTO imagenes_proyectos (proyecto_id, imagen_url) VALUES (?, ?)";
            $stmt_insertar_imagen = $conn->prepare($query_insertar_imagen);
            $stmt_insertar_imagen->bind_param("is", $post_id, $ruta_imagen);
            $stmt_insertar_imagen->execute();
        }
    }
}

// Verificar si se suben nuevos archivos
if (!empty($_FILES['archivos']['name'][0])) {
    // Eliminar todos los archivos existentes
    $query_eliminar_archivos = "DELETE FROM archivos_proyectos WHERE proyecto_id = ?";
    $stmt_eliminar_archivos = $conn->prepare($query_eliminar_archivos);
    $stmt_eliminar_archivos->bind_param("i", $post_id);
    $stmt_eliminar_archivos->execute();

    // Subir nuevos archivos
    foreach ($_FILES['archivos']['tmp_name'] as $key => $tmp_name) {
        $nombre_archivo = basename($_FILES['archivos']['name'][$key]);
        $ruta_archivo = "../uploads/threads/docs/docs" . $nombre_archivo; // Ruta donde se guarda el archivo
        if (move_uploaded_file($tmp_name, $ruta_archivo)) {
            $query_insertar_archivo = "INSERT INTO archivos_proyectos (proyecto_id, archivo_url) VALUES (?, ?)";
            $stmt_insertar_archivo = $conn->prepare($query_insertar_archivo);
            $stmt_insertar_archivo->bind_param("is", $post_id, $ruta_archivo);
            $stmt_insertar_archivo->execute();
        }
    }
}


    // Redirigir al perfil con un mensaje de éxito
    header("Location: ../pages/perfil.php?success=1");
    exit;
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Editar Publicación</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        /* Estilos personalizados para el efecto de crecimiento */
        input[type="file"]:hover + .input-group-prepend label,
        button:hover {
            transform: scale(1.1); /* Aumenta el tamaño al 110% */
            transition: transform 0.3s ease-in-out; /* Hace que el efecto sea suave */
        }

        input[type="file"]:focus + .input-group-prepend label,
        button:focus {
            transform: scale(1.1); /* Mantiene el tamaño al 110% al hacer foco */
            outline: none; /* Elimina el borde de enfoque predeterminado */
        }

        .container, .container-lg, .container-md, .container-sm, .container-xl, .container-xxl {
            max-width: 900px;
        }

        /* Fondo general de la página */
        body {
            background-color: #4a5757;
            color: white;
        }

        /* Estilo del formulario */
        .form-container {
            background-color: #293737;
            border-radius: 15px;
            box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
            padding: 20px;
        }

        /* Botón de "Publicar" */
        .btn-publish {
            background-color: #ee5d1c;
            border: none;
            color: white;
            padding: 12px 20px;
            border-radius: 20px;
            font-size: 1.1rem;
            font-weight: bold;
            cursor: pointer;
            width: 100%;
            transition: background-color 0.3s ease, transform 0.3s ease;
        }

        /* Botón de "Cancelar" */
        .btn-cancel {
            background-color: #555;
            border: none;
            color: white;
            padding: 12px 20px;
            border-radius: 20px;
            font-size: 1.1rem;
            font-weight: bold;
            cursor: pointer;
            width: 100%;
            transition: background-color 0.3s ease, transform 0.3s ease;
        }

        .btn-cancel:hover {
            background-color: #777;
        }

    </style>
</head>
<body>
<?php include("../includes/partials/navbar.php");?>
    <div class="container my-5 form-container">
        <h1 class="text-center mb-4" style="font-size: 2rem; font-weight: bold; color: #ee5d1c; text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.5);">Editar Publicación</h1>
        <form action="editar_publicacion.php?post_id=<?= $post_id; ?>" method="POST" enctype="multipart/form-data" class="p-4 rounded">
            <div class="mb-3">
                <label for="titulo" class="form-label" style="font-size: 1rem; color: white;">Título del Proyecto</label>
                <input type="text" class="form-control" name="titulo" value="<?= htmlspecialchars($publicacion['titulo']) ?>" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
            </div>
            <div class="mb-3">
                <label for="descripcion" class="form-label" style="font-size: 1rem; color: white;">Descripción</label>
                <textarea class="form-control" name="descripcion" rows="4" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;"><?= htmlspecialchars($publicacion['descripcion']) ?></textarea>
            </div>
            <div class="mb-3">
                <label for="categorias" class="form-label" style="font-size: 1rem; color: white;">Selecciona una Categoría</label>
                 <select name="categorias" id="categorias" class="form-select" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                    <?php
                     $query = "SELECT * FROM categorias";
                    $result = $conn->query($query);
                   while ($row = $result->fetch_assoc()) {
                    echo "<option style='color: white; background-color:#3f4b4b;' value='" . $row['id'] . "'>" . $row['nombre'] . "</option>";
                     }
                    ?>
                 </select>
            </div>
            
            <!-- Imágenes Actuales -->
            <div class="mb-3">
                <label class="form-label" style="font-size: 1rem; color: white;">Imágenes Actuales</label>
                <div class="d-flex flex-wrap">
                    <?php while ($imagen = $resultado_imagenes->fetch_assoc()) { ?>
                        <div class="m-1">
                            <img src="<?= $imagen['imagen_url'] ?>" alt="Imagen Actual" class="img-thumbnail" style="max-width: 150px; max-height: 150px;">
                        </div>
                    <?php } ?>
                </div>
            </div>

            <!-- Archivos Actuales -->
            <div class="mb-3">
                <label class="form-label" style="font-size: 1rem; color: white;">Archivos Actuales</label>
                <ul class="list-group">
                    <?php while ($archivo = $resultado_archivos->fetch_assoc()) { ?>
                        <li class="list-group-item" style="background-color: rgba(255, 255, 255, 0.1); color: white;">
                        <i class="bi bi-file-earmark"></i> <a href="<?= $archivo['archivo_url'] ?>" target="_blank">Descargar</a>
                        </li>
                    <?php } ?>
                </ul>
            </div>

            <!-- Subir Nuevas Imágenes -->
            <div class="mb-3">
                <label for="imagenes" class="form-label" style="font-size: 1rem; color: white;">Subir Imágenes</label>
                <div class="input-group">
                    <input type="file" name="imagenes[]" id="imagenes" class="custom-file-input" multiple accept="image/*" style="display: none; background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 12px 20px; font-size: 1rem; cursor: pointer; width: 100%;">
                    
                    <div class="input-group-prepend">
                        <label class="btn btn-primary" for="imagenes" style="border-radius: 10px; margin-right: 10px; background-color: #ee5d1c; border: 1px solid #ee5d1c; transition: background-color 0.3s, color 0.3s;">
                            Elegir Imágenes
                        </label>
                    </div>
                    
                    <span class="input-group-text flex-grow-1" id="file-name" style="border-radius: 10px; background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white;">
                        Sin archivos seleccionados
                    </span>
                </div>
            </div>

            <!-- Subir Nuevos Archivos -->
            <div class="mb-3">
                <label for="archivos" class="form-label" style="font-size: 1rem; color: white;">Subir Archivos Adjuntos</label>
                <div class="input-group">
                    <input type="file" name="archivos[]" id="archivos" class="custom-file-input" multiple accept=".pdf" style="display: none; background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 12px 20px; font-size: 1rem; cursor: pointer; width: 100%;">

                    <div class="input-group-prepend">
                        <label class="btn btn-primary" for="archivos" style="border-radius: 10px; margin-right: 10px; background-color: #ee5d1c; border: 1px solid #ee5d1c; transition: background-color 0.3s, color 0.3s;">
                            Elegir Archivo
                        </label>
                    </div>
                    
                    <span class="input-group-text flex-grow-1" id="file-name-archivos" style="border-radius: 10px; background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white;">
                        Sin archivos seleccionados
                    </span>
                </div>
            </div>

            <div class="mb-3">
            <button type="submit" class="btn btn-publish" style="background-color: #ee5d1c; border: none; color: white; padding: 12px 20px; border-radius: 20px; font-size: 1.1rem; font-weight: bold; cursor: pointer; width: 100%; transition: background-color 0.3s ease, transform 0.3s ease;">
                Guardar Cambios <i class="bi bi-upload"></i>
            </button>
            </div>
            <div class="mb-3">
                <a href="javascript:history.back()" class="btn btn-cancel">Cancelar</a>
            </div>
        </form>
    </div>

</body>
</html>

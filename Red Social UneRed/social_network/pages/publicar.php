<?php
// Inicia la sesión
session_start();

// Verifica si el usuario ha iniciado sesión, de lo contrario, redirige a la página de inicio de sesión
if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
    header("Location: ../../index.html");
    exit;
}

// configuración de la base de datos
require_once '../includes/config/database.php';
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Publicar Proyecto - UneRed</title>
    <!-- Enlace a Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Enlace a los iconos de Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">
    
    <style>
        /* Estilos personalizados para el efecto de crecimiento */
        /* Efecto de crecimiento al pasar el mouse por encima */
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

@media (max-width: 1100px)  {
    .container, .container-lg, .container-md, .container-sm, .container-xl, .container-xxl {
            max-width: 700px;
        }
    }    
/* Cambios responsive */
@media (max-width: 767.5px)  {
    .navbar {
        top: auto;
        bottom: 0; /* Mueve el navbar a la parte inferior */
        left: 0;
        height: 70px; /* Reduce la altura del navbar */
        width: 100%; /* Ocupa todo el ancho de la pantalla */
        flex-direction: row !important; /* Cambia a una fila horizontal */
        justify-content: space-around; /* Espaciado uniforme entre ítems */
        align-items: center;
        padding: 0;
    }

    .navbar-nav {
        width: 100%;
        padding: 0;
        margin: 0;
        list-style: none;
        display: flex;
        flex-direction: row !important;
    }
    .main-content {
        margin-left: 0 !important; /* Elimina el margen en móviles */
        margin-bottom: 80px !important; /* Espacio para la barra en la parte inferior */
    }

    .nav-item {
        margin: 0; /* Elimina márgenes extra */
    }

    .nav-link {
        font-size: 1.8rem; /* Íconos ligeramente más pequeños en móviles */
    }
}

    </style>
</head>
<body style="background-color: #4a5757; color: white;"> <!-- Fondo general de la página -->

<?php include("../includes/partials/navbar.php"); ?>
    <div class="container my-5" style="background-color: #293737; border-radius: 15px; box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3); padding: 20px;">
        <h1 class="text-center mb-4" style="font-size: 2rem; font-weight: bold; color: #ee5d1c; text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.5);">Publicar Proyecto</h1>
        <form action="../controllers/procesar_publicacion.php" method="POST" enctype="multipart/form-data" class="p-4 rounded">
            <div class="mb-3">
                <label for="titulo" class="form-label" style="font-size: 1rem; color: white;">Título del Proyecto</label>
                <input type="text" name="titulo" id="titulo" class="form-control" placeholder="Título del proyecto..." required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
            </div>
            <div class="mb-3">
                <label for="descripcion" class="form-label" style="font-size: 1rem; color: white;">Descripción</label>
                <textarea name="descripcion" id="descripcion" class="form-control" placeholder="Describe tu proyecto..." rows="4" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;"></textarea>
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


            <button type="submit" class="btn btn-publish" style="background-color: #ee5d1c; border: none; color: white; padding: 12px 20px; border-radius: 20px; font-size: 1.1rem; font-weight: bold; cursor: pointer; width: 100%; transition: background-color 0.3s ease, transform 0.3s ease;">
                Publicar <i class="bi bi-upload"></i>
            </button>
        </form>
    </div>

    <!-- Enlace a Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js" integrity="sha384-pzjw8f+ua7Kw1TIq0J5Lg7p1gD6EO0bX8hFwkpDbZXlDhGspgMjtcYyT1fYzxI8v" crossorigin="anonymous"></script>
    
    <script>
        // Script para mostrar los nombres de los archivos cuando se seleccionan
        document.getElementById('imagenes').addEventListener('change', function() {
            const fileNameElement = document.getElementById('file-name');
            const files = this.files;
            if (files.length > 0) {
                const fileNames = Array.from(files).map(file => file.name).join(', ');
                fileNameElement.textContent = fileNames; // Muestra los nombres de los archivos seleccionados
            } else {
                fileNameElement.textContent = 'Sin archivos seleccionados';
            }
        });

        document.getElementById('archivos').addEventListener('change', function() {
            const fileNameElement = document.getElementById('file-name-archivos');
            const files = this.files;
            if (files.length > 0) {
                const fileNames = Array.from(files).map(file => file.name).join(', ');
                fileNameElement.textContent = fileNames; // Muestra los nombres de los archivos seleccionados
            } else {
                fileNameElement.textContent = 'Sin archivos seleccionados';
            }
        });
    </script>

</body>
</html>

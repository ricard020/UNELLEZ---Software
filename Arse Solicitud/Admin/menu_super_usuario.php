<?php
// Importar la autenticación
require_once '../php/Admin_seguridad.php';
verificarRol('SA');
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Super Usuario - Gestión de Jefe de Subprograma</title>
    
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<style>
        
        body {
            margin: 0px;
            padding: 0px;
            font-family: Arial, sans-serif;
            background-image: url('../Imagen/2.jpg');
            background-size: cover;
            background-position: center;
            background-attachment: fixed;
            min-height: 100vh;
        }
    .custom-div {
        border: 4px solid black;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    }
</style>
<body class="bg-light">
    
    <div class="container">
        <header class="text-center justify-content-between align-items-center">
            <div class="border-top border-5 border-primary mb-1"></div>
            <h1 class="display-5 fw-semibold">Super Usuario</h1>
            <p class="text-black">Menú de gestión del sistema</p>
            <div class="border-bottom border-5 border-primary mt-1"></div>
</header>
        
        
       
<br>
        <main>
            <div class="container py-4">
                <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                
                    <div class="col">
                        <div class="p-3 bg-white rounded shadow custom-div">
                            <svg class="card-icon" viewBox="0 0 24 24">
                                <path fill="currentColor" d="M12,5.5A3.5,3.5 0 0,1 15.5,9A3.5,3.5 0 0,1 12,12.5A3.5,3.5 0 0,1 8.5,9A3.5,3.5 0 0,1 12,5.5M5,8C5.56,8 6.08,8.15 6.53,8.42C6.38,9.85 6.8,11.27 7.66,12.38C7.16,13.34 6.16,14 5,14A3,3 0 0,1 2,11A3,3 0 0,1 5,8M19,8A3,3 0 0,1 22,11A3,3 0 0,1 19,14C17.84,14 16.84,13.34 16.34,12.38C17.2,11.27 17.62,9.85 17.47,8.42C17.92,8.15 18.44,8 19,8M5.5,18.25C5.5,16.18 8.41,14.5 12,14.5C15.59,14.5 18.5,16.18 18.5,18.25V20H5.5V18.25M0,20V18.5C0,17.11 1.89,15.94 4.45,15.6C3.86,16.28 3.5,17.22 3.5,18.25V20H0M24,20H20.5V18.25C20.5,17.22 20.14,16.28 19.55,15.6C22.11,15.94 24,17.11 24,18.5V20Z" />
                              </svg>
                            <button class="btn btn-primary w-100 py-3" onclick="navigateTo('super_usuario/creacion_jefe_subprograma.php')">
                                Crear Cuenta de Jefe de Subprograma
                            </button>
                        </div>
                    </div>
                    
            
                    
                    <div class="col">
                        <div class="p-3 bg-white rounded shadow custom-div">
                            <svg class="card-icon" viewBox="0 0 24 24">
                                <path fill="currentColor" d="M12,3L1,9L12,15L21,10.09V17H23V9M5,13.18V17.18L12,21L19,17.18V13.18L12,17L5,13.18Z" />
                              </svg>
                            <button class="btn btn-primary w-100 py-3" onclick="navigateTo('super_usuario/gestionar_estudiantes.php')">
                                Gestionar Cuentas de Estudiantes
                            </button>
                        </div>
                    </div>
            
                  
                    <div class="col">
                        <div class="p-3 bg-white rounded shadow custom-div">
                            <svg class="card-icon" viewBox="0 0 24 24" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                <path d="M12 12C14.2091 12 16 10.2091 16 8C16 5.79086 14.2091 4 12 4C9.79086 4 8 5.79086 8 8C8 10.2091 9.79086 12 12 12ZM4 20C4 17.7909 9.58172 16 12 16C14.4183 16 20 17.7909 20 20V21H4V20ZM18.15 7.85L19.71 6.29L18.29 4.87L16.73 6.43L18.15 7.85ZM14 10H16V8H14V10ZM17.85 11.15L16.43 12.57L18.29 14.43L19.71 13.01L17.85 11.15Z" />
                              </svg>
                            <button class="btn btn-primary w-100 py-3" onclick="navigateTo('super_usuario/gestionar_jefe_subprograma.php')">
                                Gestionar Cuentas de Jefes de Subprograma
                            </button>
                        </div>
                    </div>
                </div>
            </div>
            
        </main>


        
        
    </div>

    <footer class="d-flex justify-content-center">
    <a href="../PHP/logout.php" class="btn btn-danger">Salir</a>
</footer>

    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        
        function navigateTo(page) {
            window.location.href = page;
        }
    </script>
</body>
</html>

<?php include("../includes/config/verificacion.php"); ?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="Página Principal">
    <title>Reacciones</title>
    <!-- Bootstrap CSS -->
   <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
   <link rel="stylesheet" href="../pages/home.php">
</head>
<style>

.modal {
  color: white !important;
}

.descripciondeproyecto {
  color: white !important;
}
.modal-content {

  background-color: #293737 !important;
  border-radius: 15px !important;
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3) !important;
  color: white !important;
  padding: 20px !important;
  border: none !important;
}
.h1, .h2, .h3, .h4, .h5, .h6, h1, h2, h3, h4, h5, h6 {
    margin-top: 0;
    margin-bottom: .5rem;
    font-weight: 500;
    line-height: 1.2;
    color: var(--bs-heading-color) !important; 
}
.modal-header {
  border-bottom: 1px solid rgba(255, 255, 255, 0.3) !important;
}

.modal-title {
  font-size: 1.5rem !important;
  font-weight: bold !important;
  color: #ee5d1c !important;
  text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.5) !important;
}

.btn-close {
  color: white !important;
  opacity: 0.8 !important;
}

textarea.form-control {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border: 1px solid rgba(255, 255, 255, 0.3) !important;
  color: white !important;
  border-radius: 10px !important;
  padding: 10px !important;
}

.btn-primary {
  background-color: #ee5d1c !important;
  border: none !important;
  color: white !important;
  padding: 10px 20px !important;
  border-radius: 20px !important;
  font-size: 1rem !important;
  font-weight: bold !important;
  cursor: pointer !important;
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

.justify-content-center > * {
    flex-grow: 1;
    width: 100%;
}

</style>
<body>
    
    

    <?php include("../models/like_post.php"); ?>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>


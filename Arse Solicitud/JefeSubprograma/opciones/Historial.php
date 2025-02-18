<?php 
require_once '../Funciones/Historial_funcion.php';
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Historial de Solicitudes - Jefe de Subprograma</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../../css/header.css" rel="stylesheet">
    <style>
        
        .bg-custom-orange {
            background-color: #FF6B00 !important;
            color: white !important; /* Asegura que el texto sea legible sobre el fondo naranja */
        }

        .table-bordered th, .table-bordered td {
            text-align: center;
        }

        .approved {
            background-color: #28a745;
            color: white;
        }

        .rejected {
            background-color: #dc3545;
            color: white;
        }    
        
        .btn-aproved {
          background-color: green;
          Color: White;
        }

        html {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 100%;
        }

        body {    
            font-family: Arial, sans-serif;
            background-image: url('../imagen/2.jpg');
            background-size: cover;
            background-position: center;
            color: #333;
            line-height: 1.6;
        }
    
        header h1 {
            color: rgb(255, 107, 0);
        }

        header img {
	    border-radius: 50%;
       }

       footer {
        background-color: #FF6B00;
        width: 100%;
        position: fixed;
        bottom: 0;
        padding: 10px 0;
        z-index: 1000;
        left: 0;
        }

       footer .container {
        display: flex;
        justify-content: space-between;
        align-items: center;
        width: 100%;
        padding: 0 20px;
       }

       footer a {
        color: white;
        text-decoration: none;
        padding: 5px 10px;
      }

      footer a:hover {
       text-decoration: underline;
     }

 </style>
</head>

<body class="bg-image" style="background-image: url('../../imagen/2.jpg'); background-size: cover; background-position: center;  color: #333; line-height: 1.6; background-color: #f8f9fa;">

    <!-- Header -->
    <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
        <a href="../perfil_jefesubprograma.php">
        <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
        </a>
        <h1 style="color: #FF6B00;">Historial de Solicitud</h1>
        <div class="d-flex align-items-center">
        <div class="dropdown">
            <a href="#" class="text-white" data-bs-toggle="dropdown">
                <svg width="24" height="24" fill="none" stroke="#FF6B00" stroke-width="2" viewBox="0 0 24 24">
                    <circle cx="12" cy="7" r="4"></circle>
                    <path d="M4 21v-2a4 4 0 0 1 4-4h8a4 4 0 0 1 4 4v2"></path>
                </svg>
            </a>
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="./visualizar_perfil_jefesubprograma.php">Datos del usuario</a></li>
                <li><a class="dropdown-item" href="../../PHP/logout.php">Cerrar Sesión</a></li>
            </ul>
        </div>
        </div>
    </header>

    <br>
    <!-- Botones de navegación -->
    <div class="container mt-5 pt-5 text-center">
        <button class="btn btn-aproved m-2" onclick="showTable('aceptada')">Solicitudes Aceptadas</button>
        <button class="btn btn-danger m-2" onclick="showTable('rechazadas')">Solicitudes Rechazadas</button>
        <button class="btn btn-warning m-2" onclick="showTable('diferidas')">Solicitudes Diferidas</button>
        <button class="btn btn-info m-2" onclick="showTable('elevadas')">Solicitudes Elevadas</button>
        <button class="btn btn-secondary m-2" onclick="showTable('historial')">Historial de Solicitudes</button>
    </div>

    <br>
    <div class="d-flex justify-content-center mb-4">
    <input type="text"  style="width: 300px;"  id="searchInput" class="form-control" placeholder="Buscar por número de caso" onkeyup="filterTable()">
</div>
    <!-- Tablas de solicitudes -->
    <div class="container mt-3">
        <!-- Tabla de solicitudes aceptadas -->
        <div id="aceptada" class="table-container">
            <h2 class="text-center">Solicitudes Aceptadas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>Estado</th>
                        <th>Nombre del Usuario</th>
                        <th>Tipo de Solicitud</th>
                        <th>Fecha de Solicitud</th>
                        <th>Fecha de Resolución</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>Nota</th>
                        <th>Archivo PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($solicitudes as $solicitud): ?>
                        <?php if ($solicitud['estado'] === 'Aceptada'): ?>
                            <tr class="approved">
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['nombre_usuario']); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipos_solicitud']); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_solicitud']))); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_resolucion']))); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                        <?php endif; ?>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes rechazadas -->
        <div id="rechazadas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Rechazadas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>Estado</th>
                        <th>Nombre del Usuario</th>
                        <th>Tipo de Solicitud</th>
                        <th>Fecha de Solicitud</th>
                        <th>Fecha de Resolución</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>Nota</th>
                        <th>Archivo PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($solicitudes as $solicitud): ?>
                        <?php if ($solicitud['estado'] === 'Rechazada'): ?>
                            <tr class="rejected">
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['nombre_usuario']); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipos_solicitud']); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_solicitud']))); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_resolucion']))); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                        <?php endif; ?>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes diferidas -->
        <div id="diferidas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Diferidas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>Estado</th>
                        <th>Nombre del Usuario</th>
                        <th>Tipo de Solicitud</th>
                        <th>Fecha de Solicitud</th>
                        <th>Fecha de Resolución</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>Nota</th>
                        <th>Archivo PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($solicitudes as $solicitud): ?>
                        <?php if ($solicitud['estado'] === 'Diferida'): ?>
                            <tr class="deferred">
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['nombre_usuario']); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipos_solicitud']); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_solicitud']))); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_resolucion']))); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                        <?php endif; ?>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes elevadas -->
        <div id="elevadas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Elevadas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>Estado</th>
                        <th>Nombre del Usuario</th>
                        <th>Tipo de Solicitud</th>
                        <th>Fecha de Solicitud</th>
                        <th>Fecha de Resolución</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>Nota</th>
                        <th>Archivo PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($solicitudes as $solicitud): ?>
                        <?php if ($solicitud['estado'] === 'Elevada'): ?>
                            <tr class="elevated">
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['nombre_usuario']); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipos_solicitud']); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_solicitud']))); ?></td>
                                <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_resolucion']))); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                        <?php endif; ?>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de historial de solicitudes -->
        <div id="historial" class="table-container" style="display: none;">
            <h2 class="text-center">Historial de Solicitudes</h2>
            <table class="table table-striped table-hover table-bordered" id="historialTable">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>Estado</th>
                        <th>Nombre del Usuario</th>
                        <th>Tipo de Solicitud</th>
                        <th>Fecha de Solicitud</th>
                        <th>Fecha de Resolución</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>Nota</th>
                        <th>Archivo PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php foreach ($solicitudes as $solicitud): ?>
                        <tr>
                            <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                            <td><?= htmlspecialchars($solicitud['nombre_usuario']); ?></td>
                            <td><?= htmlspecialchars($solicitud['tipos_solicitud']); ?></td>
                            <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_solicitud']))); ?></td>
                            <td><?= htmlspecialchars(date('d/m/Y h:i A', strtotime($solicitud['fecha_resolucion']))); ?></td>
                            <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                            <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                            <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                            <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>
        <div id="pagination" class="d-flex justify-content-center mt-3"></div>
    </div>

    <!-- Modal para ver notas -->
    <div style="margin-top: 15%;" class="modal fade" id="notesModal" tabindex="-1" aria-labelledby="notesModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="notesModalLabel">Nota de la Solicitud</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p id="notesContent"></p>
                </div>
                <div class="modal-footer">
                    
                </div>
            </div>
        </div>
    </div>

    
    
    <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00; height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>

    <script>
function filterTable() {
    // Obtener el valor del input de búsqueda
    var input = document.getElementById("searchInput");
    var filter = input.value.toUpperCase();
    
    // Obtener la tabla actualmente visible
    var visibleTable = document.querySelector(".table-container:not([style*='display: none']) table");
    
    if (!visibleTable) {
        alert("No hay tablas visibles para filtrar");
        return;
    }
    
    // Variable para rastrear si se encontró alguna coincidencia
    var found = false;
    
    // Iterar sobre cada fila de la tabla visible
    var tr = visibleTable.getElementsByTagName("tr");
    for (var i = 1; i < tr.length; i++) {
        var td = tr[i].getElementsByTagName("td")[5]; // Columna del número de caso
        
        if (td) {
            if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
                found = true;
            } else {
                tr[i].style.display = "none";
            }
        }
    }
    
    // Mostrar un mensaje si no se encontraron coincidencias
    if (!found && filter !== "") {
        alert("No se encontraron coincidencias");
    }

    // Restablecer la paginación si el campo de búsqueda está vacío
    if (filter === "") {
        paginateTable('historialTable');
    }
}

function showTable(tableId) {
    const tables = document.querySelectorAll('.table-container');
    tables.forEach(table => {
        table.style.display = 'none';
    });
    document.getElementById(tableId).style.display = 'block';
}

function openNotesModal(nota) {
    document.getElementById('notesContent').innerText = nota;
    var modal = new bootstrap.Modal(document.getElementById('notesModal'));
    modal.show();
}

showTable('aceptada');

const rowsPerPage = 10;
let currentPage = 1;

function paginateTable(tableId) {
    const table = document.getElementById(tableId);
    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    const totalRows = rows.length;
    const totalPages = Math.ceil(totalRows / rowsPerPage);

    for (let i = 0; i < totalRows; i++) {
        rows[i].style.display = 'none';
    }

    for (let i = (currentPage - 1) * rowsPerPage; i < currentPage * rowsPerPage && i < totalRows; i++) {
        rows[i].style.display = '';
    }

    document.getElementById('pagination').innerHTML = createPaginationControls(totalPages);
}

function createPaginationControls(totalPages) {
    let controls = '';
    for (let i = 1; i <= totalPages; i++) {
        controls += `<button class="btn btn-secondary m-1" onclick="goToPage(${i})">${i}</button>`;
    }
    return controls;
}

function goToPage(page) {
    currentPage = page;
    paginateTable('historialTable');
}

document.addEventListener('DOMContentLoaded', function() {
    paginateTable('historialTable');
});
</script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
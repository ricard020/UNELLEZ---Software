<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Solicitudes - Estudiante</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../../css/Lista_solicitud.css" rel="stylesheet">
</head>
<body>
    
<!-- Header -->
    <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <a href="../perfil_estudiante.php">
    <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
    </a>
    <h1 style="color: #FF6B00;">Mis solicitudes</h1>
    <div class="d-flex align-items-center">
        <div class="dropdown">
            <a href="#" class="text-white" data-bs-toggle="dropdown">
                <svg width="24" height="24" fill="none" stroke="#FF6B00" stroke-width="2" viewBox="0 0 24 24">
                    <circle cx="12" cy="7" r="4"></circle>
                    <path d="M4 21v-2a4 4 0 0 1 4-4h8a4 4 0 0 1 4 4v2"></path>
                </svg>
            </a>
                    <ul class="dropdown-menu dropdown-menu-end">
                        <li><a class="dropdown-item" href="./visualizar_perfil.php">Datos del usuario</a></li>
                        <li><a class="dropdown-item" href="./Lista_solicitud.php">Solicitudes</a></li>
                        <li><a class="dropdown-item" href="../../PHP/logout.php">Cerrar Sesión</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </header>

    <br>
    <br>    
<br>
<br>
        <!-- Botones de navegación -->
        <div class="d-flex justify-content-center mb-4">
            <button class="btn btn-aproved mx-2" onclick="showTable('aceptada')">Solicitudes Aceptadas</button>
            <button class="btn btn-danger mx-2" onclick="showTable('rechazadas')">Solicitudes Rechazadas</button>
            <button class="btn btn-warning mx-2" onclick="showTable('diferidas')">Solicitudes Diferidas</button>
            <button class="btn btn-info mx-2" onclick="showTable('elevadas')">Solicitudes Elevadas</button>
            <button class="btn btn-secondary mx-2" onclick="showTable('historial')">Historial de Solicitudes</button>
        </div>

        <!-- Campo de búsqueda -->
        <div class="d-flex justify-content-center mb-4">
            <input type="text"  style="width: 300px;"  id="searchInput" class="form-control" placeholder="Buscar por número de caso" onkeyup="filterTable()">
        </div>

        <!-- Tabla de solicitudes aceptadas -->
        <div id="aceptada" class="table-container">
            <h2 class="text-center">Solicitudes Aceptadas</h2>
            <table class="table table-striped table-hover table-bordered" id="solicitudesTable">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>ID</th>
                        <th>Estado</th>
                        <th>Programa</th>
                        <th>Subprograma</th>
                        <th>Fecha</th>
                        <th>Tipo de Solicitud</th>
                        <th>Nota</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php include '../Funciones/LS_funcion.php'; ?>
                    <?php 
                    $haySolicitudesAceptada = false;
                    foreach ($solicitudes as $solicitud): 
                        if ($solicitud['estado'] === 'Aceptada'): 
                            $haySolicitudesAceptada = true;
                    ?>
                            <tr class="approved">
                                <td><?= htmlspecialchars($solicitud['solicitud_id']); ?></td>
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['programa']); ?></td>
                                <td><?= htmlspecialchars($solicitud['subprograma']); ?></td>
                                <td><?= date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                    <?php endif; endforeach; ?>
                    <?php if (!$haySolicitudesAceptada): ?>
                        <tr>
                            <td colspan="10" class="text-center">No se encontraron solicitudes aceptadas.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes rechazadas -->
        <div id="rechazadas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Rechazadas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>ID</th>
                        <th>Estado</th>
                        <th>Programa</th>
                        <th>Subprograma</th>
                        <th>Fecha</th>
                        <th>Tipo de Solicitud</th>
                        <th>Nota</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php 
                    $haySolicitudesRechazadas = false;
                    foreach ($solicitudes as $solicitud): 
                        if ($solicitud['estado'] === 'Rechazada'): 
                            $haySolicitudesRechazadas = true;
                    ?>
                            <tr class="rejected">
                                <td><?= htmlspecialchars($solicitud['solicitud_id']); ?></td>
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['programa']); ?></td>
                                <td><?= htmlspecialchars($solicitud['subprograma']); ?></td>
                                <td><?= date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                    <?php endif; endforeach; ?>
                    <?php if (!$haySolicitudesRechazadas): ?>
                        <tr>
                            <td colspan="10" class="text-center">No se encontraron solicitudes rechazadas.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes diferidas -->
        <div id="diferidas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Diferidas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>ID</th>
                        <th>Estado</th>
                        <th>Programa</th>
                        <th>Subprograma</th>
                        <th>Fecha</th>
                        <th>Tipo de Solicitud</th>
                        <th>Nota</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php 
                    $haySolicitudesDiferidas = false;
                    foreach ($solicitudes as $solicitud): 
                        if ($solicitud['estado'] === 'Diferida'): 
                            $haySolicitudesDiferidas = true;
                    ?>
                            <tr class="deferred">
                                <td><?= htmlspecialchars($solicitud['solicitud_id']); ?></td>
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['programa']); ?></td>
                                <td><?= htmlspecialchars($solicitud['subprograma']); ?></td>
                                <td><?= date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                    <?php endif; endforeach; ?>
                    <?php if (!$haySolicitudesDiferidas): ?>
                        <tr>
                            <td colspan="10" class="text-center">No se encontraron solicitudes diferidas.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de solicitudes elevadas -->
        <div id="elevadas" class="table-container" style="display: none;">
            <h2 class="text-center">Solicitudes Elevadas</h2>
            <table class="table table-striped table-hover table-bordered">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>ID</th>
                        <th>Estado</th>
                        <th>Programa</th>
                        <th>Subprograma</th>
                        <th>Fecha</th>
                        <th>Tipo de Solicitud</th>
                        <th>Nota</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php 
                    $haySolicitudesElevadas = false;
                    foreach ($solicitudes as $solicitud): 
                        if ($solicitud['estado'] === 'Elevada'): 
                            $haySolicitudesElevadas = true;
                    ?>
                            <tr class="elevated">
                                <td><?= htmlspecialchars($solicitud['solicitud_id']); ?></td>
                                <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                                <td><?= htmlspecialchars($solicitud['programa']); ?></td>
                                <td><?= htmlspecialchars($solicitud['subprograma']); ?></td>
                                <td><?= date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                                <td><?= htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                                <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                                <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                                <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                            </tr>
                    <?php endif; endforeach; ?>
                    <?php if (!$haySolicitudesElevadas): ?>
                        <tr>
                            <td colspan="10" class="text-center">No se encontraron solicitudes elevadas.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <!-- Tabla de historial de solicitudes -->
        <div id="historial" class="table-container" style="display: none;">
            <h2 class="text-center">Historial de Solicitudes</h2>
            <table class="table table-striped table-hover table-bordered" id="historialTable">
                <thead class="bg-custom-orange">
                    <tr>
                        <th>ID</th>
                        <th>Estado</th>
                        <th>Programa</th>
                        <th>Subprograma</th>
                        <th>Fecha</th>
                        <th>Tipo de Solicitud</th>
                        <th>Nota</th>
                        <th>Caso</th>
                        <th>Resolución</th>
                        <th>PDF</th>
                    </tr>
                </thead>
                <tbody>
                    <?php 
                    foreach ($solicitudes as $solicitud): 
                    ?>
                        <tr>
                            <td><?= htmlspecialchars($solicitud['solicitud_id']); ?></td>
                            <td><?= htmlspecialchars($solicitud['estado']); ?></td>
                            <td><?= htmlspecialchars($solicitud['programa']); ?></td>
                            <td><?= htmlspecialchars($solicitud['subprograma']); ?></td>
                            <td><?= date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                            <td><?= htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                            <td><button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?= htmlspecialchars($solicitud['nota']); ?>')">Ver nota</button></td>
                            <td><?= htmlspecialchars($solicitud['numero_caso']); ?></td>
                            <td><?= htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                            <td><a href="../Funciones/ver_pdf.php?id=<?= htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a></td>
                        </tr>
                    <?php endforeach; ?>
                </tbody>
            </table>
        </div>
    </div>

    <!-- Modal para ver notas -->
    <div class="modal fade" id="notesModal" tabindex="-1" aria-labelledby="notesModalLabel" aria-hidden="true">
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <div id="pagination" class="d-flex justify-content-center mt-3"></div>
    
    <footer style="height: 50px;" class="custom-footer py-3 text-center position-fixed bottom-0 w-100">
    <div class="footer-container d-flex justify-content-center w-100">
    <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
</footer>

    <script>
        function showTable(tableId) {
            const tables = document.querySelectorAll('.table-container');
            tables.forEach(table => {
                table.style.display = 'none';
            });
            document.getElementById(tableId).style.display = 'block';
            paginateTable(tableId);
        }

        function openNotesModal(nota) {
            document.getElementById('notesContent').innerText = nota;
            var modal = new bootstrap.Modal(document.getElementById('notesModal'));
            modal.show();
        }

        function filterTable() {
            const input = document.getElementById('searchInput');
            const filter = input.value.toUpperCase();
            const visibleTable = document.querySelector('.table-container:not([style*="display: none"]) table');
            
            if (!visibleTable) {
                alert("No hay tablas visibles para filtrar");
                return;
            }

            const tr = visibleTable.getElementsByTagName('tr');
            let found = false;
            for (let i = 1; i < tr.length; i++) {
                const td = tr[i].getElementsByTagName('td')[7];
                if (td) {
                    const txtValue = td.textContent || td.innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = '';
                        found = true;
                    } else {
                        tr[i].style.display = 'none';
                    }
                }
            }

            if (!found && filter !== "") {
                alert("No se encontraron coincidencias");
            }

            if (filter === "") {
                paginateTable(visibleTable.parentElement.id);
            }
        }

        const rowsPerPage = 10;
        let currentPage = 1;

        function paginateTable(tableId) {
            const table = document.getElementById(tableId).getElementsByTagName('table')[0];
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
            const visibleTable = document.querySelector('.table-container:not([style*="display: none"])').id;
            paginateTable(visibleTable);
        }

        document.addEventListener('DOMContentLoaded', function() {
            showTable('aceptada');
        });
    </script>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
<?php
require_once '../Funciones/solicitudes_backend.php';
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Enviar Solicitud</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../CSS/estilo_solicitudes.css">
    
    
</head>
<header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <a href="../perfil_estudiante.php">
    <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
    </a>
    <h1 class="mb-0" style="color: #FF6B00;">Solicitud</h1>
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
</header>

<br>
<body>
    <div class="container form-container">
        <h3>Subprograma: <?= htmlspecialchars($subprogramaData['nombre_subprograma']); ?> | Jefe Actual: <?= htmlspecialchars($jefeNombreCompleto); ?></h3>
        <?php if (isset($_GET['error'])): ?>
            <div class="alert alert-danger"><?= htmlspecialchars($_GET['error']); ?></div>
        <?php endif; ?>
        <form id="solicitudForm" method="POST" action="../funciones/generar_pdf.php" enctype="multipart/form-data">
            <input type="hidden" name="subprograma_id" value="<?= htmlspecialchars($subprograma_id); ?>">
            <input type="hidden" name="tipo_solicitud" id="tipo_solicitud">
            <div class="solicitudes-container">
                <div class="solicitudes-list">
                    <h2>Opciones de Solicitud</h2>
                    <h3 for="semestre-select" style="display: inline-block;">Semestre:</h3>
                    <select id="semestre-select" name="semestre" style="display: inline-block;">
                        <option value="">Seleccione un semestre</option>
                            <?php for ($i = 1; $i <= $numeroSemestres; $i++): ?>
                        <option value="<?= $i; ?>"><?= convertirARomano($i); ?></option>
                            <?php endfor; ?>
                    </select>
                    <?= generarOpcionesSolicitud($tiposSolicitud); ?>
                </div>
                <div class="solicitudes-details">
                    <div id="selected-solicitudes">
                        <h3>Solicitudes Seleccionadas</h3>
                    </div>
                    <div id="general-description-section">
                        <label for="general-description">Descripción General:</label>
                        <textarea id="general-description" name="descripcion_general" placeholder="Agrega una descripción general" maxlength="485" onkeydown="return !(event.keyCode == 13);" oninput="removeNewLines(this)"></textarea>
                        <div id="char-count">0 / 485</div>
                    </div>
                </div>
            </div>
            <div class="action-buttons">
    <button type="submit" class="btn btn-red" id="vistaPreviaButton" disabled><i class="fa fa-eye"></i> Vista Previa</button>
    
</div>
            
        </form>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>

    <?= generarCamposDinamicos($camposPorSolicitud, $compatibilidad, $subprogramaData['nombre_subprograma']); ?>
    
    <script>
document.addEventListener('DOMContentLoaded', function() {
    const checkboxes = document.querySelectorAll('.solicitud-option');
    const selectedSolicitudesContainer = document.getElementById('selected-solicitudes');
    const vistaPreviaButton = document.getElementById('vistaPreviaButton');
    const descripcionGeneral = document.getElementById('general-description');
    const charCount = document.getElementById('char-count');
    const equivalenciaSection = document.getElementById('equivalencia-section');
    const semestreSelect = document.getElementById('semestre-select');

    const compatibilidad = <?= json_encode($compatibilidad); ?>;
    const solicitudData = {};
    const maxSolicitudes = 3;

    function actualizarCompatibilidad() {
        const seleccionadas = Array.from(checkboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.getAttribute('data-target'));

        checkboxes.forEach(checkbox => {
            const opcion = checkbox.getAttribute('data-target');
            const esCompatible = seleccionadas.every(seleccionada => 
                seleccionada === opcion || compatibilidad[seleccionada].includes(opcion)
            );

            checkbox.disabled = !esCompatible && !checkbox.checked;
        });
    }

    function actualizarCampos() {
        selectedSolicitudesContainer.innerHTML = ''; // Limpiar contenido previo

        const selectedOptions = Array.from(checkboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.getAttribute('data-target'));

        selectedOptions.forEach(option => {
            if (camposPorSolicitud[option]) {
                camposPorSolicitud[option].forEach(campo => {
                    const fieldset = document.createElement('fieldset');
                    const legend = document.createElement('legend');
                    legend.textContent = campo.label;
                    fieldset.appendChild(legend);

                    if (campo.id === 'carrera-curso-cambio' || campo.id === 'carrera-curso-equivalencia' || campo.id === 'carreras-reingreso' || campo.id === 'carrera-retiro' || (campo.id === 'extension-actual' && !campo.editable)) {
                        const input = document.createElement('input');
                        input.type = 'text';
                        input.name = `detalles[${campo.id}]`;
                        input.value = solicitudData[option]?.[campo.id] || subprogramaActual;
                        input.disabled = true;
                        input.addEventListener('input', function() {
                            solicitudData[option][campo.id] = input.value;
                        });
                        fieldset.appendChild(input);
                    } else if (campo.id.startsWith('subproyectos-inscribir') || campo.id === 'extension-nueva' || campo.id === 'subproyecto' || campo.id === 'subproyecto-prelar') {
                        const select = document.createElement('select');
                        select.name = `detalles[${campo.id}]`;
                        select.classList.add('form-select');
                        campo.options.forEach(opt => {
                            const optionElement = document.createElement('option');
                            optionElement.value = opt;
                            optionElement.textContent = opt;
                            if (solicitudData[option]?.[campo.id] === opt) {
                                optionElement.selected = true;
                            }
                            select.appendChild(optionElement);
                        });
                        select.addEventListener('change', function() {
                            solicitudData[option][campo.id] = select.value;
                        });
                        fieldset.appendChild(select);
                    } else {
                        const select = document.createElement('select');
                        select.name = `detalles[${campo.id}]`;
                        select.classList.add('form-select');
                        campo.options.forEach(opt => {
                            const optionElement = document.createElement('option');
                            optionElement.value = opt;
                            optionElement.textContent = opt;
                            if (solicitudData[option]?.[campo.id] === opt) {
                                optionElement.selected = true;
                            }
                            select.appendChild(optionElement);
                        });
                        select.addEventListener('change', function() {
                            solicitudData[option][campo.id] = select.value;
                        });
                        fieldset.appendChild(select);
                    }

                    selectedSolicitudesContainer.appendChild(fieldset);
                });
            }
        });
    }

    function actualizarEstadoCheckboxes() {
        const seleccionadas = Array.from(checkboxes).filter(checkbox => checkbox.checked);
        const anyChecked = seleccionadas.length > 0;
        vistaPreviaButton.disabled = !anyChecked;

        if (seleccionadas.length >= maxSolicitudes) {
            checkboxes.forEach(checkbox => {
                if (!checkbox.checked) {
                    checkbox.disabled = true;
                }
            });
        } else {
            checkboxes.forEach(checkbox => {
                if (!checkbox.checked) {
                    checkbox.disabled = false;
                }
            });
            actualizarCompatibilidad();
        }

        // Mostrar u ocultar la sección de equivalencia
        const equivalenciaChecked = seleccionadas.some(checkbox => checkbox.getAttribute('data-target') === 'Equivalencia');
        equivalenciaSection.style.display = equivalenciaChecked ? 'block' : 'none';
    }

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            const option = checkbox.getAttribute('data-target');
            if (checkbox.checked) {
                solicitudData[option] = solicitudData[option] || {};
            } else {
                delete solicitudData[option];
            }
            actualizarCampos();
            actualizarEstadoCheckboxes();
        });
    });

    descripcionGeneral.addEventListener('input', function() {
        const currentLength = descripcionGeneral.value.length;
        charCount.textContent = `${currentLength} / 485`;

        if (currentLength <= 300) {
            charCount.className = 'green';
        } else if (currentLength <= 450) {
            charCount.className = 'yellow';
        } else {
            charCount.className = 'red';
        }

        if (currentLength > 485) {
            descripcionGeneral.value = descripcionGeneral.value.substring(0, 485);
            alert('La descripción general no puede exceder los 485 caracteres.');
        }
    });

    // Validación antes de proceder a la vista previa
    document.getElementById('solicitudForm').addEventListener('submit', function(event) {
        const inscripcionesTemporalesChecked = Array.from(checkboxes)
            .some(checkbox => checkbox.checked && checkbox.getAttribute('data-target') === 'Inscripciones temporales');
        if (inscripcionesTemporalesChecked) {
            const selections = Array.from(document.querySelectorAll('select[name^="detalles[subproyectos-inscribir"]'));
            const validSelections = selections.filter(select => select.value !== "Seleccione una opción");
            if (validSelections.length === 0) {
                event.preventDefault();
                alert('Debe seleccionar al menos una opción válida para los subproyectos.');
                return;
            }
        }

// Validación para Levante Prelación
const levantePrelacionChecked = Array.from(checkboxes)
    .some(checkbox => checkbox.checked && checkbox.getAttribute('data-target') === 'Levante Prelación');
if (levantePrelacionChecked) {
    const subproyecto = document.querySelector('select[name="detalles[subproyecto-cursando]"]').value;
    const subproyectoPrelar = document.querySelector('select[name="detalles[subproyecto-a-prelar]"]').value;
    if (subproyecto === "Seleccione una opción" || subproyectoPrelar === "Seleccione una opción") {
        event.preventDefault();
        alert('Debe seleccionar ambas opciones para Levante Prelación.');
        return;
    }
    if (subproyecto === subproyectoPrelar) {
        event.preventDefault();
        alert('No puede seleccionar el mismo subproyecto para Levante Prelación.');
        return;
    }
}

        // Recopilar las solicitudes seleccionadas y sus detalles
        const tipoSolicitud = Array.from(checkboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.getAttribute('data-target'))
            .join(',');

        if (tipoSolicitud.split(',').length > 3) {
            event.preventDefault();
            alert('No puede seleccionar más de 3 tipos de solicitud.');
            return;
        }

        // Validar compatibilidad de las solicitudes seleccionadas
        const seleccionadas = tipoSolicitud.split(',');
        for (let i = 0; i < seleccionadas.length; i++) {
            for (let j = i + 1; j < seleccionadas.length; j++) {
                if (!compatibilidad[seleccionadas[i]].includes(seleccionadas[j])) {
                    event.preventDefault();
                    alert('Las opciones elegidas son inválidas.');
                    return;
                }
            }
        }

        // Verificar si el subprograma para "Cambio de carrera" ya está inscrito
        const cambioCarreraSubprograma = solicitudData['Cambio de carrera']?.['carrera-equivalencia-cambio'];
        if (cambioCarreraSubprograma) {
            const subprogramasInscritos = <?= json_encode(array_column($subprogramasInscritos, 'nombre_subprograma')); ?>;
            if (subprogramasInscritos.includes(cambioCarreraSubprograma)) {
                event.preventDefault();
                alert('Error: Ya estás inscrito en el subprograma seleccionado para Cambio de carrera.');
                return;
            }
        }

        // Validar que se haya seleccionado un semestre
        if (semestreSelect.value === "") {
            event.preventDefault();
            alert('Debe seleccionar un semestre para continuar.');
            return;
        }

        document.getElementById('tipo_solicitud').value = tipoSolicitud;
    });

    // Función para eliminar saltos de línea
    function removeNewLines(textarea) {
        textarea.value = textarea.value.replace(/\r?\n|\r/g, '');
    }

    descripcionGeneral.addEventListener('paste', function(event) {
        setTimeout(function() {
            removeNewLines(descripcionGeneral);
        }, 0);
    });

    actualizarCampos();
    actualizarEstadoCheckboxes();
});
</script>
<footer style="height: 50px;" class="custom-footer py-3 text-center position-fixed bottom-0 w-100">
    <div class="footer-container d-flex justify-content-center w-100">
    <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
</footer>
</body>
</html>
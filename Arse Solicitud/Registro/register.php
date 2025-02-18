<?php
require_once '../PHP/bd_conexion.php';

// Obtener programas, subprogramas y subprogramas_sedes
$programas = $pdo->query("SELECT id, nombre_programa FROM programas")->fetchAll(PDO::FETCH_ASSOC);
$subprogramas = $pdo->query("SELECT id, nombre_subprograma, programa_id FROM subprogramas")->fetchAll(PDO::FETCH_ASSOC);
$subprogramas_sedes = $pdo->query("SELECT subprograma_id, sede_id FROM subprogramas_sedes")->fetchAll(PDO::FETCH_ASSOC);
$sedes = $pdo->query("SELECT id, nombre_sede FROM sedes")->fetchAll(PDO::FETCH_ASSOC);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Registro de Estudiante</title>
    <link href="../css/estilo_base.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .form-step {
            display: none;
        }
        .form-step.active-step {
            display: block;
        }
        .password-strength-bar {
            height: 5px;
            background: red;
            width: 0;
        }
        .met {
            color: green;
        }
        .input-group {
            position: relative;
            margin-bottom: 1rem;
        }
        .input-group input {
            width: 100%;
            padding: 0.5rem;
            border: 1px solid #ccc;
            border-radius: 4px;
        }
        .input-group label {
            position: absolute;
            top: -1.5rem;
            left: 0;
            font-size: 0.875rem;
            color: #555;
        }
        .password-strength {
            margin-top: 0.5rem;
        }
        .password-strength-bar {
            height: 5px;
            background: red;
            width: 0;
        }
        .requirements {
            margin-top: 0.5rem;
            font-size: 0.875rem;
        }
        .requirement {
            color: #555;
        }
        .requirement.met {
            color: green;
        }
        .error-message {
            color: red;
            display: none;
        }
        .is-invalid {
            border-color: red;
        }
    </style>
</head>
<body class="bg-light">
    <div class="container py-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title">Registro de Estudiante</h5>
                    </div>
                    <div class="card-body">
                        <form id="registerForm" action="../php/procesar_registro.php" method="POST" enctype="multipart/form-data">
                            <!-- Paso 1: Información Personal -->
                            <div class="form-step active-step" id="step-1">
                                <div class="mb-3">
                                    <label for="username" class="form-label">Nombre de Usuario</label>
                                    <input type="text" id="username" name="nombre_usuario" class="form-control" required>
                                    <div id="usernameError" class="invalid-feedback">El nombre de usuario ya está en uso.</div>
                                </div>
                                <div class="mb-3">
                                    <label for="email" class="form-label">Correo Electrónico</label>
                                    <input type="email" id="email" name="correo" class="form-control" required>
                                    <div id="emailError" class="invalid-feedback">El correo electrónico ya está en uso.</div>
                                </div>
                                <div class="input-group">
                                    <input type="password" id="password" name="password" required />
                                    <label for="password">Contraseña</label>
                                    <div class="password-strength">
                                        <div class="password-strength-bar"></div>
                                    </div>
                                    <div class="requirements">
                                        <div class="requirement" id="length">● Mínimo 8 caracteres</div>
                                        <div class="requirement" id="uppercase">● Al menos una mayúscula</div>
                                        <div class="requirement" id="lowercase">● Al menos una minúscula</div>
                                        <div class="requirement" id="number">● Al menos un número</div>
                                    </div>
                                </div>
                                <div class="input-group">
                                    <input type="password" id="confirmPassword" name="confirmPassword" required />
                                    <label for="confirmPassword">Confirmar Contraseña</label>
                                    <div class="error-message" id="passwordMismatchError">Las contraseñas no coinciden</div>
                                </div>
                                <button type="button" class="btn following" onclick="nextStep()">Siguiente</button>
                                <a href="../index.html" class="btn register">&larr; Volver al Inicio</a>
                            </div>

                            <!-- Paso 2: Información Personal -->
                            <div class="form-step" id="step-2">
                                <div class="mb-3">
                                    <label for="primer_nombre" class="form-label">Primer Nombre</label>
                                    <input type="text" id="primer_nombre" name="primer_nombre" class="form-control" required>
                                </div>
                                <div class="mb-3">
                                    <label for="segundo_nombre" class="form-label">Segundo Nombre</label>
                                    <input type="text" id="segundo_nombre" name="segundo_nombre" class="form-control" pattern="[A-Za-z]*" placeholder="Opcional">
                                </div>
                                <div class="mb-3">
                                    <label for="primer_apellido" class="form-label">Primer Apellido</label>
                                    <input type="text" id="primer_apellido" name="primer_apellido" class="form-control" required>
                                </div>
                                <div class="mb-3">
                                    <label for="segundo_apellido" class="form-label">Segundo Apellido</label>
                                    <input type="text" id="segundo_apellido" name="segundo_apellido" class="form-control" pattern="[A-Za-z]*" placeholder="Opcional">
                                </div>
 
                                <button type="button" class="btn btn following" onclick="nextStep()">Siguiente</button>
                                <button type="button" class="btn btn-primary" onclick="prevStep()">Atrás</button>
                            </div>

                            <!-- Paso 3: Foto de Perfil y Cédula de Identidad -->
                            <div class="form-step" id="step-3">
                                <div class="mb-3">
                                    <label for="foto_perfil" class="form-label">Foto de Perfil</label>
                                    <input type="file" id="foto_perfil" name="foto_perfil" class="form-control" required>
                                </div>
                                <div class="mb-3">
                                    <label for="cedula" class="form-label">Cédula de Identidad</label>
                                    <input type="text" id="cedula" name="cedula" class="form-control" pattern="[0-9]{0,9}" maxlength="9" required>
                                    <div id="cedulaError" class="invalid-feedback">La cédula de identidad ya está en uso.</div>
                                    <small class="form-text text-muted">Máximo 9 dígitos.</small>
                                </div>
                                
                                <button type="button" class="btn btn following" onclick="nextStep()">Siguiente</button>
                                <button type="button" class="btn btn-primary" onclick="prevStep()">Atrás</button>
                            </div>

                            <!-- Paso 4: Selección de Programas y Subprogramas -->
                            <div class="form-step" id="step-4">
                                <div class="mb-3">
                                    <label for="cantidadCarreras" class="form-label">¿Cuántas carreras cursa?</label>
                                    <select id="cantidadCarreras" name="cantidadCarreras" class="form-select" required onchange="mostrarProgramas()">
                                        <option value="" disabled selected>Seleccione una opción</option>
                                        <option value="1">1</option>
                                        <option value="2">2</option>
                                        <option value="3">3</option>
                                    </select>
                                </div>
                                <div id="programasContainer"></div>
                                
                                <button type="submit" class="btn btn following" id="submitButton" disabled>Registrarse</button>
                                <button type="button" class="btn btn-primary" onclick="prevStep()">Atrás</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="../js/rol.js"></script>
    <script>
        const programas = <?= json_encode($programas); ?>;
        const subprogramas = <?= json_encode($subprogramas); ?>;
        const subprogramasSedes = <?= json_encode($subprogramas_sedes); ?>;
        const sedes = <?= json_encode($sedes); ?>;

        function mostrarProgramas() {
            const cantidadCarreras = document.getElementById('cantidadCarreras').value;
            const programasContainer = document.getElementById('programasContainer');
            programasContainer.innerHTML = '';

            for (let i = 0; i < cantidadCarreras; i++) {
                const programaSelect = document.createElement('select');
                programaSelect.name = `programa${i + 1}`;
                programaSelect.classList.add('form-select', 'mb-3');
                programaSelect.required = true;
                programaSelect.innerHTML = '<option value="" disabled selected>Seleccione un programa</option>';
                programas.forEach(programa => {
                    programaSelect.innerHTML += `<option value="${programa.id}">${programa.nombre_programa}</option>`;
                });

                const subprogramaSelect = document.createElement('select');
                subprogramaSelect.name = `subprograma${i + 1}`;
                subprogramaSelect.classList.add('form-select', 'mb-3');
                subprogramaSelect.required = true;
                subprogramaSelect.innerHTML = '<option value="" disabled selected>Seleccione un subprograma</option>';

                programaSelect.addEventListener('change', function() {
                    const programaId = this.value;
                    subprogramaSelect.innerHTML = '<option value="" disabled selected>Seleccione un subprograma</option>';
                    subprogramas.filter(subprograma => subprograma.programa_id == programaId).forEach(subprograma => {
                        subprogramaSelect.innerHTML += `<option value="${subprograma.id}">${subprograma.nombre_subprograma}</option>`;
                    });
                });

                const sedeSelect = document.createElement('select');
                sedeSelect.name = `sede${i + 1}`;
                sedeSelect.classList.add('form-select', 'mb-3');
                sedeSelect.required = true;
                sedeSelect.innerHTML = '<option value="" disabled selected>Seleccione una sede</option>';

                subprogramaSelect.addEventListener('change', function() {
                    const subprogramaId = this.value;
                    sedeSelect.innerHTML = '<option value="" disabled selected>Seleccione una sede</option>';
                    const sedesDisponibles = subprogramasSedes.filter(ss => ss.subprograma_id == subprogramaId).map(ss => ss.sede_id);
                    sedes.filter(sede => sedesDisponibles.includes(sede.id)).forEach(sede => {
                        sedeSelect.innerHTML += `<option value="${sede.id}">${sede.nombre_sede}</option>`;
                    });
                });

                programasContainer.appendChild(programaSelect);
                programasContainer.appendChild(subprogramaSelect);
                programasContainer.appendChild(sedeSelect);
            }
        }

       
    </script>
</body>
</html>
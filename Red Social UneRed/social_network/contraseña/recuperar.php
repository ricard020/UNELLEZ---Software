<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Recuperación de contraseña</title>

</head>

<style>

body {
    font-family: Arial, sans-serif;
    background-color: #4a5757;
    background-image: url("data:image/svg+xml,%3Csvg width='42' height='44' viewBox='0 0 42 44' xmlns='http://www.w3.org/2000/svg'%3E%3Cg id='Page-1' fill='none' fill-rule='evenodd'%3E%3Cg id='brick-wall' fill='%23293737' fill-opacity='0.14'%3E%3Cpath d='M0 0h42v44H0V0zm1 1h40v20H1V1zM0 23h20v20H0V23zm22 0h20v20H22V23z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E");
    position: relative;
    color: white;
    text-align: center;
    margin: 0;
    padding: 0;
}

.container {
    max-width: 400px;
    margin: 150px auto;
    padding: 20px;
    background-color: #293737;
    border-radius: 10px;
    box-shadow: 0 4px 10px rgba(0, 0, 0, 0.3);
}

h1 {
    color: #ee5d1c;
}

p {
    font-size: 16px;
    margin-bottom: 20px;
}

.controls {
    width: 95%;
    padding: 10px;
    margin: 10px 0;
    border: none;
    border-radius: 5px;
    background-color: #ffffff;
    color: black;
}

.controls::placeholder {
    color: #bbb;
}

.btn button {
    background-color: #ee5d1c;
    color: white;
    border: none;
    padding: 12px;
    width: 100%;
    font-size: 16px;
    cursor: pointer;
    border-radius: 5px;
    transition: background 0.3s;
    margin-top: 30px;
}

.btn button:hover {
    background-color: #d54b15;
}

.footer {
    margin-top: 15px;
}

.footer-link {
    color: #ee5d1c;
    text-decoration: none;
    font-weight: bold;
}

.footer-link:hover {
    text-decoration: underline;
}

.password-requirements {
    list-style: none;
    padding: 0;
    font-size: 14px;
    color: #bbb;
    text-align: left;
}

.password-strength {
    width: 100%;
    height: 10px;
    background: #354646;
    border-radius: 5px;
    margin-top: 10px;
}

.strength-bar {
    height: 100%;
    width: 0%;
    border-radius: 5px;
}

.strength-text {
    margin-top: 5px;
    font-size: 14px;
}

</style>

<body>
    <main class="container">
        <!-- Paso 1: Olvidaste tu contraseña -->
        <section class="step" id="step-1">
            <div class="img">
                <img src="../public/images/logo.png" width="100px" height="100px" alt="Unered">
            </div>
            <h1>Recuperar contraseña</h1>
            <p>Ingresa tu correo electrónico para recibir un código de restablecimiento.</p>
            <form class="form">
                <input class="controls" type="email" id="email" name="email" placeholder="Correo electrónico..." required>
            </form>
            <div class="btn">
                <button type="button" onclick="sendCode()">Enviar código</button>
            </div>
            <div class="footer">
                <a href="../index.html" class="footer-link">Regresar al inicio</a>
            </div>
        </section>

        <!-- Paso 2: Código de restauración -->
        <section class="step" id="step-2" style="display: none;">
            <div class="img">
                <img src="../public/images/logo.png" width="100px" height="100px" alt="Unered">
            </div>
            <h1>Verificar código</h1>
            <p>Ingresa el código que recibiste en tu correo electrónico.</p>
            <form class="form">
                <input class="controls" type="text" id="codigo" name="codigo" pattern="[0-9]*" inputmode="numeric" placeholder="Código de verificación" required>
            </form>
            <div class="btn">
                <button type="button" onclick="verifyCode()">Continuar</button>
            </div>
            <div class="footer">
                <a href="../index.html" class="footer-link">Regresar al inicio</a>
            </div>
        </section>

        <!-- Paso 3: Restauración de contraseña -->
        <section class="step" id="step-3" style="display: none;">
            <div class="img">
                <img src="../public/images/logo.png" width="100px" height="100px" alt="Unered">
            </div>
            <h1>Restablecer contraseña</h1>
            <p>Ingresa y confirma tu nueva contraseña.</p>
            <form class="form">
                <input class="controls" type="password" id="new_password" name="new_password" placeholder="Nueva contraseña" required oninput="checkPasswordStrength()">
                <ul class="password-requirements">
                    <li id="length">Mínimo 8 caracteres</li>
                    <li id="uppercase">Al menos una mayúscula</li>
                    <li id="lowercase">Al menos una minúscula</li>
                    <li id="number">Al menos un número</li>
                </ul>
                <input class="controls" type="password" id="confirm_password" name="confirm_password" placeholder="Confirmar contraseña" required>
                <div class="password-strength">
                    <div class="strength-bar"></div>
                    <div class="strength-text"></div>
                </div>

            </form>
            <div class="btn">
                <button type="button" onclick="resetPassword()">Restablecer contraseña</button>
            </div>
            <div class="footer">
                <a href="../index.html" class="footer-link">Regresar al inicio</a>
            </div>
        </section>
    </main>

    <script>
        function sendCode() {
            const email = document.getElementById('email').value;
            if (validateEmail(email)) {
                // Enviar solicitud al servidor para enviar el código
                fetch('send_code.php', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ email: email })
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert('Código enviado a ' + email);
                        showStep(2);
                    } else {
                        alert('Error al enviar el código: ' + data.message);
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Error al enviar el código.');
                });
            } else {
                alert('Por favor, ingrese un correo válido.');
            }
        }

        function verifyCode() {
            const code = document.getElementById('codigo').value;
            if (code.length === 6) { // Suponiendo que el código tiene 6 dígitos
                // Enviar solicitud al servidor para verificar el código
                fetch('verify_code.php', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ code: code })
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert('Código verificado');
                        showStep(3);
                    } else {
                        alert('Código incorrecto: ' + data.message);
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Error al verificar el código.');
                });
            } else {
                alert('Por favor, ingrese un código válido.');
            }
        }

        function resetPassword() {
            const newPassword = document.getElementById('new_password').value;
            const confirmPassword = document.getElementById('confirm_password').value;

            if (newPassword === confirmPassword) {
                if (validatePassword(newPassword)) {
                    // Enviar solicitud al servidor para restablecer la contraseña
                    fetch('reset_password.php', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({ password: newPassword })
                    })
                    .then(response => response.json())
                    .then(data => {
                        if (data.success) {
                            alert('Contraseña restablecida correctamente');
                            // Redirigir al usuario a la página de inicio de sesión
                            window.location.href = '../index.html';
                        } else {
                            alert('Error al restablecer la contraseña: ' + data.message);
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        alert('Error al restablecer la contraseña.');
                    });
                } else {
                    alert('La contraseña no cumple con los requisitos.');
                }
            } else {
                alert('Las contraseñas no coinciden.');
            }
        }

        function showStep(stepNumber) {
            const steps = document.querySelectorAll('.step');
            steps.forEach(step => step.style.display = 'none');
            document.getElementById('step-' + stepNumber).style.display = 'block';
        }

        function validateEmail(email) {
            const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            return re.test(email);
        }

        function validatePassword(password) {
            const hasMinLength = password.length >= 8;
            const hasUpperCase = /[A-Z]/.test(password);
            const hasLowerCase = /[a-z]/.test(password);
            const hasNumber = /[0-9]/.test(password);
            return hasMinLength && hasUpperCase && hasLowerCase && hasNumber;
        }

        function checkPasswordStrength() {
            const password = document.getElementById('new_password').value;
            const strengthBar = document.querySelector('.strength-bar');
            const strengthText = document.querySelector('.strength-text');

            // Validar requisitos
            const hasMinLength = password.length >= 8;
            const hasUpperCase = /[A-Z]/.test(password);
            const hasLowerCase = /[a-z]/.test(password);
            const hasNumber = /[0-9]/.test(password);

            // Actualizar lista de requisitos
            document.getElementById('length').style.color = hasMinLength ? '#4dff4d' : '#ff4d4d';
            document.getElementById('uppercase').style.color = hasUpperCase ? '#4dff4d' : '#ff4d4d';
            document.getElementById('lowercase').style.color = hasLowerCase ? '#4dff4d' : '#ff4d4d';
            document.getElementById('number').style.color = hasNumber ? '#4dff4d' : '#ff4d4d';

            // Calcular fortaleza
            let strength = 0;
            if (hasMinLength) strength += 1;
            if (hasUpperCase) strength += 1;
            if (hasLowerCase) strength += 1;
            if (hasNumber) strength += 1;

            const strengthLevels = ['Muy débil', 'Débil', 'Moderada', 'Fuerte'];
            const colors = ['#ff4d4d', '#ffa64d', '#ffd24d', '#4dff4d'];

            strengthBar.style.width = `${(strength / 4) * 100}%`;
            strengthBar.style.backgroundColor = colors[strength - 1];
            strengthText.textContent = strengthLevels[strength - 1] || '';
        }
    </script>
</body>
</html>

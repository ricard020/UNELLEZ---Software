// Manejo de Pasos del Formulario
let currentStep = 0;
const steps = document.querySelectorAll('.form-step');

function showStep(index) {
    steps.forEach((step, i) => {
        step.classList.toggle('active-step', i === index);
    });
    currentStep = index;
}

function validateStep(stepIndex) {
    const step = steps[stepIndex];
    const inputs = step.querySelectorAll('input, select, textarea');
    let isValid = true;

    inputs.forEach(input => {
        if (!input.checkValidity()) {
            isValid = false;
            input.classList.add('is-invalid');
        } else {
            input.classList.remove('is-invalid');
        }
    });

    // Validar contraseñas si estamos en el paso de contraseñas
    if (stepIndex === 0) {
        const isStrong = updatePasswordStrength(passwordField ? passwordField.value : '');
        const passwordsMatch = validatePasswords();
        if (!isStrong || !passwordsMatch) {
            isValid = false;
        }
    }

    return isValid;
}

let usernameTimeout, emailTimeout, cedulaTimeout;
let isUsernameAvailable = true;
let isEmailAvailable = true;
let isCedulaAvailable = true;

function checkUsername() {
    clearTimeout(usernameTimeout);
    usernameTimeout = setTimeout(() => {
        const username = document.getElementById('username').value;
        if (username) {
            fetch(`verificar_usuario.php?username=${username}`)
                .then(response => response.json())
                .then(data => {
                    const usernameError = document.getElementById('usernameError');
                    if (data.exists) {
                        usernameError.style.display = 'block';
                        isUsernameAvailable = false;
                    } else {
                        usernameError.style.display = 'none';
                        isUsernameAvailable = true;
                    }
                    toggleSubmitButton();
                });
        }
    }, 1000);
}

function checkEmail() {
    clearTimeout(emailTimeout);
    emailTimeout = setTimeout(() => {
        const email = document.getElementById('email').value;
        if (email) {
            fetch(`verificar_usuario.php?email=${email}`)
                .then(response => response.json())
                .then(data => {
                    const emailError = document.getElementById('emailError');
                    if (data.exists) {
                        emailError.style.display = 'block';
                        isEmailAvailable = false;
                    } else {
                        emailError.style.display = 'none';
                        isEmailAvailable = true;
                    }
                    toggleSubmitButton();
                });
        }
    }, 1000);
}

function checkCedula() {
    clearTimeout(cedulaTimeout);
    cedulaTimeout = setTimeout(() => {
        const cedula = document.getElementById('cedula').value;
        if (cedula) {
            fetch(`verificar_usuario.php?cedula=${cedula}`)
                .then(response => response.json())
                .then(data => {
                    const cedulaError = document.getElementById('cedulaError');
                    if (data.exists) {
                        cedulaError.style.display = 'block';
                        isCedulaAvailable = false;
                    } else {
                        cedulaError.style.display = 'none';
                        isCedulaAvailable = true;
                    }
                    toggleSubmitButton();
                });
        }
    }, 1000);
}

document.getElementById('username').addEventListener('input', checkUsername);
document.getElementById('email').addEventListener('input', checkEmail);
document.getElementById('cedula').addEventListener('input', checkCedula);

function nextStep() {
    if (validateStep(currentStep) && currentStep < steps.length - 1 && isUsernameAvailable && isEmailAvailable && isCedulaAvailable) {
        showStep(currentStep + 1);
    }
}

function prevStep() {
    if (currentStep > 0) {
        showStep(currentStep - 1);
    }
}

// Barra de Progreso para Contraseña
const passwordField = document.getElementById('password');
const confirmPasswordField = document.getElementById('confirmPassword');
const submitButton = document.getElementById('submitButton');
const strengthBar = document.querySelector('.password-strength-bar');

const requirements = {
    length: str => str.length >= 8,
    uppercase: str => /[A-Z]/.test(str),
    lowercase: str => /[a-z]/.test(str),
    number: str => /[0-9]/.test(str),
};

function updatePasswordStrength(password) {
    let strength = 0;
    if (requirements.length(password)) strength += 25;
    if (requirements.uppercase(password)) strength += 25;
    if (requirements.lowercase(password)) strength += 25;
    if (requirements.number(password)) strength += 25;

    if (strengthBar) {
        strengthBar.style.width = `${strength}%`;
        strengthBar.style.background = strength <= 25 ? '#ff3333' : 
                                        strength <= 50 ? '#ffa533' : 
                                        strength <= 75 ? '#ffd133' : '#4CAF50';
    }

    Object.keys(requirements).forEach(req => {
        const element = document.getElementById(req);
        if (requirements[req](password)) {
            element.classList.add('met');
        } else {
            element.classList.remove('met');
        }
    });

    return strength === 100;
}

function validatePasswords() {
    const errorMessage = document.getElementById('passwordMismatchError');
    if (confirmPasswordField && passwordField && confirmPasswordField.value !== passwordField.value) {
        if (errorMessage) errorMessage.style.display = 'block';
        return false;
    } else {
        if (errorMessage) errorMessage.style.display = 'none';
        return true;
    }
}

function toggleSubmitButton() {
    const isStrong = updatePasswordStrength(passwordField ? passwordField.value : '');
    const passwordsMatch = validatePasswords();
    if (submitButton) {
        submitButton.disabled = !(isStrong && passwordsMatch && isUsernameAvailable && isEmailAvailable && isCedulaAvailable);
    }
}

if (passwordField) passwordField.addEventListener('input', toggleSubmitButton);
if (confirmPasswordField) confirmPasswordField.addEventListener('input', toggleSubmitButton);

document.addEventListener('DOMContentLoaded', () => showStep(0));
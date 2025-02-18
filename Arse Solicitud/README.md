# Sistema de Gestión Académica

Este proyecto es un sistema de gestión académica diseñado para administrar usuarios, subprogramas y solicitudes en un entorno académico. La aplicación está desarrollada en PHP y utiliza una estructura de directorios bien organizada para separar las diferentes funcionalidades.

## Estructura del Proyecto

- **Admin/**: Funcionalidades administrativas, incluyendo la gestión de usuarios y subprogramas.
  - **funcion/**: Funciones específicas para la administración.
  - **Super_usuario/**: Funcionalidades para superusuarios.
  - **menu_super_usuario.php**: Menú de navegación para superusuarios.

- **contraseña/**: Recuperación y verificación de contraseñas.
  - **recuperar.php**: Maneja la recuperación de contraseñas.
  - **reset_password.php**: Restablecimiento de contraseñas.
  - **send_code.php**: Envío de códigos de verificación.
  - **verify_code.php**: Verificación de códigos.

- **CSS/**: Archivos CSS para el estilo de la aplicación.

- **Estudiantes/**: Funcionalidades específicas para estudiantes.

- **fpdf/**: Biblioteca FPDF para la generación de documentos PDF.
  - **doc/**: Documentación de FPDF.
  - **tutorial/**: Tutoriales para FPDF.

- **HTML/**: Archivos HTML estáticos.

- **imagen/**: Almacenamiento de imágenes.

- **JefeSubprograma/**: Funcionalidades para jefes de subprograma.
  - **Funciones/**: Funciones PHP específicas para jefes de subprograma.

- **js/**: Archivos JavaScript.

- **logs/**: Archivos de registro.

- **PHP/**: Archivos PHP generales, incluyendo la conexión a la base de datos.
  - **bd_conexion.php**: Configuración y manejo de la conexión a la base de datos.

- **Registro/**: Funcionalidades relacionadas con el registro de usuarios.

- **Solicitudes/**: Gestión de solicitudes.

## Funcionalidades Principales

- **Gestión de Usuarios**: Crear, actualizar y eliminar usuarios.
- **Gestión de Subprogramas**: Asignar subprogramas a usuarios.
- **Recuperación de Contraseñas**: Envío y verificación de códigos de recuperación.
- **Generación de Documentos PDF**: Utilizando la biblioteca FPDF.
- **Registro de Usuarios**: Funcionalidades para el registro de nuevos usuarios.

## Tecnologías Utilizadas

- **PHP**: Lógica del servidor.
- **CSS**: Estilo de la aplicación.
- **FPDF**: Generación de documentos PDF.
- **PDO**: Interacción segura con la base de datos.

## Instalación

1. Clonar el repositorio.
2. Configurar la base de datos en `PHP/bd_conexion.php`.
3. Ejecutar el servidor local (por ejemplo, XAMPP).
4. Acceder a la aplicación a través del navegador.

## Contribuciones

Las contribuciones son bienvenidas. Por favor, abre un issue o envía un pull request para discutir cualquier cambio que desees realizar.

## Licencia

Este proyecto está licenciado bajo la [MIT License](LICENSE).

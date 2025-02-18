¡Bienvenido a UneRed!

Esta aplicación está diseñada para funcionar con XAMPP, por lo que es necesario tenerlo instalado en el sistema.



Configuración para Recuperación de Contraseña y Envío de Código por Correo



Para habilitar la funcionalidad de recuperación de contraseña y envío de códigos por correo electrónico, se deben realizar los siguientes pasos:

Ir a la carpeta importante incluida en el paquete de instalación.

Copiar los archivos php.ini y sendmail.ini de esta carpeta.

Reemplazar los archivos con el mismo nombre en las siguientes ubicaciones dentro de la carpeta de instalación de XAMPP:

php.ini debe reemplazarse en la carpeta xampp/php/

sendmail.ini debe reemplazarse en la carpeta xampp/sendmail/

Una vez realizados estos cambios, reiniciar el servidor Apache desde el panel de control de XAMPP para aplicar la configuración.



Instalación de UneRed en XAMPP


Para instalar la red social UneRed, seguir los siguientes pasos:

Copiar la carpeta del proyecto dentro de la carpeta htdocs de XAMPP.

Asegurarse de que el servidor Apache y MySQL estén en ejecución desde el panel de control de XAMPP.

Acceder a la aplicación a través del navegador ingresando la siguiente URL:

http://localhost/social_network

Importar la base de datos unered.sql (que se encuentra en la carpeta) en phpMyAdmin y asegurarse de asignarle el mismo nombre unered.


Con estos pasos, UneRed estará lista para su uso en el entorno local.



Notas Adicionales


Si existen problemas con el envío de correos, verificar que el servidor SMTP esté correctamente configurado en sendmail.ini.

Para cualquier configuración adicional, revisar la documentación de PHP Mail y XAMPP.
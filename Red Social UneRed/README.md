Descripción del Proyecto "UneRed"

UneRed es una red social diseñada exclusivamente para los estudiantes de la UNELLEZ, con el propósito de proporcionar un espacio digital donde puedan interactuar, compartir conocimientos y desarrollar proyectos en un entorno seguro y colaborativo. La plataforma facilita la creación de contenido académico y social, permitiendo a los usuarios publicar posts, debatir en foros, compartir imágenes, documentos y proyectos, así como participar en discusiones temáticas de interés común.

Además, UneRed promueve la integración estudiantil mediante herramientas interactivas que favorecen la comunicación y el trabajo en equipo, fomentando el aprendizaje colectivo y el intercambio de ideas dentro de la comunidad universitaria. Los estudiantes pueden reaccionar y compartir publicaciones, comentar en los temas de discusión y establecer conexiones significativas con sus compañeros de carrera y facultad.

La plataforma también busca incentivar la colaboración en proyectos de investigación y desarrollo, brindando la posibilidad de gestionar archivos y documentos de manera organizada. Con una interfaz intuitiva y funcionalidades adaptadas a las necesidades académicas, UneRed representa una solución innovadora para fortalecer la vinculación entre los estudiantes de la UNELLEZ y mejorar la experiencia educativa en el ámbito digital.




Requisitos Previos

Esta aplicación está diseñada para funcionar con XAMPP, por lo que es necesario tenerlo instalado en el sistema.




Configuración para Recuperación de Contraseña y Envío de Código por Correo

Para habilitar la funcionalidad de recuperación de contraseña y envío de códigos por correo electrónico, se deben realizar los siguientes pasos:

1.- Ir a la carpeta importante incluida en el paquete de instalación.

2.- Copiar los archivos php.ini y sendmail.ini de esta carpeta.

3.- Reemplazar los archivos con el mismo nombre en las siguientes ubicaciones dentro de la carpeta de instalación de XAMPP:

php.ini debe reemplazarse en la carpeta xampp/php/

sendmail.ini debe reemplazarse en la carpeta xampp/sendmail/

Una vez realizados estos cambios, reiniciar el servidor Apache desde el panel de control de XAMPP para aplicar la configuración.





Instalación de UneRed en XAMPP

Para instalar la red social UneRed, seguir los siguientes pasos:

1.- Copiar la carpeta del proyecto dentro de la carpeta htdocs de XAMPP.

2.- Asegurarse de que el servidor Apache y MySQL estén en ejecución desde el panel de control de XAMPP.

3.- Acceder a la aplicación a través del navegador ingresando la siguiente URL: http://localhost/social_network

4.- Importar la base de datos unered.sql en phpMyAdmin y asegurarse de asignarle el mismo nombre unered.

Con estos pasos, UneRed estará lista para su uso en el entorno local.




Notas Adicionales

Si existen problemas con el envío de correos, verificar que el servidor SMTP esté correctamente configurado en sendmail.ini.

Para cualquier configuración adicional, revisar la documentación de PHP Mail y XAMPP.

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Estadísticas de Solicitudes</title>
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

        .dropdown-menu-custom {
            background-color: rgba(230, 93, 6, 0.7);
            border: none;
            transform: translateY(-20px);
            opacity: 0;
            transition: transform 0.3s ease, opacity 0.3s ease;
        }

        .dropdown-menu-custom.show {
            transform: translateY(0);
            opacity: 1;
        }

        

        footer {
            background-color: #FF6B00;
            width: 100%;
            position: fixed;
            bottom: 0;
            padding: 10px 0;
            z-index: 1000; /* Asegura que el footer esté por encima de otros elementos */
            left: 0; /* Asegura que el footer comience desde el borde izquierdo */
        }

        footer .container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            width: 100%;
            padding: 0 20px; /* Añade algo de padding para que no esté pegado a los bordes */
        }

        footer a {
            color: white;
            text-decoration: none;
            padding: 5px 10px;
        }

        footer a:hover {
            text-decoration: underline; /* O cualquier estilo que desees */
        }

        body {
            font-family: Arial, sans-serif;
            text-align: center;
            margin: 25px;
        }

        input[type="month"] {
            margin: 20px 0;
            padding: 10px;
            font-size: 20px;
        }

        canvas {
            margin: 20px auto;
            max-width: 800px;
            max-height: 400px;
        }
    </style>
</head>
<body class="bg-image" style="background-image: url('../../imagen/2.jpg'); background-size: cover; background-position: center;  color: #333; line-height: 1.6; background-color: #f8f9fa;">

    <!-- Header -->
     <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
     <a href="../perfil_jefesubprograma.php">
    <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
    </a>
    <h1 style="color: #FF6B00;">Estadisticas</h1>
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

    <!-- Selector de fecha -->
    <div class="container mt-5 pt-5 text-start" style="font-size: 20px; position: absolute; top: 10; left: 10;">
        <label for="selectorFecha">Selecciona un mes:</label>
        <input type="month" id="selectorFecha" />
        <br>
        <button id="toggleChartType" class="btn btn-primary mt-3" style="font-size: 20px; position: absolute; top: 10; left: 10;">Cambiar a Gráfica de Pastel</button>
    </div>

    <!-- Gráfica -->
    <div class="container mt-3 d-flex justify-content-center align-items-center" style="height: 900px;">
            
            <canvas id="histogramaSolicitudes" width="1000" height="600"></canvas>
    </div>

    
    <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00; height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    
    </footer>

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/chartjs-plugin-datalabels"></script>
    <script>
        document.addEventListener('DOMContentLoaded', async () => {
            const selectorFecha = document.getElementById('selectorFecha');
            const canvas = document.getElementById('histogramaSolicitudes');
            const toggleChartTypeButton = document.getElementById('toggleChartType');
            const hoy = new Date();
            const mesActual = hoy.toISOString().slice(0, 7); // "YYYY-MM"
            let mesesDisponibles = []; // Guardar meses habilitados
            let chart; // Referencia a la gráfica
            let chartType = 'bar'; // Tipo de gráfica inicial

            // Colores específicos para cada tipo de solicitud
            const colores = {
                'Cambio de carrera': 'rgba(255, 99, 132, 0.7)',
                'Inscripciones temporales': 'rgba(54, 162, 235, 0.7)',
                'Reingreso': 'rgba(255, 206, 86, 0.7)',
                'Retiro temporal': 'rgba(75, 192, 192, 0.7)',
                'Traslado': 'rgba(153, 102, 255, 0.7)',
                'Unidades de crédito extras': 'rgba(255, 159, 64, 0.7)'
            };

            // Función para obtener los meses disponibles
            async function obtenerMesesDisponibles() {
                try {
                    const response = await fetch(`../Funciones/Estadisticas.php?meses_disponibles=1`);
                    const datos = await response.json();
                    return datos.map(item => `${item.anio}-${String(item.mes).padStart(2, '0')}`); // Formato "YYYY-MM"
                } catch (error) {
                    console.error("Error al obtener meses disponibles:", error);
                    return [];
                }
            }

            // Función para cargar datos desde el servidor
            async function cargarDatos(fecha) {
                try {
                    const response = await fetch(`../Funciones/Estadisticas.php?mes=${fecha}`);
                    const datos = await response.json();
                    return datos;
                } catch (error) {
                    console.error("Error al cargar datos:", error);
                    return [];
                }
            }

            // Función para actualizar la gráfica
            async function actualizarGrafica(fecha) {
                const datos = await cargarDatos(fecha);
                const etiquetas = datos.map(item => item.tipo_solicitud);
                const valores = datos.map(item => item.total);
                const coloresAsignados = datos.map(item => colores[item.tipo_solicitud] || 'rgba(255, 107, 0, 0.7)');

                if (chart) {
                    chart.destroy();
                }

                chart = new Chart(canvas, {
                    type: chartType,
                    data: {
                        labels: etiquetas,
                        datasets: [{
                            label: 'Número de Solicitudes',
                            data: valores,
                            backgroundColor: coloresAsignados,
                            borderColor: coloresAsignados.map(color => color.replace('0.7', '1')),
                            borderWidth: 1
                        }]
                    },
                    options: {
                        plugins: {
                            datalabels: {
                                color: '#fff',
                                anchor: 'end',
                                align: 'start',
                                offset: -10,
                                borderWidth: 2,
                                borderColor: '#fff',
                                borderRadius: 25,
                                backgroundColor: (context) => {
                                    return context.dataset.backgroundColor;
                                },
                                font: {
                                    weight: 'bold',
                                },
                                formatter: (value, context) => {
                                    const dataset = context.chart.data.datasets[0];
                                    const total = dataset.data.reduce((prev, curr) => prev + curr, 0);
                                    const percentage = ((value / total) * 100).toFixed(2) + '%';
                                    return percentage;
                                }
                            }
                        },
                        legend: {
                            display: true,
                            position: 'top',
                            labels: {
                                fontColor: '#333',
                                fontSize: 12,
                                boxWidth: 20,
                                padding: 15
                            }
                        }
                    }
                });
            }

            // Inicializar la gráfica con el mes actual
            selectorFecha.value = mesActual;
            mesesDisponibles = await obtenerMesesDisponibles();
            if (mesesDisponibles.includes(mesActual)) {
                await actualizarGrafica(mesActual);
            }

            // Actualizar la gráfica cuando se seleccione un nuevo mes
            selectorFecha.addEventListener('change', async () => {
                const fechaSeleccionada = selectorFecha.value;
                if (mesesDisponibles.includes(fechaSeleccionada)) {
                    await actualizarGrafica(fechaSeleccionada);
                } else {
                    alert('No hay datos disponibles para el mes seleccionado.');
                }
            });

            // Alternar entre gráfica de barras y gráfica de pastel
            toggleChartTypeButton.addEventListener('click', async () => {
                chartType = chartType === 'bar' ? 'pie' : 'bar';
                toggleChartTypeButton.textContent = chartType === 'bar' ? 'Cambiar a Gráfica de Pastel' : 'Cambiar a Gráfica de Barras';
                const fechaSeleccionada = selectorFecha.value;
                if (mesesDisponibles.includes(fechaSeleccionada)) {
                    await actualizarGrafica(fechaSeleccionada);
                }
            });
        });
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>

<title>Buscador</title>
<link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
<meta name="viewport" content="width=device-width, initial-scale=1">
<link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">
<?php include("../includes/partials/navbar.php"); ?>
<style>
  body {
    color: black;
  }

  .p-4 {
    padding-bottom: 3rem !important;
  }

  .filter-buttons {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    gap: 5px;
  }
  .filter-buttons button {
    flex: 1 1 calc(10% - 5px);
    white-space: nowrap;
  }
  
  @media (max-width: 576px) {
    .filter-buttons button {
      flex: 1 1 48%;
    }
  }


/* Cambios responsive */
@media (max-width: 767.5px)  {
    .navbar {
        top: auto;
        bottom: 0; /* Mueve el navbar a la parte inferior */
        left: 0;
        height: 70px; /* Reduce la altura del navbar */
        width: 100%; /* Ocupa todo el ancho de la pantalla */
        flex-direction: row !important; /* Cambia a una fila horizontal */
        justify-content: space-around; /* Espaciado uniforme entre ítems */
        align-items: center;
        padding: 0;
    }

    .navbar-nav {
        width: 100%;
        padding: 0;
        margin: 0;
        list-style: none;
        display: flex;
        flex-direction: row !important;
    }
    .main-content {
        margin-left: 0 !important; /* Elimina el margen en móviles */
        margin-bottom: 80px !important; /* Espacio para la barra en la parte inferior */
    }

    .nav-item {
        margin: 0; /* Elimina márgenes extra */
    }

    .nav-link {
        font-size: 1.8rem; /* Íconos ligeramente más pequeños en móviles */
    }
}

.searchContainer {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100vh;
}
.searchCard {
  background-color: #293737;
  max-height: 90vh;
}
/* Proyectos - Azul claro */
#filterProjects {
  background-color: #293737;
  color: white;
  border-color: #4a5757;
}
#filterProjects:hover {
  background-color: #17a2b8; /* Azul claro */
  color: white;
  border-color: #17a2b8;
}

#filterProjects.active {
  background-color: #17a2b8; /* Azul claro */
  color: white;
  border-color: #17a2b8;
}

/* Categorías - Amarillo */
#filterCategories {
  background-color: #293737;
  color: white;
  border-color: #4a5757;
}
#filterCategories:hover {
  background-color: #ffc107; /* Amarillo */
  color: white;
  border-color: #ffc107;
}

#filterCategories.active {
  background-color: #ffc107; /* Amarillo */
  color: white;
  border-color: #ffc107;
}

/* Usuarios - Morado */
#filterUsers {
  background-color: #293737;
  color: white;
  border-color: #4a5757;
}
#filterUsers:hover {
  background-color: #860efd; /* Morado */
  color: white;
  border-color: #860efd;
}

#filterUsers.active {
  background-color: #860efd; /* Morado */
  color: white;
  border-color: #860efd;
}

/* Retweet - Verde */
#filterRetweets {
  background-color: #293737;
  color: white;
  border-color: #4a5757;
}
#filterRetweets:hover {
  background-color: rgb(63, 224, 146); /* Verde */
  color: white;
  border-color: rgb(63, 224, 146);
}

#filterRetweets.active {
  background-color: rgb(63, 224, 146); /* Verde */
  color: white;
  border-color: rgb(63, 224, 146);
}

/* Valorado - Rojo */
#filterValuated {
  background-color: #293737;
  color: white;
  border-color: #4a5757;
}
#filterValuated:hover {
  background-color: rgb(215, 68, 63); /* Rojo */
  color: white;
  border-color: rgb(215, 68, 63);
}

#filterValuated.active {
  background-color: rgb(215, 68, 63);/* Rojo */
  color: white;
  border-color: rgb(215, 68, 63);
}

</style>
</head>
<body>



<div class="searchContainer container d-flex justify-content-center align-items-center min-vh-200">
  <div class="searchCard card p-4 w-100">
    <div class="form-group">
      <input type="text" class="form-control form-control-lg searchBar" placeholder="Buscar..." id="searchInput">
    </div>
    <div class="filter-buttons">
      <button id="filterProjects" class="btn btn-outline-primary active" data-filter="projects">
        <i class="bi bi-journal-text"></i> Proyectos
      </button>
      <button id="filterCategories" class="btn btn-outline-primary" data-filter="categories">
        <i class="bi bi-tags"></i> Categorías
      </button>
      <button id="filterUsers" class="btn btn-outline-primary" data-filter="users">
        <i class="bi bi-person"></i> Usuarios
      </button>
      <button id="filterRetweets" class="btn btn-outline-primary" data-filter="retweets">
        <i class="bi bi-reply-all"></i> Compartidos
      </button>
      <button id="filterValuated" class="btn btn-outline-primary" data-filter="valuated">
        <i class="bi bi-heart"></i> Valoraciones
      </button>
    </div>
    <br>
    <div id="resultados"></div>
  </div>
</div>

<script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.3/dist/umd/popper.min.js"></script>
<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
<script>
  const searchInput = document.getElementById('searchInput');
  const filterButtons = document.querySelectorAll('.filter-buttons button');
  const resultados = document.getElementById('resultados');

  async function fetchProyectos(searchText) {
  try {
    const response = await fetch(`../controllers/buscar_proyectos.php?query=${encodeURIComponent(searchText)}`);
    const proyectos = await response.json();

    resultados.innerHTML = proyectos
      .map(proyecto => `
        <div class="proyectos-card card mb-3" onclick="verDetalles(${proyecto.post_id})">
          <div class="card-body">
            <h5 class="card-title">${proyecto.titulo}</h5>
            <p class="card-text">${proyecto.descripcion}</p>
          </div>
        </div>
      `)
      .join('') || '<p>No hay resultados.</p>';
  } catch (error) {
    console.error("Error al buscar proyectos:", error);
    resultados.innerHTML = '<p>Error al cargar los resultados.</p>';
  }
}

// Función que redirige a la página de detalles del proyecto
function verDetalles(postId) {
  window.location.href = `../controllers/publicacion_detalle.php?post_id=${postId}`;
}



  // Manejar eventos de búsqueda
  searchInput.addEventListener('input', () => {
    const activeFilter = document.querySelector('.filter-buttons .btn.active').dataset.filter;
    if (activeFilter === 'projects') {
      fetchProyectos(searchInput.value);
    }
  });

  // Manejar los botones de filtro
  filterButtons.forEach(button => {
    button.addEventListener('click', () => {
      filterButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');

      if (button.dataset.filter === 'projects') {
        fetchProyectos(searchInput.value);
      } else {
        resultados.innerHTML = '<p></p>';
      }
    });
  });

  // Cargar proyectos predeterminados al inicio
  fetchProyectos('');
</script>

<script>
  // Función para obtener resultados de categorías desde el servidor
  async function fetchCategorias(searchText) {
    try {
      const response = await fetch(`../controllers/buscar_categorias.php?query=${encodeURIComponent(searchText)}`);
      const categorias = await response.json();

      resultados.innerHTML = categorias
        .map(categoria => `
          <div class="categorias-card card mb-3" onclick="verDetallesCategoria(${categoria.id})">
            <div class="card-body">
              <h5 class="card-title">${categoria.nombre}</h5>
            </div>
          </div>
        `)
        .join('') || '<p>No hay resultados.</p>';
    } catch (error) {
      console.error("Error al buscar categorías:", error);
      resultados.innerHTML = '<p>Error al cargar los resultados.</p>';
    }
  }

  // Función que redirige a la página de detalles de la categoría
  function verDetallesCategoria(categoriaId) {
    window.location.href = `../controllers/resultados_categorias.php?categoria_id=${categoriaId}`;
  }

  // Escuchar eventos del buscador para el filtro "categorías"
  searchInput.addEventListener('input', () => {
    const activeFilter = document.querySelector('.filter-buttons .btn.active').dataset.filter;
    if (activeFilter === 'categories') {
      fetchCategorias(searchInput.value);
    }
  });

  filterButtons.forEach(button => {
    button.addEventListener('click', () => {
      filterButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');

      if (button.dataset.filter === 'categories') {
        fetchCategorias(searchInput.value);
      } else {
        resultados.innerHTML = '<p></p>';
      }
    });
  });
</script>

<script>
  // Función para obtener resultados de usuarios desde el servidor
  async function fetchUsuarios(searchText) {
    try {
      const response = await fetch(`../controllers/buscar_usuarios.php?query=${encodeURIComponent(searchText)}`);
      const usuarios = await response.json();

      resultados.innerHTML = usuarios
        .map(usuario => `
          <div class="usuario-card card mb-3" onclick="verDetallesUsuario(${usuario.id})">
            <div class="card-body d-flex align-items-center">
              <img src="${usuario.foto_perfil || 'https://via.placeholder.com/80'}" 
                   alt="Foto de ${usuario.nombre}" 
                   class="rounded-circle me-3"
                   style="width: 80px; height: 80px;">
              <div>
                <h5 class="card-title">${usuario.nombre} ${usuario.apellido}</h5>
                <p class="card-text">${usuario.email || 'Sin correo disponible'}</p>
              </div>
            </div>
          </div>
        `)
        .join('') || '<p>No hay resultados.</p>';
    } catch (error) {
      console.error("Error al buscar usuarios:", error);
      resultados.innerHTML = '<p>Error al cargar los resultados.</p>';
    }
  }

  // Función que redirige a la página de detalles del usuario
  function verDetallesUsuario(usuarioId) {
    window.location.href = `./usuarios_perfil.php?usuario_id=${usuarioId}`;
  }

  // Escuchar eventos del buscador para el filtro "usuarios"
  searchInput.addEventListener('input', () => {
    const activeFilter = document.querySelector('.filter-buttons .btn.active').dataset.filter;
    if (activeFilter === 'users') {
      fetchUsuarios(searchInput.value);
    }
  });

  filterButtons.forEach(button => {
    button.addEventListener('click', () => {
      filterButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');

      if (button.dataset.filter === 'users') {
        fetchUsuarios(searchInput.value);
      } else {
        resultados.innerHTML = '<p></p>';
      }
    });
  });
</script>

<script>
  // Función para obtener los resultados de retweets desde el servidor
async function fetchRetweets(searchText) {
  try {
    const response = await fetch(`../controllers/buscar_retweets.php?query=${encodeURIComponent(searchText)}`);
    const proyectos = await response.json();

    resultados.innerHTML = proyectos
      .map(proyecto => `
         <div class="proyecto-card card mb-3" onclick="verDetallesRetweets(${proyecto.id}, ${proyecto.post_id})">
            <div class="card-body">
              <h5>${proyecto.titulo}</h5>
              <p class="card-text">${proyecto.descripcion}</p>
              <small>Compartidos: ${proyecto.retweet_count}</small>
            </div>
          </div>
        `)
      .join('') || '<p>No hay proyectos con retweets.</p>';
  } catch (error) {
    console.error("Error al buscar retweets:", error);
    resultados.innerHTML = '<p>Error al cargar los resultados.</p>';
  }
}

// Función que redirige a la página de detalles del proyecto retweets
function verDetallesRetweets(proyectoId, postId) {
  console.log("Proyecto ID:", proyectoId); // Muestra el proyectoId
  console.log("Post ID:", postId);         // Muestra el postId
  window.location.href = `../controllers/publicacion_detalle.php?proyecto_id=${proyectoId}&post_id=${postId}`;
}

  // Escuchar eventos del buscador para el filtro "retweets"
  searchInput.addEventListener('input', () => {
    const activeFilter = document.querySelector('.filter-buttons .btn.active').dataset.filter;
    if (activeFilter === 'retweets') {
      fetchRetweets(searchInput.value);
    }
  });

  filterButtons.forEach(button => {
    button.addEventListener('click', () => {
      filterButtons.forEach(btn => btn.classList.remove('active'));
      button.classList.add('active');

      if (button.dataset.filter === 'retweets') {
        fetchRetweets(searchInput.value);
      } else {
        resultados.innerHTML = '<p></p>';
      }
    });
  });
</script>

<script>
// Función para obtener los resultados de proyectos valorados desde el servidor
async function fetchValorados(searchText) {
  try {
    const response = await fetch(`../controllers/buscar_valorados.php?query=${encodeURIComponent(searchText)}`);
    const Valorado = await response.json();

    resultados.innerHTML = Valorado
      .map(proyecto => `
          <div class="proyecto-card card mb-3" onclick="verDetallesValorado(${proyecto.id}, ${proyecto.post_id})">
            <div class="card-body">
              <h5>${proyecto.titulo}</h5>
              <p class="card-text">${proyecto.descripcion}</p>
              <small>Me gusta: ${proyecto.like_count}</small>
            </div>
          </div>
        `)
      .join('') || '<p>No hay proyectos valorados.</p>';
  } catch (error) {
    console.error("Error al buscar valorados:", error);
    resultados.innerHTML = '<p>Error al cargar los resultados.</p>';
  }
}

// Función que redirige a la página de detalles del proyecto valorado
function verDetallesValorado(proyectoId, postId) {
  console.log("Proyecto ID:", proyectoId); // Muestra el proyectoId
  console.log("Post ID:", postId);         // Muestra el postId
  window.location.href = `../controllers/publicacion_detalle.php?proyecto_id=${proyectoId}&post_id=${postId}`;
}


// Escuchar eventos del buscador para el filtro "valorados"
searchInput.addEventListener('input', () => {
  const activeFilter = document.querySelector('.filter-buttons .btn.active').dataset.filter;
  if (activeFilter === 'valuated') {
    fetchValorados(searchInput.value);
  }
});

filterButtons.forEach(button => {
  button.addEventListener('click', () => {
    filterButtons.forEach(btn => btn.classList.remove('active'));
    button.classList.add('active');

    if (button.dataset.filter === 'valuated') {
      fetchValorados(searchInput.value);
    } else {
      resultados.innerHTML = '<p></p>';
    }
  });
});
</script>






<style>
  /* Ajustar el tamaño del contenedor de resultados */
  #resultados {
    margin-top: 20px;
    padding: 15px;
    border-radius: 10px;
    background-color: #293737;
    min-height: 50vh;
    max-height: 100%;
    overflow-y: auto;
  }

  /* Ajustar las tarjetas de los usuarios */
  .usuario-card, .categorias-card, .proyectos-card, .proyecto-card {
    color: white;
    display: flex;
    padding: 15px;
    border-radius: 10px;
    margin-bottom: 15px;
    background-color: #4a5757;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s, box-shadow 0.2s;
  }

  .usuario-card:hover, .categorias-card:hover, .proyectos-card:hover, .proyecto-card:hover{
    transform: scale(1.02);
    box-shadow: 0 6px 8px rgba(0, 0, 0, 0.15);
  }

  /* Imagen de perfil */
  .usuario-card img {
    width: 80px;
    height: 80px;
    margin-right: 15px;
    object-fit: cover;
    border: 5px solid #ee5d1c;
  }

  /* Texto de los usuarios */
  .usuario-card h5 {
    margin: 0;
    font-size: 1.25rem;
  }

  .usuario-card p {
    margin: 0;
    color: #ee5d1c;
  }
  .searchBar {
    background-color: #293737;
    color: white;
    border-color: #4a5757;
  }
  .searchBar:active, .searchBar:focus {
    background-color: #293737;
    color: white;
    border-color: #4a5757;
  }
</style>

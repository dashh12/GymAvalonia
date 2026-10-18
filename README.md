 FITPULSE GYM

Trabajo Práctico 4 - Programación II 

## Integrantes
- Lautaro Ramos
- Diego Gonzalez

Es una Aplicacion de escritorio hecha con Avalonia UI que consume una Web API de ASP.NET Core con base de datos SQLite (Entity Framework Core). Sirve para administrar un gimnasio.


La tematica es una Gestión de un gimnasio: miembros y sus membresías, entrenadores y sus rutinas, métodos de pago y maquinaria.


## Estructura de la solución

- ApiWeb: servidor, ASP.NET Core Web API con Controllers, EF Core y SQLite.
- MyAvaloniaApp: cliente de escritorio, Avalonia UI con MVVM.
- GymAvalonia.slnx: solución con los dos proyectos.

## Requisitos

- .NET SDK 10.
- Visual Studio 2022 o superior (o la CLI de .NET).
- Herramienta dotnet-ef, solo si se quiere crear la base a mano: dotnet tool install --global dotnet-ef

No hace falta instalar ninguna base de datos. Se usa SQLite y los archivos se crean solos.

## Configuración de la conexión

- La API se conecta a la base con la cadena que está en ApiWeb/appsettings.json (ConnectionStrings > Default = "Data Source=gym.db").
- Avalonia se conecta a la API con la dirección que está en MyAvaloniaApp/Services/ApiService.cs (http://localhost:5141/). Si se cambia el puerto de la API en ApiWeb/Properties/launchSettings.json, hay que cambiarlo también en ese archivo.

## Creación de la base de datos

La base se crea sola. Al iniciar, la API aplica las migraciones y carga los datos de prueba (membresías, entrenadores, rutinas, maquinaria, métodos de pago y dos miembros de ejemplo).

Para hacerlo a mano, con la API detenida, desde la carpeta ApiWeb:

    dotnet ef database update

Para empezar de cero, borrar los archivos gym.db, gym.db-shm y gym.db-wal de la carpeta ApiWeb y volver a iniciar la API.

## Cómo ejecutar (primero la API y después Avalonia)

La API tiene que estar en ejecución antes de abrir la aplicación de escritorio.

Opción A (abrir un proyecto a la vez):

1. Abrir GymAvalonia.slnx en Visual Studio.
2. Ejecutar el proyecto ApiWeb (perfil http). Se abre Swagger en http://localhost:5141/swagger
3. Ejecutar el proyecto MyAvaloniaApp.
4. Iniciar sesión con usuario: gym, contraseña: 123.

Opción B (iniciar los dos proyectos a la vez):

1. Abrir GymAvalonia.slnx en Visual Studio.
2. Clic derecho sobre la solución, Configurar proyectos de inicio, Varios proyectos de inicio. Poner ApiWeb primero y MyAvaloniaApp después, y elegir Iniciar en los dos. Esta configuración no se guarda en el repositorio, hay que hacerla en cada computadora.
3. Iniciar sesión con usuario: gym, contraseña: 123.

IMPORTANTE: si Avalonia muestra "No se pudo conectar con la API", es porque la API no está en ejecución.

## Entidades (tablas)

- Miembro: socio del gimnasio (nombre, apellido y email unico). Tiene una membresía y, opcionalmente, una rutina y un metodo de pago.
- Membresia: tipo de plan (Basico, Estandar, Avanzado, Vip).
- Pago: metodo de pago disponible (Efectivo o Transferencia).
- Entrenador: profesional que arma las rutinas (nombre, apellido y especialidad).
- Rutina: rutina de entrenamiento (descripción y nivel de dificultad) a cargo de un entrenador.
- Maquinaria: máquina del gimnasio (nombre, músculo que trabaja, estado y fecha del último mantenimiento).

## Relaciones

- Membresia 1 a N Miembro.
- Entrenador 1 a N Rutina.
- Rutina 1 a N Miembro.
- Pago 1 a N Miembro.

## Pantallas (Avalonia)

- Login: acceso al sistema con validación de usuario y contraseña.
- Inicio: menú principal con la navegacion y accesibilidad al resto de pantallas.
- Miembros: listado con busqueda, alta, edicion y baja. Permite elegir la membresía, la rutina y el método de pago. Usa la API.
- Entrenadores: listado con busqueda, alta, edicion y baja. Usa la API.
- Maquinaria: listado con búsqueda, alta, edición y baja. Usa la API.
- Pagos: muestra los tipos de membresía con sus beneficios.

Los desplegables de rutina y de método de pago de la pantalla Miembros muestran las opciones de los datos de prueba, y lo que se elige se guarda en la base a través de la API.

## Endpoints

Se pueden probar desde Swagger (http://localhost:5141/swagger) o con el archivo ApiWeb/ApiWeb.http.

Miembros:

- GET /api/miembros (acepta ?buscar=texto)
- GET /api/miembros/{id}
- POST /api/miembros
- PUT /api/miembros/{id}
- DELETE /api/miembros/{id}

Membresias:

- GET /api/membresias
- GET /api/membresias/{id}
- POST /api/membresias
- PUT /api/membresias/{id}
- DELETE /api/membresias/{id}

Entrenadores:

- GET /api/entrenadores
- GET /api/entrenadores/{id}
- POST /api/entrenadores
- PUT /api/entrenadores/{id}
- DELETE /api/entrenadores/{id}

Maquinaria:

- GET /api/maquinaria
- GET /api/maquinaria/{id}
- POST /api/maquinaria
- PUT /api/maquinaria/{id}
- DELETE /api/maquinaria/{id}


## Consulta LINQ

En los ViewModels de Avalonia (por ejemplo MiembrosViewModel, método Filtrar) el buscador usa Where y OrderBy para filtrar y ordenar la lista.

Arquitectura:

MVVM con CommunityToolkit.Mvvm:

- Views (.axaml): solo el diseño, con bindings.
- ViewModels: estado de cada pantalla y sus comandos.
- Services (ApiService).
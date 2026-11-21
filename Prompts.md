Prompt 1:
Estoy desarrollando un TP integrador de Programación II: sistema de reserva de canchas
(Cancha, Cliente, Turno). Proyecto "Testing", .NET 8, Web API con arquitectura por capas,
SQL Server con Entity Framework Core 8.0.5, LINQ, y frontend en HTML, CSS y JavaScript
vainilla (sin frameworks).
Estructura existente (leela antes de escribir código y respetá mis nombres):
- Entities/: Cancha, Cliente, Turno
- Business/: DTOs/ (DTOCANCHA, DTOCLIENTE, DTOTURNO), CanchaService, ClienteService, TurnoService
- Controllers/: CanchaController, ClienteController, TurnosController
- Data/: AppDbContext, CanchaRepository, ClienteRepository, TurnoRepository, Migrations/
- Program.cs ya registra con AddScoped los 3 repositories, los 3 services y el AppDbContext.
  No dupliques registros.
No modifiques entities, DTOs ni AppDbContext salvo que el prompt lo pida explícitamente.
REGLAS DE ESTILO (obligatorias, es el estilo que usamos en clase):
1. Arquitectura
- Sin interfaces. Services y Repositories son clases concretas.
- Namespaces: Testing.Entities, Testing.Data, Testing.Business, Testing.Controllers.
  Para los DTOs usá el namespace que ya tienen los DTOs existentes.
- Entities: campos privados y propiedades con get / set. Los strings se inicializan
  con = string.Empty.
2. Formato del código (muy importante)
- Una instrucción por línea. Nada de todo en una sola línea.
- Usá variables intermedias y después return. Ejemplo:
    public async Task<Cancha?> GetById(int id)
    {
    var canchaBuscada = await _context.Canchas.FirstOrDefaultAsync(c => c.Id == id);
    return canchaBuscada;
    }
- Una validación por if, cada una con su propio mensaje. Nunca encadenar condiciones
  distintas con || en un solo if. Ejemplo:
    if (cancha is null)
    {
    return BadRequest("Debe enviar los datos de la cancha.");
    }
    if (string.IsNullOrWhiteSpace(cancha.Nombre))
    {
    return BadRequest("El nombre de la cancha es obligatorio.");
    }
- Llaves siempre en líneas propias, incluso en ifs de una sola instrucción.
- Las condiciones largas dentro de un Where se escriben en varias líneas.
3. Prohibido (no lo vimos en clase)
- Select, SelectMany ni otros métodos de LINQ para mapear. Para convertir una lista de
  Entities a DTOs usá foreach, creando cada DTO con new y agregándolo a una List.
- AsNoTracking, Task.CompletedTask, const ni static readonly para mensajes,
  [FromBody], ternarios (? :), AutoMapper, genéricos, DataAnnotations, FluentValidation,
  patrón Repository genérico.
- LINQ se usa solo para consultar la base: FirstOrDefaultAsync, AnyAsync, Where,
  ToListAsync, OrderBy.
- Si necesitás algo que no esté en estas reglas, avisame y explicámelo antes de usarlo.
4. Responsabilidades de cada capa
- Repository: solo acceso a datos con EF Core, métodos async/await correctos. Tiene su
  propio SaveChanges(). Nada de lógica de negocio ni de validaciones.
- Service: usa el repository, mapea Entity <-> DTO a mano y llama a _repository.SaveChanges().
  Los métodos que modifican datos devuelven string? con el mensaje de error
  (null significa que salió bien). No lanza excepciones para reglas de negocio.
  Solo tiene reglas que necesitan consultar la base o el estado del sistema
  (que el id exista, unicidad, superposición, etc.). NO valida formato de campos.
- Controller: [ApiController], [Route("[controller]")], inyecta el Service y ILogger.
  Cada acción va en try/catch (en el catch: _logger.LogError(ex, "mensaje") y
  return StatusCode(500, "Ocurrió un error al procesar la solicitud.")).
  Acá van las validaciones de lo que llega en el request (id <= 0, body nulo, campos
  vacíos, largos máximos, formatos, rangos). Siempre con await.
5. Cómo distinguir 404 de 400
- GET por id: si el service devuelve null, el controller responde NotFound.
- PUT y DELETE: el controller primero llama a _service.GetByIdAsync(id). Si devuelve null,
  responde NotFound. Si existe, llama a UpdateAsync / DeleteAsync, y cualquier mensaje de
  error que devuelvan lo responde como BadRequest.
- POST/PUT/DELETE exitosos responden Ok("mensaje de éxito.") como en el parcial.





Prompt 2: 
necesito que de acuerdo a este parcial, realices un crud de canchas, utilizando la estructura en capas y la comunicacion entre ellas. solamente utiliza las funcionalidades o formas de escribir el codigo que esta en este parcial , no agregues cosas de mas ni funcionalidades o metodos nuevos.
las validaciones de campos que se traen del body o request van en el controller,las demas validaciones en el service, y si una cancha tiene un turno no se puede borrar.
A la hora de devolver un error usa el manejo de errores del parcial.




Prompt 3:
ahora con la misma logica y consigna necesito el crud de cliente




Prompt 4:
bien, ahora necesito el crud de turno con las mismas consignas mencionadas anteriormente en el que se verifique tambien que:
La cancha debe existir. - El cliente debe existir. - La cancha debe estar activa. - El turno no puede estar en el pasado (Fecha + HoraInicio menor a DateTime.Now). - No puede haber superposición en la misma cancha y fecha. UpdateAsync y DeleteAsync: si el id no existe, "No se encontró un turno con ese id."  Validaciones del TurnosController (cada una en su if): - id <= 0 y body nulo - IdCancha e IdCliente mayores a 0 - Fecha distinta del valor por defecto (DateTime.MinValue) - HoraFin mayor que HoraInicio - El turno debe estar entre las 08:00 y las 23:00 - Horas exactas: HoraInicio y HoraFin sin minutos (el turno dura 1 hora o más, en bloques   de hora completa)





prompt 5:

Hacé estas correcciones respetando las reglas de estilo del Prompt 0. Leé los
archivos actuales primero. Se permiten estos cambios en DTOs:
- GetResponseCancha: agregar PrecioXhora.
- PostRequestCancha: sacar Id y agregar bool Activa con valor por defecto true.
- GetResponseCliente y PostRequestCliente: agregar Dni, Telefono y Email
  (strings = string.Empty). Sacar Id de PostRequestCliente.
- GetResponseTurno: agregar IdCancha, IdCliente, NombreCancha, NombreCliente y
  PrecioTotal (int: horas del turno x PrecioXhora, no se guarda en la base).
- PostRequestTurno: sacar Id.
CanchaService: mapear PrecioXhora en GetAllAsync y GetByIdAsync, y usar cancha.Activa
tanto en AddCanchaAsync (hoy fuerza true) como en UpdateAsync (hoy no la modifica).
CanchaController: en Create y Update agregar dos validaciones, cada una en su if:
nombre de más de 50 caracteres y precio mayor a 1000000.
TurnoService: en GetAllAsync y GetByIdAsync completar IdCancha, IdCliente,
NombreCancha, NombreCliente y PrecioTotal buscando la cancha y el cliente con sus
repositories. Si alguno no se encuentra, dejar el nombre vacío y PrecioTotal en 0.





Prompt 6: 

Respetando la misma consigna (solo lo del parcial, validaciones de campos en el controller, el resto en el service, manejo de errores del parcial), hacé estos cambios en cancha:
- GetResponseCancha debe devolver también PrecioXhora.
- PostRequestCancha: sacale el Id y agregale Activa (bool, valor por defecto true). Usá esa propiedad al crear y al modificar la cancha (hoy al crear se fuerza true y al modificar no se cambia).
- En el controller agregá, cada una en su if: nombre de máximo 50 caracteres y precio por hora máximo de 1000000.





prompt 7:

Mismo criterio y consigna. Al cliente le faltan campos: agregá Dni, Telefono y Email a GetResponseCliente y PostRequestCliente (sacale el Id al PostRequest), y hacé que el service los guarde y los mapee en crear, modificar y listar.
Validaciones del controller, cada una en su if:
- nombre y apellido obligatorios, máximo 50 caracteres
- DNI obligatorio, solo números, 7 u 8 dígitos
- teléfono obligatorio, solo números, de 8 a 15 dígitos
  Validaciones del service: DNI único y email único, tanto al crear como al modificar (al modificar, ignorando al propio cliente).
  Para eso podés agregar al repository los métodos ExisteDni y ExisteEmail.


Prompt8:

Creá una carpeta frontend (en la raíz del repositorio, fuera del proyecto de la API) con
HTML, CSS y JavaScript vainilla, sin frameworks ni librerías. Estructura:
frontend/
  index.html       (menú con links a las 3 pantallas)
  canchas.html, clientes.html, turnos.html
  css/estilos.css
  js/comun.js, js/canchas.js, js/clientes.js, js/turnos.js
En este prompt hacé index.html, estilos.css, comun.js, canchas.html y canchas.js.
Dejá clientes y turnos para los siguientes prompts.
Reglas de estilo del JavaScript:
- Una instrucción por línea, variables intermedias, funciones async/await con
  try/catch. Sin librerías, sin módulos ES (cargá los scripts con <script> en orden:
  comun.js primero).
- Una función por acción: cargarCanchas, guardarCancha, editarCancha, eliminarCancha,
  limpiarFormulario.
- En comun.js: const API_URL con la URL base que corresponda (leé launchSettings.json) y
  funciones reutilizables: mostrarMensaje(texto, esError) que muestra el mensaje en un
  div de la pantalla (NO usar alert, salvo confirm() antes de eliminar) y
  leerRespuestaTexto si hace falta.
- Las filas de la tabla se arman con document.createElement y textContent (no innerHTML
  con datos de la API).
Comportamiento de la API (importante):
- Los GET devuelven JSON.
- POST, PUT y DELETE devuelven texto plano, tanto si salió bien como si hay error
  (400, 404, 500). Mostrá ese texto al usuario.
Pantalla de Canchas: tabla con Nombre, Tipo, Precio por hora, Activa y botones Editar y
Eliminar; formulario de alta y edición (el mismo formulario; al editar carga los datos y
cambia el botón a "Guardar cambios" con un botón Cancelar); casilla Activa. Validaciones
en el frontend antes de enviar: nombre obligatorio y de hasta 50 caracteres, tipo
obligatorio, precio mayor a 0. Menú de navegación igual en todas las pantallas. Diseño
simple, prolijo y responsive con CSS propio.





Prompt 9:"

Configurá CORS en Program.cs para que el frontend (HTML/JS abierto con Live Server de
VS Code, u otro origen) pueda consumir la API. Usá una política simple que permita
cualquier origen, cualquier header y cualquier método (es un TP). Agregá el
builder.Services.AddCors(...) y app.UseCors(...) en el lugar correcto del pipeline.
Explicame qué hace cada línea y por qué importa el orden.
Además:
1. Leé Properties/launchSettings.json e indicame la URL base exacta que debe usar el
   frontend (puerto y http/https).
2. Decime si app.UseHttpsRedirection() puede romper los fetch desde el frontend y cómo
   evitarlo en desarrollo con el menor cambio posible.
3. Sacá los using que no se usan en Program.cs.





Prompt 10: 

Hacé clientes.html y clientes.js con el mismo patrón, estilo y reglas que canchas
(comun.js, mostrarMensaje, filas con createElement, misma estructura de funciones:
cargarClientes, guardarCliente, editarCliente, eliminarCliente, limpiarFormulario).
Campos: Nombre, Apellido, DNI, Teléfono, Email. Validaciones en el frontend antes de
enviar, espejo de las del backend: nombre y apellido obligatorios (máx. 50), DNI solo
números de 7 u 8 dígitos, teléfono solo números de 8 a 15 dígitos, email con formato
válido. Mostrá al usuario los mensajes que devuelve la API (por ejemplo DNI duplicado o
cliente con turnos asociados).




Prompt 11:

Hacé turnos.html y turnos.js con el mismo patrón y reglas que las otras pantallas.
- Tabla de turnos con Fecha, Hora inicio, Hora fin, Cancha (NombreCancha), Cliente
  (NombreCliente), Precio total y botones Editar y Eliminar.
- Formulario con: select de cancha (cargado desde la API, solo canchas activas), select
  de cliente (cargado desde la API, mostrando "Apellido, Nombre - DNI"), input de fecha
  y selects de hora de inicio y de fin (de 08:00 a 23:00, solo horas exactas).
- Botón "Ver horarios libres": con la cancha y la fecha elegidas llama a
  GET /Turnos/disponibilidad y muestra los bloques libres como lista debajo del
  formulario.
- Validaciones en el frontend: todos los campos obligatorios, hora fin mayor que hora
  inicio, fecha no pasada.
- Mostrar al usuario los mensajes que devuelve la API (superposición, cancha inactiva,
  turno en el pasado, etc.).
- El formato de fecha y hora que se envía debe coincidir con lo que espera el DTO de
  Turno de mi API (leélo). Explicame cómo armás y mostrás las fechas y horas.




Prompt 12:

Misma consigna: solo lo que está en el parcial, validaciones de campos en el controller, el resto en el service, manejo de errores del parcial. Necesito agregar una funcionalidad de lógica de negocio: consultar los horarios libres de una cancha en una fecha.
Endpoint: GET /Turnos/disponibilidad?idCancha=1&fecha=2026-11-25
TurnoRepository: agregá GetByCanchaYFecha(int idCancha, DateTime fecha) que traiga la lista de turnos de esa cancha en esa fecha (comparando .Date). Escribí las condiciones del Where en varias líneas.
TurnoService: agregá GetDisponibilidadAsync(int idCancha, DateTime fecha) que devuelva List<string>? con los bloques libres de 1 hora, con textos como "08:00 - 09:00", "09:00 - 10:00", hasta "22:00 - 23:00". Reglas:
- Si la cancha no existe, devolvé null.
- Si la cancha está inactiva, devolvé una lista vacía.
- Un bloque está ocupado si se superpone con algún turno existente (inicioBloque < turno.HoraFin && finBloque > turno.HoraInicio).
- Un bloque que ya pasó (fecha + inicio del bloque menor a DateTime.Now) no se considera libre.
  Usá for y foreach, con una instrucción por línea y variables intermedias.
TurnosController: agregá la acción con [HttpGet("disponibilidad")], con try/catch y ILogger como las demás. Validaciones, cada una en su if: idCancha mayor a 0 y fecha distinta de DateTime.MinValue (si no se envía, es obligatoria). Si el service devuelve null, respondé NotFound con "No se encontró una cancha con ese id.". Si no, Ok con la lista.
Explicame cómo recorrés los bloques, cómo detectás si están ocupados, y qué hacen TimeSpan.FromHours y ToString("00") si los usás.



Prompt 13:"

Misma consigna de estilo del JavaScript que usaste en turnos.js (una instrucción por línea, variables intermedias, async/await con try/catch, filas y elementos con createElement y textContent, mensajes con mostrarMensaje). Agregá a la pantalla de turnos un botón "Ver horarios libres".
- En turnos.html: un botón type="button" cerca de la fecha y, debajo del formulario, un contenedor donde se muestran los horarios (un título "Horarios libres" y una lista).
- En turnos.js: una función verHorariosLibres que lea la cancha y la fecha del formulario y valide, cada una en su if: que haya una cancha elegida, que haya una fecha y que no sea pasada. Después hace GET a API_URL + "/Turnos/disponibilidad?idCancha=...&fecha=..." y:
  - si la respuesta no es ok, muestra el texto del error con mostrarMensaje;
  - si la lista viene vacía, muestra "No hay horarios libres para esa cancha en esa fecha.";
  - si no, muestra cada bloque en un item de la lista.
- limpiarFormulario también tiene que ocultar y vaciar esa lista.
- No agregues nada más (por ejemplo, que al hacer clic en un horario se complete el formulario).
  Usá las clases de estilos.css que ya existen y, si hace falta algo mínimo, agregalo al final del archivo.



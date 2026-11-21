function obtenerFechaActual()
{
    var hoy = new Date();
    var anio = hoy.getFullYear();
    var mes = String(hoy.getMonth() + 1).padStart(2, "0");
    var dia = String(hoy.getDate()).padStart(2, "0");
    var fecha = anio + "-" + mes + "-" + dia;
    return fecha;
}

function formatearFecha(fecha)
{
    var fechaSinHora = fecha.substring(0, 10);
    var partes = fechaSinHora.split("-");
    var fechaMostrada = partes[2] + "/" + partes[1] + "/" + partes[0];
    return fechaMostrada;
}

function formatearHora(hora)
{
    var horaMostrada = hora.substring(0, 5);
    return horaMostrada;
}

function cargarHoras()
{
    var selectInicio = document.getElementById("hora-inicio");
    var selectFin = document.getElementById("hora-fin");

    for (var hora = 8; hora <= 23; hora++)
    {
        var horaTexto = String(hora).padStart(2, "0");
        var valor = horaTexto + ":00:00";
        var etiqueta = horaTexto + ":00";
        var opcionInicio = document.createElement("option");
        var opcionFin = document.createElement("option");
        opcionInicio.value = valor;
        opcionInicio.textContent = etiqueta;
        opcionFin.value = valor;
        opcionFin.textContent = etiqueta;
        selectInicio.appendChild(opcionInicio);
        selectFin.appendChild(opcionFin);
    }
}

async function cargarOpciones()
{
    try
    {
        var selectCancha = document.getElementById("id-cancha");
        var selectCliente = document.getElementById("id-cliente");
        var canchaSeleccionada = selectCancha.value;
        var clienteSeleccionado = selectCliente.value;
        var urlCanchas = API_URL + "/Cancha";
        var respuestaCanchas = await fetch(urlCanchas);

        if (respuestaCanchas.ok === false)
        {
            var errorCanchas = await leerRespuestaTexto(respuestaCanchas);
            return errorCanchas;
        }

        var canchas = await respuestaCanchas.json();
        var urlClientes = API_URL + "/Cliente";
        var respuestaClientes = await fetch(urlClientes);

        if (respuestaClientes.ok === false)
        {
            var errorClientes = await leerRespuestaTexto(respuestaClientes);
            return errorClientes;
        }

        var clientes = await respuestaClientes.json();
        selectCancha.textContent = "";
        selectCliente.textContent = "";

        var opcionCancha = document.createElement("option");
        opcionCancha.value = "";
        opcionCancha.textContent = "Elegí una cancha";
        selectCancha.appendChild(opcionCancha);

        var cantidadActivas = 0;

        for (var cancha of canchas)
        {
            if (cancha.activa)
            {
                var opcion = document.createElement("option");
                opcion.value = cancha.id;
                opcion.textContent = cancha.nombre;
                selectCancha.appendChild(opcion);
                cantidadActivas++;
            }
        }

        selectCancha.disabled = false;

        if (cantidadActivas === 0)
        {
            opcionCancha.textContent = "No hay canchas activas";
            selectCancha.disabled = true;
        }

        var opcionCliente = document.createElement("option");
        opcionCliente.value = "";
        opcionCliente.textContent = "Elegí un cliente";
        selectCliente.appendChild(opcionCliente);

        for (var cliente of clientes)
        {
            var opcion = document.createElement("option");
            opcion.value = cliente.id;
            opcion.textContent = cliente.apellido + ", " + cliente.nombre + " - " + cliente.dni;
            selectCliente.appendChild(opcion);
        }

        selectCliente.disabled = false;

        if (clientes.length === 0)
        {
            opcionCliente.textContent = "No hay clientes registrados";
            selectCliente.disabled = true;
        }

        selectCancha.value = canchaSeleccionada;
        selectCliente.value = clienteSeleccionado;
        return null;
    }
    catch (error)
    {
        return "No se pudieron cargar las canchas y los clientes. Verificá la conexión con la API y presioná Actualizar para reintentar.";
    }
}

async function cargarTurnos()
{
    var cuerpoTabla = document.getElementById("tabla-turnos");
    var estadoTabla = document.getElementById("estado-tabla");
    var botonActualizar = document.getElementById("boton-actualizar");

    try
    {
        botonActualizar.disabled = true;
        mostrarMensaje("", false);
        cuerpoTabla.textContent = "";
        estadoTabla.textContent = "Cargando turnos...";
        estadoTabla.hidden = false;

        var url = API_URL + "/Turnos";
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            estadoTabla.textContent = "No se pudieron cargar los turnos.";
            mostrarMensaje(mensajeError, true);
            return false;
        }

        var turnos = await respuesta.json();

        if (turnos.length === 0)
        {
            estadoTabla.textContent = "Todavía no hay turnos registrados. Creá el primero con el formulario.";
            return true;
        }

        for (var turno of turnos)
        {
            var fila = document.createElement("tr");
            var celdaFecha = document.createElement("td");
            var celdaInicio = document.createElement("td");
            var celdaFin = document.createElement("td");
            var celdaCancha = document.createElement("td");
            var celdaCliente = document.createElement("td");
            var celdaPrecio = document.createElement("td");
            var celdaAcciones = document.createElement("td");
            var acciones = document.createElement("div");
            var botonEditar = document.createElement("button");
            var botonEliminar = document.createElement("button");

            var fechaMostrada = formatearFecha(turno.fecha);
            var inicioMostrado = formatearHora(turno.horaInicio);
            var finMostrado = formatearHora(turno.horaFin);
            var precioFormateado = turno.precioTotal.toLocaleString("es-AR");
            celdaFecha.textContent = fechaMostrada;
            celdaInicio.textContent = inicioMostrado;
            celdaFin.textContent = finMostrado;
            celdaCancha.textContent = turno.nombreCancha;
            celdaCliente.textContent = turno.nombreCliente;
            celdaPrecio.textContent = "$ " + precioFormateado;

            var descripcion = turno.nombreCancha + " del " + fechaMostrada + " a las " + inicioMostrado;
            acciones.className = "acciones-tabla";
            botonEditar.type = "button";
            botonEditar.className = "boton boton-secundario";
            botonEditar.textContent = "Editar";
            botonEditar.setAttribute("aria-label", "Editar turno de " + descripcion);
            botonEditar.dataset.id = turno.id;
            botonEditar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                editarTurno(id);
            });

            botonEliminar.type = "button";
            botonEliminar.className = "boton boton-eliminar";
            botonEliminar.textContent = "Eliminar";
            botonEliminar.setAttribute("aria-label", "Eliminar turno de " + descripcion);
            botonEliminar.dataset.id = turno.id;
            botonEliminar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                eliminarTurno(id);
            });

            acciones.appendChild(botonEditar);
            acciones.appendChild(botonEliminar);
            celdaAcciones.appendChild(acciones);
            fila.appendChild(celdaFecha);
            fila.appendChild(celdaInicio);
            fila.appendChild(celdaFin);
            fila.appendChild(celdaCancha);
            fila.appendChild(celdaCliente);
            fila.appendChild(celdaPrecio);
            fila.appendChild(celdaAcciones);
            cuerpoTabla.appendChild(fila);
        }

        estadoTabla.hidden = true;
        return true;
    }
    catch (error)
    {
        estadoTabla.textContent = "No se pudieron cargar los turnos.";
        estadoTabla.hidden = false;
        mostrarMensaje("No se pudo conectar con la API. Verificá que esté iniciada y que permita el acceso desde este frontend.", true);
        return false;
    }
    finally
    {
        botonActualizar.disabled = false;
    }
}

async function guardarTurno(evento)
{
    evento.preventDefault();
    var botonGuardar = document.getElementById("boton-guardar");

    try
    {
        var id = document.getElementById("id-turno").value;
        var idCancha = Number(document.getElementById("id-cancha").value);
        var idCliente = Number(document.getElementById("id-cliente").value);
        var fecha = document.getElementById("fecha").value;
        var horaInicio = document.getElementById("hora-inicio").value;
        var horaFin = document.getElementById("hora-fin").value;

        if (idCancha <= 0)
        {
            mostrarMensaje("Debe elegir una cancha activa.", true);
            document.getElementById("id-cancha").focus();
            return;
        }

        if (idCliente <= 0)
        {
            mostrarMensaje("Debe elegir un cliente.", true);
            document.getElementById("id-cliente").focus();
            return;
        }

        if (fecha === "")
        {
            mostrarMensaje("La fecha del turno es obligatoria.", true);
            document.getElementById("fecha").focus();
            return;
        }

        var fechaActual = obtenerFechaActual();
        document.getElementById("fecha").min = fechaActual;

        if (fecha < fechaActual)
        {
            mostrarMensaje("La fecha del turno no puede estar en el pasado.", true);
            document.getElementById("fecha").focus();
            return;
        }

        if (horaInicio === "")
        {
            mostrarMensaje("Debe elegir la hora de inicio.", true);
            document.getElementById("hora-inicio").focus();
            return;
        }

        if (horaFin === "")
        {
            mostrarMensaje("Debe elegir la hora de fin.", true);
            document.getElementById("hora-fin").focus();
            return;
        }

        if (horaFin <= horaInicio)
        {
            mostrarMensaje("La hora de fin debe ser mayor que la hora de inicio.", true);
            document.getElementById("hora-fin").focus();
            return;
        }

        var fechaHoraInicio = new Date(fecha + "T" + horaInicio);
        var ahora = new Date();

        if (fechaHoraInicio < ahora)
        {
            mostrarMensaje("El turno no puede estar en el pasado.", true);
            document.getElementById("hora-inicio").focus();
            return;
        }

        var fechaParaApi = fecha + "T00:00:00";
        var turno =
        {
            fecha: fechaParaApi,
            horaInicio: horaInicio,
            horaFin: horaFin,
            idCancha: idCancha,
            idCliente: idCliente
        };

        var url = API_URL + "/Turnos";
        var metodo = "POST";

        if (id !== "")
        {
            url = url + "/" + id;
            metodo = "PUT";
        }

        botonGuardar.disabled = true;
        var cuerpo = JSON.stringify(turno);
        var respuesta = await fetch(url,
        {
            method: metodo,
            headers:
            {
                "Content-Type": "application/json"
            },
            body: cuerpo
        });

        var textoRespuesta = await leerRespuestaTexto(respuesta);

        if (respuesta.ok === false)
        {
            mostrarMensaje(textoRespuesta, true);
            return;
        }

        limpiarFormulario();
        var listadoActualizado = await cargarTurnos();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo guardar el turno. Verificá la conexión con la API antes de volver a intentar.", true);
    }
    finally
    {
        botonGuardar.disabled = false;
    }
}

async function editarTurno(id)
{
    try
    {
        var errorOpciones = await cargarOpciones();

        if (errorOpciones !== null)
        {
            mostrarMensaje(errorOpciones, true);
            return;
        }

        var url = API_URL + "/Turnos/" + id;
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            mostrarMensaje(mensajeError, true);
            return;
        }

        var turno = await respuesta.json();
        document.getElementById("id-turno").value = turno.id;
        document.getElementById("id-cancha").value = String(turno.idCancha);
        document.getElementById("id-cliente").value = String(turno.idCliente);
        document.getElementById("fecha").value = turno.fecha.substring(0, 10);
        document.getElementById("hora-inicio").value = turno.horaInicio.substring(0, 8);
        document.getElementById("hora-fin").value = turno.horaFin.substring(0, 8);
        document.getElementById("titulo-formulario").textContent = "Editar turno";
        document.getElementById("boton-guardar").textContent = "Guardar cambios";
        document.getElementById("boton-cancelar").hidden = false;

        var aviso = "";

        if (document.getElementById("id-cancha").value === "")
        {
            aviso = "La cancha del turno ya no está activa o no está disponible. Elegí otra cancha para guardar cambios.";
        }

        if (document.getElementById("id-cliente").value === "")
        {
            aviso = aviso + " El cliente del turno ya no está disponible. Elegí otro cliente para guardar cambios.";
        }

        mostrarMensaje(aviso.trim(), aviso !== "");
        document.getElementById("id-cancha").focus();
    }
    catch (error)
    {
        mostrarMensaje("No se pudo cargar el turno para editar. Verificá la conexión con la API.", true);
    }
}

async function eliminarTurno(id)
{
    try
    {
        var confirmado = confirm("¿Querés eliminar este turno?");

        if (confirmado === false)
        {
            return;
        }

        var url = API_URL + "/Turnos/" + id;
        var respuesta = await fetch(url,
        {
            method: "DELETE"
        });

        var textoRespuesta = await leerRespuestaTexto(respuesta);

        if (respuesta.ok === false)
        {
            mostrarMensaje(textoRespuesta, true);
            return;
        }

        var idEdicion = document.getElementById("id-turno").value;

        if (idEdicion === String(id))
        {
            limpiarFormulario();
        }

        var listadoActualizado = await cargarTurnos();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo eliminar el turno. Verificá la conexión con la API antes de volver a intentar.", true);
    }
}

async function verHorariosLibres()
{
    var botonHorarios = document.getElementById("boton-horarios-libres");
    var contenedorHorarios = document.getElementById("contenedor-horarios-libres");
    var listaHorarios = document.getElementById("lista-horarios-libres");

    try
    {
        contenedorHorarios.hidden = true;
        listaHorarios.textContent = "";
        mostrarMensaje("", false);

        var idCancha = Number(document.getElementById("id-cancha").value);
        var fecha = document.getElementById("fecha").value;

        if (idCancha <= 0)
        {
            mostrarMensaje("Debe elegir una cancha activa.", true);
            document.getElementById("id-cancha").focus();
            return;
        }

        if (fecha === "")
        {
            mostrarMensaje("La fecha es obligatoria.", true);
            document.getElementById("fecha").focus();
            return;
        }

        var fechaActual = obtenerFechaActual();

        if (fecha < fechaActual)
        {
            mostrarMensaje("La fecha no puede estar en el pasado.", true);
            document.getElementById("fecha").focus();
            return;
        }

        botonHorarios.disabled = true;
        var fechaConsulta = encodeURIComponent(fecha);
        var url = API_URL + "/Turnos/disponibilidad?idCancha=" + idCancha + "&fecha=" + fechaConsulta;
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            mostrarMensaje(mensajeError, true);
            return;
        }

        var horarios = await respuesta.json();

        if (horarios.length === 0)
        {
            mostrarMensaje("No hay horarios libres para esa cancha en esa fecha.", false);
            return;
        }

        for (var horario of horarios)
        {
            var item = document.createElement("li");
            item.textContent = horario;
            listaHorarios.appendChild(item);
        }

        contenedorHorarios.hidden = false;
    }
    catch (error)
    {
        mostrarMensaje("No se pudieron consultar los horarios libres. Verificá la conexión con la API.", true);
    }
    finally
    {
        botonHorarios.disabled = false;
    }
}

function limpiarFormulario()
{
    document.getElementById("formulario-turno").reset();
    document.getElementById("id-turno").value = "";
    document.getElementById("fecha").min = obtenerFechaActual();
    document.getElementById("titulo-formulario").textContent = "Nuevo turno";
    document.getElementById("boton-guardar").textContent = "Crear turno";
    document.getElementById("boton-cancelar").hidden = true;
    document.getElementById("contenedor-horarios-libres").hidden = true;
    document.getElementById("lista-horarios-libres").textContent = "";
    mostrarMensaje("", false);
}

async function iniciarPantalla()
{
    var botonActualizar = document.getElementById("boton-actualizar");

    try
    {
        botonActualizar.disabled = true;
        document.getElementById("fecha").min = obtenerFechaActual();
        var errorOpciones = await cargarOpciones();
        await cargarTurnos();

        if (errorOpciones !== null)
        {
            mostrarMensaje(errorOpciones, true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo cargar la pantalla de turnos. Presioná Actualizar para reintentar.", true);
    }
    finally
    {
        botonActualizar.disabled = false;
    }
}

document.getElementById("formulario-turno").addEventListener("submit", guardarTurno);
document.getElementById("boton-cancelar").addEventListener("click", limpiarFormulario);
document.getElementById("boton-actualizar").addEventListener("click", iniciarPantalla);
document.getElementById("boton-horarios-libres").addEventListener("click", verHorariosLibres);
cargarHoras();
iniciarPantalla();

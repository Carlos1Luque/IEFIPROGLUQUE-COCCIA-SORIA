async function cargarCanchas()
{
    var cuerpoTabla = document.getElementById("tabla-canchas");
    var estadoTabla = document.getElementById("estado-tabla");
    var botonActualizar = document.getElementById("boton-actualizar");

    try
    {
        botonActualizar.disabled = true;
        mostrarMensaje("", false);
        cuerpoTabla.textContent = "";
        estadoTabla.textContent = "Cargando canchas...";
        estadoTabla.hidden = false;

        var url = API_URL + "/Cancha";
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            estadoTabla.textContent = "No se pudieron cargar las canchas.";
            mostrarMensaje(mensajeError, true);
            return false;
        }

        var canchas = await respuesta.json();

        if (canchas.length === 0)
        {
            estadoTabla.textContent = "Todavía no hay canchas registradas. Creá la primera con el formulario.";
            return true;
        }

        for (var cancha of canchas)
        {
            var fila = document.createElement("tr");
            var celdaNombre = document.createElement("td");
            var celdaTipo = document.createElement("td");
            var celdaPrecio = document.createElement("td");
            var celdaActiva = document.createElement("td");
            var celdaAcciones = document.createElement("td");
            var estado = document.createElement("span");
            var acciones = document.createElement("div");
            var botonEditar = document.createElement("button");
            var botonEliminar = document.createElement("button");

            celdaNombre.textContent = cancha.nombre;
            celdaTipo.textContent = cancha.tipoCancha;
            var precioFormateado = cancha.precioXhora.toLocaleString("es-AR");
            celdaPrecio.textContent = "$ " + precioFormateado;

            estado.className = "estado";
            estado.textContent = "No";

            if (cancha.activa)
            {
                estado.className = "estado estado-activa";
                estado.textContent = "Sí";
            }

            celdaActiva.appendChild(estado);
            acciones.className = "acciones-tabla";
            botonEditar.type = "button";
            botonEditar.className = "boton boton-secundario";
            botonEditar.textContent = "Editar";
            botonEditar.setAttribute("aria-label", "Editar " + cancha.nombre);
            botonEditar.dataset.id = cancha.id;
            botonEditar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                editarCancha(id);
            });

            botonEliminar.type = "button";
            botonEliminar.className = "boton boton-eliminar";
            botonEliminar.textContent = "Eliminar";
            botonEliminar.setAttribute("aria-label", "Eliminar " + cancha.nombre);
            botonEliminar.dataset.id = cancha.id;
            botonEliminar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                eliminarCancha(id);
            });

            acciones.appendChild(botonEditar);
            acciones.appendChild(botonEliminar);
            celdaAcciones.appendChild(acciones);
            fila.appendChild(celdaNombre);
            fila.appendChild(celdaTipo);
            fila.appendChild(celdaPrecio);
            fila.appendChild(celdaActiva);
            fila.appendChild(celdaAcciones);
            cuerpoTabla.appendChild(fila);
        }

        estadoTabla.hidden = true;
        return true;
    }
    catch (error)
    {
        estadoTabla.textContent = "No se pudieron cargar las canchas.";
        estadoTabla.hidden = false;
        mostrarMensaje("No se pudo conectar con la API. Verificá que esté iniciada y que permita el acceso desde este frontend.", true);
        return false;
    }
    finally
    {
        botonActualizar.disabled = false;
    }
}

async function guardarCancha(evento)
{
    evento.preventDefault();
    var botonGuardar = document.getElementById("boton-guardar");

    try
    {
        var id = document.getElementById("id-cancha").value;
        var nombre = document.getElementById("nombre").value.trim();
        var tipoCancha = document.getElementById("tipo-cancha").value.trim();
        var precioIngresado = document.getElementById("precio").value;
        var precioXhora = Number(precioIngresado);
        var activa = document.getElementById("activa").checked;

        if (nombre === "")
        {
            mostrarMensaje("El nombre de la cancha es obligatorio.", true);
            document.getElementById("nombre").focus();
            return;
        }

        if (nombre.length > 50)
        {
            mostrarMensaje("El nombre de la cancha no puede superar los 50 caracteres.", true);
            document.getElementById("nombre").focus();
            return;
        }

        if (tipoCancha === "")
        {
            mostrarMensaje("El tipo de cancha es obligatorio.", true);
            document.getElementById("tipo-cancha").focus();
            return;
        }

        if (Number.isFinite(precioXhora) === false)
        {
            mostrarMensaje("El precio por hora debe ser un número válido.", true);
            document.getElementById("precio").focus();
            return;
        }

        if (precioXhora <= 0)
        {
            mostrarMensaje("El precio por hora debe ser mayor que cero.", true);
            document.getElementById("precio").focus();
            return;
        }

        if (Number.isInteger(precioXhora) === false)
        {
            mostrarMensaje("El precio por hora debe ser un número entero.", true);
            document.getElementById("precio").focus();
            return;
        }

        var cancha =
        {
            nombre: nombre,
            tipoCancha: tipoCancha,
            precioXhora: precioXhora,
            activa: activa
        };

        var url = API_URL + "/Cancha";
        var metodo = "POST";

        if (id !== "")
        {
            url = url + "/" + id;
            metodo = "PUT";
        }

        botonGuardar.disabled = true;
        var cuerpo = JSON.stringify(cancha);
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
        var listadoActualizado = await cargarCanchas();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo guardar la cancha. Verificá la conexión con la API antes de volver a intentar.", true);
    }
    finally
    {
        botonGuardar.disabled = false;
    }
}

async function editarCancha(id)
{
    try
    {
        var url = API_URL + "/Cancha/" + id;
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            mostrarMensaje(mensajeError, true);
            return;
        }

        var cancha = await respuesta.json();
        document.getElementById("id-cancha").value = cancha.id;
        document.getElementById("nombre").value = cancha.nombre;
        document.getElementById("tipo-cancha").value = cancha.tipoCancha;
        document.getElementById("precio").value = cancha.precioXhora;
        document.getElementById("activa").checked = cancha.activa;
        document.getElementById("titulo-formulario").textContent = "Editar cancha";
        document.getElementById("boton-guardar").textContent = "Guardar cambios";
        document.getElementById("boton-cancelar").hidden = false;
        mostrarMensaje("", false);
        document.getElementById("nombre").focus();
    }
    catch (error)
    {
        mostrarMensaje("No se pudo cargar la cancha para editar. Verificá la conexión con la API.", true);
    }
}

async function eliminarCancha(id)
{
    try
    {
        var confirmado = confirm("¿Querés eliminar esta cancha?");

        if (confirmado === false)
        {
            return;
        }

        var url = API_URL + "/Cancha/" + id;
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

        var idEdicion = document.getElementById("id-cancha").value;

        if (idEdicion === String(id))
        {
            limpiarFormulario();
        }

        var listadoActualizado = await cargarCanchas();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo eliminar la cancha. Verificá la conexión con la API antes de volver a intentar.", true);
    }
}

function limpiarFormulario()
{
    document.getElementById("formulario-cancha").reset();
    document.getElementById("id-cancha").value = "";
    document.getElementById("activa").checked = true;
    document.getElementById("titulo-formulario").textContent = "Nueva cancha";
    document.getElementById("boton-guardar").textContent = "Crear cancha";
    document.getElementById("boton-cancelar").hidden = true;
    mostrarMensaje("", false);
}

document.getElementById("formulario-cancha").addEventListener("submit", guardarCancha);
document.getElementById("boton-cancelar").addEventListener("click", limpiarFormulario);
document.getElementById("boton-actualizar").addEventListener("click", cargarCanchas);
cargarCanchas();

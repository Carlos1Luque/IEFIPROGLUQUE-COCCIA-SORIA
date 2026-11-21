async function cargarClientes()
{
    var cuerpoTabla = document.getElementById("tabla-clientes");
    var estadoTabla = document.getElementById("estado-tabla");
    var botonActualizar = document.getElementById("boton-actualizar");

    try
    {
        botonActualizar.disabled = true;
        mostrarMensaje("", false);
        cuerpoTabla.textContent = "";
        estadoTabla.textContent = "Cargando clientes...";
        estadoTabla.hidden = false;

        var url = API_URL + "/Cliente";
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            estadoTabla.textContent = "No se pudieron cargar los clientes.";
            mostrarMensaje(mensajeError, true);
            return false;
        }

        var clientes = await respuesta.json();

        if (clientes.length === 0)
        {
            estadoTabla.textContent = "Todavía no hay clientes registrados. Creá el primero con el formulario.";
            return true;
        }

        for (var cliente of clientes)
        {
            var fila = document.createElement("tr");
            var celdaNombre = document.createElement("td");
            var celdaApellido = document.createElement("td");
            var celdaDni = document.createElement("td");
            var celdaTelefono = document.createElement("td");
            var celdaEmail = document.createElement("td");
            var celdaAcciones = document.createElement("td");
            var acciones = document.createElement("div");
            var botonEditar = document.createElement("button");
            var botonEliminar = document.createElement("button");

            celdaNombre.textContent = cliente.nombre;
            celdaApellido.textContent = cliente.apellido;
            celdaDni.textContent = cliente.dni;
            celdaTelefono.textContent = cliente.telefono;
            celdaEmail.textContent = cliente.email;

            var nombreCompleto = cliente.nombre + " " + cliente.apellido;
            acciones.className = "acciones-tabla";
            botonEditar.type = "button";
            botonEditar.className = "boton boton-secundario";
            botonEditar.textContent = "Editar";
            botonEditar.setAttribute("aria-label", "Editar " + nombreCompleto);
            botonEditar.dataset.id = cliente.id;
            botonEditar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                editarCliente(id);
            });

            botonEliminar.type = "button";
            botonEliminar.className = "boton boton-eliminar";
            botonEliminar.textContent = "Eliminar";
            botonEliminar.setAttribute("aria-label", "Eliminar " + nombreCompleto);
            botonEliminar.dataset.id = cliente.id;
            botonEliminar.addEventListener("click", function (evento)
            {
                var id = Number(evento.currentTarget.dataset.id);
                eliminarCliente(id);
            });

            acciones.appendChild(botonEditar);
            acciones.appendChild(botonEliminar);
            celdaAcciones.appendChild(acciones);
            fila.appendChild(celdaNombre);
            fila.appendChild(celdaApellido);
            fila.appendChild(celdaDni);
            fila.appendChild(celdaTelefono);
            fila.appendChild(celdaEmail);
            fila.appendChild(celdaAcciones);
            cuerpoTabla.appendChild(fila);
        }

        estadoTabla.hidden = true;
        return true;
    }
    catch (error)
    {
        estadoTabla.textContent = "No se pudieron cargar los clientes.";
        estadoTabla.hidden = false;
        mostrarMensaje("No se pudo conectar con la API. Verificá que esté iniciada y que permita el acceso desde este frontend.", true);
        return false;
    }
    finally
    {
        botonActualizar.disabled = false;
    }
}

async function guardarCliente(evento)
{
    evento.preventDefault();
    var botonGuardar = document.getElementById("boton-guardar");

    try
    {
        var id = document.getElementById("id-cliente").value;
        var nombre = document.getElementById("nombre").value;
        var apellido = document.getElementById("apellido").value;
        var dni = document.getElementById("dni").value;
        var telefono = document.getElementById("telefono").value;
        var email = document.getElementById("email").value;

        if (nombre.trim() === "")
        {
            mostrarMensaje("El nombre del cliente es obligatorio.", true);
            document.getElementById("nombre").focus();
            return;
        }

        if (nombre.length > 50)
        {
            mostrarMensaje("El nombre del cliente no puede superar los 50 caracteres.", true);
            document.getElementById("nombre").focus();
            return;
        }

        if (apellido.trim() === "")
        {
            mostrarMensaje("El apellido del cliente es obligatorio.", true);
            document.getElementById("apellido").focus();
            return;
        }

        if (apellido.length > 50)
        {
            mostrarMensaje("El apellido del cliente no puede superar los 50 caracteres.", true);
            document.getElementById("apellido").focus();
            return;
        }

        if (dni.trim() === "")
        {
            mostrarMensaje("El DNI del cliente es obligatorio.", true);
            document.getElementById("dni").focus();
            return;
        }

        if (dni.length < 7)
        {
            mostrarMensaje("El DNI debe tener al menos 7 dígitos.", true);
            document.getElementById("dni").focus();
            return;
        }

        if (dni.length > 8)
        {
            mostrarMensaje("El DNI no puede superar los 8 dígitos.", true);
            document.getElementById("dni").focus();
            return;
        }

        for (var caracter of dni)
        {
            if (caracter < "0")
            {
                mostrarMensaje("El DNI debe contener solo números.", true);
                document.getElementById("dni").focus();
                return;
            }

            if (caracter > "9")
            {
                mostrarMensaje("El DNI debe contener solo números.", true);
                document.getElementById("dni").focus();
                return;
            }
        }

        if (telefono.trim() === "")
        {
            mostrarMensaje("El teléfono del cliente es obligatorio.", true);
            document.getElementById("telefono").focus();
            return;
        }

        if (telefono.length < 8)
        {
            mostrarMensaje("El teléfono debe tener al menos 8 dígitos.", true);
            document.getElementById("telefono").focus();
            return;
        }

        if (telefono.length > 15)
        {
            mostrarMensaje("El teléfono no puede superar los 15 dígitos.", true);
            document.getElementById("telefono").focus();
            return;
        }

        for (var caracter of telefono)
        {
            if (caracter < "0")
            {
                mostrarMensaje("El teléfono debe contener solo números.", true);
                document.getElementById("telefono").focus();
                return;
            }

            if (caracter > "9")
            {
                mostrarMensaje("El teléfono debe contener solo números.", true);
                document.getElementById("telefono").focus();
                return;
            }
        }

        if (email.trim() === "")
        {
            mostrarMensaje("El email del cliente es obligatorio.", true);
            document.getElementById("email").focus();
            return;
        }

        var cantidadArrobas = 0;

        for (var caracter of email)
        {
            if (caracter.trim() === "")
            {
                mostrarMensaje("El email no puede contener espacios.", true);
                document.getElementById("email").focus();
                return;
            }

            if (caracter === "@")
            {
                cantidadArrobas++;
            }
        }

        if (cantidadArrobas !== 1)
        {
            mostrarMensaje("El email debe contener una sola @.", true);
            document.getElementById("email").focus();
            return;
        }

        var posicionArroba = email.indexOf("@");

        if (posicionArroba === 0)
        {
            mostrarMensaje("El email debe tener un nombre antes de la @.", true);
            document.getElementById("email").focus();
            return;
        }

        var posicionPunto = email.indexOf(".", posicionArroba + 1);

        if (posicionPunto === -1)
        {
            mostrarMensaje("El email debe contener un punto después de la @.", true);
            document.getElementById("email").focus();
            return;
        }

        if (posicionPunto === posicionArroba + 1)
        {
            mostrarMensaje("El email debe tener un dominio entre la @ y el punto.", true);
            document.getElementById("email").focus();
            return;
        }

        if (posicionPunto === email.length - 1)
        {
            mostrarMensaje("El email debe tener una terminación después del punto.", true);
            document.getElementById("email").focus();
            return;
        }

        var cliente =
        {
            nombre: nombre,
            apellido: apellido,
            dni: dni,
            telefono: telefono,
            email: email
        };

        var url = API_URL + "/Cliente";
        var metodo = "POST";

        if (id !== "")
        {
            url = url + "/" + id;
            metodo = "PUT";
        }

        botonGuardar.disabled = true;
        var cuerpo = JSON.stringify(cliente);
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
        var listadoActualizado = await cargarClientes();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo guardar el cliente. Verificá la conexión con la API antes de volver a intentar.", true);
    }
    finally
    {
        botonGuardar.disabled = false;
    }
}

async function editarCliente(id)
{
    try
    {
        var url = API_URL + "/Cliente/" + id;
        var respuesta = await fetch(url);

        if (respuesta.ok === false)
        {
            var mensajeError = await leerRespuestaTexto(respuesta);
            mostrarMensaje(mensajeError, true);
            return;
        }

        var cliente = await respuesta.json();
        document.getElementById("id-cliente").value = cliente.id;
        document.getElementById("nombre").value = cliente.nombre;
        document.getElementById("apellido").value = cliente.apellido;
        document.getElementById("dni").value = cliente.dni;
        document.getElementById("telefono").value = cliente.telefono;
        document.getElementById("email").value = cliente.email;
        document.getElementById("titulo-formulario").textContent = "Editar cliente";
        document.getElementById("boton-guardar").textContent = "Guardar cambios";
        document.getElementById("boton-cancelar").hidden = false;
        mostrarMensaje("", false);
        document.getElementById("nombre").focus();
    }
    catch (error)
    {
        mostrarMensaje("No se pudo cargar el cliente para editar. Verificá la conexión con la API.", true);
    }
}

async function eliminarCliente(id)
{
    try
    {
        var confirmado = confirm("¿Querés eliminar este cliente?");

        if (confirmado === false)
        {
            return;
        }

        var url = API_URL + "/Cliente/" + id;
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

        var idEdicion = document.getElementById("id-cliente").value;

        if (idEdicion === String(id))
        {
            limpiarFormulario();
        }

        var listadoActualizado = await cargarClientes();
        mostrarMensaje(textoRespuesta, false);

        if (listadoActualizado === false)
        {
            mostrarMensaje(textoRespuesta + " No se pudo actualizar el listado. Presioná Actualizar para reintentar.", true);
        }
    }
    catch (error)
    {
        mostrarMensaje("No se pudo eliminar el cliente. Verificá la conexión con la API antes de volver a intentar.", true);
    }
}

function limpiarFormulario()
{
    document.getElementById("formulario-cliente").reset();
    document.getElementById("id-cliente").value = "";
    document.getElementById("titulo-formulario").textContent = "Nuevo cliente";
    document.getElementById("boton-guardar").textContent = "Crear cliente";
    document.getElementById("boton-cancelar").hidden = true;
    mostrarMensaje("", false);
}

document.getElementById("formulario-cliente").addEventListener("submit", guardarCliente);
document.getElementById("boton-cancelar").addEventListener("click", limpiarFormulario);
document.getElementById("boton-actualizar").addEventListener("click", cargarClientes);
cargarClientes();

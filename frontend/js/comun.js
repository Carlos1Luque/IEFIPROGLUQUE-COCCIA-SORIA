const API_URL = "https://localhost:7057";

function mostrarMensaje(texto, esError)
{
    var mensaje = document.getElementById("mensaje");
    mensaje.textContent = texto;
    mensaje.hidden = false;

    if (esError)
    {
        mensaje.className = "mensaje mensaje-error";
        mensaje.setAttribute("role", "alert");
    }
    else
    {
        mensaje.className = "mensaje mensaje-exito";
        mensaje.setAttribute("role", "status");
    }

    if (texto === "")
    {
        mensaje.hidden = true;
    }
}

async function leerRespuestaTexto(respuesta)
{
    try
    {
        var texto = await respuesta.text();
        return texto;
    }
    catch (error)
    {
        throw new Error("No se pudo leer la respuesta de la API.");
    }
}

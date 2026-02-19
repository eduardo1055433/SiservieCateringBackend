using Microsoft.AspNetCore.Mvc;
using SiservieCatering.API.Models.Catalogo;
using Microsoft.AspNetCore.Authorization;

namespace SiservieCatering.API.Controllers.Catalogo;

[Authorize]
[ApiController]
[Route("api/catalogo")]
public class CatalogoController : ControllerBase
{
    /// <summary>
    /// Obtiene el catálogo de Tipos de Cliente.
    /// </summary>
    /// <returns>Lista de tipos de cliente (Label/Value).</returns>
    [HttpGet("tipo-cliente")]
    public ActionResult<IEnumerable<CatalogoItem>> GetTipoCliente()
    {
        var lista = new List<CatalogoItem>
        {
            new CatalogoItem("TODOS", "TODOS"),
            new CatalogoItem("TRABAJADORES REGISTRADOS", "TRAB"),
            new CatalogoItem("VISITA", "Visita") 
        };

        return Ok(lista);
    }
}

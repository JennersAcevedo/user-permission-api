using Microsoft.AspNetCore.Mvc;
using MediatR;
using userPermissionApi.CQRS.commands;
using userPermissionApi.CQRS.Queries;

namespace userPermissionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisoController : ControllerBase
    {
        public readonly IMediator _mediator;

        public PermisoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // CREAR NUEVO PERMISO
         [HttpPost]
        public async Task<IActionResult> CrearPermiso([FromBody] CreatePermisoCommand command)
        {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObtenerPermiso), new { id = result.id }, result);
        }

        // OBTENER PERMISO BASADO EN EL ID
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPermiso(int id)
        {
            var query = new GetPermisoByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        // OBTENER TODOS LOS PERMISOS
        [HttpGet]
        public async Task<IActionResult> ObtenerTodosLosPermisos()
        {
            var query = new GetAllPermisosQuery();
            var result = await _mediator.Send(query);

            return result != null ? Ok(result) : NotFound();
        }

        // EDITAR PERMISO
        [HttpPut("{id}")]
        public async Task<IActionResult> EditarPermiso(int id, [FromBody] UpdatePermisoCommand command)
        {
            if (id != command.id)
            {
                return BadRequest("El ID en la URL y en el cuerpo deben coincidir.");
            }

            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

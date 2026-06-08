using LinhaExpressa.Application.Frota;
using LinhaExpressa.Domain.Frota;
using Microsoft.AspNetCore.Mvc;

namespace LinhaExpressa.API.Controllers;

[ApiController]
[Route("onibus")]
[Produces("application/json")]
public class OnibusController : ControllerBase
{
    private readonly IOnibusService _service;
    public OnibusController(IOnibusService service) => _service = service;

    /// <summary>Cria um novo ônibus.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(OnibusDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OnibusDto>> Create([FromBody] CreateOnibusRequest request, CancellationToken ct)
    {
        try
        {
            var dto = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }

    /// <summary>Lista ônibus com filtros opcionais e paginação.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<OnibusDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<OnibusDto>>> List(
        [FromQuery] StatusOnibus? status,
        [FromQuery] string? prefixo,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await _service.ListAsync(new OnibusFiltro(status, prefixo, page, pageSize), ct);
        return Ok(result);
    }

    /// <summary>Obtém um ônibus pelo Id.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OnibusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OnibusDto>> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _service.GetByIdAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>Atualiza dados cadastrais de um ônibus.</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OnibusDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OnibusDto>> Update(Guid id, [FromBody] UpdateOnibusRequest request, CancellationToken ct)
    {
        try
        {
            var dto = await _service.UpdateAsync(id, request, ct);
            return Ok(dto);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { error = ex.Message }); }
    }

    /// <summary>Soft delete: marca o ônibus como Inativo preservando o histórico.</summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        try
        {
            await _service.SoftDeleteAsync(id, ct);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { error = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { error = ex.Message }); }
    }
}

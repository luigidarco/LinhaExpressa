using LinhaExpressa.Application.Frota;

namespace LinhaExpressa.Application.Frota;

public interface IOnibusService
{
    Task<OnibusDto> CreateAsync(CreateOnibusRequest request, CancellationToken ct = default);
    Task<PagedResult<OnibusDto>> ListAsync(OnibusFiltro filtro, CancellationToken ct = default);
    Task<OnibusDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<OnibusDto> UpdateAsync(Guid id, UpdateOnibusRequest request, CancellationToken ct = default);
    Task SoftDeleteAsync(Guid id, CancellationToken ct = default);
}

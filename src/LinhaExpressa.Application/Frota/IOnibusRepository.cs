using LinhaExpressa.Application.Frota;
using LinhaExpressa.Domain.Frota;
using LinhaExpressa.Domain.Shared.ValueObjects;

namespace LinhaExpressa.Application.Frota;

public interface IOnibusRepository
{
    Task<Onibus?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Onibus?> GetByPrefixoAsync(string prefixo, CancellationToken ct = default);
    Task<Onibus?> GetByPlacaAsync(string placa, CancellationToken ct = default);
    Task<(IReadOnlyList<Onibus> Items, int Total)> ListAsync(
        StatusOnibus? status, string? prefixo, int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(Onibus onibus, CancellationToken ct = default);
    void Update(Onibus onibus);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

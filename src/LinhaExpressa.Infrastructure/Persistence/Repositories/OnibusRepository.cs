using LinhaExpressa.Application.Frota;
using LinhaExpressa.Domain.Frota;
using Microsoft.EntityFrameworkCore;

namespace LinhaExpressa.Infrastructure.Persistence.Repositories;

public class OnibusRepository : IOnibusRepository
{
    private readonly AppDbContext _db;

    public OnibusRepository(AppDbContext db) => _db = db;

    public Task<Onibus?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        _db.Onibus
           .Include(o => o.ManutencoesPreventivas)
           .Include(o => o.ManutencoesCorretivas)
           .Include(o => o.Licenciamentos)
           .Include(o => o.Pneus)
           .FirstOrDefaultAsync(o => o.Id == id, ct);

    public Task<Onibus?> GetByPrefixoAsync(string prefixo, CancellationToken ct = default) =>
        _db.Onibus.AsNoTracking().FirstOrDefaultAsync(o => o.Prefixo == prefixo.Trim().ToUpper(), ct);

    public Task<Onibus?> GetByPlacaAsync(string placa, CancellationToken ct = default) =>
        _db.Onibus.AsNoTracking().FirstOrDefaultAsync(o => o.Placa == placa.Trim().ToUpper(), ct);

    public async Task<(IReadOnlyList<Onibus> Items, int Total)> ListAsync(
        StatusOnibus? status, string? prefixo, int page, int pageSize, CancellationToken ct = default)
    {
        var q = _db.Onibus.AsNoTracking().AsQueryable();

        if (status.HasValue) q = q.Where(o => o.Status == status.Value);
        if (!string.IsNullOrWhiteSpace(prefixo))
            q = q.Where(o => o.Prefixo.Contains(prefixo.Trim().ToUpper()));

        var total = await q.CountAsync(ct);
        var items = await q
            .OrderBy(o => o.Prefixo)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        return (items, total);
    }

    public async Task AddAsync(Onibus onibus, CancellationToken ct = default) =>
        await _db.Onibus.AddAsync(onibus, ct);

    public void Update(Onibus onibus)
    {
        _db.Onibus.Update(onibus);
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}

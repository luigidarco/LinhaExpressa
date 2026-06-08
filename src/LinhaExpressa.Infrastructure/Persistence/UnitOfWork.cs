using LinhaExpressa.Application.Frota;
using LinhaExpressa.Domain.Frota;
using Microsoft.EntityFrameworkCore;

namespace LinhaExpressa.Infrastructure.Persistence;

/// <summary>Implementação de UnitOfWork sobre o AppDbContext.</summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;
    public UnitOfWork(AppDbContext db) => _db = db;
    public Task<int> SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}

/// <summary>Implementação de IConfiguracaoGlobalProvider.</summary>
public class ConfiguracaoGlobalProvider : IConfiguracaoGlobalProvider
{
    private readonly AppDbContext _db;
    public ConfiguracaoGlobalProvider(AppDbContext db) => _db = db;

    public async Task<string> GetValorAsync(string chave, CancellationToken ct = default)
    {
        var cfg = await _db.ConfiguracoesGlobais
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Chave == chave, ct);
        return cfg?.Valor ?? string.Empty;
    }
}

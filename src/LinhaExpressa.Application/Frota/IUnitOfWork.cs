namespace LinhaExpressa.Application.Frota;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

public interface IConfiguracaoGlobalProvider
{
    Task<string> GetValorAsync(string chave, CancellationToken ct = default);
}

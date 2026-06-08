namespace LinhaExpressa.Domain.Shared;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CriadoEm { get; protected set; } = DateTime.UtcNow;
    public DateTime AtualizadoEm { get; protected set; } = DateTime.UtcNow;

    public void Touch() => AtualizadoEm = DateTime.UtcNow;
}

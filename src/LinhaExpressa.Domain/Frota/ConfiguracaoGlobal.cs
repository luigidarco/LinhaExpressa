using LinhaExpressa.Domain.Shared;

namespace LinhaExpressa.Domain.Frota;

public class ConfiguracaoGlobal : Entity
{
    public string Chave { get; private set; } = string.Empty;
    public string Valor { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }

    protected ConfiguracaoGlobal() { }

    public ConfiguracaoGlobal(string chave, string valor, string? descricao = null)
    {
        if (string.IsNullOrWhiteSpace(chave))
            throw new ArgumentException("Chave obrigatória.", nameof(chave));
        Chave = chave.Trim();
        Valor = valor?.Trim() ?? string.Empty;
        Descricao = descricao;
    }

    public void AtualizarValor(string valor)
    {
        Valor = valor?.Trim() ?? string.Empty;
        Touch();
    }
}

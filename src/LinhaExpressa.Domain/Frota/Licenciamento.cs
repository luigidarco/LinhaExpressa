using LinhaExpressa.Domain.Shared;

namespace LinhaExpressa.Domain.Frota;

public class Licenciamento : Entity
{
    public Guid OnibusId { get; private set; }
    public TipoLicenciamento Tipo { get; private set; }
    public DateTime Vencimento { get; private set; }
    public bool Alertado { get; private set; }

    public Onibus? Onibus { get; private set; }

    protected Licenciamento() { }

    public Licenciamento(Guid onibusId, TipoLicenciamento tipo, DateTime vencimento)
    {
        OnibusId = onibusId;
        Tipo = tipo;
        Vencimento = vencimento;
    }

    public void MarcarAlertado() { Alertado = true; Touch(); }
    public void Renovar(DateTime novoVencimento) { Vencimento = novoVencimento; Alertado = false; Touch(); }
}

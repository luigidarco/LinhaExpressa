using LinhaExpressa.Domain.Shared;

namespace LinhaExpressa.Domain.Frota;

public class ManutencaoPreventiva : Entity
{
    public Guid OnibusId { get; private set; }
    public string DescricaoServico { get; private set; } = string.Empty;
    public DateTime DataAgendada { get; private set; }
    public DateTime? DataRealizada { get; private set; }
    public int IntervaloDias { get; private set; }
    public int KmParaProxima { get; private set; }
    public StatusManutencaoPreventiva Status { get; private set; } = StatusManutencaoPreventiva.Agendada;

    public Onibus? Onibus { get; private set; }

    protected ManutencaoPreventiva() { }

    public ManutencaoPreventiva(
        Guid onibusId,
        string descricaoServico,
        DateTime dataAgendada,
        int intervaloDias,
        int kmParaProxima)
    {
        OnibusId = onibusId;
        SetDescricao(descricaoServico);
        SetDataAgendada(dataAgendada);
        SetIntervalo(intervaloDias);
        SetKmProxima(kmParaProxima);
    }

    public void Concluir(DateTime dataRealizada)
    {
        DataRealizada = dataRealizada;
        Status = StatusManutencaoPreventiva.Concluida;
        Touch();
    }

    private void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição obrigatória.");
        DescricaoServico = descricao.Trim();
    }
    private void SetDataAgendada(DateTime data) => DataAgendada = data;
    private void SetIntervalo(int dias)
    {
        if (dias <= 0) throw new ArgumentException("Intervalo de dias deve ser positivo.");
        IntervaloDias = dias;
    }
    private void SetKmProxima(int km)
    {
        if (km < 0) throw new ArgumentException("KmParaProxima não pode ser negativo.");
        KmParaProxima = km;
    }
}

using LinhaExpressa.Domain.Shared;

namespace LinhaExpressa.Domain.Frota;

public class ManutencaoCorretiva : Entity
{
    public Guid OnibusId { get; private set; }
    public Guid OcorrenciaId { get; private set; }
    public string DescricaoDefeito { get; private set; } = string.Empty;
    public DateTime DataAbertura { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public TipoRecolhimento TipoRecolhimento { get; private set; }
    public TipoOficina TipoOficina { get; private set; }
    public StatusManutencaoCorretiva Status { get; private set; } = StatusManutencaoCorretiva.Aberta;

    public Onibus? Onibus { get; private set; }

    protected ManutencaoCorretiva() { }

    public ManutencaoCorretiva(
        Guid onibusId,
        Guid ocorrenciaId,
        string descricaoDefeito,
        TipoRecolhimento tipoRecolhimento,
        TipoOficina tipoOficina)
    {
        OnibusId = onibusId;
        OcorrenciaId = ocorrenciaId;
        SetDescricao(descricaoDefeito);
        TipoRecolhimento = tipoRecolhimento;
        TipoOficina = tipoOficina;
        DataAbertura = DateTime.UtcNow;
    }

    public void Concluir(DateTime dataConclusao)
    {
        DataConclusao = dataConclusao;
        Status = StatusManutencaoCorretiva.Concluida;
        Touch();
    }

    public void IniciarReparo()
    {
        Status = StatusManutencaoCorretiva.EmReparo;
        Touch();
    }

    public void Cancelar()
    {
        Status = StatusManutencaoCorretiva.Cancelada;
        Touch();
    }

    private void SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição do defeito obrigatória.");
        DescricaoDefeito = descricao.Trim();
    }
}

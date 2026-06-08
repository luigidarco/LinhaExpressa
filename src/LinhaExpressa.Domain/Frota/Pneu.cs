using LinhaExpressa.Domain.Shared;

namespace LinhaExpressa.Domain.Frota;

public class Pneu : Entity
{
    public Guid OnibusId { get; private set; }
    public PosicaoPneu Posicao { get; private set; }
    public DateTime DataInstalacao { get; private set; }
    public int KmInstalacao { get; private set; }
    public int KmLimite { get; private set; }

    public Onibus? Onibus { get; private set; }

    protected Pneu() { }

    public Pneu(Guid onibusId, PosicaoPneu posicao, DateTime dataInstalacao, int kmInstalacao, int kmLimite)
    {
        OnibusId = onibusId;
        Posicao = posicao;
        DataInstalacao = dataInstalacao;
        KmInstalacao = kmInstalacao;
        if (kmLimite <= kmInstalacao)
            throw new ArgumentException("KmLimite deve ser maior que KmInstalacao.");
        KmLimite = kmLimite;
    }
}

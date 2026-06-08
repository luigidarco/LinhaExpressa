using LinhaExpressa.Domain.Shared;
using LinhaExpressa.Domain.Shared.ValueObjects;

namespace LinhaExpressa.Domain.Frota;

public class Onibus : Entity
{
    public string Prefixo { get; private set; } = string.Empty;
    public string Placa { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;
    public string Montadora { get; private set; } = string.Empty;
    public int AnoFabricacao { get; private set; }
    public string Combustivel { get; private set; } = string.Empty;
    public int CapacidadePassageiros { get; private set; }
    public StatusOnibus Status { get; private set; } = StatusOnibus.Ativo;
    public int KmAtual { get; private set; }
    public FonteKm FonteKm { get; private set; } = FonteKm.Odometro;
    public DateTime? DataUltimaAtualizacaoKm { get; private set; }

    // Navegação
    public ICollection<ManutencaoPreventiva> ManutencoesPreventivas { get; private set; } = new List<ManutencaoPreventiva>();
    public ICollection<ManutencaoCorretiva> ManutencoesCorretivas { get; private set; } = new List<ManutencaoCorretiva>();
    public ICollection<Licenciamento> Licenciamentos { get; private set; } = new List<Licenciamento>();
    public ICollection<Pneu> Pneus { get; private set; } = new List<Pneu>();

    protected Onibus() { }

    public Onibus(
        string prefixo,
        Placa placa,
        string modelo,
        string montadora,
        int anoFabricacao,
        string combustivel,
        int capacidadePassageiros,
        int kmAtual = 0,
        FonteKm fonteKm = FonteKm.Odometro)
    {
        SetPrefixo(prefixo);
        SetPlaca(placa);
        SetModelo(modelo);
        SetMontadora(montadora);
        SetAnoFabricacao(anoFabricacao);
        SetCombustivel(combustivel);
        SetCapacidade(capacidadePassageiros);
        SetKmAtual(kmAtual);
        FonteKm = fonteKm;
        DataUltimaAtualizacaoKm = DateTime.UtcNow;
    }

    public void Atualizar(
        string modelo,
        string montadora,
        int anoFabricacao,
        string combustivel,
        int capacidadePassageiros)
    {
        SetModelo(modelo);
        SetMontadora(montadora);
        SetAnoFabricacao(anoFabricacao);
        SetCombustivel(combustivel);
        SetCapacidade(capacidadePassageiros);
        Touch();
    }

    public void AtualizarKm(int novoKm, FonteKm fonte)
    {
        if (novoKm < KmAtual)
            throw new InvalidOperationException(
                $"KmAtual ({novoKm}) não pode ser menor que o valor registrado ({KmAtual}).");

        KmAtual = novoKm;
        FonteKm = fonte;
        DataUltimaAtualizacaoKm = DateTime.UtcNow;
        Touch();
    }

    public void ColocarEmManutencao()
    {
        if (Status == StatusOnibus.Ativo)
            Status = StatusOnibus.EmManutencao;
        else if (Status == StatusOnibus.EmManutencao)
            return;
        else
            throw new InvalidOperationException("Veículo inativo não pode entrar em manutenção diretamente.");
        Touch();
    }

    public void TornarAtivo()
    {
        Status = StatusOnibus.Ativo;
        Touch();
    }

    /// <summary>Soft delete — preserva histórico.</summary>
    public void Inativar()
    {
        if (Status == StatusOnibus.Ativo)
            throw new InvalidOperationException(
                "Veículo Ativo em operação não pode ser inativado diretamente. Coloque-o em manutenção primeiro.");
        Status = StatusOnibus.Inativo;
        Touch();
    }

    // --- validações ---
    private void SetPrefixo(string prefixo)
    {
        if (string.IsNullOrWhiteSpace(prefixo))
            throw new ArgumentException("Prefixo obrigatório.", nameof(prefixo));
        Prefixo = prefixo.Trim().ToUpperInvariant();
    }

    private void SetPlaca(Placa placa) => Placa = placa.Valor;

    private void SetModelo(string modelo)
    {
        if (string.IsNullOrWhiteSpace(modelo)) throw new ArgumentException("Modelo obrigatório.", nameof(modelo));
        Modelo = modelo.Trim();
    }

    private void SetMontadora(string montadora)
    {
        if (string.IsNullOrWhiteSpace(montadora)) throw new ArgumentException("Montadora obrigatória.", nameof(montadora));
        Montadora = montadora.Trim();
    }

    private void SetAnoFabricacao(int ano)
    {
        var anoAtual = DateTime.UtcNow.Year;
        if (ano < 1950 || ano > anoAtual)
            throw new ArgumentException($"AnoFabricacao deve estar entre 1950 e {anoAtual}.", nameof(ano));
        AnoFabricacao = ano;
    }

    private void SetCombustivel(string combustivel)
    {
        if (string.IsNullOrWhiteSpace(combustivel)) throw new ArgumentException("Combustível obrigatório.", nameof(combustivel));
        Combustivel = combustivel.Trim();
    }

    private void SetCapacidade(int capacidade)
    {
        if (capacidade <= 0) throw new ArgumentException("Capacidade deve ser maior que zero.", nameof(capacidade));
        CapacidadePassageiros = capacidade;
    }

    private void SetKmAtual(int km)
    {
        if (km < 0) throw new ArgumentException("KmAtual não pode ser negativo.", nameof(km));
        KmAtual = km;
    }
}

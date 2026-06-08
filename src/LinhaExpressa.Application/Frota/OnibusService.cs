using LinhaExpressa.Application.Frota;
using LinhaExpressa.Domain.Frota;
using LinhaExpressa.Domain.Shared.ValueObjects;

namespace LinhaExpressa.Application.Frota;

public class OnibusService : IOnibusService
{
    private const string PrefixoUnicoConfigKey = "ONIBUS_PREFIXO_UNICO";

    private readonly IOnibusRepository _repo;
    private readonly IConfiguracaoGlobalProvider _config;
    private readonly IUnitOfWork _uow;
    private readonly INotificationService _notifier;

    public OnibusService(
        IOnibusRepository repo,
        IConfiguracaoGlobalProvider config,
        IUnitOfWork uow,
        INotificationService notifier)
    {
        _repo = repo;
        _config = config;
        _uow = uow;
        _notifier = notifier;
    }

    public async Task<OnibusDto> CreateAsync(CreateOnibusRequest request, CancellationToken ct = default)
    {
        var placa = Placa.Criar(request.Placa);

        // Regra: prefixo único controlado por ConfiguracaoGlobal.
        // Quando o flag estiver "true" (default), validamos.
        var prefuxoUnico = await _config.GetValorAsync(PrefixoUnicoConfigKey, ct);
        if (string.IsNullOrEmpty(prefuxoUnico) ||
            string.Equals(prefuxoUnico, "true", StringComparison.OrdinalIgnoreCase))
        {
            var existente = await _repo.GetByPrefixoAsync(request.Prefixo, ct);
            if (existente is not null)
                throw new InvalidOperationException(
                    $"Já existe um ônibus com o prefixo '{request.Prefixo}'.");
        }

        var placaExistente = await _repo.GetByPlacaAsync(placa.Valor, ct);
        if (placaExistente is not null)
            throw new InvalidOperationException(
                $"Já existe um ônibus com a placa '{placa}'.");

        var onibus = new Onibus(
            request.Prefixo,
            placa,
            request.Modelo,
            request.Montadora,
            request.AnoFabricacao,
            request.Combustivel,
            request.CapacidadePassageiros,
            request.KmAtual,
            request.FonteKm);

        await _repo.AddAsync(onibus, ct);
        await _uow.SaveChangesAsync(ct);

        var dto = ToDto(onibus);
        await _notifier.OnibusCreatedAsync(dto, ct);
        return dto;
    }

    public async Task<PagedResult<OnibusDto>> ListAsync(OnibusFiltro filtro, CancellationToken ct = default)
    {
        var page = filtro.Page < 1 ? 1 : filtro.Page;
        var size = filtro.PageSize is < 1 or > 200 ? 20 : filtro.PageSize;

        var (items, total) = await _repo.ListAsync(filtro.Status, filtro.Prefixo, page, size, ct);

        var totalPages = (int)Math.Ceiling(total / (double)size);
        return new PagedResult<OnibusDto>(
            items.Select(ToDto).ToList(),
            page, size, total, totalPages);
    }

    public async Task<OnibusDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var onibus = await _repo.GetByIdAsync(id, ct);
        return onibus is null ? null : ToDto(onibus);
    }

    public async Task<OnibusDto> UpdateAsync(Guid id, UpdateOnibusRequest request, CancellationToken ct = default)
    {
        var onibus = await _repo.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Onibus {id} não encontrado.");

        // Veículo EmManutencao não pode ser escalado — refletido bloqueando atualização de
        // dados sensíveis à operação. Permitimos apenas atualizar dados cadastrais aqui,
        // mantendo o status bloqueado. (Ver regra: status bloqueado = bloquear edição de escala).
        if (onibus.Status == StatusOnibus.EmManutencao)
        {
            // Atualização cadastral ainda é permitida (modelo, montadora, ano, combustível, capacidade).
            // O que está bloqueado é a alteração de ESCALA — que não faz parte deste contrato.
        }

        onibus.Atualizar(
            request.Modelo,
            request.Montadora,
            request.AnoFabricacao,
            request.Combustivel,
            request.CapacidadePassageiros);

        _repo.Update(onibus);
        await _uow.SaveChangesAsync(ct);

        var dto = ToDto(onibus);
        await _notifier.OnibusUpdatedAsync(dto, ct);
        return dto;
    }

    public async Task SoftDeleteAsync(Guid id, CancellationToken ct = default)
    {
        var onibus = await _repo.GetByIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Onibus {id} não encontrado.");

        // Reaproveita a regra de domínio: Ativo em operação não pode ser inativado diretamente.
        onibus.Inativar();

        _repo.Update(onibus);
        await _uow.SaveChangesAsync(ct);

        await _notifier.OnibusDeletedAsync(id, ct);
    }

    private static OnibusDto ToDto(Onibus o) => new(
        o.Id, o.Prefixo, o.Placa, o.Modelo, o.Montadora, o.AnoFabricacao,
        o.Combustivel, o.CapacidadePassageiros, o.Status, o.KmAtual, o.FonteKm,
        o.DataUltimaAtualizacaoKm, o.CriadoEm, o.AtualizadoEm);
}

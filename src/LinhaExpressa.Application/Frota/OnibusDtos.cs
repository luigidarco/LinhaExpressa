using LinhaExpressa.Domain.Frota;

namespace LinhaExpressa.Application.Frota;

public record OnibusDto(
    Guid Id,
    string Prefixo,
    string Placa,
    string Modelo,
    string Montadora,
    int AnoFabricacao,
    string Combustivel,
    int CapacidadePassageiros,
    StatusOnibus Status,
    int KmAtual,
    FonteKm FonteKm,
    DateTime? DataUltimaAtualizacaoKm,
    DateTime CriadoEm,
    DateTime AtualizadoEm);

public record CreateOnibusRequest(
    string Prefixo,
    string Placa,
    string Modelo,
    string Montadora,
    int AnoFabricacao,
    string Combustivel,
    int CapacidadePassageiros,
    int KmAtual = 0,
    FonteKm FonteKm = FonteKm.Odometro);

public record UpdateOnibusRequest(
    string Modelo,
    string Montadora,
    int AnoFabricacao,
    string Combustivel,
    int CapacidadePassageiros);

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages);

public record OnibusFiltro(
    StatusOnibus? Status = null,
    string? Prefixo = null,
    int Page = 1,
    int PageSize = 20);

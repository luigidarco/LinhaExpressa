using System.Text.RegularExpressions;

namespace LinhaExpressa.Domain.Shared.ValueObjects;

/// <summary>
/// Value Object representing a Brazilian vehicle license plate.
/// Supports both the old format (AAA-9999) and the Mercosul format (AAA9A99).
/// </summary>
public sealed class Placa : IEquatable<Placa>
{
    // Mercosul: 3 letras + 1 número + 1 letra + 2 números  (e.g. ABC1D23)
    private static readonly Regex Mercosul = new("^[A-Z]{3}[0-9][A-Z][0-9]{2}$", RegexOptions.Compiled);
    // Antigo:    3 letras + 1 hífen (opcional) + 4 números   (e.g. ABC-1234 / ABC1234)
    private static readonly Regex Antigo = new("^[A-Z]{3}-?[0-9]{4}$", RegexOptions.Compiled);

    public string Valor { get; private set; }

    private Placa(string valor) => Valor = valor;

    public static Placa Criar(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Placa não pode ser vazia.", nameof(valor));

        var normalizado = valor.Trim().ToUpperInvariant().Replace("-", "");

        if (normalizado.Length is < 7 or > 8)
            throw new ArgumentException($"Placa '{valor}' inválida.", nameof(valor));

        // Re-aplica o hífen para o formato antigo apenas para validação visual.
        var padraoAntigo = normalizado.Length == 7
            ? $"{normalizado[..3]}-{normalizado[3..]}"
            : normalizado;

        if (!Mercosul.IsMatch(normalizado) && !Antigo.IsMatch(padraoAntigo))
            throw new ArgumentException($"Placa '{valor}' não está em formato Mercosul nem antigo.", nameof(valor));

        return new Placa(normalizado);
    }

    public override string ToString() => Valor;

    public bool Equals(Placa? other) => other is not null && Valor == other.Valor;
    public override bool Equals(object? obj) => obj is Placa p && Equals(p);
    public override int GetHashCode() => Valor.GetHashCode();

    public static implicit operator string(Placa placa) => placa.Valor;
}

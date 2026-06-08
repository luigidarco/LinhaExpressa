using LinhaExpressa.Domain.Frota;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LinhaExpressa.Infrastructure.Persistence.Configurations;

public class OnibusConfiguration : IEntityTypeConfiguration<Onibus>
{
    public void Configure(EntityTypeBuilder<Onibus> b)
    {
        b.ToTable("ONIBUS");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("Id").HasColumnType("uniqueidentifier");

        b.Property(x => x.Prefixo).HasColumnName("Prefixo").HasMaxLength(20).IsRequired();
        b.HasIndex(x => x.Prefixo).IsUnique();

        b.Property(x => x.Placa).HasColumnName("Placa").HasMaxLength(10).IsRequired();
        b.HasIndex(x => x.Placa).IsUnique();

        b.Property(x => x.Modelo).HasColumnName("Modelo").HasMaxLength(80).IsRequired();
        b.Property(x => x.Montadora).HasColumnName("Montadora").HasMaxLength(80).IsRequired();
        b.Property(x => x.AnoFabricacao).HasColumnName("AnoFabricacao").IsRequired();
        b.Property(x => x.Combustivel).HasColumnName("Combustivel").HasMaxLength(40).IsRequired();
        b.Property(x => x.CapacidadePassageiros).HasColumnName("CapacidadePassageiros").IsRequired();
        b.Property(x => x.Status).HasColumnName("Status").HasConversion<int>().IsRequired();
        b.Property(x => x.KmAtual).HasColumnName("KmAtual").IsRequired();
        b.Property(x => x.FonteKm).HasColumnName("FonteKm").HasConversion<int>().IsRequired();
        b.Property(x => x.DataUltimaAtualizacaoKm).HasColumnName("DataUltimaAtualizacaoKm");
        b.Property(x => x.CriadoEm).HasColumnName("CriadoEm").IsRequired();
        b.Property(x => x.AtualizadoEm).HasColumnName("AtualizadoEm").IsRequired();

        b.HasMany(x => x.ManutencoesPreventivas)
         .WithOne(x => x.Onibus!)
         .HasForeignKey(x => x.OnibusId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.ManutencoesCorretivas)
         .WithOne(x => x.Onibus!)
         .HasForeignKey(x => x.OnibusId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.Licenciamentos)
         .WithOne(x => x.Onibus!)
         .HasForeignKey(x => x.OnibusId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.Pneus)
         .WithOne(x => x.Onibus!)
         .HasForeignKey(x => x.OnibusId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ManutencaoPreventivaConfiguration : IEntityTypeConfiguration<ManutencaoPreventiva>
{
    public void Configure(EntityTypeBuilder<ManutencaoPreventiva> b)
    {
        b.ToTable("MANUTENCAO_PREVENTIVA");
        b.HasKey(x => x.Id);
        b.Property(x => x.DescricaoServico).HasMaxLength(200).IsRequired();
        b.Property(x => x.DataAgendada).IsRequired();
        b.Property(x => x.DataRealizada);
        b.Property(x => x.IntervaloDias).IsRequired();
        b.Property(x => x.KmParaProxima).IsRequired();
        b.Property(x => x.Status).HasConversion<int>().IsRequired();
    }
}

public class ManutencaoCorretivaConfiguration : IEntityTypeConfiguration<ManutencaoCorretiva>
{
    public void Configure(EntityTypeBuilder<ManutencaoCorretiva> b)
    {
        b.ToTable("MANUTENCAO_CORRETIVA");
        b.HasKey(x => x.Id);
        b.Property(x => x.DescricaoDefeito).HasMaxLength(500).IsRequired();
        b.Property(x => x.DataAbertura).IsRequired();
        b.Property(x => x.DataConclusao);
        b.Property(x => x.TipoRecolhimento).HasConversion<int>().IsRequired();
        b.Property(x => x.TipoOficina).HasConversion<int>().IsRequired();
        b.Property(x => x.Status).HasConversion<int>().IsRequired();
    }
}

public class LicenciamentoConfiguration : IEntityTypeConfiguration<Licenciamento>
{
    public void Configure(EntityTypeBuilder<Licenciamento> b)
    {
        b.ToTable("LICENCIAMENTO");
        b.HasKey(x => x.Id);
        b.Property(x => x.Tipo).HasConversion<int>().IsRequired();
        b.Property(x => x.Vencimento).IsRequired();
        b.Property(x => x.Alertado).IsRequired();
    }
}

public class PneuConfiguration : IEntityTypeConfiguration<Pneu>
{
    public void Configure(EntityTypeBuilder<Pneu> b)
    {
        b.ToTable("PNEU");
        b.HasKey(x => x.Id);
        b.Property(x => x.Posicao).HasConversion<int>().IsRequired();
        b.Property(x => x.DataInstalacao).IsRequired();
        b.Property(x => x.KmInstalacao).IsRequired();
        b.Property(x => x.KmLimite).IsRequired();
    }
}

public class ConfiguracaoGlobalConfiguration : IEntityTypeConfiguration<ConfiguracaoGlobal>
{
    public void Configure(EntityTypeBuilder<ConfiguracaoGlobal> b)
    {
        b.ToTable("CONFIGURACAO_GLOBAL");
        b.HasKey(x => x.Id);
        b.Property(x => x.Chave).HasMaxLength(80).IsRequired();
        b.HasIndex(x => x.Chave).IsUnique();
        b.Property(x => x.Valor).HasMaxLength(4000).IsRequired();
        b.Property(x => x.Descricao).HasMaxLength(200);
    }
}

using LinhaExpressa.Domain.Frota;
using Microsoft.EntityFrameworkCore;

namespace LinhaExpressa.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Onibus> Onibus => Set<Onibus>();
    public DbSet<ManutencaoPreventiva> ManutencoesPreventivas => Set<ManutencaoPreventiva>();
    public DbSet<ManutencaoCorretiva> ManutencoesCorretivas => Set<ManutencaoCorretiva>();
    public DbSet<Licenciamento> Licenciamentos => Set<Licenciamento>();
    public DbSet<Pneu> Pneus => Set<Pneu>();
    public DbSet<ConfiguracaoGlobal> ConfiguracoesGlobais => Set<ConfiguracaoGlobal>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

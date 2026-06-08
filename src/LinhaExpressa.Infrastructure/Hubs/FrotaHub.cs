using LinhaExpressa.Application.Frota;
using Microsoft.AspNetCore.SignalR;

namespace LinhaExpressa.Infrastructure.Hubs;
public class FrotaHub : Hub
{
    public async Task EntrarGrupoGestores() => await Groups.AddToGroupAsync(Context.ConnectionId, "gestores");
    public async Task EntrarGrupoDespachantes() => await Groups.AddToGroupAsync(Context.ConnectionId, "despachantes");
}

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<FrotaHub> _hub;
    public SignalRNotificationService(IHubContext<FrotaHub> hub) => _hub = hub;

    public Task OnibusCreatedAsync(OnibusDto onibus, CancellationToken ct = default) =>
        _hub.Clients
            .Groups("gestores", "despachantes")
            .SendAsync("OnibusCreated", onibus, ct);

    public Task OnibusUpdatedAsync(OnibusDto onibus, CancellationToken ct = default) =>
        _hub.Clients
            .Groups("gestores", "despachantes")
            .SendAsync("OnibusUpdated", onibus, ct);

    public Task OnibusDeletedAsync(Guid id, CancellationToken ct = default) =>
        _hub.Clients
            .Groups("gestores", "despachantes")
            .SendAsync("OnibusDeleted", id, ct);
}

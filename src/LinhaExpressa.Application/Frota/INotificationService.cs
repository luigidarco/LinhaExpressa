using LinhaExpressa.Application.Frota;

namespace LinhaExpressa.Application.Frota;

public interface INotificationService
{
    Task OnibusCreatedAsync(OnibusDto onibus, CancellationToken ct = default);
    Task OnibusUpdatedAsync(OnibusDto onibus, CancellationToken ct = default);
    Task OnibusDeletedAsync(Guid id, CancellationToken ct = default);
}

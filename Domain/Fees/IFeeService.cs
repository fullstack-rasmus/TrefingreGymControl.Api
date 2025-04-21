namespace TrefingreGymControl.Api.Domain.Fees
{
    public interface IFeeService
    {
        Task<Fee> GetFeeAsync(Guid feeId, CancellationToken cancellationToken = default);
        Task CreateFeeAsync(string description, decimal amount, bool isRecurring, CancellationToken cancellationToken = default);
        Task<List<Fee>> GetFeesAsync(CancellationToken cancellationToken = default);
        Task DeleteFeeAsync(Guid feeId, CancellationToken cancellationToken = default);
    }
}
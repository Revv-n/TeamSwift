using PlanSwift9;
using System.Threading;

namespace PlanSwiftApi.Services
{
    public interface IApiService
    {
        void PlanSwiftApiConect(CancellationToken token);
        void IsRunCurrent(CancellationToken token);
        CancellationTokenSource CancellationTokenSource { get; set; }
        PlanSwift PlanSwiftApi { get; }
    }
}

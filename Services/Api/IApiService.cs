using PlanSwift9;
using System.Threading;

namespace PlanSwiftApi.Services
{
    public interface IApiService
    {
        void PlanSwiftApiConect(CancellationToken token);
        bool IsRunCurrent(CancellationToken token);
        CancellationTokenSource CancellationTokenSource { get; set; }
    }
}

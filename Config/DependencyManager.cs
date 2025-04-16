using System;
using System.Threading;
using PlanSwiftApi.Services;
using PlanSwift9;

namespace PlanSwiftApi.Config
{
    public class DependencyManager
    {
        private readonly IApiService apiService;

        private readonly JobsManager _jobsmanager;
        private readonly PlanSwift _planSwift;


        public JobsManager JobsManager => _jobsmanager;
        public PlanSwift PlanSwift => _planSwift;






        public DependencyManager(JobsManager jobsManager)
        {

            _jobsmanager = jobsManager;

            // Call ApiServices to find PlanSwift -- using threads //


           apiService = new ApiService

           
           
           
            {
                CancellationTokenSource = new CancellationTokenSource()
            };

            Thread thread0 = new Thread(() => apiService.PlanSwiftApiConect(apiService.CancellationTokenSource.Token));
            thread0.Start();

            Thread thread1 = new Thread(() => apiService.IsRunCurrent(apiService.CancellationTokenSource.Token));
            thread1.Start();

            PlanSwift _planSwift = apiService.PlanSwiftApi;
        }



        // Clean memory App break all threads and app memory for optimize and liberate all ram and close definitly the program // 
        public void CleanMemory()
        {
            apiService.CancellationTokenSource.Cancel();
            Console.WriteLine("cerrando");
        }
    }
}

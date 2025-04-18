using System;
using System.Threading;
using PlanSwiftApi.Services;
using PlanSwift9;
using PlanSwiftApi.Config;

namespace PlanSwiftApi.ApiConfigs
{
    public class ApiConfig
    {
        private readonly ApiService _apiService;

        public ApiConfig(ApiService apiService)
        {
            _apiService = apiService;

            _apiService.CancellationTokenSource = new CancellationTokenSource();

            Thread thread0 = new Thread(() => _apiService.PlanSwiftApiConect(_apiService.CancellationTokenSource.Token));
            thread0.Start();

            Thread thread1 = new Thread(() => _apiService.IsRunCurrent(_apiService.CancellationTokenSource.Token));
            thread1.Start();

            _apiService.OnPlanSwiftClose += () =>
            {
                _apiService.CancellationTokenSource.Cancel();
                _apiService.CancellationTokenSource = new CancellationTokenSource();

                Thread threadReconect0 = new Thread(() =>
                {
                    _apiService.PlanSwiftApiConect(_apiService.CancellationTokenSource.Token);
                    if(_apiService.Loaded == true)
                    {
                        Thread threadReconect1 = new Thread(() =>
                        {
                            _apiService.IsRunCurrent(_apiService.CancellationTokenSource.Token);
                        });
                        threadReconect1.Start();
                    }
                });
                threadReconect0.Start();

            };

        }

        public void CleanMemory()
        {
            _apiService.CancellationTokenSource.Cancel();
            Console.WriteLine("cerrando");
        }
    }
}

using System;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using PlanSwift9;
using PlanSwiftApi.Config;
using System.Security.Cryptography.X509Certificates; 


namespace PlanSwiftApi.Services
{
    public class ApiService : IApiService
    {

        // Private variables //

        private PlanSwift _planSwift; // PlanSwift api Class Private //
        private bool _loaded = false;

        // public variables //
        public CancellationTokenSource CancellationTokenSource { get; set; } // Cancellation tokens for bucles //
        public bool Loaded => _loaded; // flag PlanSwift is conect //
        public PlanSwift PlanSwift => _planSwift; // PlanSwift api Class Public //
        public ManualResetEvent Bucle0Completed { get; private set; }


        public ApiService()
        {
            Bucle0Completed = new ManualResetEvent(false);
        }

        public void PlanSwiftApiConect(CancellationToken token) // Stablish conection to PlanSwift //
        {
            while (!token.IsCancellationRequested) // thread connection and optimized whit clean memory using cancellation tokens //
            {
                try
                {
                    if (Process.GetProcessesByName("PlanSwift").Length > 0) // seraching the PlanSwift proces to verify planSwift is open //
                    {
                        Thread.Sleep(5000); // optimize Memory and correctly opening PlanSwift whaiting ones seconds //
                        _planSwift = new PlanSwift(); // Stablish Conection //
                        Console.WriteLine("PlanSwift Conected"); // verify in console //
                        Bucle0Completed.Set(); // Bucle is ok and finish, flag to bucle1 start //
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10000); // optimize memory and run search of PlanSwift //
                        Console.WriteLine("find PlanSwift"); // Verify search in console //

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (token.IsCancellationRequested) // Optimization and prevent errors //
            {
                Bucle0Completed.Set();

            }
        }

        // PlanSwift is open and Current runing,  if the program closes alert a the program that as closed //
        public bool IsRunCurrent(CancellationToken token)
        {
            Bucle0Completed.WaitOne(); // Wait to bucle 0 is close or completed //


            while (!token.IsCancellationRequested)
            {
                try
                {
                    _planSwift.IsLoaded(); // verify PlanSwift is run current
                    _loaded = true;

                    Thread.Sleep(10000); // wait time for the optimize that program //

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message} PlanSwift is close"); // Alert PlanSwift are closed //
                    Thread.Sleep(5000); // Optimize again //
                }
            }return _loaded;

        }

    }
}

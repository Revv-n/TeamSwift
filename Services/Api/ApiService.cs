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
        public CancellationTokenSource CancellationTokenSource { get; set; } // Cancellation tokens for bucles //
        public bool Loaded { get; private set; } // flag PlanSwift is conect //
        public PlanSwift PlanSwiftApi { get; private set; } // PlanSwift api Class //
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
                        PlanSwiftApi = new PlanSwift(); // Stablish Conection //
                        Console.WriteLine("PlanSwift Conected"); // verify in console //
                        Bucle0Completed.Set(); // Bucle is ok and finish, flag to bucle1 start //
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10000); // optimize memory and run search of PlanSwift //
                        Bucle0Completed.Set();
                        Console.WriteLine("find PlanSwift"); // Verify search in console // 
                    }
                }
                catch (Exception ex)
                {
                    Bucle0Completed.Set(); // Avoid crashes if the app crashes //
                    Console.WriteLine(ex.Message);
                }


            }
        }

        // PlanSwift is open and Current runing,  if the program closes alert a the program that as closed //
        public void IsRunCurrent(CancellationToken token)
        {
            Bucle0Completed.WaitOne(); // Wait to bucle 0 is close or completed //

            while (!token.IsCancellationRequested)
            {
                try
                {
                    PlanSwiftApi.IsLoaded(); // verify PlanSwift is run current
                    Loaded = true;

                    

                    Thread.Sleep(10000); // wait time for the optimize that program //

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message} PlanSwift is close"); // Alert PlanSwift are closed //
                    Thread.Sleep(5000); // Optimize again //
                }
            }
        }
    }
}

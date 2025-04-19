using System;
using System.Threading;
using PlanSwiftApi.Services;
using PlanSwift9;
using PlanSwiftApi.Config;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

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

            CreateConfigs();
        }

        public void CreateConfigs()
        {

            _apiService.CreateConfig += () =>
            {
                string pathFile = "Not Found";
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
                Directory.CreateDirectory(folderPath);

                string jsonPath = Path.Combine(folderPath, "config.json");

                if (!File.Exists(jsonPath))
                {
                    try
                    {
                        var process = Process.GetProcessesByName("PlanSwift");

                        foreach (var pro in process)
                        {
                            pathFile = pro.MainModule.FileName;
                        }

                        var path = Path.GetDirectoryName(pathFile);

                        if (path == "Not Found")
                        {
                            path = Constants.DefaultPath;
                        }

                        var pathConfig = new Dictionary<string, string>
                        {
                            {"Path", path}
                        };

                        var config = new Dictionary<string, Dictionary<string, string>>
                    {
                        {"Configs", pathConfig}
                    };

                        string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(jsonPath, json);
                        Console.WriteLine("Archivo JSON con estructura creado correctamente." + jsonPath);


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Error al crear el archivo de configuración: " + ex.Message);

                    }
                }else
                {
                    Console.WriteLine("ℹ️ El archivo de configuración ya existe: " + jsonPath);
                }


            };





        }

        public void CleanMemory()
        {
            _apiService.CancellationTokenSource.Cancel();
            Console.WriteLine("cerrando");
        }
    }
}

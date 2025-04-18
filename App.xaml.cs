﻿using PlanSwiftApi.Windows;
using PlanSwiftApi;
using System;
using System.Windows;
using System.Windows.Threading;
using PlanSwiftApi.Resources.Animations;
using Microsoft.Extensions.DependencyInjection;
using PlanSwiftApi.Services;
using PlanSwiftApi.Views;
using PlanSwiftApi.Config;
using PlanSwiftApi.Config.WindowsConfig;
using PlanSwiftApi.ApiConfigs;
using PlanSwiftApi.Helpers;


namespace PlanSwiftApi
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            // Registrar dependencias
            services.AddSingleton<DependencyManager>();
            services.AddSingleton<FilesManager>();
            services.AddTransient<HomeView>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ApiService>();
            services.AddSingleton<ApiConfig>();
            services.AddSingleton<JsonManager>();

            ServiceProvider = services.BuildServiceProvider();

            var splash = new SplashWindow();
            splash.Show();
        }
    }
}

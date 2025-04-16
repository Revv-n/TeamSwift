using PlanSwiftApi.Windows;
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
            services.AddSingleton<JobsManager>();
            services.AddTransient<HomeView>();
            services.AddSingleton<MainWindow>();

            ServiceProvider = services.BuildServiceProvider();

            var splash = new SplashWindow();
            splash.Show();
        }
    }
}

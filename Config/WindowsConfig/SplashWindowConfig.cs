using PlanSwiftApi.Windows;
using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace PlanSwiftApi.Config.WindowsConfig
{
    internal class SplashWindowConfig
    {
        private readonly DispatcherTimer _dispatcherTimer; // Cambiado a _dispatcherTimer
        private readonly SplashWindow _splashWindow; // Ventana Splash recibida por constructor
        private readonly IServiceProvider _serviceProvider;

        public SplashWindowConfig(SplashWindow window, IServiceProvider serviceProvider)
        {
            _splashWindow = window ?? throw new ArgumentNullException(nameof(window));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            // Configurar el timer para esperar 2 segundos
            _dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _dispatcherTimer.Tick += OnSplashTimerTick;
            _dispatcherTimer.Start();
        }

        private void OnSplashTimerTick(object sender, EventArgs e)
        {
            _dispatcherTimer.Stop(); // Detener el timer después de que se complete

            try
            {
                if (_serviceProvider == null)
                {
                    MessageBox.Show("ServiceProvider no está disponible.", "Error");
                    return;
                }

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>(); // Obtener MainWindow desde DI
                mainWindow.Show();

                _splashWindow.Close(); // Cerrar SplashWindow solo después de abrir MainWindow
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana principal: {ex.Message}", "Error");
            }
        }
    }
}
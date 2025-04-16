using PlanSwiftApi.Config;
using System.Windows;
using System.Threading;
using PlanSwiftApi.Config.WindowsConfig;
using PlanSwiftApi.Services;
using PlanSwiftApi.Resources.Animations;



namespace PlanSwiftApi.Windows
{
    public partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();

            var app = (App)Application.Current;
            var serviceProvider = app.ServiceProvider;

            SplashWindowConfig splashWindowConfig = new SplashWindowConfig(this, serviceProvider);

            
            
            WindowAnimations windowAnimations = new WindowAnimations();

            windowAnimations.OpacityAnimation(this);

            
            
            
        }

    }
}
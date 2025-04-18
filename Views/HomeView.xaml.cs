using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlanSwiftApi.Config;
using PlanSwiftApi.Services;

namespace PlanSwiftApi.Views
{
    /// <summary>
    /// Lógica de interacción para HomeView.xaml
    /// </summary>
    public partial class HomeView : Page
    {


        private readonly DependencyManager _config;
        public HomeView(DependencyManager config)
        {
            InitializeComponent();

            _config = config;

            ListStorages();

        }


        // StackPanel Array //


        private void ListStorages()
        {
            
            List<string> storages = _config.JobsManager.FindStorages();
            foreach (var storage in storages)
            {
                var storageList = new Border
                {
                    Height = 25,
                    Margin = new Thickness(0, 3, 0, 0),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2A4A2A2")),
                    CornerRadius = new CornerRadius(5),
                    Child = new DockPanel
                    {
                        Margin = new Thickness(10, 0, 0, 0),
                        Children =
                        {
                            new TextBlock
                            {
                            VerticalAlignment = VerticalAlignment.Center,
                            FontFamily = new FontFamily("Global User Interface"),
                            FontWeight = FontWeights.DemiBold,
                            Foreground = Brushes.White,
                            Text = storage
                            }
                        }
                    }
                };
                StoragesPanel.Children.Add(storageList);
            }
        }

    }
}


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
        private Point _startPoint; // Posición inicial del mouse
        private bool _isDragging; // Indica si el usuario está arrastrando
        private const double BounceLimit = 50; // Límite del rebote (en píxeles)


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

        private void ScrollViewer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _startPoint = e.GetPosition(this); // Captura la posición inicial del mouse
                _isDragging = true; // Activa el arrastre
                ScrollViewer.CaptureMouse(); // Captura el mouse para el ScrollViewer
            }
        }

        private void ScrollViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Calcula el desplazamiento basado en la posición actual del mouse
                Point currentPoint = e.GetPosition(this);
                double deltaY = _startPoint.Y - currentPoint.Y;

                // Desplaza el contenido del ScrollViewer
                double newOffset = ScrollViewer.VerticalOffset + deltaY;

                // Permite un desplazamiento más allá de los límites con el rebote
                if (newOffset < -BounceLimit || newOffset > ScrollViewer.ExtentHeight - ScrollViewer.ViewportHeight + BounceLimit)
                {
                    deltaY /= 2; // Reduce la velocidad cuando se exceden los límites
                }

                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + deltaY);

                // Actualiza la posición inicial
                _startPoint = currentPoint;
            }
        }

        private void ScrollViewer_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && _isDragging)
            {
                _isDragging = false; // Finaliza el arrastre
                ScrollViewer.ReleaseMouseCapture(); // Libera el mouse

                // Verifica si el contenido está fuera de los límites y aplica el rebote
                if (ScrollViewer.VerticalOffset < 0)
                {
                    AnimateScroll(0); // Rebota al inicio
                }
                else if (ScrollViewer.VerticalOffset > ScrollViewer.ExtentHeight - ScrollViewer.ViewportHeight)
                {
                    AnimateScroll(ScrollViewer.ExtentHeight - ScrollViewer.ViewportHeight); // Rebota al final
                }
            }
        }

        private void AnimateScroll(double targetOffset)
        {
            // Crea una animación para suavizar el rebote
            DoubleAnimation animation = new DoubleAnimation
            {
                From = ScrollViewer.VerticalOffset,
                To = targetOffset,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            animation.Completed += (s, e) =>
            {
                ScrollViewer.ScrollToVerticalOffset(targetOffset); // Asegura que el desplazamiento finalice en el lugar correcto
            };

            // Aplica la animación
            ScrollViewer.BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, animation);
        }
    }

    // Clase auxiliar para animar VerticalOffset
    public static class ScrollViewerBehavior
    {
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.RegisterAttached("VerticalOffset", typeof(double), typeof(ScrollViewerBehavior),
                new PropertyMetadata(0.0, OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
            }
        }

        public static double GetVerticalOffset(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalOffsetProperty);
        }

        public static void SetVerticalOffset(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalOffsetProperty, value);
        }
    }
}


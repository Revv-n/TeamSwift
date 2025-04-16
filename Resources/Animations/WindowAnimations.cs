using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace PlanSwiftApi.Resources.Animations
{
    internal class WindowAnimations
    {



        public void OpacityAnimation(Window window)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation fadeInAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.4))
            };
            storyboard.Children.Clear();
            Storyboard.SetTarget(fadeInAnimation, window);
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath(Window.OpacityProperty));

            storyboard.Children.Add(fadeInAnimation);
            storyboard.Begin();
        }




        public void MoveWindow(Window window)
        {
            window.MouseDown += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    window.DragMove();
                }
            };


        }
    }
}

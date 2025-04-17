using Microsoft.Extensions.DependencyInjection;
using PlanSwift9;
using PlanSwiftApi.Config;
using PlanSwiftApi.Resources.Animations;
using PlanSwiftApi.Services;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using PlanSwiftApi.Views;





namespace PlanSwiftApi
{

    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DependencyManager _config;


        public MainWindow(DependencyManager config, IServiceProvider serviceProvider)
        {



            InitializeComponent();

            

            _config = config ?? throw new ArgumentNullException(nameof(config));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));



            var anim = new WindowAnimations();

            anim.OpacityAnimation(this);
            anim.MoveWindow(this);
            Loaded += MainWindow_Loaded;

            var homeView = _serviceProvider.GetRequiredService<HomeView>();

            MainFrame.Navigate(homeView);

            
            
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
        }

        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
            var hwnd = windowHelper.Handle;

            var accent = new AccentPolicy();
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(hwnd, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 6,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
            ACCENT_ENABLE_HOSTBACKDROP = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        private void OnCloseWIndow(object sender, CancelEventArgs e) 
        {
            
            _config.CleanMemory(); 

            
        }


    }
}

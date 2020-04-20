using CefSharp;
using CefSharp.Wpf;
using System;
using System.Windows;

namespace WpfAppDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //ChromiumWebBrowser chromiumWebBrowser = new CefSharp.Wpf.ChromiumWebBrowser();
        //CefSettings settings = new CefSettings();
        //UI 线程
        System.Windows.Threading.DispatcherTimer dtimer;
        //非UI 线程
        System.Timers.Timer timer;
        public MainWindow()
        {
            InitializeComponent();//初始化组件
            //TimersStar();
            CefSharpBrowser();
        }

        /// <summary>
        /// 内置浏览器测试 （CefSharp应用）
        /// </summary>
        private void CefSharpBrowser()
        {

            var setting = new CefSettings();
            Cef.Initialize(setting);
            var webView = new CefSharp.Wpf.ChromiumWebBrowser();//谷歌浏览器内核
            this.Content = webView;
            webView.Address = "http://www.zoaosoft.com:8001/swagger/ui/index#!/Game/Game_PNG_PlayerGame";

        }


        /// <summary>
        /// 线程测试
        /// </summary>
        private void TimersStar()
        {

            this.Label_OtherResult.Content = DateTime.Now.ToString();
            this.Label_Result.Content = DateTime.Now.ToString();

            if (dtimer == null)
            {
                dtimer = new System.Windows.Threading.DispatcherTimer();
                dtimer.Interval = TimeSpan.FromSeconds(1);
                dtimer.Tick += dtimer_Tick;
            }

            if (timer == null)
            {
                timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += timer_Elapsed;
            }
        }


        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Label_OtherResult.Content = DateTime.Now.ToString();
            }), null);
        }

        void dtimer_Tick(object sender, EventArgs e)
        {
            this.Label_Result.Content = DateTime.Now.ToString();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dtimer.Start();
            timer.Start();
        }
        
        void test()
        {
            //var Report = new GridppReport();

        }

    }
}

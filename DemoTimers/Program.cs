using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DemoTimers
{
    class Program
    {
        private static System.Timers.Timer aTimer;
        static string timerXmlUrl = @"C:\Users\Administrator\Desktop\demo\DemoTimers\TimersConfig.xml";
        private static bool IsTimerOn = true;
        static void Main(string[] args)
        {
            //TimerModel timerModel = ConfigureXML.GetValueByCode(timerXmlUrl);
            ////实例化Timer类，设置间隔时间为10000毫秒；
            //aTimer = new System.Timers.Timer(10000);

            ////注册计时器的事件
            //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            ////设置时间间隔为2秒（2000毫秒），覆盖构造函数设置的间隔
            ////小时转换（60 * 60 * 1000）
            //aTimer.Interval = timerModel.Hour * 1000;

            ////设置是执行一次（false）还是一直执行(true)，默认为true
            //aTimer.AutoReset = true;

            ////开始计时
            //aTimer.Enabled = true;
            //TimerJob job = new TimerJob();
            //job.Start();

            DateTime NowTimes = DateTime.Now;

            funSetTimer();

            System.Threading.Timer timer = new System.Threading.Timer(start => { 
            
            },null, 0, 86400000);

            Console.WriteLine("按任意键退出程序。");
            Console.ReadLine();
        }

        //指定Timer触发的事件
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (IsTimerOn)
            {//防重（上个未执行完）
                IsTimerOn = false;
                Console.WriteLine("触发的事件发生在： {0}", e.SignalTime);
                IsTimerOn = true;
            }
        }


        /// <summary>
                /// timer1的第一次时间间隔
                /// </summary>
        private static void funSetTimer()
        {
            try
            {
                Timer timers = new Timer(1000);//86400000
                //获取当前时间,设置系统启动时间 
                DateTime dt = DateTime.Now;
                DateTime strTime;
                strTime = Convert.ToDateTime("15:00");
                //如果当前的时间小于12：00，则dtAfter为今天的12：00 ;否则dtAfter为明天的12：00
                string dtAfter;
                if (dt <= strTime)
                {
                    dtAfter = strTime.ToString();
                }
                else
                {
                    dtAfter = strTime.AddDays(1).ToString();
                }

                //获取定时器的启动间隔时间
                DateTime dtTime = Convert.ToDateTime(dtAfter);
                TimeSpan ts1 = new TimeSpan(dt.Ticks);
                TimeSpan ts2 = new TimeSpan(dtTime.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string dateDi = ts.TotalSeconds.ToString();
                string strDatedi = dateDi.Substring(0, dateDi.IndexOf('.'));

                //设置timerTemp的时间隔
                int dateDiff = Convert.ToInt32(strDatedi);
                //timers.Interval = dateDiff * 1000;
                timers.Enabled = true;
                timers.Elapsed += new System.Timers.ElapsedEventHandler(UpdateOA);
                timers.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；   
                timers.Enabled = true;     //是否执行System.Timers.Timer.Elapsed事件； 

                GC.KeepAlive(timers);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateOA(object source, System.Timers.ElapsedEventArgs e)
        {
            Timer Stimer = (Timer)source;
            Stimer.Interval = 86400000;

            string asd = string.Empty;

            asd = "123456";
            asd += "123456";
            asd += "123456";

            Console.WriteLine(asd);

        }
    }
}

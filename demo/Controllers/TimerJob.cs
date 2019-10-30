using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DemoTimers
{
    public class TimerJob
    {
        string timerXmlUrl = @"C:\Users\Administrator\Desktop\demo\DemoTimers\TimersConfig.xml";
        bool IsTimers = true;
        public TimerJob()
        {

        }
        public void Start()
        {
            TimerModel timerModel = ConfigureXML.GetValueByCode(timerXmlUrl);
            if (timerModel != null && timerModel.Start)
            {
                Timer timer = timer = new Timer(timerModel.Interval);
                timer.AutoReset = timerModel.AutoReset;
                timer.Enabled = timerModel.Enabled;
                timer.Elapsed += new ElapsedEventHandler(OnElapsedEvent);
                timer.Start();
            }
        }
        // <summary>
        /// Timer的Elapsed事件执行的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnElapsedEvent(Object sender, ElapsedEventArgs e)
        {
            TimerModel timerModel = ConfigureXML.GetValueByCode(timerXmlUrl);
            //控制是否执行逻辑代码
            if (timerModel != null && !timerModel.Suspend)
            {
                //限制上次执行完成
                if (IsTimers)
                {
                    IsTimers = false;
                    //执行代码
                    var time = DateTime.Now;

                    Console.Write("执行代码:" + time);
                    Console.WriteLine();

                    IsTimers = true ;
                }
            }
        }

    }
}

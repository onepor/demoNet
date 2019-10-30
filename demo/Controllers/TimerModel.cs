using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTimers
{
    public class TimerModel
    {
        /// <summary>
        /// 获取或设置一个值，该值指示 System.Timers.Timer 是应在每次指定的间隔结束时引发 System.Timers.Timer.Elapsed事件，还是仅在指定的间隔第一次结束后引发该事件。
        /// </summary>
        public bool AutoReset { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示 System.Timers.Timer 是否应引发 System.Timers.Timer.Elapsed 事件。
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 获取或设置引发 System.Timers.Timer.Elapsed 事件的间隔
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// 是否启用定时器
        /// </summary>
        public bool Start { get; set; }

        /// <summary>
        /// 是否暂停定时器
        /// </summary>
        public bool Suspend { get; set; }
    }
}

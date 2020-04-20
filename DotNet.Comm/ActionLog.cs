using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Comm
{
    public class ActionLog
    {
        public string ActionLogId { get; set; }
        public string ActionLogName { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }
}

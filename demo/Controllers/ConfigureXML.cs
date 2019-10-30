using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DemoTimers
{
    /// <summary>
    /// 读取xml配置文件
    /// </summary>
    public class ConfigureXML
    {
        public static TimerModel GetValueByCode(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);
            if (doc == null)
            {
                return null;
            }
            XmlNode timerNode = doc.SelectSingleNode("Timer");
            if (timerNode == null)
            {
                return null;
            }

            TimerModel timerModel = new TimerModel();
            try
            {
                timerModel.AutoReset = timerNode.SelectSingleNode("AutoReset").InnerText.Equals("1") ? true : false;
            }
            catch
            {
                timerModel.AutoReset = true;
            }
            try
            {
                timerModel.Enabled = timerNode.SelectSingleNode("Enabled").InnerText.Equals("1") ? true : false;
            }
            catch
            {
                timerModel.Enabled = true;
            }
            try
            {
                timerModel.Interval = Convert.ToDouble("0" + timerNode.SelectSingleNode("Interval").InnerText);
            }
            catch
            {
                timerModel.Interval = 60000d;
            }
            try
            {
                timerModel.Start = timerNode.SelectSingleNode("Start").InnerText.Equals("1") ? true : false;
            }
            catch
            {
                timerModel.Start = false;
            }
            try
            {
                timerModel.Suspend = timerNode.SelectSingleNode("Suspend").InnerText.Equals("1") ? true : false;
            }
            catch
            {
                timerModel.Suspend = true;
            }
            return timerModel;
        }
    }
}

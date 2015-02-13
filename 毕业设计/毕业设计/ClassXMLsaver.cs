using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;

namespace 毕业设计
{
    class XMLsaver
    {
        //xml文件的位置
        private string xmlFileLocation = @"..\..\XMLFileDeviceHistory.xml";
        XElement DealerElememt;     //文件对象

        /// <summary>
        /// 初始化时候就看文件是否存在，不在就创建一个
        /// </summary>
        public XMLsaver()
        {
            try
            {
                DealerElememt = XElement.Load(xmlFileLocation);
            }
            catch
            {
                XmlDocument doc = new XmlDocument();//如果文件不存在，就创建一个新的
                XmlDeclaration Declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(Declaration);
                XmlElement root = doc.CreateElement("DeviceTable");
                doc.AppendChild(root);
                doc.Save(xmlFileLocation);
            }
        }

        /// <summary>
        /// 查找到新设备插入
        /// </summary>
        /// <param name="Device"></param>
        public void XMLInsertDevice(TempHumiDevice Device)
        {
            DealerElememt = XElement.Load(xmlFileLocation);
            XElement InsertRecord = new XElement(
                new XElement("Device",
                    new XAttribute("DeviceAddr", Device.DeviceAddr),
                    new XElement("FirstCheckInTime", DateTime.Now.ToString()),
                    new XElement("NameInApp", "新增设备")
                    ));
            DealerElememt.Add(InsertRecord);
            DealerElememt.Save(xmlFileLocation);     
        }

        /// <summary>
        /// 查找在xml文件中是否存在该设备
        /// </summary>
        /// <param name="Device"></param>
        /// <returns></returns>
        public XElement QueryDevice(TempHumiDevice Device)
        {   //找出设备
            DealerElememt = XElement.Load(xmlFileLocation);
            IEnumerable<XElement> elememntList = from ele in DealerElememt.Elements("Device")
                                                 where ele.Attribute("DeviceAddr").Value == Device.DeviceAddr
                                                 select ele;
            //找到就返回
            return elememntList.Count()>0 ? elememntList.First() : null;
        }

        /// <summary>
        /// 在xml文件中查找旧的NameInApp
        /// </summary>
        /// <param name="Device"></param>
        /// <returns></returns>
        public string ReloadDeviceOldNameInApp(TempHumiDevice Device)
        {   //返回旧的名字
            XElement xe = QueryDevice(Device);
            try
            {
                return xe.Element("NameInApp").Value;
            }
            catch   //出错就返回一个默认的
            {
                return "新增设备";
            }            
        }

        /// <summary>
        /// 更新设备在XML文件中的相应数据
        /// </summary>
        /// <param name="Device"></param>
        /// <param name="ParamID"></param>
        public void UpdateDeviceElememtInXML(TempHumiDevice Device,string Name,object Value)
        {
            try
            {
                XElement targetElement = QueryDevice(Device);
                targetElement.SetElementValue(Name, Value);
                DealerElememt.Save(xmlFileLocation);//保存起来
            }
            catch
            {

            }

        }

        /// <summary>
        /// 设备更新订阅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeviceData_Update(object sender, DeviceEventArgs e)
        {
            TempHumiDevice Device = sender as TempHumiDevice;
            if (e.UpdateElementXName == "NameInDevice")
            {
                UpdateDeviceElememtInXML(Device, e.UpdateElementXName, Device.DeviceName);
            }
            else if (e.UpdateElementXName == "LastDisapperTime")
            {
                UpdateDeviceElememtInXML(Device, e.UpdateElementXName, DateTime.Now.ToString());
            }
            else if (e.UpdateElementXName == "NameInApp")
            {
                UpdateDeviceElememtInXML(Device, e.UpdateElementXName, Device.NameInApplication);
            }
        }
    }
}

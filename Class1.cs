using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Xml.Linq;
using System.Threading;
using Org.BouncyCastle.Utilities;


namespace Mynamespace
{
    public static class SerialCom
    {
        public static SerialPort com { get; set; } = new SerialPort();

        public static string? com_name { get; set; }
        // 波特率

        public static int com_Bound { get; set; }
        //数据位

        public static int com_DataBit { get; set; }
        // 校验位

        public static string? com_Verify { get; set; }
        // 停止位

        public static string? com_StopBit { get; set; }
        // 串口的打开状态标记位

        public static bool OpenState { get; set; }
        //数据显示
        public static List<string> comdata = new List<string>();
    }
    public class Mycom
    {
        public void ComOpen()
        {
            if (SerialCom.OpenState == false)
            {
                SerialCom.com.PortName = SerialCom.com_name;
                SerialCom.com.BaudRate = SerialCom.com_Bound;
                SerialCom.com.DataBits = SerialCom.com_DataBit;
                if (SerialCom.com_StopBit == "1") SerialCom.com.StopBits = System.IO.Ports.StopBits.One;
                if (SerialCom.com_StopBit == "2") SerialCom.com.StopBits = System.IO.Ports.StopBits.Two;

                if (SerialCom.com_Verify == "None") SerialCom.com.Parity = System.IO.Ports.Parity.None;
                if (SerialCom.com_Verify == "odd") SerialCom.com.Parity = System.IO.Ports.Parity.Odd;
                if (SerialCom.com_Verify == "Even") SerialCom.com.Parity = System.IO.Ports.Parity.Even;
                SerialCom.com.NewLine = "\r\n";
                comthread();

            }
            else
            {
                // 关闭串口
                SerialCom.comdata.Add("关闭串口");
                SerialCom.OpenState = false;
                SerialCom.com.Close();
            }
        }


        private void ReadDada()
        {
            SerialCom.comdata.Add("打开串口完成");

            SerialCom.OpenState = true;
            while (SerialCom.OpenState)
            {
                Thread.Sleep(50);
                try
                {
                    //查询串口中目前保存了多少数据
                    int n = SerialCom.com.BytesToRead;
                    byte[] buf = new byte[n];
                    SerialCom.com.Read(buf, 0, n);
                    if (buf.Length > 0)
                    {
                        string str = Encoding.Default.GetString(buf);
                        SerialCom.comdata.Add(str);
                    }
                }
                catch
                {
                    SerialCom.OpenState = false;
                    SerialCom.com.Close();

                }
            }
        }
        private void comthread()
        {
            SerialCom.com.Open();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            SerialCom.com.Encoding = Encoding.GetEncoding("GB2312");
            Thread thread = new Thread(ReadDada);
            thread.IsBackground = true;
            thread.Start();
        }


        public void WriteData(byte[] bytes)
        {
            try
            {
                if (SerialCom.OpenState && bytes != null)
                {
                    SerialCom.com.Write(bytes, 0, bytes.Length);
                }
            }

            catch
            {

            }
        }





    }

}
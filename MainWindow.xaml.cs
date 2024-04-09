using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Threading;
using Mynamespace;
using System.Text.Encodings;
using Org.BouncyCastle.Ocsp;
using System.Data;
namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dtimer = new DispatcherTimer();
        private Mycom mCom = new Mycom();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (SerialCom.OpenState == true)
                Revdata.Text = string.Join("\n", SerialCom.comdata);

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Senddata.IsFocused)
            {
                // 阻止Enter键在TextBox中产生默认的换行行为  
                e.Handled = true;

                // 调用按钮的点击事件  
                mCom.WriteData(Encoding.UTF8.GetBytes(Senddata.Text));
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtimer.Interval = TimeSpan.FromMilliseconds(100);
            dtimer.Tick += new EventHandler(timer_Tick);
            dtimer.Start();
            string[] ports = SerialPort.GetPortNames();
            this.PortName.ItemsSource = ports;
            this.PortName.SelectedIndex = 0;

            string[] baudrate = new string[] { "300", "600", "1200", "2400", "4800", "9600", "19200", "38400", "43000", "56000", "57600", "115200" };
            this.Buadrate.ItemsSource = baudrate;
            this.Buadrate.SelectedIndex = 0;

            this.Databit.Items.Add(8);
            this.Databit.Items.Add(7);
            this.Databit.SelectedIndex = 0;

            this.Stopbit.Items.Add(2);
            this.Stopbit.Items.Add(1);
            this.Stopbit.SelectedIndex = 0;

            this.Parity.Items.Add("None");
            this.Parity.Items.Add("Even");
            this.Parity.Items.Add("Odd");
            this.Parity.SelectedIndex = 0;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            this.PortName.ItemsSource = ports;
            this.PortName.SelectedIndex = 0;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            mCom.WriteData(Encoding.UTF8.GetBytes(Senddata.Text));
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            SerialCom.com_name = PortName.Text;
            SerialCom.com_Bound = int.Parse(Buadrate.Text);
            SerialCom.com_DataBit = int.Parse(Databit.Text);
            SerialCom.com_StopBit = Stopbit.Text;
            SerialCom.com_Verify = Parity.Text;
            mCom.ComOpen();
        }

    }
}


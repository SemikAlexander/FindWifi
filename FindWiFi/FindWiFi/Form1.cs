using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NativeWifi;

namespace FindWiFi
{
    public partial class Form1 : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wmsg, int wparam, int lparam);
        public Form1()
        {
            InitializeComponent();
        }

        #region Design
        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            WlanClient client = new WlanClient();

            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                //очищаем листвью, что бы не дублировать найденные сети при повторном нажатии
                dataGridView1.Rows.Clear();
                foreach (Wlan.WlanAvailableNetwork network in wlanIface.GetAvailableNetworkList(0))
                {
                    //назначаем ему имя нашей первой найденной сети, в конце убираем нулевые символы - Trim((char)0)
                    //узнаём дополнительную информацию о сети и так же добавляем её в листвью, но уже в наш только что созданный итэм.
                    //network.wlanSignalQuality.ToString() + "%" - качество связи в процентах
                    //network.dot11DefaultAuthAlgorithm.ToString().Trim((char)0) - тип безопасности
                    //network.dot11DefaultCipherAlgorithm.ToString().Trim((char)0) - тип шифрования
                    //ну и добавляем элемент непосредственно в листвью
                    dataGridView1.Rows.Add(System.Text.Encoding.ASCII.GetString(network.dot11Ssid.SSID).Trim((char)0), network.wlanSignalQuality.ToString() + "%", network.dot11DefaultAuthAlgorithm.ToString().Trim((char)0), network.dot11DefaultCipherAlgorithm.ToString().Trim((char)0));
                }
            }
        }
    }
}

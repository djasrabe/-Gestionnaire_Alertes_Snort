using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Text.RegularExpressions;
using System.Threading;


using System.Timers;


namespace Gestionnaire_Alertes_Snort
{
    public partial class UserControl_Test : UserControl
    {
        public UserControl_Test()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("cmd.exe", "/C echo test"); 
 


        }

        private static System.Timers.Timer montimer;
        private void button2_Click(object sender, EventArgs e)
        {
            montimer = new System.Timers.Timer (5000);
            montimer.Elapsed += new System.Timers.ElapsedEventHandler(onMafonction);

            //montimer.Interval = 5000;
            montimer.Enabled=true;
            MessageBox.Show("press the enter ke to exit");
            
        }

        private static void onMafonction(object sender, ElapsedEventArgs e)
        {
            MessageBox.Show("Bonjour djas le timer fonctionne.  Intervalle" + e.SignalTime);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReactionClass reac = new ReactionClass();
            reac.Filtrer_IP();

        //    byte[] bytes;
        //    NetworkStream netStream;
        //    TcpClient tcpClient;

        //    tcpClient = new TcpClient();
        //    tcpClient.Connect("192.168.16.4", 23);
        //    netStream = tcpClient.GetStream();

        //    Byte[] sendBytes = Encoding.UTF8.GetBytes("djas\r\nbeckham23\r\nshow version\r\n");
        //    // Byte[] sendBytes = Encoding.UTF8.GetBytes("beckham23\r\nShow ip interface brief\r\n");
        //    // Byte[] sendBytes = Encoding.UTF8.GetBytes("beckham23\r\nenable\r\nbeckham23\r\nconf t\r\n int fa0/1\r\nip address 192.168.12.1 255.255.255.0\r\nno shut\r\nexit\r\nexit\r\nsh ip int brief\n");

        //    netStream.Write(sendBytes, 0, sendBytes.Length);
        //    bytes = new byte[1000];
        //    System.Threading.Thread.Sleep(2000);
        //    netStream.Read(bytes, 0, 1000);
        //    string returndata = Encoding.UTF8.GetString(bytes);
        //    MessageBox.Show("" + returndata);
                                  
        }
    }
}
 
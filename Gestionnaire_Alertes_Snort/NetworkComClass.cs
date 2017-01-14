using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.Sockets;

namespace Gestionnaire_Alertes_Snort
{
    class NetworkComClass
    {

        public void CommanderRouteur(string IPaddressRouteur,string MotdePasse,string LesCommandesIos)
        {
            byte[] bytes;
            NetworkStream netStream;
            TcpClient tcpClient;

            tcpClient = new TcpClient();
            tcpClient.Connect(IPaddressRouteur, 23);
            netStream = tcpClient.GetStream();

             //Byte[] sendBytes = Encoding.UTF8.GetBytes("beckham23\r\nshow version\r\n");
             Byte[] sendBytes = Encoding.UTF8.GetBytes(MotdePasse + LesCommandesIos);        
            // Byte[] sendBytes = Encoding.UTF8.GetBytes("beckham23\r\nenable\r\nbeckham23\r\nconf t\r\n int fa0/1\r\nip address 192.168.12.1 255.255.255.0\r\nno shut\r\nexit\r\nexit\r\nsh ip int brief\n");

            netStream.Write(sendBytes, 0, sendBytes.Length);
            bytes = new byte[1000];
            System.Threading.Thread.Sleep(2000);
            netStream.Read(bytes, 0, 1000);
            string returndata = Encoding.UTF8.GetString(bytes);
            MessageBox.Show("" + returndata);
        
        }
                
        
    }
}


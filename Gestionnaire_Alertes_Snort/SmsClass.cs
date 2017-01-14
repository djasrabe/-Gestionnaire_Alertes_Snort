using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
    class SmsClass
    {
        SerialPort serialPort;
        public SmsClass(string comPort)
        {
            this.serialPort = new SerialPort();
            this.serialPort.PortName = comPort;
            this.serialPort.BaudRate = 9600;
            this.serialPort.Parity = Parity.None;
            this.serialPort.DataBits = 8;
            this.serialPort.StopBits = StopBits.One;
            this.serialPort.Handshake = Handshake.RequestToSend;
            this.serialPort.DtrEnable = true;
            this.serialPort.RtsEnable = true;
            this.serialPort.NewLine = System.Environment.NewLine;
        }
        public bool sendSms(string NumTel, string CorpsSms)
        {
            string messages = null;
            messages = CorpsSms;
            if (this.serialPort.IsOpen == true)
            {
                try
                {
                    this.serialPort.WriteLine("AT" + (char)(13));
                    Thread.Sleep(4);
                    this.serialPort.WriteLine("AT+CMGF=1" + (char)(13));
                    Thread.Sleep(5);
                    this.serialPort.WriteLine("AT+CMGS=\"" + NumTel + "\"");
                    Thread.Sleep(10);
                    this.serialPort.WriteLine(">" + messages + (char)(26));                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Source);
                }
                return true;
            }
            else
                return false;
        }

        public void Opens(bool AutomatiqueMode)
        {
            if (this.serialPort.IsOpen == false)
            {
                try
                {
                    this.serialPort.Open();
                }
                catch
                {
                   if(AutomatiqueMode==false) MessageBox.Show("Aucun Modem GSM detecté");
                }
            }
        }
        public void Closes()
        {
            if (this.serialPort.IsOpen == true)
            {
                this.serialPort.Close();
            }
        }


    }
}

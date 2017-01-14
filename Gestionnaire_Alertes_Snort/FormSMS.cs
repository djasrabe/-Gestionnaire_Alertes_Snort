using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Gestionnaire_Alertes_Snort
{
    public partial class FormSMS : Form
    {
        public FormSMS()
        {
            InitializeComponent();
            loadPorts();
        }
        private void loadPorts()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                cboPorts.Items.Add(port);
            }
        }

        private void Envoyer_Click(object sender, EventArgs e)
        {
            bool estAutomatique = false;

            SmsClass sm = new SmsClass(cboPorts.Text);
            sm.Opens(estAutomatique);
            sm.sendSms(txtPhone.Text, txtMessage.Text);
            sm.Closes();
          
        }
    }
}

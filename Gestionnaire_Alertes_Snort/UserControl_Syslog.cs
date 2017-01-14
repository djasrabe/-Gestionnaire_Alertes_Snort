using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
    public partial class UserControl_Syslog : UserControl
    {
        public UserControl_Syslog()
        {
            InitializeComponent();
        }
         public bool auto;
        public UserControl_Syslog(bool t)
        {
            InitializeComponent();
            auto = t;
        }

        private void UserControl_Syslog_Load(object sender, EventArgs e)
        {

            AccesBDsyslog obj1 = new AccesBDsyslog();
            dataGridView1.DataSource = obj1.Visualiser("select * from logs");                      //obj1.Visualiser("acid_event", "*", "1=1");  

            AccesBDsyslog obj2 = new AccesBDsyslog();
            dataGridView2.DataSource = obj2.Visualiser("select * from AlerteSyslog");

        }
        bool tri = true;
        private void Actualiser_Click(object sender, EventArgs e)
        {
            TraitementClass traitsys = new TraitementClass();
            traitsys.TraiterAlerteSyslog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReactionClass al = new ReactionClass();
            al.ExecuterReactionSyslog(tri);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;


namespace Gestionnaire_Alertes_Snort
{
    public partial class UserControl_Evenement : UserControl
    {
        public UserControl_Evenement()
        {
            InitializeComponent();
        }

        private void UserControl_Evenement_Load(object sender, EventArgs e)
        {         
            TraitementClass tr = new TraitementClass();
            dataGridView1.DataSource = tr.LireEvenementSnort();
            //---------------------------------------------
            AccesBD obj1 = new AccesBD();
            dataGridView2.DataSource =obj1.Visualiser("acid_event", "*","1=1");              
        }

        private void Actualiser_Click(object sender, EventArgs e)
        {
            TraitementClass tr = new TraitementClass();
            dataGridView1.DataSource = tr.LireEvenementSnort();
            //---------------------------------------------
            AccesBD obj1 = new AccesBD();
            dataGridView2.DataSource = obj1.Visualiser("acid_event", "*", "1=1");            

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class UserControl_Evenement_Regroupe : UserControl
    {
        public UserControl_Evenement_Regroupe()
        {
            InitializeComponent();
        }

        public bool auto;
        public UserControl_Evenement_Regroupe(bool t)
        {
            InitializeComponent();
            auto = t;
        }

        private void UserControl_Evenement_Regroupe_Load(object sender, EventArgs e)
        {    
             label1.Text = "" + auto;        
          //
            AccesBD obj = new AccesBD();
            dataGridView1.DataSource = obj.Visualiser("select * from signature ");

            TraitementClass trait = new TraitementClass();
            dataGridView2.DataSource = trait.AgregerEvenementsParSignature();

            //Gestion des Events           
            if (auto == true)
            {
                AutomatisationClass monauth = new AutomatisationClass();
                monauth.ModeAutoActive += button1_Click; //
                monauth.LeveeEvenement();                           
            }
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
             object [] tab= new object [20];
             int nligne = dataGridView2.Rows.Count;

             for (int i = 0; i < nligne - 1; i++)
             {
                 for (int j = 0; j < 10; j++)
                 {
                     object donnee = (object)dataGridView2.Rows[i].Cells[j].Value;
                     tab[j] = donnee;
                 }                 
                 int a = Convert.ToInt32(tab[8]);
                 int b = Convert.ToInt32(tab[5]);
                 
                 TraitementClass traitement = new TraitementClass(tab[2], tab[3], tab[6].ToString(), tab[7].ToString(), a,b,Convert.ToInt32(tab[1]));
                 traitement.EnregistrerIntrusionsAgreges(Convert.ToInt32(tab[5]));                 
             }
             if (auto == false) MessageBox.Show("Insertion ou Mise a jour reussie de  " + (nligne - 1) + "  Evenements ");
        }
    }
}

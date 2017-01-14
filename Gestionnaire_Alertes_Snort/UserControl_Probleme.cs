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
//using System.Data.SqlClient;

namespace Gestionnaire_Alertes_Snort
{
    public partial class UserControl_Probleme : UserControl
    {
        public UserControl_Probleme()
        {
            InitializeComponent();
        }
        

        public bool auto;
        public UserControl_Probleme(bool t)
        {
            InitializeComponent();
            auto = t;
        }

        private void button2_Click(object sender, EventArgs e)
        {              
            object [] tab= new object [20];
            int nligne = dataGridView1.Rows.Count;

            for (int i=0;i<nligne-1 ;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    object donnee = (object)dataGridView1.Rows[i].Cells[j].Value;
                    tab[j]= donnee;
                } 
                            
                AlertesClass alerte = new AlertesClass(tab[1],null,Convert.ToInt32(tab[0]));
                alerte.GenererAlertes(Convert.ToInt32(tab[0]));
            }
            if (auto == false) MessageBox.Show("Insertion ou Mise a jour reussie de  " + (nligne - 1) + "  Problemes de securite");            
         }

        
        private void UserControl_Probleme_Load(object sender, EventArgs e)
        {
            //Les intrusions problematiques           
            TraitementClass traitement = new TraitementClass();
            dataGridView1.DataSource=traitement.DeciderIntrusionProblematiques();
            
            ////Les Problemes
            AlertesClass alerte = new AlertesClass();
            dataGridView2.DataSource= alerte.LireAlertes();

            //Gestion des Events
            //if (auto == true)
            //{
            //    AutomatisationClass monauth = new AutomatisationClass();
            //    monauth.ModeAutoActive += button2_Click; //
            //    monauth.ModeAutoActive += button1_Click; //
            //    monauth.LeveeEvenement();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReactionClass reaction = new ReactionClass();
            reaction.ExecuterReaction(auto);
        }//fin de la methode

        private void EnvoyerSMS_Click(object sender, EventArgs e)
        {
            FormSMS maform = new FormSMS();
            maform.Show();
        }



    }
}

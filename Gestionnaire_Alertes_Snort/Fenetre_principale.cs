using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
    public partial class Fenetre_principale : Form
    {
        public Fenetre_principale()
        {
            InitializeComponent();
        }

     


        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Evenement());
        }

        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Evenement_Regroupe());
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Probleme());
        }

        private void linkLabel4_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Test());
        }



        //gerer les evenements
        private void Commencer_Click(object sender, EventArgs e)
        {
            AutomatisationClass monauth = new AutomatisationClass();
            monauth.ModeAutoActive += onLkLabel3;
            monauth.ModeAutoActive += onArriveeClient;
            monauth.ModeAutoActive += onLkLabel2;
            monauth.LeveeEvenement();
        }

        private static void onArriveeClient(object sender, EventArgs e)
        {
            MessageBox.Show("l'evenement est bon");
        }
        private void onLkLabel3(object sender, EventArgs e)
        {
            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Evenement_Regroupe());
        }

        private void onLkLabel2(object sender, EventArgs e)
        {

            splitContainer2.Panel2.Controls.Clear();
            splitContainer2.Panel2.Controls.Add(new UserControl_Probleme());
        }
    }
}

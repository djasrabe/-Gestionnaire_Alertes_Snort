using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace Gestionnaire_Alertes_Snort
{
    public partial class Interface : Form
    {
        private BackgroundWorker bw;
        public bool IsAutomatique=false;  //variable ki definit le traitement automatik

        public bool TraitementAutoEnCours = false;

        public Interface()
        {
            InitializeComponent();
               if (automatiqueToolStripMenuItem.Checked == true)  IsAutomatique = true;
               if (manuelToolStripMenuItem.Checked == true)       IsAutomatique = false;

               bw = new BackgroundWorker
               {
                   WorkerSupportsCancellation=true,
                   WorkerReportsProgress=true
               };
               bw.DoWork += bw_DoWork;
               bw.ProgressChanged += bw_ProgressChanged;
               bw.RunWorkerCompleted += bw_RunWorkerCompleted;
        }     
        //def des methodes pr le backgroundworker
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
           BackgroundWorker bw = (BackgroundWorker)sender;
         
  
          
          progressBar1.BeginInvoke(new Action(() =>
           {
               progressBar1.Visible = false;
               progressBar1.Maximum = 3;
               progressBar1.Minimum = 0;
               progressBar1.Step = 1;
           }
            ));
            
                TraitementClass trait = new TraitementClass();
                trait.EnregistrerIntrusionsAgregesAuto();

             progressBar1.BeginInvoke(new Action(() =>
              {
                  progressBar1.Visible = true;
                  progressBar1.PerformStep();
               }
            ));

            //enregistrer pbm
                AlertesClass alerte = new AlertesClass();
                alerte.GenererAlertesAuto();
                progressBar1.BeginInvoke(new Action(() =>
               {
                progressBar1.PerformStep();
               }

            ));
                
            //Reagir
                ReactionClass reaction = new ReactionClass();
                reaction.ExecuterReaction(IsAutomatique);
               progressBar1.BeginInvoke(new Action(() =>
           {
             
               progressBar1.PerformStep();
               progressBar1.Visible = false;
               progressBar1.Value = 0;
           }
            ));

               if (bw.CancellationPending == true)
               {
                   e.Cancel = true;

               }



        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           // BackgroundWorker worker = (BackgroundWorker)sender;
            //MessageBox.Show("continue");
           // textBox1.Text = e.ProgressPercentage.ToString() + " %";
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //BackgroundWorker worker = (BackgroundWorker)sender;

            if (e.Cancelled)
            {
                MessageBox.Show("Vous avez arreté le traitement automatique");
                EstDemmare = false;
            }            
            else    MessageBox.Show("Traitement terminé" );

            TraitementAutoEnCours = false;
        }


        public bool dispo()
        {
            return IsAutomatique;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer3.Panel1.Controls.Clear();
            splitContainer3.Panel1.Controls.Add(new UserControl_Evenement());
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer3.Panel1.Controls.Clear();
           splitContainer3.Panel1.Controls.Add(new UserControl_Evenement_Regroupe());
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer3.Panel1.Controls.Clear();
            splitContainer3.Panel1.Controls.Add(new UserControl_Probleme());
        }
        //Gestion automatique des evenments

        public bool EstDemmare = false;
        private static System.Timers.Timer montimer;
        private void TraitementAuto_Click(object sender, EventArgs e)
        {
            if (IsAutomatique == false) MessageBox.Show("Le Mode automatique n'est pas activé!!!! Faites le d'abord dans le Menu Parametres");
            else
            {
                if (EstDemmare == false)
                {
                    EstDemmare = true;
                    DialogResult dialogResult = MessageBox.Show("Demmarer le traitement Autamatique en arriere plan?", "Démarrage du Traitrement automatique", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        TraitementAutoEnCours = true;
                        montimer = new System.Timers.Timer(1000);
                        montimer.Elapsed += new System.Timers.ElapsedEventHandler(onBoucle);
                        montimer.Interval = 5000;
                        montimer.Enabled = true;
                        TraitementAutoEnCours = true;
                        //if (!bw.IsBusy)
                        //bw.RunWorkerAsync();

                    }
                }
                else MessageBox.Show("Le demarrage est deja en cours");
            }
            
        }
        public void onBoucle(object sender,EventArgs e )
        {
            if (!bw.IsBusy)
                bw.RunWorkerAsync();
                TraitementAutoEnCours = true;
        }
                  
        
        
        //gestion des menus


        private void manuelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsAutomatique == false) MessageBox.Show("Le Mode de traitement actuel est deja manuel");
            else
            {
                if (EstDemmare == true) MessageBox.Show("Vous ne pouveez changer de mode.Arretez le traitement automatique");
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Activer le Mode de traitement Manuel?", "Traitrement Manuel", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        IsAutomatique = false;

                        automatiqueToolStripMenuItem.Checked = false;
                        manuelToolStripMenuItem.Checked = true;
                    }
                }
            }

             
        }

        private void automatiqueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsAutomatique == true) MessageBox.Show("Le Mode de traitement actuel est deja automatique");
            else
            {
                DialogResult dialogResult = MessageBox.Show("Activer le Mode de traitement Automatique?", "Traitrement Automatique", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    IsAutomatique = true;
                    manuelToolStripMenuItem.Checked = false;
                    automatiqueToolStripMenuItem.Checked = true;
                }
            }


        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Quitter ?", "Fermeture de l'application", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Close();
            }  
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Ouvrir le shell du routeur ?", "Shell Routeur", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    // System.Diagnostics.Process mm = System.Diagnostics.Process.Start("C:\\Windows\\System32\\cmd.exe");//, "/C echo test"); 
                    System.Diagnostics.Process mm = System.Diagnostics.Process.Start("C:\\Users\\nadjial\\Documents\\Visual Studio 2012\\Projects\\Gestionnaire_Alertes_Snort\\ComTelnet\\bin\\Debug\\ComTelnet.exe");
                }
                catch
                {
                    MessageBox.Show("Le Routeur est innaccessible"); 
                }
            }
        
        }

        private void Interface_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;
        }

        private void ArreterTraitementAuto_Click(object sender, EventArgs e)
        {
            if (TraitementAutoEnCours == false) MessageBox.Show("Le traitement automatique n'est pas en cours d'execution");
            else
            { 
              DialogResult dialogResult = MessageBox.Show("Arreter le traitement automatique en arriere plan?", "Arret  traitement", MessageBoxButtons.YesNo);
              if (dialogResult == DialogResult.Yes)
              {
                  if (bw.IsBusy)//if (bw.WorkerSupportsCancellation)
                  {
                      bw.CancelAsync();
                      TraitementAutoEnCours = false;
                       montimer.Stop();
                       
                  }
              }
            }
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            splitContainer3.Panel1.Controls.Clear();
            splitContainer3.Panel1.Controls.Add(new UserControl_Syslog());
        }      

        //
        
    }
}

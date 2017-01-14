using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
    public class ReactionClass
    {
        public ReactionClass()
        { 
        }
         private void Envoyer_Mail(string Objet ,string Corps,bool auto)
        {
            MailClass monmail = new MailClass("djasrabeaymard@gmail.com", "djasrabeaymard@yahoo.fr", Objet, Corps); //un fichier de config sera ajouté pr enregistrer ces parametres
            monmail.SendMail("smtp.gmail.com", "djasrabeaymard@gmail.com", "beckham23",auto);      
        }
        private void Envoyer_Sms(string Corps,bool auto)
        {
            SmsClass sm = new SmsClass("COM11");
            sm.Opens( auto);
            sm.sendSms("66595701", Corps);
            sm.Closes();
        }
        public void Filtrer_IP( )//string IpAFiltrer)
        {
            NetworkComClass nt = new NetworkComClass();
            nt.CommanderRouteur("192.168.16.4", "beckham23","\r\nshow version\r\n");  // test si le routeur est bien atteint par la commande envoyé
            // nt.CommanderRouteur("192.168.16.4", "beckham23","\r\nenable\r\nbeckham23\r\nconf t\r\n int fa0/1\r\nip address 192.168.12.1 255.255.255.0\r\nno shut\r\nexit\r\nexit\r\nsh ip int brief\n");

        }
               
        public void ExecuterReaction (bool AutomatiqueMode)      //(int NumPro)
        {   
            List <object> info =AlertesClass.QuelquesInfos();
            Int64 rec =(Int64)info[0];
            if (AutomatiqueMode==false)MessageBox.Show("nombre de probleme de securité a traiter   " + rec);          

            if (rec != 0)
            {
                int min =(int)info[1];
                int max = (int)info[2];

                if (AutomatiqueMode == false) MessageBox.Show("min  " + min);
                if (AutomatiqueMode == false) MessageBox.Show("Max  " + max);

                for (Int64 i = min; i <= max; i++)
                {
                    AlertesClass alerte = new AlertesClass();
                    List<object> list = alerte.RetournerCaraceristiquesAlertes(i); //recuperer les infos sur une alerte

                    // formatage des messages a envoyer par mail et/ou sms
                    string objet = "Attaque  :" + list[5] + "  du:  " + list[1];
                    string corpsMail = "vous subissez une Intrusion.Caracteristiques:---Numero Probleme:  " + i + "---Numero Intrusion : " + list[0] + "---Numero Signature : " + list[5] + "---Nom de l'attaque : " + list[6] + "---Categorie : " + list[7] + " ---IP source : " + list[2] + " ---IP destination : " + list[3] + "---Protocole : " + list[4];
                    string corpsSms = "Intrusion!!! N°Pbleme:" + i + "--N° Intrusion:" + list[0] + "-Sid:" + list[5] + ":" + list[6] + "-Categorie:" + list[7] + "-IP source:" + list[2] + "-IP Cible:" + list[3] + "-Protocole:" + list[4];
                    //
                    string Categorie = alerte.IdentifierCategorieAlerte(i);//recuperer la categorie de l'alerte
                    if (AutomatiqueMode == false) MessageBox.Show("Code de l'Action à Engager suivant la categorie du Probleme :   " + Categorie);

                    if (Categorie == "Act_M")
                    {
                        Envoyer_Mail(objet, corpsMail, AutomatiqueMode);
                        AlertesClass.MarquerCommeTraiter(i);
                    }
                    if (Categorie == "Act_MS")
                    {
                        Envoyer_Sms(corpsSms, AutomatiqueMode);
                        Envoyer_Mail(objet, corpsMail, AutomatiqueMode);
                       // AlertesClass.MarquerCommeTraiter(i);
                    }
                    if (Categorie == "Act_FMS")
                    {
                        //Filtrer_IP();
                        Envoyer_Mail(objet, corpsMail, AutomatiqueMode);
                       // Envoyer_Sms(corpsSms, AutomatiqueMode);
                        //AlertesClass.MarquerCommeTraiter(i);
                    }

                }//end for
                //if (auto == false)
                if (AutomatiqueMode == false) MessageBox.Show("Tous les problemes en attente viennent d'etre traités");

            }//end if
            else //if (auto == false)
                 MessageBox.Show("Aucun Probleme en Attente.tous ont été traités");
        
        }//end methode

        public void ExecuterReactionSyslog(bool AutomatiqueMode)
        { 
        List <object> info =AlertesClass.QuelquesInfosSyslog();
            Int64 rec =(Int64)info[0];
            //if (AutomatiqueMode==false)
                MessageBox.Show("nombre de probleme de securité a traiter   " + rec);

            if (rec != 0)
            {
                int min = (int)info[1];
                int max = (int)info[2];

                //if (AutomatiqueMode == false) 
                MessageBox.Show("min  " + min);
                //if (AutomatiqueMode == false) 
                MessageBox.Show("Max  " + max);

                for (Int64 i = min; i <= max; i++)
                {
                   // AccesBDsyslog ip = new AccesBDsyslog();
                   // string add = (string)ip.Select("select host from AlerteSyslog where Etat is null NumAlerte=" + i );
                   // MessageBox.Show("" + add);
                   // Envoyer_Sms(add, AutomatiqueMode);
                   // Envoyer_Mail("Syslogng Alert", "Le routeur " + "" + add + "est down", AutomatiqueMode);


                    AlertesClass aler = new AlertesClass();
                    List<object> listsyslog = aler.RetournerCaraceristiquesAlertesSyslog(i);
                    string corpsemail= "Le routeur " + "" + listsyslog[0] + "  est down "+"  "+"  Message:  " + listsyslog[1];
                    string corpsSMS = "Le routeur " + "" + listsyslog[0] + " est down ";

                    Envoyer_Mail("SyslogAlert", corpsemail, AutomatiqueMode);
                    //Envoyer_Sms(corpsSMS, AutomatiqueMode);

                    AlertesClass.MarquerAlerteSyslogCommeTraite(i);
                    MessageBox.Show("Tous les alertes Syslog viennent d'etre traités");
                  
                }
            }
            else //if (auto == false)
                MessageBox.Show("Aucun Evenement Syslog a traiter");

        }


    }
}

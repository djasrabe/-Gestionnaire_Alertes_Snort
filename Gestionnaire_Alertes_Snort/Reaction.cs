using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
    public class Reaction
    {
        //string cause;

        public Reaction()
        { 
        }

        public void Envoyer_Mail(string Objet ,string Corps)
        {
            MailClass monmail = new MailClass("djasrabeaymard@gmail.com", "djasrabeaymard@yahoo.fr", Objet, Corps);
            monmail.SendMail("smtp.gmail.com", "djasrabeaymard@gmail.com", "beckham23");
                   
        }
        public void Envoyer_Sms(string Corps)
        {
            SmsClass sm = new SmsClass("COM11");
            sm.Opens();
            sm.sendSms("66595701", Corps);
            sm.Closes();
        }
        public void Filtrer_IP( )//string IpAFiltrer)
        {
            NetworkComClass nt = new NetworkComClass();
            nt.CommanderRouteur("192.168.16.4", "beckham23","\r\nshow version\r\n");        
        }

        public string IdentifierCategorieProbleme()
        {         
            AccesBD cond = new AccesBD();
            string act = (string)cond.Select("select Action.CodeAction from Action,Categorie,Signature_Inventaire,Intrusion,Probleme where NumProbleme=" + i + " and  Probleme.NumIntrusion=Intrusion.NumIntrusion and Intrusion.Signature_Sid=Signature_Inventaire.Signature_Sid and Categorie.CodeCategorie=Signature_Inventaire.CodeCategorie and Action.CodeAction=Categorie.CodeAction");
            return act;    
        }
        public void Executer(int NumeroProbleme)
        {
            AccesBD ObjetInfo = new AccesBD();

            List<object> list = new List<object>();
            list.Add(ObjetInfo.Select("select NumIntrusion from Probleme where NumProbleme=" + NumeroProbleme));
            list.Add(ObjetInfo.Select("select Date_Observation from Probleme where NumProbleme=" + NumeroProbleme));
            list.Add(ObjetInfo.Select("select Ip_Source from Intrusion where NumIntrusion=" + list[0]));
            list.Add(ObjetInfo.Select("select Ip_Destination from Intrusion where NumIntrusion=" + list[0]));
            list.Add(ObjetInfo.Select("select Ip_Protocole from Intrusion where NumIntrusion=" + list[0]));
            list.Add(ObjetInfo.Select("select Signature_Sid from Intrusion where NumIntrusion=" + list[0]));
            list.Add(ObjetInfo.Select("select Nom from Signature_Inventaire where Signature_Sid=" + list[5]));
            list.Add(ObjetInfo.Select("select CodeCategorie from Signature_Inventaire where Signature_Sid=" + list[5]));

            // formatage des messages a envoyer par mail et/ou sms
            string objet = "Attaque  :"  + list[5] + "  du:  " + list[1];
            string corpsMail = "vous subissez une Intrusion.Caracteristiques:---Numero Probleme:  " + NumeroProbleme + "---Numero Intrusion : " + list[0] + "---Numero Signature : " + list[6] + "---Nom de l'attaque : " + list[7] + "---Categorie : " + cat + " ---IP source : " + ip_source + " ---IP destination : " + ip_destination + "---Protocole : " + protocole;

            string corpsSms = "Intrusion!!! N°Pbleme:" + i + "--N° Intrusion:" + numintrusion + "-Sid:" + sid + ":" + nomsignature + "-Categorie:" + cat + "-IP source:" + ip_source + "-IP Cible:" + ip_destination + "-Protocole:" + protocole;

        
        }

    }
}

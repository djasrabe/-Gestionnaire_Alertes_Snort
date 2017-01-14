using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionnaire_Alertes_Snort
{
    public class AlertesClass
    {
        object DateObservation;
        int IntrusionCorrespondant;
        object EtatTraitement;

        public AlertesClass() { }
        public AlertesClass(object DateObservation,object EtatTraitement,int IntrusionCorrespondant)
        {
            this.DateObservation = DateObservation;
            this.EtatTraitement = EtatTraitement; 
            this.IntrusionCorrespondant = IntrusionCorrespondant;
                   
        }

        public object LireAlertes()
        {
            object source;
            AccesBD Acc = new AccesBD();
            source = Acc.Visualiser("Probleme","*", "1=1");
            return source;
        }

        public void GenererAlertes(int numIntrusion)
        {
            AccesBD monacces = new AccesBD();
            Int64 conteneur = (Int64)monacces.Select("select count(NumIntrusion) from Probleme where NumIntrusion=" + numIntrusion);
            
            if (conteneur == 0)
            {
                object[] tabvaleur = new object[7];
                tabvaleur[0] = DateObservation;
                tabvaleur[1] = EtatTraitement ;
                tabvaleur[2] = IntrusionCorrespondant;
              
                AccesBD monAcces = new AccesBD();
                monAcces.Insert("Probleme", "Date_Observation,Etat,NumIntrusion", 3, tabvaleur);
            }
            else
            {
                AccesBD monaccesDB = new AccesBD();
                monaccesDB.Update("Probleme", "Date_Observation ", DateObservation, "NumIntrusion="  + numIntrusion);
            }
        }


        public void GenererAlertesAuto()
        {
            //on decide de l'intrusion problematique et on enregistre le probleme
            AccesBD objetAcces = new AccesBD();

            objetAcces.Requete_Void("create TEMPORARY TABLE  Tabletempo (select Date_Fin,Occurence,NumIntrusion FROM Intrusion WHERE NumIntrusion not in(SELECT NumIntrusion from Probleme) and Occurence > 2)");
            objetAcces.Requete_Void("INSERT into Probleme select '',Tabletempo.Date_Fin,null,Tabletempo.NumIntrusion from Tabletempo");
            objetAcces.Requete_Void("Drop table Tabletempo");  //supprimer la tqble temporaire

            //Pour le update des pbm deja enregisrés  ..pr mettre a jour la date de la dernier occurence

            objetAcces.Requete_Void("create TEMPORARY TABLE  Tabletempo2 (select Date_Fin,Occurence,NumIntrusion FROM Intrusion WHERE NumIntrusion in (SELECT NumIntrusion from Probleme) and Occurence > 2)");
            objetAcces.Requete_Void("Update Probleme INNER JOIN Tabletempo2 ON Probleme.NumIntrusion=Tabletempo2.NumIntrusion SET Date_Observation=Tabletempo2.Date_Fin");
            objetAcces.Requete_Void("Drop table Tabletempo2");  //supprimer la table temporaire

        }
        
        public string IdentifierCategorieAlerte(Int64 NumeroProbleme)
        {
            AccesBD cond = new AccesBD();
            string CodeCat = (string)cond.Select("select Action.CodeAction from Action,Categorie,Signature_Inventaire,Intrusion,Probleme where NumProbleme=" + NumeroProbleme + " and  Probleme.NumIntrusion=Intrusion.NumIntrusion and Intrusion.Signature_Sid=Signature_Inventaire.Signature_Sid and Categorie.CodeCategorie=Signature_Inventaire.CodeCategorie and Action.CodeAction=Categorie.CodeAction");
            return CodeCat;    
        }
        public List<object> RetournerCaraceristiquesAlertes(Int64 NumeroProbleme)
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
           
            return list;         
         }
        public static List<object> QuelquesInfos()
        {
            AccesBD ObjetInfo = new AccesBD();
            List<object> list = new List<object>();
            list.Add(ObjetInfo.Select("select count(NumProbleme) from Probleme where  Etat is  null"));
            list.Add(ObjetInfo.Select("select min(NumProbleme) from Probleme where Etat is null"));
            list.Add(ObjetInfo.Select("select max(NumProbleme) from Probleme where  Etat is  null"));
            return list; 
        }

        public static void MarquerCommeTraiter(Int64 NumProbleme)
        {
            AccesBD monacces = new AccesBD();
            monacces.Requete_Void("update Probleme set Etat=1 where NumProbleme=" + NumProbleme);
        }

        public static void MarquerAlerteSyslogCommeTraite(Int64 NumAlerte)
        {
            AccesBDsyslog monacces = new AccesBDsyslog();
            monacces.Requete_Void("update AlerteSyslog set Etat=1 where NumAlerte=" + NumAlerte);
        }

        //public void RetournerProblemesNonTraites()   //pr gerer le fait de ne ,ettre a jour qu les pbm dont la resolution a abouti  et utiliser for each
        //{
        //    AccesBD acc = new AccesBD();
        //   Int64 nombre=Convert.ToInt64( acc.Select("select COUNT(NumProbleme) from Probleme where Etat is null"));
        //    acc.Select("select NumProbleme from Probleme where Etat is null")

        //}

        public List<object> RetournerCaraceristiquesAlertesSyslog(Int64 NumAlerte)
        {
            AccesBDsyslog ObjetInf = new AccesBDsyslog();

            List<object> listsys = new List<object>();
            listsys.Add(ObjetInf.Select("select host from AlerteSyslog where Etat is null and NumAlerte=" + NumAlerte));
            listsys.Add(ObjetInf.Select("select msg from AlerteSyslog where Etat is null and NumAlerte=" + NumAlerte));
                        
                    return listsys;
        }

       
        public static List<object> QuelquesInfosSyslog()
        {
            AccesBDsyslog ObjetInfo = new AccesBDsyslog();
            List<object> list1 = new List<object>();
            list1.Add(ObjetInfo.Select("select count(NumAlerte) from AlerteSyslog where  Etat is  null"));
            list1.Add(ObjetInfo.Select("select min(NumAlerte) from AlerteSyslog where Etat is null"));
            list1.Add(ObjetInfo.Select("select max(NumAlerte) from AlerteSyslog where  Etat is  null"));
            return list1;
        }

    }
}

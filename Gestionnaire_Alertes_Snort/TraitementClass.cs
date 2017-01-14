using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionnaire_Alertes_Snort
{
    class TraitementClass
    {
        int IntrusionID,Occurence,ProtocoleTransport,SignatureSid;

        string IPsource, IPdestination;
        object  DateDebut, DateFin;

        public TraitementClass()
        { 
        }
        public TraitementClass(object DateDebut,object DateFin,string IPsource,string IPdestination,int ProtocoleTransport ,int SignatureSid,int Occurence)
        {
            this.DateDebut = DateDebut;
            this.DateFin = DateFin;
            this.IPsource = IPsource;
            this.IPdestination = IPdestination;
            this.ProtocoleTransport = ProtocoleTransport;
            this.SignatureSid = SignatureSid;
            this.Occurence = Occurence;
        }

        public object LireEvenementSnort()
        {
            object source;
            AccesBD Acc = new AccesBD();
            source = Acc.Visualiser("event", "cid,signature,timestamp", "1=1"); 
            return source;
        }

        public object AgregerEvenementsParSignature()
        {
            object source;
            AccesBD Acc = new AccesBD();
            source = Acc.Visualiser("select signature,count(signature)as quantite,min(timestamp),max(timestamp),acid_event.sig_name,sig_sid,ip_src,ip_dst,ip_proto,layer4_sport,layer4_dport FROM acid_event,signature WHERE acid_event.signature=signature.sig_id and signature.sig_sid in(select Signature_Sid from Signature_Inventaire) GROUP BY signature");
            return source;
        }

        //public object AgregerEvenementsParSignature()//pr auto
        //{
        //    object source;
        //    AccesBD Acc = new AccesBD();
        //    source = Acc.("select signature,count(signature)as quantite,min(timestamp),max(timestamp),acid_event.sig_name,sig_sid,ip_src,ip_dst,ip_proto,layer4_sport,layer4_dport FROM acid_event,signature WHERE acid_event.signature=signature.sig_id and signature.sig_sid in(select Signature_Sid from Signature_Inventaire) GROUP BY signature");
        //    return source;
        //}

        public void EnregistrerIntrusionsAgreges(int sid)
        {
            AccesBD monacces = new AccesBD();
            Int64 conteneur = (Int64)monacces.Select("select count(Signature_Sid) from Intrusion where Signature_Sid=" + sid);
            if (conteneur == 0)
            {
                object[] tabvaleur = new object[7];
                tabvaleur[0] = DateDebut;
                tabvaleur[1] = DateFin;
                tabvaleur[2] = IPsource;
                tabvaleur[3] = IPdestination;
                tabvaleur[4] = ProtocoleTransport;
                tabvaleur[5] = SignatureSid;
                tabvaleur[6] = Occurence;

                AccesBD maacc = new AccesBD();
                maacc.Insert("Intrusion", "Date_Debut,Date_Fin,Ip_Source,Ip_Destination,Ip_Protocole,Signature_Sid,Occurence", 7, tabvaleur);
            }
            else
            {
                AccesBD monaccesDB = new AccesBD();
                monaccesDB.Update("Intrusion", "Occurence", Occurence, "Signature_Sid=" + sid);
                
            }
        }
        public void EnregistrerIntrusionsAgregesAuto()
        {
           
                AccesBD maacc = new AccesBD();
                maacc.Requete_Void("create temporary table Tabletemp(select min(timestamp),max(timestamp),ip_src,ip_dst,ip_proto,sig_sid,count(signature)as quantite FROM acid_event,signature WHERE acid_event.signature=signature.sig_id and signature.sig_sid in(select Signature_Sid from Signature_Inventaire)  and signature.sig_sid not in (select Signature_Sid from Intrusion)   GROUP BY signature)");
                maacc.Requete_Void("insert into Intrusion select '',Tabletemp.* from Tabletemp");
                maacc.Requete_Void("Drop table Tabletemp");
            //pr le update
                maacc.Requete_Void("create temporary table Tabletemp2(select min(timestamp),max(timestamp),ip_src,ip_dst,ip_proto,sig_sid,count(signature)as quantite FROM acid_event,signature WHERE acid_event.signature=signature.sig_id and signature.sig_sid in(select Signature_Sid from Signature_Inventaire)  and signature.sig_sid  in (select Signature_Sid from Intrusion)   GROUP BY signature)");
                maacc.Requete_Void("update Intrusion INNER JOIN Tabletemp2 ON Intrusion.Signature_Sid=Tabletemp2.sig_sid SET Intrusion.Occurence=Tabletemp2.quantite ");
                maacc.Requete_Void("Drop table Tabletemp2");
               // maacc.Requete_Void("update Intrusion,Tabletemp2 set Intrusion.Occurence=Tabletemp2.quantite where Intrusion.Signature_Sid=Tabletemp2.sig_sid");


               // maacc.Requete_Void("update Action,Categorie set  Libelle=Categorie.CodeAction where Action.CodeAction=Categorie.CodeAction");  ou
               // maacc.Requete_Void("update Action inner join Categorie on  Action.CodeAction=Categorie.CodeAction set  Libelle=Categorie.CodeAction ");
        }

        public object DeciderIntrusionProblematiques()
        {
            object source;
            AccesBD Acc = new AccesBD();
            //source = Acc.Visualiser("Intrusion", "NumIntrusion,Date_Fin,Occurence", "Occurence >2");
            source = Acc.Visualiser("Intrusion,Signature_Inventaire,Categorie,Action", "NumIntrusion,Date_Fin,Occurence,Intrusion.Signature_Sid,Signature_Inventaire.Nom,Signature_Inventaire.CodeCategorie,Categorie.CodeAction", "Intrusion.Signature_Sid=Signature_Inventaire.Signature_Sid and Signature_Inventaire.CodeCategorie=Categorie.CodeCategorie and Categorie.CodeAction=Action.CodeAction and Intrusion.Occurence >2 ");
            return source;
        }

        public void TraiterAlerteSyslog()
        {
            AccesBDsyslog objetAcces = new AccesBDsyslog();
            objetAcces.Requete_Void("create TEMPORARY TABLE  Tabletempo (select id,host,facility,level,datetime,msg FROM logs WHERE level='notice' and id not in(SELECT id from AlerteSyslog))");
            objetAcces.Requete_Void("INSERT into AlerteSyslog select '',null,Tabletempo.* from Tabletempo");
            objetAcces.Requete_Void("Drop table Tabletempo");  //supprimer la tqble temporaire
        }

    }
}

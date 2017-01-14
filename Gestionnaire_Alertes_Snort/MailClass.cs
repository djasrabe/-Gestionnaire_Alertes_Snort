using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;

namespace Gestionnaire_Alertes_Snort
{
   public class MailClass
    {
       string Source, Destination, Objet,Corps;

       public MailClass(string Source,string Destination,string Objet,string Corps )
       {
           this.Source = Source;
           this.Destination = Destination;
           this.Objet = Objet;
           this.Corps = Corps;
                                  
       }
       public void SendMail(string SmtpServeur,string Credential ,string Password,bool AutomatiqueMode)
       {
           MailMessage email = new MailMessage();
           SmtpClient smtp = new SmtpClient();

           email.From = new MailAddress(Source);
           email.To.Add(new MailAddress(Destination));
           email.IsBodyHtml = true;
           email.Subject = Objet;
           email.Body = Corps;
           smtp.Host = SmtpServeur;                  // "smtp.gmail.com";
           smtp.Port = 587;
           smtp.Credentials = new System.Net.NetworkCredential(Credential, Password);             //("djasrabeaymard@gmail.com", "be3");
           smtp.EnableSsl = true;
           try
           {
               smtp.Send(email);
               if (AutomatiqueMode==false)MessageBox.Show(" Mail envoyé");
           }
           catch (SmtpException ex)
           {
               if (AutomatiqueMode==false)MessageBox.Show(ex.Message + " Le mail ne peut pas etre Envoyé,Verifiez votre conection Internet");
               
           }      
       
       }
    }
}

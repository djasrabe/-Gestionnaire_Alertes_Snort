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

namespace Gestionnaire_Alertes_Snort
{
   public class AccesBDsyslog
    {

         MySqlConnection connection = new MySqlConnection();
        MySqlCommand commande = new MySqlCommand();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataSet dataset = new DataSet();
        DataTable table = new DataTable();      

        public AccesBDsyslog()
        {
            try
            {
                connection.ConnectionString = "Database=syslog;Server=192.168.17.5 ;User ID =syslogUser; Password=beckham23";
                connection.Open();
                commande.Connection = connection;
             }
            catch { MessageBox.Show("serveur BD inaccessible"); };

        }
        public DataTable Visualiser(string Requete)
        {
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = Requete;
            adapter.Fill(table);
            //con.Close();
            return table;
        }
        public object Select(string Requete)
        {
            commande.CommandText = Requete;
            // commande.Connection = connection;
            object li = (object)commande.ExecuteScalar();
            return li;
        }
        public void Requete_Void(string Requete)
        {
            //connection.Open();
            commande.CommandText = Requete;
            // commande.Connection = connection;
            commande.ExecuteNonQuery();
            //connection.Close();
        }
    }
}

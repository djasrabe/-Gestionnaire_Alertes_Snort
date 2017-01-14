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
   public class AccesBD
    {
        MySqlConnection connection = new MySqlConnection();
        MySqlCommand commande = new MySqlCommand();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        DataSet dataset = new DataSet();
        DataTable table = new DataTable();
       

        public AccesBD()
        {
            try
            {
                connection.ConnectionString = "Database=snortDB;Server=192.168.17.5 ;User ID =snortUser; Password=beckham23";
                connection.Open();
                commande.Connection = connection;

             }
            catch { MessageBox.Show("serveur BD inaccessible"); };

        }

        public object Select(string Requete)
        {
            commande.CommandText = Requete;
           // commande.Connection = connection;
            object li=(object)commande.ExecuteScalar();
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

        public DataTable  Visualiser(string Table,string Champ,string Condition)
        {
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = string.Format("select {0} from {1} where {2}",Champ,Table,Condition);
            adapter.Fill(table);
            //con.Close();
            return table;      
         }

        public DataTable VisualiserNouvo(string Table, string Champ, string Condition)
        {
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = string.Format("select {0} from {1} where {2}", Champ, Table, Condition);
            adapter.Fill(table);
            //con.Close();
            return table;
        }
        public DataTable Visualiser(string Requete)
        {
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = Requete;
            adapter.Fill(table);
            //con.Close();
            return table;
        }
        public DataTable Visualiser2(string Requete)
        {
            adapter.SelectCommand = connection.CreateCommand();
            adapter.SelectCommand.CommandText = Requete;
            adapter.Fill(table);
            //con.Close();
            return table;
        }

        public void Insert(string table,string GroupeChamp,int NbreChamp,object [] valeur)
        {
            int j = 1;
            string parametres = "@param1";
            while (j < NbreChamp)
            {
                parametres = parametres + ",@param" + (j + 1);  //correspond a  string parametres="@param1,@param2,@param3.....";
                j++;

            }
          
            commande.CommandText =string.Format( "insert into {0} ({1}) values ({2}) " ,table,GroupeChamp,parametres );
            for (int i = 0; i < NbreChamp; i++)
            {
                commande.Parameters.Add(new MySqlParameter("@param" + (i + 1), valeur[i]));
            }
        
            //commande.Connection = connection;
            commande.ExecuteNonQuery();
            commande.Parameters.Clear();
        }

        public void Update(string table, string ChampAmettreAjour,object NvelleValeur,string Condition)
        {
            commande.CommandText = string.Format("update {0} set {1}= @param  where {2}", table, ChampAmettreAjour, Condition);
            commande.Parameters.Add(new MySqlParameter("@param", NvelleValeur));

           // commande.Connection = connection;
            commande.ExecuteNonQuery();
            commande.Parameters.Clear();
        }

    }
}

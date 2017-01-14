using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.IO;


namespace ComTelnet
{
    class Program
    {
        static void Main(string[] args)
        {

            //create a new telnet connection to hostname "gobelijn" on port "23"
            Console.WriteLine("***Acceder à la console de votre routeur******** ");

            Console.WriteLine("Entrer l'adresse IP du routeur  ");
            string IPaddress = Console.ReadLine();
            Console.WriteLine("Login sur le routeur  "); //entrer "root"
            string Login = Console.ReadLine();
            Console.WriteLine("Mot de passe  ");   //"beckham23"
            string Pass = Console.ReadLine();

           TelnetConnectionClass tc = new TelnetConnectionClass(IPaddress, 23);
            //TelnetConnectionClass tc = new TelnetConnectionClass("192.168.16.4", 23);

            //login with user "root",password "rootpassword", using a timeout of 100ms, and show server output
           string s = tc.Login(Login, Pass, 100);//string s = tc.Login("root","beckham23",100);
            Console.Write(s);

            // server output should end with "$" or ">", otherwise the connection failed
            string prompt = s.TrimEnd();
            prompt = s.Substring(prompt.Length - 1, 1);
            if (prompt != "$" && prompt != ">")
                throw new Exception("Connection failed");

            prompt = "";

            // while connected
            while (tc.IsConnected && prompt.Trim() != "exit")
            {
                // display server output
                Console.Write(tc.Read());

                // send client input to server
                prompt = Console.ReadLine();
                tc.WriteLine(prompt);

                // display server output
                Console.Write(tc.Read());
            }
            Console.WriteLine("***DISCONNECTED");
            Console.ReadLine();



        }




    }
}

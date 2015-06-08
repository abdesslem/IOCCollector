using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace AccessInvestigation
{
    class Authentication
    {
            public static NetworkStream  streamsock;
            public static void  authentification(App form, TcpClient tcpclient) 
        {
            streamsock = tcpclient.GetStream();
            string nom = form.Username.Text;
            string password = form.password.Text;
            string auth = nom + "&" + password;
            form.AppendText(auth);
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] passphrase = asen.GetBytes(auth);
           // byte[] passphrase = new byte[128];
           // passphrase=AES.AuthEncrypt(auth,Connection.key,Connection.IV);
            //string s = passphrase.ToString();
            streamsock.Write(passphrase, 0, passphrase.Length);
            streamsock.Flush();
            form.AppendText(auth);
        }
    }
}

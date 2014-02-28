using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace main
{
    class Connection
    {
        public static void connection()
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Connecting.....");

                tcpclnt.Connect("192.168.1.7", 5000);
                // Use the ipaddress as in the server program

                Console.WriteLine("Connected...");
                Tuple<string, string> tuple = RSA.CreateKeyPair();
                //  string privateKey = tuple.Item1;
                string publicKey = tuple.Item2;
                //  Console.Write("Enter the string to be sent: ");

                // String str = Console.ReadLine();
                String str = publicKey;
                Stream stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                Console.WriteLine("Sending...");
                Console.WriteLine(ba.Length);
                stm.Write(ba, 0, ba.Length);
                byte[] bb = new byte[253];
                int k = stm.Read(bb, 0, 253);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                string s = RSA.Decrypt(tuple.Item1, bb);
                Console.WriteLine("decrypted");
                Console.WriteLine(s);
                // Console.ReadLine();
                // System.Threading.Thread.Sleep(5000);
                /*byte[] cc = new byte[128];
                int l = stm.Read(cc, 0, 128);

                for (int i = 0; i < l; i++)
                {
                    Console.Write(Convert.ToChar(cc[i]));
                }*/
                tcpclnt.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Error! " + e.StackTrace);
            }
        }
    }
}
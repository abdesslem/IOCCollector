using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace AccessInvestigation
{
    class Connection
    {
        public static byte[] key;
        public static byte[] IV;
        public static TcpClient tcpclnt;
        public static Stream stm;
        public static int port=5050;
        public static string IP = "127.0.0.1";
        public static void connection(App form)
        {
            try
            {

                tcpclnt = new TcpClient();
               // Console.WriteLine("Connecting.....");
                form.AppendText("Connecting.....\r\n");
                tcpclnt.Connect(IP,port);
                // Use the ipaddress as in the server program
               
               // Console.WriteLine("Connected...");
                Tuple<string, string> tuple = RSA.CreateKeyPair();
                //  string privateKey = tuple.Item1;
                string publicKey = tuple.Item2;
                // String str = Console.ReadLine();
                //String str = publicKey;
                stm = tcpclnt.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(publicKey);
                form.AppendText("Sending of public key...\r\n");
                //Console.WriteLine(ba.Length);
                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[253];
                int k = stm.Read(bb, 0, 253);
                for (int i = 0; i < k; i++)
                {
                    Console.Write(Convert.ToChar(bb[i]));
                }
                string s = RSA.Decrypt(tuple.Item1, bb);
                form.AppendText("decrypted shared key\r\n");
                form.AppendText(s);
                string[] stringSeparators = new string[] { "key=","#IV=" };
                string[] result;
                result = s.Split(stringSeparators, StringSplitOptions.None);
                key = asen.GetBytes(result[1]);
                IV = asen.GetBytes(result[2]);
                
                //form.AppendText("\r\n"+key+"et ;)"+IV+"\r\n");
                // Console.ReadLine();
                // System.Threading.Thread.Sleep(5000);
                /*byte[] cc = new byte[128];
                int l = stm.Read(cc, 0, 128);
                
                for (int i = 0; i < l; i++)
                {
                    Console.Write(Convert.ToChar(cc[i]));
                }*/
               // tcpclnt.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ops! Error! " + e.StackTrace);
            }
        }
    }
}
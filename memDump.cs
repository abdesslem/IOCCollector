using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;


namespace AccessInvestigation
{
    class memDump
    {
        public void createDump()
        {
            System.Diagnostics.ProcessStartInfo procStartInfo;
            if (IntPtr.Size == 8) 
            {
                procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "win64dd.exe -h");
            }
            // 32Bit    
            else { 
                procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "win32dd.exe -h"); 
            }
           // System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "win64dd.exe -h");
            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Write the string to a file.
                System.IO.StreamWriter file = new System.IO.StreamWriter("Dump.txt", true);
                file.WriteLine(result);
                file.Close();
                proc.Close();
        }
        public void sendDumpFile(App ap)
        {
          const int BufferSize = 1024;
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
     //   IPEndPoint IP = new IPEndPoint(ip, 5555);
        string path = @"files.zip";
        TcpClient client = new TcpClient();
        client.Connect("127.0.0.1", 5555);
        NetworkStream stm = client.GetStream();
       // Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //sock.Connect(IP);
        using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            long length = reader.Length;
            byte[] header=new byte[64];
            byte[] filelength = BitConverter.GetBytes(length);
            byte[] fileNameByte = Encoding.ASCII.GetBytes(path);
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
            filelength.CopyTo(header, 0);
            fileNameLen.CopyTo(header, 8);
            fileNameByte.CopyTo(header, 12);
            string fileName = Path.GetFileName(path);
           // sock.Send(BitConverter.GetBytes(fileName.Length));
            stm.Write(header,0,64);
            byte[] buffer = new byte[BufferSize];
            int read;
            while ((read = reader.Read(buffer, 0, BufferSize)) != 0)
            {
                stm.Write(buffer,0,buffer.Length);
                ap.UpdateProgress(read, length);
            }
            Console.WriteLine(" Send finish.");
            stm.Close();
            Console.ReadLine();
         }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.IO.Compression;
namespace main
{
    public partial class App : Form
    {
        
        // The TCP client will connect to the server using an IP and a port
        TcpClient tcpClient;
        // The file stream will read bytes from the local file you are sending
        FileStream fstFile;
        // The network stream will send bytes to the server application
        NetworkStream strRemote;
        public App()
        {
            InitializeComponent();
           ConnectToServer("192.168.1.2", 5050);
        }

       
        public void sendFile(String filename, NetworkStream strRemote, TcpClient tcpClient)
        {
            if (tcpClient.Connected == false)
            {
                ConnectToServer("192.168.1.2", 5050);
            }
            strRemote = tcpClient.GetStream();
            byte[] byteSend = new byte[tcpClient.ReceiveBufferSize];
            // The file stream will read bytes from the file that the user has chosen

            FileStream fstFile = new FileStream(filename, FileMode.Open, FileAccess.Read);
            // Read the file as binary
            BinaryReader binFile = new BinaryReader(fstFile);

            // Get information about the opened file
            FileInfo fInfo = new FileInfo(filename);

            // Get and store the file name
            string FileName = fInfo.Name;
            // Store the file name as a sequence of bytes
            byte[] ByteFileName = new byte[2048];
            ByteFileName = System.Text.Encoding.ASCII.GetBytes(FileName.ToCharArray());
            // Write the sequence of bytes (the file name) to the network stream
            strRemote.Write(ByteFileName, 0, ByteFileName.Length);

            // Get and store the file size
            long FileSize = fInfo.Length;
            // Store the file size as a sequence of bytes
            byte[] ByteFileSize = new byte[2048];
            ByteFileSize = System.Text.Encoding.ASCII.GetBytes(FileSize.ToString().ToCharArray());
            // Write the sequence of bytes (the file size) to the network stream
            strRemote.Write(ByteFileSize, 0, ByteFileSize.Length);


            // Reset the number of read bytes
            int bytesSize = 0;
            // Define the buffer size
            byte[] downBuffer = new byte[2048];

            // Loop through the file stream of the local file
            while ((bytesSize = fstFile.Read(downBuffer, 0, downBuffer.Length)) > 0)
            {
                // Write the data that composes the file to the network stream
                strRemote.Write(downBuffer, 0, bytesSize);
            }
            tcpClient.Close();
            strRemote.Close();
            fstFile.Close();
        }
        private void ConnectToServer(string ServerIP, int ServerPort)
        {
            // Create a new instance of a TCP client
            tcpClient = new TcpClient();
            try
            {
                // Connect the TCP client to the specified IP and port
                tcpClient.Connect(ServerIP, ServerPort);
                textBox1.Text += "Successfully connected to server\r\n";
            }
            catch (Exception exMessage)
            {
                // Display any possible error
                textBox1.Text += exMessage.Message;
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           /*Register r=new Register();
           r.ShowErrorMessage();*/
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // If tclClient is not connected, try a connection
            if (tcpClient.Connected == false)
            {
                // Call the ConnectToServer method and pass the parameters entered by the user
                // ConnectToServer(txtServer.Text, Convert.ToInt32(txtPort.Text));
                ConnectToServer("192.168.1.2", 5050);
            }

            // Prompt the user for opening a file
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                sendFile("file1.txt", strRemote, tcpClient);
                /*
                textBox1.Text += "Sending file information\r\n";
                // Get a stream connected to the server
                strRemote = tcpClient.GetStream();
                byte[] byteSend = new byte[tcpClient.ReceiveBufferSize];
                // The file stream will read bytes from the file that the user has chosen

                fstFile = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                // Read the file as binary
                BinaryReader binFile = new BinaryReader(fstFile);

                // Get information about the opened file
                FileInfo fInfo = new FileInfo(openFile.FileName);

                // Get and store the file name
                string FileName = fInfo.Name;
                // Store the file name as a sequence of bytes
                byte[] ByteFileName = new byte[2048];
                ByteFileName = System.Text.Encoding.ASCII.GetBytes(FileName.ToCharArray());
                // Write the sequence of bytes (the file name) to the network stream
                strRemote.Write(ByteFileName, 0, ByteFileName.Length);

                // Get and store the file size
                long FileSize = fInfo.Length;
                // Store the file size as a sequence of bytes
                byte[] ByteFileSize = new byte[2048];
                ByteFileSize = System.Text.Encoding.ASCII.GetBytes(FileSize.ToString().ToCharArray());
                // Write the sequence of bytes (the file size) to the network stream
                strRemote.Write(ByteFileSize, 0, ByteFileSize.Length);

                textBox1.Text += "Sending the file " + FileName + " (" + FileSize + " bytes)\r\n";

                // Reset the number of read bytes
                int bytesSize = 0;
                // Define the buffer size
                byte[] downBuffer = new byte[2048];

                // Loop through the file stream of the local file
                while ((bytesSize = fstFile.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    // Write the data that composes the file to the network stream
                    strRemote.Write(downBuffer, 0, bytesSize);
                }

                // Update the log textbox and close the connections and streams
                textBox1.Text += "File sent. Closing streams and connections.\r\n";
                tcpClient.Close();
                strRemote.Close();
                fstFile.Close();
                textBox1.Text += "Streams and connections are now closed.\r\n";
            }*/
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
/*        HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\
 HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\
CurrentVersion\
 HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsNT\
CurrentVersion\
 HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ */
          /*new Execute("REG QUERY HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Run");
          new Execute("netstat -anb");
           new Execute("forfiles /P C:\\Users\\slouma\\Desktop\\ /D +14/2/2014 /M *.exe");
           new systemLog().AppLog("application");
           new systemLog().AppLog("system");
            new Processes().RunProcess();*/
       //  CompressDirectory("files", "files.zip");
            /*
 * 
 * List<string> files = new List<string>(){
    @"C:\NewImages\file1.txt",
    @"C:\NewImages\file2.txt",
    @"C:\NewImages\file3.txt",
    @"C:\xd",
};*/
            if (tcpClient.Connected == false)
            {
                // Call the ConnectToServer method and pass the parameters entered by the user
                // ConnectToServer(txtServer.Text, Convert.ToInt32(txtPort.Text));
                ConnectToServer("192.168.1.2", 5050);
            }
         sendFile("file1.txt", strRemote, tcpClient);
  //       textBox1.Text += "Streams and connections are now closed.\r\n";
         // DecompressToDirectory("file.zip", "slouma");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}

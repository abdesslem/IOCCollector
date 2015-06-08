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
using System.Net;
namespace AccessInvestigation
{
    public partial class App : Form
    {
       public int PercentProgress;
        
        // The TCP client will connect to the server using an IP and a port
        public bool ButtonConnectClicked = false;
       TcpClient tcpClient;
        // The file stream will read bytes from the local file you are sending
        FileStream fstFile;
        // The network stream will send bytes to the server application
        NetworkStream strRemote;
        public App()
        {
            InitializeComponent();
            this.MaximizeBox = false;
          // ConnectToServer("192.168.1.2", 5050);
           Connection.connection(this);
        }
        public void AppendText(String text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendText), new object[] { text });
                return;
            }
            this.textBox1.Text += text;
        }

        public void sendFile(TcpClient tcp, string fileName)
        {
          //  string fileName = "test.dmp";// " File Name";
            //string filePath = @"test.txt";// "File Path";
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
            byte[] fileData = File.ReadAllBytes(fileName);
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
            NetworkStream st = tcp.GetStream();
            st.Write(clientData,0,clientData.Length);
           // Console.WriteLine("File {0} has been sent", fileName);
            st.Close();   
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (FileToScan.ShowDialog() == DialogResult.OK)
            {
              //fstFile = new FileStream(FileToScan.FileName, FileMode.Open, FileAccess.Read);
                FileInfo infoFile = new FileInfo(FileToScan.FileName);
                string file = infoFile.Name;
                Scan newScan = new Scan();
                newScan.uploadFile(FileToScan.FileName);
                newScan.showResult(FileToScan.FileName);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Connection cn = new Connection();
            // If tclClient is not connected, try a connection
            if (this.ButtonConnectClicked == true)
            {
                MessageBox.Show("You have to connect first press connect button");
            }
            else
            {
                // Prompt the user for opening a file
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    
                    fstFile = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                    FileInfo fInfo = new FileInfo(openFile.FileName);
                    string FileName = fInfo.Name;
                    byte[] fileNameByte = Encoding.ASCII.GetBytes(FileName);
                    byte[] fileData = File.ReadAllBytes(openFile.FileName);
                    byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
                    byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
                    fileNameLen.CopyTo(clientData, 0);
                    fileNameByte.CopyTo(clientData, 4);
                    fileData.CopyTo(clientData, 4 + fileNameByte.Length);
                    NetworkStream st = Connection.tcpclnt.GetStream();
                    st.Write(clientData, 0, clientData.Length);
                    // Console.WriteLine("File {0} has been sent", fileName);
                    st.Close(); 
                    //AES.EncryptFile("file1.txt", "enc.txt", Connection.key, Connection.IV);
                    //sendFile("enc.txt", Connection.stm, Connection.tcpclnt);
                }
                }
            }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.sysLog.Checked == true)
            {
                new systemLog().AppLog("application");
                new systemLog().AppLog("system");
            }

            if (this.process.Checked == true)
            {
                new Execute("netstat -anb");
                new Execute("REG QUERY HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Run");
                new Execute("REG QUERY HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services");
                new Execute("REG QUERY HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows");
                new Execute("REG QUERY HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsNT\\CurrentVersion");
                new Execute("REG QUERY HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion");
                new Execute("netstat -anb");
                new Execute("forfiles /P C:\\Users\\slouma\\Desktop\\ /D +14/2/2014 /M *.exe");

                new Processes().RunProcess();
            }
            if (this.Dump.Checked == true)
            {
                 memDump mem=new memDump();
                 mem.createDump();
                 mem.sendDumpFile(this);
            }
                List<string> files0 = new List<string>(){"files"};
                Compression.CreateZipFile(files0, "files.zip");
                AES.EncryptFile("files.zip","encfiles.zip",Connection.key,Connection.IV);
                sendFile(Connection.tcpclnt, @"encfiles.zip");
                //memDump mem2 = new memDump();
              //  mem.sendDumpFile(this);
                //this.sendFile(Connection.tcpclnt,"Dump.rar");
             //   List<string> files1 = new List<string>(){
               // @"files",
                //};
                //Compression.CreateZipFile(files1, "files.zip");
                //AES.EncryptFile("files.zip", "Encfiles.zip", Connection.key, Connection.IV);
                //sendFile(Connection.tcpclnt, "Encfiles.zip");
                //         sendFile("ModifiedFiles.txt", Connection.stm, Connection.tcpclnt);
                //       textBox1.Text += "Streams and connections are now closed.\r\n";
                // DecompressToDirectory("file.zip", "slouma");
            //}
        }
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void connect_Click(object sender, EventArgs e)
        {
            ButtonConnectClicked = true;
            Authentication.authentification(this, Connection.tcpclnt);
        }
        public void UpdateProgress(Int64 BytesRead, Int64 TotalBytes)
        {
            if (TotalBytes > 0)
            {
                // Calculate the download progress in percentages
                PercentProgress = Convert.ToInt32((BytesRead * 100) / TotalBytes);
                // Make progress on the progress bar
                progressUpload.Value = PercentProgress;
            }
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            //new memDump();
            //sendFile(Connection.tcpclnt);
            Connection.stm.Close();
            Connection.tcpclnt.Close();
        }
        
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit form2 = new Edit();
            form2.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About ab=new About();
            ab.Show();

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}

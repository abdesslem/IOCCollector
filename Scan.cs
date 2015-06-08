using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Net;
using System.Web.Script.Serialization;

namespace AccessInvestigation
{
    class Scan
    {
        private string fileName;
        private static int errStatus = 0;
        private static string lastError = null;
        private string hashValue = null;
        private string VTAPIKEY = "6c7ab2dc60d0ea1fc36210853a9704626581fe98b2e7a40f9ca773ef1eef2d5c";
        //Paste your VirusTotal API Key here



        public const string uploadURL = "https://www.virustotal.com/vtapi/v2/file/scan";

        public string VTServerResponse;

        public static int response_code;
        public static string resource;
        public static string verbose_msg;
        public static string resultLink;

        public Scan()
        {

        }

        public Scan(string p)
        {
            // TODO: Complete member initialization
            this.fileName = p;

        }
        public void showResult(string file2Test)
        {
            //hashValue = getSHA256(file2Test);
            hashValue = getSHA128(file2Test);
            System.Diagnostics.Debug.WriteLine(hashValue);
            if (hashValue != null)
            {
                //uploadFile(file2Test);
                chkHashResult(hashValue);
                if (Scan.response_code == 0)
                    uploadFile(file2Test);
                if (Scan.response_code == 1)
                    launcher();
            }
        }

        public void launcher()
        {

            System.Windows.Forms.Clipboard.SetText(Scan.resultLink);
            System.Diagnostics.Process.Start(Scan.resultLink);
        }
        public void uploadFile(string fileName)//,string param,string contentType, NameValueCollection nvc)
        {
            try
            {

                string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uploadURL);
                request.ContentType = "multipart/form-data;boundary=" + boundary;
                request.Method = "POST";
                request.KeepAlive = true;
                request.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Stream s = request.GetRequestStream();


                boundary = "--" + boundary;

                string basicMultiPartForm = "Content-Disposition: form-data; name=\"";
                string apiPad = basicMultiPartForm + "apikey\"\r\n\r\n" + VTAPIKEY + "\r\n";
                string fNamePad = basicMultiPartForm + "file\"; filename=\"" + Path.GetFileName(fileName) + "\"\r\n\r\n";

                byte[] postData = System.Text.Encoding.UTF8.GetBytes(boundary + "\r\n" + apiPad + boundary + "\r\n" + fNamePad);

                s.Write(postData, 0, postData.Length);


                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[20480];
                int bytesRead = 0;
                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                {
                    s.Write(buffer, 0, buffer.Length);
                }
                fs.Close();

                byte[] trailer = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                s.Write(trailer, 0, trailer.Length);
                s.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string VTServerResponse = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();

                System.Diagnostics.Debug.WriteLine(VTServerResponse);
                reponseParser(VTServerResponse);
            }
            catch (WebException wex)
            {
                lastError = wex.Message;
                errStatus = 1;
            }
        }
        public void reponseParser(string VTServerResponse)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            Dictionary<string, object> jsonObj = jss.Deserialize<Dictionary<string, object>>(VTServerResponse);

            Scan.response_code = int.Parse(jsonObj["response_code"].ToString());
            if (Scan.response_code == 1)
                Scan.resultLink = jsonObj["permalink"].ToString();
            Scan.resource = jsonObj["resource"].ToString();
            Scan.verbose_msg = jsonObj["verbose_msg"].ToString();
        }
        public void chkHashResult(string hashValue)
        {
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.virustotal.com/vtapi/v2/file/report");
                request.Method = "POST";

                string postData = "resource=" + hashValue + "&apikey=" + VTAPIKEY;
                StreamWriter postWriter = new StreamWriter(request.GetRequestStream());
                postWriter.Write(postData);
                postWriter.Flush();
                postWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string VTServerResponse = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8).ReadToEnd();

                reponseParser(VTServerResponse);


            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
        public static string getSHA256(string fileName)
        {
            string hashValue = null;
            try
            {
                BufferedStream file = new BufferedStream(File.OpenRead(fileName), 81920);
                SHA256Managed sha = new SHA256Managed();
                byte[] checksum = sha.ComputeHash(file);
                hashValue = BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
            catch (FileNotFoundException fex)
            {
                Scan.errStatus = 1;
                Scan.lastError = "File Not Found";
                hashValue = Scan.lastError;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return hashValue;
        }
        public static string getSHA128(string fileName)
        {
            string hashValue = null;
            try
            {
                BufferedStream file = new BufferedStream(File.OpenRead(fileName), 81920);
                SHA1Managed sha128 = new SHA1Managed();
                byte[] sha128ChkSum = sha128.ComputeHash(file);
                hashValue = BitConverter.ToString(sha128ChkSum).Replace("-", String.Empty);
                System.Diagnostics.Debug.WriteLine(hashValue);
                Console.Read();

            }
            catch (FileNotFoundException fex)
            {
                Scan.errStatus = 1;
                Scan.lastError = "File Not Found";
                hashValue = Scan.lastError;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return hashValue;
        }
    }
}

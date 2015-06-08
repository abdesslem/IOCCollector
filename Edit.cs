using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AccessInvestigation
{
    public partial class Edit : Form
    {
        public Edit()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection.IP = serverIP.Text;
            Connection.port = int.Parse(serverPort.Text);
        }

        private void Edit_Load(object sender, EventArgs e)
        {

        }
    }
}

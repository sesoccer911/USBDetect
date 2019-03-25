using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace USBDetect
{
    public partial class RunForm1 : Form
    {
        static SerialPort port = new SerialPort("COM4", 112500);
        public RunForm1()
        {
            InitializeComponent();           

            System.Threading.Thread.Sleep(5000);
            port.WriteLine("root");
            System.Threading.Thread.Sleep(5000);
            port.WriteLine("root");

            port.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

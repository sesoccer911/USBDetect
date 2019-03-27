using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace USBDetect
{
    public partial class PrimaryForm : Form
    {
        //CommunicationManager comm = new CommunicationManager();
        static SerialPort comm = new SerialPort("COM4", 112500);
        string transType = string.Empty;
        

        public PrimaryForm()
        {
            InitializeComponent();
            comm.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void PrimaryForm_Load(object sender, EventArgs e)
        {
           // setDefaults();
        }
        static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)

        {

            SerialPort sp = (SerialPort)sender;

            string indata = sp.ReadExisting();

            Console.WriteLine("Data Received:");

            Console.Write(indata);
            Console.SetOut(richTextBox1);

        }

        private void btnPortState_Click(object sender, EventArgs e)
        {
            // string portName = Convert.ToString(cboPorts.Text);
            //int baudRate = Convert.ToInt32(cboBaudRate.Text);


            //comm.PortName = cboPorts.Text;
            //comm.Parity = "None";
            //comm.StopBits = "1";
            //comm.DataBits = "8";
            //comm.BaudRate = cboBaudRate.Text;
            //SerialPort port = new SerialPort(portName, baudRate);
            

            if (btnPortState.Text == "Port Closed")
            {
                btnPortState.Text = "Port Open";

                //port = new SerialPort(portName, baudRate);
                //port.Open();
                comm.Open();
                richTextBox1.Text = comm.ReadExisting();
                txtbxInput.Enabled = true;
                btnRun.Enabled = true;
                //you can do work now
                //System.Threading.Thread.Sleep(5000);
                comm.WriteLine("root");
                System.Threading.Thread.Sleep(5000);
                comm.WriteLine("!vxpulse2014!");


            }
            else if (btnPortState.Text == "Port Open")
            {
                comm.Close();
                btnPortState.Text = "Port Closed";
            }
        }



        private void btnGetSerialPorts_Click(object sender, System.EventArgs e)
        {
            loadValues();

        }

        private void setDefaults()
        {
            cboPorts.Text = "COM4";
            cboBaudRate.Text = "112500";
        }

        private void loadValues()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            //Com Ports
            ArrayComPortsNames = SerialPort.GetPortNames();
            do
            {
                index += 1;
                cboPorts.Items.Add(ArrayComPortsNames[index]);


            } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
            Array.Sort(ArrayComPortsNames);

            if (index == ArrayComPortsNames.GetUpperBound(0))
            {
                ComPortName = ArrayComPortsNames[0];
            }
            //get first item print in text
            cboPorts.Text = ArrayComPortsNames[0];
            //Baud Rate
            cboBaudRate.Items.Add(115200);
            cboBaudRate.Items.ToString();
            //get first item print in text
            cboBaudRate.Text = cboBaudRate.Items[0].ToString();


        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void btnRun_Click(object sender, System.EventArgs e)
        {
            comm.WriteLine("\r");

            System.Threading.Thread.Sleep(5000);

            comm.WriteLine("root");

            System.Threading.Thread.Sleep(5000);

            comm.WriteLine("!vxpulse2014!");

            System.Threading.Thread.Sleep(5000);

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            btnRun.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            comm.WriteLine(txtbxInput.Text);
            richTextBox1.("Sending ");
            txtbxInput.Text = "";
        }
    }
}

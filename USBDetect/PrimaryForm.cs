using System;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

namespace USBDetect
{
    public partial class PrimaryForm : Form
    {
        //CommunicationManager comm = new CommunicationManager();
        static SerialPort comm = new SerialPort();
        //string transType = string.Empty;
        public StreamWriter transType;

        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;

        public PrimaryForm()
        {
            InitializeComponent();
            SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            comm.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived_1);

            
            //comm.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            transType = new StreamWriter(Console.OpenStandardOutput());
            Console.SetOut(transType);
            rtbIncoming.AppendText(Convert.ToString(transType));
            
        }

        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChange SerialPinChange1 = 0;
            bool signalState = false;

            SerialPinChange1 = e.EventType;
            lblCTSStatus.BackColor = Color.Green;
            lblDSRStatus.BackColor = Color.Green;
            lblRIStatus.BackColor = Color.Green;
            lblBreakStatus.BackColor = Color.Green;
            switch (SerialPinChange1)
            {
                case SerialPinChange.Break:
                    lblBreakStatus.BackColor = Color.Red;
                    //MessageBox.Show("Break is Set");
                    break;
                case SerialPinChange.CDChanged:
                    signalState = comm.CtsHolding;
                    //  MessageBox.Show("CD = " + signalState.ToString());
                    break;
                case SerialPinChange.CtsChanged:
                    signalState = comm.CDHolding;
                    lblCTSStatus.BackColor = Color.Red;
                    //MessageBox.Show("CTS = " + signalState.ToString());
                    break;
                case SerialPinChange.DsrChanged:
                    signalState = comm.DsrHolding;
                    lblDSRStatus.BackColor = Color.Red;
                    // MessageBox.Show("DSR = " + signalState.ToString());
                    break;
                case SerialPinChange.Ring:
                    lblRIStatus.BackColor = Color.Red;
                    //MessageBox.Show("Ring Detected");
                    break;
            }
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


        }
        private void port_DataReceived_1(object sender, SerialDataReceivedEventArgs e)
        {
            InputData = comm.ReadExisting();
            if (InputData != String.Empty)
            {
                this.BeginInvoke(new SetTextCallback(SetText), new object[] { InputData });
            }
        }
        private void SetText(string text)
        {
            this.rtbIncoming.Text += text;
        }

        void btnPortState_Click(object sender, EventArgs e)
        {
            string portName = Convert.ToString(cboPorts.Text);
            int baudRate = Convert.ToInt32(cboBaudRate.Text);


            comm.PortName = cboPorts.Text;
            comm.Parity = (Parity)System.Enum.Parse(typeof(Parity), cboParity.Text);
            comm.StopBits = (StopBits)System.Enum.Parse(typeof(StopBits), cboStopBits.Text);
            comm.DataBits = Convert.ToInt32("8");
            comm.BaudRate = Convert.ToInt32(cboBaudRate.Text);
            SerialPort port = new SerialPort(portName, baudRate);


            if (btnPortState.Text == "Port Closed")
            {
                btnPortState.Text = "Port Open";
                //port = new SerialPort(portName, baudRate);
                //port.Open();
                comm.Open();
                rtbIncoming.Text = comm.ReadExisting();
                txtCommand.Enabled = true;
                btnRun.Enabled = true;
                //you can do work now
                //System.Threading.Thread.Sleep(5000);
                //comm.WriteLine("root");
                //System.Threading.Thread.Sleep(5000);
                //comm.WriteLine("!vxpulse2014!");


            }
            else if (btnPortState.Text == "Port Open")
            {
                comm.Close();
                btnPortState.Text = "Port Closed";
            }
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
            string Command1 = txtCommand.Text;
            string CommandSent;
            int Length, j = 0;

            Length = Command1.Length;

            for (int i = 0; i < Length; i++)
            {
                CommandSent = Command1.Substring(j, 1);
                comm.Write(CommandSent);
                j++;
            }
        }

        private void btnGetSerialPorts_Click(object sender, EventArgs e)
        {
            setDefaults();
        }
    }
}

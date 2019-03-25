using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace USBDetect
{
    public partial class PrimaryForm : Form
    {
        
        private const int WM_DEVICECHANGE = 0x219;
        //Tells when devices have been added/removed
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        //Tells when devices are storage
        private const int DBT_DEVTYP_VOLUME = 0x00000002;

        public PrimaryForm()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            //used to handle DEVICEARRIVAL and DEVICEREMOVECOMPLETE
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case DBT_DEVICEARRIVAL:
                            listBox1.Items.Add("New Device Arrived");

                            int devType = Marshal.ReadInt32(m.LParam, 4);
                            if (devType == DBT_DEVTYP_VOLUME)
                            {
                                DevBroadcastVolume vol;
                                vol = (DevBroadcastVolume)Marshal.PtrToStructure(m.LParam,
                                   typeof(DevBroadcastVolume));
                                listBox1.Items.Add("Mask is " + vol.Mask);
                            }
                            break;

                        case DBT_DEVICEREMOVECOMPLETE:
                            listBox1.Items.Add("Device Removed");
                            break;
                    }
                    break;
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
    }
}

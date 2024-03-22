using AForge.Video.DirectShow;
using System.Windows.Forms;
using ZXing;

namespace BCRD
{
    public partial class Form1 : Form
    {
        FilterInfoCollection filterInfo;
        VideoCaptureDevice captureDevice;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            captureDevice = new VideoCaptureDevice(filterInfo[comboBox1.SelectedIndex].MonikerString);
            captureDevice.NewFrame += CaptureDevice_NewFrame;
            captureDevice.Start();
            timer1.Start();
        }

        private void CaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            pictureBox1.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            filterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filter in filterInfo)
            {
                comboBox1.Items.Add(filter.Name);
            }
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (captureDevice.IsRunning) captureDevice.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            if (pictureBox1.Image != null)
            {
                BarcodeReader reader = new BarcodeReader();
                var image = Image.FromFile(@"C:\Users\Acer\Desktop\QR code.png");
                var result = reader.Decode(ImageToByteArray(Image.FromFile(@"C:\Users\Acer\Desktop\QR.png")));
                if (result != null) { 
                    MessageBox.Show(result.ToString());
                    timer1.Stop();
                }
            }
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

    }
}
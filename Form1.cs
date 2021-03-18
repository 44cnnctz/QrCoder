using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;

namespace QrCoder
{
    public partial class Form1 : Form
    {
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        public Bitmap qrcode;
        public Form1()
        {
            InitializeComponent();
            string qrtext = richTextBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            Bitmap qrcode = encoder.Encode(qrtext);
            pictureBox1.Image = qrcode as Image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            while (this.Opacity > 0.0)
            {
                this.Opacity -= 0.0002;
                if (this.Opacity == 0)
                {
                    Application.Exit();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            while (this.Opacity < 1)
            {
                this.Opacity += 0.0002;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x00020000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string qrtext = richTextBox1.Text;
            QRCodeEncoder encoder = new QRCodeEncoder();
            qrcode = encoder.Encode(qrtext);
            pictureBox1.Image = qrcode as Image;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.OverwritePrompt = true;
            save.Filter = "PNG|*.png|JPEG|*.jpg|GIF|*.gif|BMP|*.bmp";
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                qrcode.Save(save.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();
            if (load.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap qr = new Bitmap(load.FileName);
                pictureBox1.Image = qr;
                QRCodeDecoder decoder = new QRCodeDecoder();
                richTextBox1.Text = decoder.Decode(new QRCodeBitmapImage(qr));
            }
        }
    }
}

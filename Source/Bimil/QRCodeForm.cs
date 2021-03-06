using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bimil {
    internal partial class QRCodeForm : Form {
        public QRCodeForm(string text) {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            this.Coder = new QRCoder.QRCode(text);

            int qrSize;
            using (var bmp = this.Coder.GetBitmap()) {
                qrSize = Math.Max(bmp.Width, bmp.Height);
            }

            var screen = Screen.GetWorkingArea(this);
            var factor = Math.Min(screen.Width, screen.Height) / qrSize / 2;

            var scaledBitmap = this.Coder.GetBitmap(factor);
            this.ClientSize = scaledBitmap.Size;
            this.BackgroundImage = scaledBitmap;
        }

        private readonly QRCoder.QRCode Coder;


        #region Disable minimize

        protected override void WndProc(ref Message m) {
            if ((Environment.OSVersion.Platform == PlatformID.Win32NT) && (Environment.OSVersion.Version.Major < 10)
                && (m.Msg == NativeMethods.WM_SYSCOMMAND) && (m.WParam == NativeMethods.SC_MINIMIZE)) {
                m.Result = IntPtr.Zero;
            } else {
                base.WndProc(ref m);
            }
        }


        private class NativeMethods {
            internal const Int32 WM_SYSCOMMAND = 0x0112;
            internal readonly static IntPtr SC_MINIMIZE = new IntPtr(0xF020);
        }

        #endregion


        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            if ((e.KeyData == Keys.Escape) || (e.KeyData == Keys.Return)) { this.DialogResult = DialogResult.Cancel; }
        }

    }
}

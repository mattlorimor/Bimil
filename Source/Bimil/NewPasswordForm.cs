using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bimil {
    internal partial class NewPasswordForm : Form {

        public NewPasswordForm() {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            erp.SetIconAlignment(txtPassword2, ErrorIconAlignment.MiddleLeft);
            erp.SetIconPadding(txtPassword2, SystemInformation.Border3DSize.Width);
        }


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


        private void txtPassword_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                txtPassword2.Select();
            }
        }

        private void txtPassword2_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyData == Keys.Enter) {
                if (btnOK.Enabled) {
                    btnOK_Click(null, null);
                    this.DialogResult = DialogResult.OK;
                }
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e) {
            if (string.Equals(txtPassword.Text, txtPassword2.Text, StringComparison.Ordinal)) {
                btnOK.Enabled = true;
                erp.SetError(txtPassword2, null);
            } else {
                erp.SetError(txtPassword2, "Passwords don't match.");
                btnOK.Enabled = false;
            }
            this.AcceptButton = btnOK.Enabled ? btnOK : null;
        }


        private void btnOK_Click(object sender, EventArgs e) {
            this.Password = txtPassword.Text;
        }

        public string Password { get; private set; }

    }
}

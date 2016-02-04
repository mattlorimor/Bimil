using Medo.Security.Cryptography.PasswordSafe;
using System.Windows.Forms;

namespace Bimil {
    internal static class Helpers {

        #region ToolStripBorderlessProfessionalRenderer

        internal static ToolStripBorderlessProfessionalRenderer ToolStripBorderlessSystemRendererInstance { get { return new ToolStripBorderlessProfessionalRenderer(); } }

        internal class ToolStripBorderlessProfessionalRenderer : ToolStripProfessionalRenderer {

            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) {
            }

        }

        #endregion


        public static string GetRecordCaption(Record record) {
            return GetRecordCaption(record.RecordType);
        }

        public static string GetRecordCaption(RecordType recordType) {
            switch (recordType) {
                case RecordType.UserName: return "User name";
                case RecordType.Password: return "Password";
                case RecordType.Url: return "URL";
                case RecordType.EmailAddress: return "E-mail";
                case RecordType.Notes: return "Notes";

                case RecordType.BimilTwoFactorKey: return "Two-factor key";
                case RecordType.BimilCreditCardNumber: return "Card number";
                case RecordType.BimilCreditCardExpiration: return "Card expiration";
                case RecordType.BimilCreditCardSecurityCode: return "Card security code";
                case RecordType.BimilCreditCardPin: return "Card PIN";

                default: return null; //all other fields are not really supported
            }
        }

    }
}

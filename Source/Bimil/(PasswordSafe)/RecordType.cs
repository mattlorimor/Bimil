using System;

namespace Medo.Security.Cryptography.PasswordSafe {
    /// <summary>
    /// Record field types.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "This values map directly into the Password Safe type and there is no record type with value 0.")]
    public enum RecordType {
        /// <summary>
        /// UUID.
        /// </summary>
        Uuid = 0x01,
        /// <summary>
        /// Group.
        /// </summary>
        Group = 0x02,
        /// <summary>
        /// Title.
        /// </summary>
        Title = 0x03,
        /// <summary>
        /// User name.
        /// </summary>
        UserName = 0x04,
        /// <summary>
        /// Notes.
        /// </summary>
        Notes = 0x05,
        /// <summary>
        /// Password.
        /// </summary>
        Password = 0x06,
        /// <summary>
        /// Creation time.
        /// </summary>
        CreationTime = 0x07,
        /// <summary>
        /// Password modification time.
        /// </summary>
        PasswordModificationTime = 0x08,
        /// <summary>
        /// Last access time.
        /// </summary>
        LastAccessTime = 0x09,
        /// <summary>
        /// Password expiry time.
        /// </summary>
        PasswordExpiryTime = 0x0a,
        /// <summary>
        /// Last modification time.
        /// </summary>
        LastModificationTime = 0x0c,
        /// <summary>
        /// URL.
        /// </summary>
        Url = 0x0d,
        /// <summary>
        /// Autotype.
        /// </summary>
        Autotype = 0x0e,
        /// <summary>
        /// Password history.
        /// </summary>
        PasswordHistory = 0x0f,
        /// <summary>
        /// Password policy.
        /// </summary>
        PasswordPolicy = 0x10,
        /// <summary>
        /// Password expiry interval.
        /// </summary>
        PasswordExpiryInterval = 0x11,
        /// <summary>
        /// Run command.
        /// </summary>
        RunCommand = 0x12,
        /// <summary>
        /// Double-click action.
        /// </summary>
        DoubleClickAction = 0x13,
        /// <summary>
        /// E-mail address.
        /// </summary>
        EmailAddress = 0x14,
        /// <summary>
        /// Protected entry.
        /// </summary>
        ProtectedEntry = 0x15,
        /// <summary>
        /// Own symbols for password.
        /// </summary>
        OwnSymbolsForPassword = 0x16,
        /// <summary>
        /// Shift double-click action.
        /// </summary>
        ShiftDoubleClickAction = 0x17,
        /// <summary>
        /// Password policy name.
        /// </summary>
        PasswordPolicyName = 0x18,
        /// <summary>
        /// Entry keyboard shortcut.
        /// </summary>
        EntryKeyboardShortcut = 0x19,
        /// <summary>
        /// End of entries.
        /// </summary>
        EndOfEntry = 0xFF,



        BimilTwoFactorKey = 0xf0,
        BimilCreditCardNumber = 0xf1,
        BimilCreditCardExpiration = 0xf2,
        BimilCreditCardSecurityCode = 0xf3,
        BimilCreditCardPin = 0xf4,
    }
}

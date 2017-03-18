using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Medo.Security.Cryptography.PasswordSafe;

namespace Bimil {
    internal static class ClipboardHelper {

        private static readonly string FormatName = "Bimil";

        public static void SetClipboardData(IEnumerable<Entry> entries) {
            var bytes = new List<byte>();

            foreach (var entry in entries) {
                foreach (var record in entry.Records) {
                    bytes.AddRange(BitConverter.GetBytes((int)record.RecordType));
                    bytes.AddRange(BitConverter.GetBytes(record.RawDataDirect.Length));
                    bytes.AddRange(record.RawDataDirect);
                }

                //add entry separator
                bytes.AddRange(new byte[] { 0, 0, 0, 0 }); //RecordType=0
                bytes.AddRange(new byte[] { 0, 0, 0, 0 }); //Length=0
            }
            var buffer = bytes.ToArray();
            for (int i = 0; i < bytes.Count; i++) { bytes[i] = 0; }

            var protectedBuffer = ProtectedData.Protect(buffer, null, DataProtectionScope.CurrentUser);
            Array.Clear(buffer, 0, buffer.Length);

            Clipboard.Clear();
            Clipboard.SetData(FormatName, protectedBuffer);
        }

        public static IEnumerable<Entry> GetClipboardData() {
            if (Clipboard.ContainsData(FormatName)) {
                if (Clipboard.GetData(FormatName) is byte[] protectedBuffer) {
                    var buffer = ProtectedData.Unprotect(protectedBuffer, null, DataProtectionScope.CurrentUser);
                    var offset = 0;
                    var records = new List<Record>();
                    try {
                        while (offset < buffer.Length) {
                            var type = BitConverter.ToInt32(buffer, offset); offset += 4;
                            var length = BitConverter.ToInt32(buffer, offset); offset += 4;
                            if ((type == 0) && (length == 0)) { //end of item
                                yield return new Entry(records);
                                records.Clear();
                                continue;
                            }

                            var dataBytes = new byte[length];
                            Buffer.BlockCopy(buffer, offset, dataBytes, 0, length); offset += length;
                            var record = new Record((RecordType)type, dataBytes);
                            records.Add(record);
                        }
                        if (records.Count > 0) { //return any records left (compatibility with old single-item copy)
                            yield return new Entry(records);
                            records.Clear();
                        }
                    } finally {
                        Array.Clear(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        public static bool HasDataOnClipboard {
            get {
                return Clipboard.ContainsData(FormatName);
            }
        }

    }
}
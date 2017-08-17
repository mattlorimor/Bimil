using System;
using Xunit;
using PwSafe = Medo.Security.Cryptography.PasswordSafe;

namespace PasswordSafe.Test {
    public class RecordTests {

        [Fact(DisplayName = "PasswordSafe: Record: New")]
        public void Record_New() {
            var field = new PwSafe.Record(PwSafe.RecordType.Title) { Text = "Test" };
            Assert.Equal("Test", field.Text);
        }

        [Fact(DisplayName = "PasswordSafe: Record: New (wrong type)")]
        public void Record_New_WrongType() {
            Assert.Throws<FormatException>(() => {
                var field = new PwSafe.Record(PwSafe.RecordType.Title) { Time = DateTime.Now };
            });
        }

        [Fact(DisplayName = "PasswordSafe: Record: New (auto-type)")]
        public void Record_New_Autotype() {
            var field = new PwSafe.Record(PwSafe.RecordType.Autotype);
            Assert.Equal(@"\u\t\p\n", field.Text);
        }


        [Fact(DisplayName = "PasswordSafe: Record: Change (read-only document)")]
        public void Record_ReadOnly() {
            Assert.Throws<NotSupportedException>(() => {
                var doc = new PwSafe.Document("Password");
                doc.Entries["Test"].Password = "Old";

                doc.IsReadOnly = true;
                doc.Entries[0].Records[PwSafe.RecordType.Password].Text = "New";
            });
        }

    }
}
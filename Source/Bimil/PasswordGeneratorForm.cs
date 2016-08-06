using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Bimil {
    internal partial class PasswordGeneratorForm : Form {
        public PasswordGeneratorForm(bool useCopyAsSave = false) {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;

            this.UseCopyAsSave = useCopyAsSave;

            btnCopy.Text = useCopyAsSave ? "Save" : "Copy";
            tip.SetToolTip(btnCopy, useCopyAsSave ? "Save password" : "Copy to clipboard");
        }

        private readonly bool UseCopyAsSave;

        protected override bool ProcessDialogKey(Keys keyData) {
            if (keyData == Keys.Escape) {
                this.Close();
                return true;
            } else {
                return base.ProcessDialogKey(keyData);
            }
        }


        private void Form_Load(object sender, EventArgs e) {
            {
                chbWordIncludeUpperCase.Checked = Settings.PasswordGeneratorWordIncludeUpperCase;
                chbWordIncludeNumber.Checked = Settings.PasswordGeneratorWordIncludeNumber;
                chbWordIncludeSpecialCharacter.Checked = Settings.PasswordGeneratorWordIncludeSpecialCharacter;
                chbWordIncludeIncomplete.Checked = Settings.PasswordGeneratorWordIncludeIncomplete;

                chbWordRestrictAddSpace.Checked = Settings.PasswordGeneratorWordRestrictAddSpace;
                chbWordRestrictBreak.Checked = Settings.PasswordGeneratorWordRestrictBreak;
                chbWordRestrictTitleCase.Checked = Settings.PasswordGeneratorWordRestrictTitleCase;

                txtWordCount.Text = Settings.PasswordGeneratorWordCount.ToString("0", CultureInfo.CurrentCulture);
            }

            {
                chbIncludeUpperCase.Checked = Settings.PasswordGeneratorIncludeUpperCase;
                chbIncludeLowerCase.Checked = Settings.PasswordGeneratorIncludeLowerCase;
                chbIncludeNumbers.Checked = Settings.PasswordGeneratorIncludeNumbers;
                chbIncludeSpecialCharacters.Checked = Settings.PasswordGeneratorIncludeSpecialCharacters;

                chbRestrictSimilar.Checked = Settings.PasswordGeneratorRestrictSimilar;
                chbRestrictMovable.Checked = Settings.PasswordGeneratorRestrictMovable;
                chbRestrictPronounceable.Checked = Settings.PasswordGeneratorRestrictPronounceable;
                chbRestrictRepeated.Checked = Settings.PasswordGeneratorRestrictRepeated;

                txtLength.Text = Settings.PasswordGeneratorLength.ToString("0", CultureInfo.CurrentCulture);
            }

            tabStyle.SelectedTab = Settings.PasswordGeneratorUseWord ? tabStyle_Words : tabStyle_Classic;

            btnGenerate_Click(null, null);
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e) {
            {
                Settings.PasswordGeneratorWordIncludeUpperCase = chbWordIncludeUpperCase.Checked;
                Settings.PasswordGeneratorWordIncludeNumber = chbWordIncludeNumber.Checked;
                Settings.PasswordGeneratorWordIncludeSpecialCharacter = chbWordIncludeSpecialCharacter.Checked;
                Settings.PasswordGeneratorWordIncludeIncomplete = chbWordIncludeIncomplete.Checked;

                Settings.PasswordGeneratorWordRestrictAddSpace = chbWordRestrictAddSpace.Checked;
                Settings.PasswordGeneratorWordRestrictBreak = chbWordRestrictBreak.Checked;
                Settings.PasswordGeneratorWordRestrictTitleCase = chbWordRestrictTitleCase.Checked;

                int count;
                if (int.TryParse(txtWordCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out count)) {
                    Settings.PasswordGeneratorWordCount = count;
                }
            }

            {
                Settings.PasswordGeneratorIncludeUpperCase = chbIncludeUpperCase.Checked;
                Settings.PasswordGeneratorIncludeLowerCase = chbIncludeLowerCase.Checked;
                Settings.PasswordGeneratorIncludeNumbers = chbIncludeNumbers.Checked;
                Settings.PasswordGeneratorIncludeSpecialCharacters = chbIncludeSpecialCharacters.Checked;

                Settings.PasswordGeneratorRestrictSimilar = chbRestrictSimilar.Checked;
                Settings.PasswordGeneratorRestrictMovable = chbRestrictMovable.Checked;
                Settings.PasswordGeneratorRestrictPronounceable = chbRestrictPronounceable.Checked;
                Settings.PasswordGeneratorRestrictRepeated = chbRestrictRepeated.Checked;

                int length;
                if (int.TryParse(txtLength.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out length)) {
                    Settings.PasswordGeneratorLength = length;
                }
            }

            Settings.PasswordGeneratorUseWord = !tabStyle.SelectedTab.Equals(tabStyle_Classic);
        }


        private void chbIncludeUpperCase_CheckedChanged(object sender, EventArgs e) {
            if (chbIncludeUpperCase.Checked == false) { chbIncludeLowerCase.Checked = true; }
            btnGenerate_Click(null, null);
        }

        private void chbIncludeLowerCase_CheckedChanged(object sender, EventArgs e) {
            if (chbIncludeLowerCase.Checked == false) { chbIncludeUpperCase.Checked = true; }
            btnGenerate_Click(null, null);
        }


        private void txtNumber_KeyDown(object sender, KeyEventArgs e) {
            TextBox textBox = (TextBox)sender;

            switch (e.KeyData) {
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                    break;

                case Keys.Left:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.Back:
                case Keys.Delete:
                case Keys.Alt | Keys.F4:
                    break;

                case Keys.PageUp:
                    ChangeLength(textBox, -10);
                    e.SuppressKeyPress = true;
                    break;

                case Keys.PageDown:
                    ChangeLength(textBox, +10);
                    e.SuppressKeyPress = true;
                    break;

                default:
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        private void txtWordCount_Leave(object sender, EventArgs e) {
            int count;
            if (!int.TryParse(txtWordCount.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out count)) {
                count = Settings.PasswordGeneratorWordCount;
            }
            txtLength.Text = Math.Min(Math.Max(count, 1), 9).ToString(CultureInfo.CurrentCulture);
        }

        private void txtLength_Leave(object sender, EventArgs e) {
            int length;
            if (!int.TryParse(txtLength.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out length)) {
                length = Settings.PasswordGeneratorLength;
            }
            txtLength.Text = Math.Min(Math.Max(length, 1), 99).ToString(CultureInfo.CurrentCulture);
        }

        private void ChangeLength(TextBox textBox, int delta) {
            int length;
            if (int.TryParse(textBox.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out length)) {
                var newLength = Math.Min(Math.Max(length + delta, 1), (int)Math.Pow(10, textBox.TextLength) - 1);
                textBox.Text = newLength.ToString(CultureInfo.CurrentCulture);
                textBox.SelectAll();
            }
        }


        public string Password { get; private set; }

        private void btnCopy_Click(object sender, EventArgs e) {
            if (this.UseCopyAsSave) {
                this.Password = txtPassword.Text;
            } else {
                Clipboard.Clear();
                Clipboard.SetText(txtPassword.Text, TextDataFormat.Text);
            }
        }


        private readonly char[] UpperCaseConsonants = new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z' };
        private readonly char[] UpperCaseVowels = new char[] { 'A', 'E', 'I', 'O', 'U', 'Y' };
        private readonly char[] LowerCaseConsonants = new char[] { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z' };
        private readonly char[] LowerCaseVowels = new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };
        private readonly char[] Digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private readonly char[] SpecialCharacters = new char[] { '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', '{', ']', '}', '\\', '|', ';', ':', '\'', '\"', ',', '<', '.', '>', '/', '?' };

        private readonly char[] RestrictedSimilar = new char[] { 'I', 'O', 'Q', 'l', '0', '`', '-', '_', ';', ':', '\'', '\"', ',', '.' };
        private readonly char[] RestrictedMoveable = new char[] { 'Y', 'Z', 'y', 'z', '`', '~', '#', '$', '%', '-', ',', '.' };

        private readonly double CracksPerSecond = 100000000000000;

        private static readonly RandomNumberGenerator Rnd = RandomNumberGenerator.Create();

        private void btnGenerate_Click(object sender, EventArgs e) {
            string password = null;
            double combinations = double.NaN;

            if (tabStyle.SelectedTab.Equals(tabStyle_Classic)) {
                int length;
                if (int.TryParse(txtLength.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out length) && (length >= 1)) {
                    var includeUpperCase = chbIncludeUpperCase.Checked;
                    var includeLowerCase = chbIncludeLowerCase.Checked;
                    var includeNumbers = chbIncludeNumbers.Checked;
                    var includeSpecial = chbIncludeSpecialCharacters.Checked;
                    var restrictSimilar = chbRestrictSimilar.Checked;
                    var restrictMovable = chbRestrictMovable.Checked;
                    var restrictPronounceable = chbRestrictPronounceable.Checked;
                    var restrictRepeated = chbRestrictRepeated.Checked;

                    password = GenerateClassicPassword(includeUpperCase, includeLowerCase, includeNumbers, includeSpecial, restrictSimilar, restrictMovable, restrictPronounceable, restrictRepeated, length);
                    combinations = CalculateClassicCombinations(includeUpperCase, includeLowerCase, includeNumbers, includeSpecial, restrictSimilar, restrictMovable, restrictPronounceable, restrictRepeated, length);
                }
            } else {
                int count;
                if (int.TryParse(txtWordCount.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out count) && (count >= 1)) {
                    var includeUpperCase = chbWordIncludeUpperCase.Checked;
                    var includeNumber = chbWordIncludeNumber.Checked;
                    var includeSpecial = chbWordIncludeSpecialCharacter.Checked;
                    var includeIncomplete = chbWordIncludeIncomplete.Checked;
                    var restrictAddSpace = chbWordRestrictAddSpace.Checked;
                    var restrictBreak = chbWordRestrictBreak.Checked;
                    var restrictTitleCase = chbWordRestrictTitleCase.Checked;

                    password = GenerateWordPassword(includeUpperCase, includeNumber, includeSpecial, includeIncomplete, restrictAddSpace, restrictTitleCase, restrictBreak, count);
                    combinations = CalculateWordCombinations(includeUpperCase, includeNumber, includeSpecial, includeIncomplete, restrictAddSpace, restrictTitleCase, restrictBreak, count);
                }
            }

            if (!double.IsNaN(combinations)) {
                var crackDurationText = GetCrackDuration(combinations);
                lblCombinations.Text = "About " + crackDurationText + " to crack";
                tip.SetToolTip(lblCombinations, combinations.ToString("#,##0", CultureInfo.CurrentCulture) + " combinations");
            } else {
                lblCombinations.Text = "?";
                tip.SetToolTip(lblCombinations, null);
            }

            txtPassword.Text = password;

            btnCopy.Enabled = (txtPassword.TextLength > 0);
        }


        #region Word password

        private string[] Words = null;

        private string GenerateWordPassword(bool includeUpperCase, bool includeNumber, bool includeSpecial, bool includeIncomplete, bool spaceSeparated, bool restrictTitleCase, bool restrictBreak, int count) {
            var sb = new StringBuilder();

            if (this.Words == null) {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Bimil.Resources.Words.txt"))
                using (var textStream = new StreamReader(stream)) {
                    this.Words = textStream.ReadToEnd().Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                }
            }

            var selectedWords = new List<List<char>>();
            for (var i = 0; i < count; i++) {
                var wordIndex = GetRandomNumber(this.Words.Length);
                selectedWords.Add(new List<char>(this.Words[wordIndex]));
            }

            if (includeIncomplete) {
                var wordIndex = GetRandomNumber(selectedWords.Count);
                if (restrictBreak) { //break restriction only removes last character
                    selectedWords[wordIndex].RemoveAt(selectedWords[wordIndex].Count - 1);
                } else {
                    var charIndex = GetRandomNumber(selectedWords[wordIndex].Count);
                    selectedWords[wordIndex].RemoveAt(charIndex);
                }
            }

            if (includeUpperCase) {
                var wordIndex = GetRandomNumber(selectedWords.Count);
                int charIndex;
                if (restrictBreak) { //break restriction only uppercases the first character.
                    charIndex = 0;
                } else {
                    charIndex = GetRandomNumber(selectedWords[wordIndex].Count);
                }
                selectedWords[wordIndex][charIndex] = char.ToUpperInvariant(selectedWords[wordIndex][charIndex]);
            }

            if (restrictTitleCase) {
                foreach (var selectedWord in selectedWords) {
                    selectedWord[0] = char.ToUpperInvariant(selectedWord[0]);
                }
            }

            if (includeNumber) {
                var wordIndex = GetRandomNumber(selectedWords.Count);
                int charIndex;
                if (restrictBreak) { //break restriction only adds number before or after the word
                    charIndex = (GetRandomNumber(2) == 0) ? 0 : selectedWords[wordIndex].Count;
                } else {
                    charIndex = GetRandomNumber(selectedWords[wordIndex].Count + 1);
                }
                var number = GetRandomNumber(100);
                selectedWords[wordIndex].InsertRange(charIndex, number.ToString(CultureInfo.InvariantCulture));
            }

            if (includeSpecial) {
                var wordIndex = GetRandomNumber(selectedWords.Count);
                int charIndex;
                if (restrictBreak) { //break restriction only adds character before or after the word
                    charIndex = (GetRandomNumber(2) == 0) ? 0 : selectedWords[wordIndex].Count;
                } else {
                    charIndex = GetRandomNumber(selectedWords[wordIndex].Count + 1);
                }
                var specialIndex = GetRandomNumber(SpecialCharacters.Length);
                selectedWords[wordIndex].Insert(charIndex, SpecialCharacters[specialIndex]);
            }

            for (var i = 0; i < selectedWords.Count; i++) {
                if (spaceSeparated && (sb.Length > 0)) { sb.Append(" "); }
                sb.Append(new string(selectedWords[i].ToArray()));
            }

            return sb.ToString();
        }

        private double CalculateWordCombinations(bool includeUpperCase, bool includeNumber, bool includeSpecial, bool includeIncomplete, bool spaceSeparated, bool restrictTitleCase, bool restrictBreak, int count) {
            //this is really rough calculation assuming everybody knows exactly how password was created and it assumes all words are 5 characters only

            var words = this.Words.Length;
            if (includeUpperCase) { words *= (1 + (restrictBreak ? 1 : 5) - (restrictTitleCase ? 1 : 0)); } //1 original + 5 characters (shortest length) that can be upper case; if break is restricted, only the first character will be upper-case; in case of title-case, first character is assumed fixed
            if (includeIncomplete) { words *= (1 + (restrictBreak ? 1 : 5)); } //1 original + 5 characters (shortest length) that can be upper case; if break is restricted, only the last character will be removed thus only doubling the space
            if (includeNumber) { words *= (restrictBreak ? 2 : 6) * 100; } //number can be inserted at any place in 5 character string (thus 6); if break is restricted, it still can be inserted at the end or beginning of any word
            if (includeSpecial) { words *= (1 + SpecialCharacters.Length * (restrictBreak ? 2 : 6)); } //special character can be inserted in any word at any place; if break is restricted, only start and end are good
            var wordCombinations = Math.Pow(words, count);

            return wordCombinations;
        }

        #endregion


        #region Classic password

        private double CalculateClassicCombinations(bool includeUpperCase, bool includeLowerCase, bool includeNumbers, bool includeSpecial, bool restrictSimilar, bool restrictMovable, bool restrictPronounceable, bool restrictRepeated, int length) {
            var allCharacters = new List<char>();
            var vowelCharacters = new List<char>();
            var consonantCharacters = new List<char>();

            if (includeUpperCase) {
                IncludeCharacters(allCharacters, UpperCaseVowels, UpperCaseConsonants);
                IncludeCharacters(vowelCharacters, UpperCaseVowels);
                IncludeCharacters(consonantCharacters, UpperCaseConsonants);
            }
            if (includeLowerCase) {
                IncludeCharacters(allCharacters, LowerCaseVowels, LowerCaseConsonants);
                IncludeCharacters(vowelCharacters, LowerCaseVowels);
                IncludeCharacters(consonantCharacters, LowerCaseConsonants);
            }
            if (includeNumbers) {
                IncludeCharacters(allCharacters, Digits);
                IncludeCharacters(vowelCharacters, Digits);
                IncludeCharacters(consonantCharacters, Digits);
            }
            if (includeSpecial) {
                IncludeCharacters(allCharacters, SpecialCharacters);
                IncludeCharacters(vowelCharacters, SpecialCharacters);
                IncludeCharacters(consonantCharacters, SpecialCharacters);
            }

            if (restrictSimilar) {
                RemoveCharacters(allCharacters, RestrictedSimilar);
                RemoveCharacters(vowelCharacters, RestrictedSimilar);
                RemoveCharacters(consonantCharacters, RestrictedSimilar);
            }
            if (restrictMovable) {
                RemoveCharacters(allCharacters, RestrictedMoveable);
                RemoveCharacters(vowelCharacters, RestrictedMoveable);
                RemoveCharacters(consonantCharacters, RestrictedMoveable);
            }

            double combinations;
            if (restrictPronounceable) {
                var vowels = length / 2;
                var consonants = length - vowels;
                if (restrictRepeated) {
                    combinations = Math.Pow(vowelCharacters.Count - 1, vowels) * Math.Pow(consonantCharacters.Count - 1, consonants - 1) * (vowelCharacters.Count + consonantCharacters.Count);
                } else {
                    combinations = Math.Pow(vowelCharacters.Count, vowels) * Math.Pow(consonantCharacters.Count, consonants - 1) * (vowelCharacters.Count + consonantCharacters.Count);
                }
            } else {
                if (restrictRepeated) {
                    combinations = allCharacters.Count * Math.Pow(allCharacters.Count - 1, length - 1);
                } else {
                    combinations = Math.Pow(allCharacters.Count, length);
                }
            }

            return combinations;
        }

        private string GenerateClassicPassword(bool includeUpperCase, bool includeLowerCase, bool includeNumbers, bool includeSpecial, bool restrictSimilar, bool restrictMovable, bool restrictPronounceable, bool restrictRepeated, int length) {
            var sb = new StringBuilder();

            var useVowelNext = false;
            while (sb.Length < length) {
                var sixteenth = GetRandomNumber(16);

                List<char> characters = new List<char>();
                if (includeUpperCase && (sixteenth >= 0) && (sixteenth <= 5)) { //Uppercase: 6/16th ~ 37.5%
                    if (restrictPronounceable) {
                        IncludeCharacters(characters, useVowelNext ? UpperCaseVowels : UpperCaseConsonants);
                        useVowelNext = !useVowelNext;
                    } else {
                        IncludeCharacters(characters, UpperCaseVowels, UpperCaseConsonants);
                    }
                } else if (includeLowerCase && (sixteenth >= 6) && (sixteenth <= 11)) { //Lowercase: 6/16th ~ 37.5%
                    if (restrictPronounceable) {
                        IncludeCharacters(characters, useVowelNext ? LowerCaseVowels : LowerCaseConsonants);
                        useVowelNext = !useVowelNext;
                    } else {
                        IncludeCharacters(characters, LowerCaseVowels, LowerCaseConsonants);
                    }
                } else if (includeNumbers && (sixteenth >= 12) && (sixteenth <= 13)) { //Number: 2/16th ~ 12.5%
                    if (restrictPronounceable && !useVowelNext) { continue; } //treat numbers as vowels
                    IncludeCharacters(characters, Digits);
                    useVowelNext = false;
                } else if (includeSpecial && (sixteenth >= 14) && (sixteenth <= 15)) { //Number: 2/16th ~ 12.5%
                    if (restrictPronounceable && !useVowelNext) { continue; } //treat specials as vowels
                    IncludeCharacters(characters, SpecialCharacters);
                    useVowelNext = false;
                }

                if (restrictSimilar) { RemoveCharacters(characters, RestrictedSimilar); }
                if (restrictMovable) { RemoveCharacters(characters, RestrictedMoveable); }

                if (characters.Count > 0) {
                    var charIndex = GetRandomNumber(characters.Count);
                    var nextChar = characters[charIndex];
                    if (restrictRepeated && (sb.Length > 1) && (sb[sb.Length - 1] == nextChar)) { continue; }
                    sb.Append(nextChar);
                }
            }

            return sb.ToString();
        }


        private static void IncludeCharacters(List<char> characterList, params ICollection<char>[] characterCollections) {
            foreach (var characterCollection in characterCollections) {
                characterList.AddRange(characterCollection);
            }
        }

        private static void RemoveCharacters(List<char> characterList, params ICollection<char>[] characterCollections) {
            foreach (var characterCollection in characterCollections) {
                foreach (var character in characterCollection) {
                    characterList.Remove(character);
                }
            }
        }

        #endregion

        private static int GetRandomNumber(int upperLimit) {
            var rndBuffer = new byte[4];
            Rnd.GetBytes(rndBuffer);

            uint maxRandomCount = uint.MaxValue - (uint.MaxValue % (uint)upperLimit);
            uint randomNumber;
            do {
                Rnd.GetBytes(rndBuffer);
                randomNumber = BitConverter.ToUInt32(rndBuffer, 0);
            } while (randomNumber >= maxRandomCount);
            return (int)(randomNumber % (uint)upperLimit);
        }

        private string GetCrackDuration(double combinations) {
            var cracksPerSecond = CracksPerSecond;
            for (var i = 2016; i <= DateTime.Now.Year - 1; i += 2) { cracksPerSecond *= 2; } //Moore's law

            var secondsToCrack = Math.Floor(combinations / cracksPerSecond);
            var minutesToCrack = Math.Floor(secondsToCrack / 60);
            var hoursToCrack = Math.Floor(minutesToCrack / 60);
            var daysToCrack = Math.Floor(hoursToCrack / 24);
            var yearsToCrack = Math.Floor(daysToCrack / 365);
            var centuriesToCrack = Math.Floor(yearsToCrack / 100);
            var millenniumsToCrack = Math.Floor(yearsToCrack / 1000);
            var millionYearsToCrack = Math.Floor(yearsToCrack / 1000000);

            if (millionYearsToCrack >= 1000) {
                return "eternity";
            } else if (millionYearsToCrack >= 1) {
                return millenniumsToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " million years";
            } else if (millenniumsToCrack >= 1) {
                return millenniumsToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((millenniumsToCrack % 10) == 1) ? "millennium" : "millenniums");
            } else if (centuriesToCrack >= 1) {
                return centuriesToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((centuriesToCrack % 10) == 1) ? "century" : "centuries");
            } else if (yearsToCrack >= 1) {
                return yearsToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((yearsToCrack % 10) == 1) ? "year" : "years");
            } else if (daysToCrack >= 1) {
                return daysToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((daysToCrack % 10) == 1) ? "day" : "days");
            } else if (hoursToCrack >= 1) {
                return hoursToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((hoursToCrack % 10) == 1) ? "hour" : "hours");
            } else if (minutesToCrack >= 1) {
                return minutesToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((minutesToCrack % 10) == 1) ? "minute" : "minutes");
            } else {
                return secondsToCrack.ToString("#,##0", CultureInfo.CurrentCulture) + " " + (((secondsToCrack % 10) == 1) ? "second" : "seconds");
            }
        }

    }
}
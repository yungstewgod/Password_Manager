using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saugumo_4_praktinis.UserData
{
    public class User
    {
        private string nickname;
        private string accountPassword;
        private List<PasswordEntry> passwords;

        public User(string nickname, string password)
        {
            if (String.IsNullOrWhiteSpace(nickname))
                throw new Exception("Vartotojo vardo laukasturi būti užpildytas!");
            if (String.IsNullOrWhiteSpace(password))
                throw new Exception("Slaptažodžio laukas turi būti užpildytas!");

            this.nickname = nickname;
            this.accountPassword = password;
            passwords = new List<PasswordEntry>();
        }

        public string GetNickname() => nickname;
        public string GetAccountPassword() => accountPassword;
        public void SetAccountPassword(string data)
        {
            accountPassword = data;
        }
        public PasswordEntry GetPassword(string title) => passwords.Find(x => x.Title == title);
        public void UpdatePassword(string title, PasswordEntry passwordEntry)
        {
            int index = passwords.FindIndex(x => x.Title == title);
            if (index == -1)
            {
                throw new Exception("Slaptažodis nerastas, bandykite dar kartą.");
            }
            passwords[index] = passwordEntry;
        }

        public void AddPassword(PasswordEntry password)
        {
            if (GetPasswordEntry(password.Title) != null)
                throw new Exception("Įvestas slaptažodis jau egzistuoja.");

            passwords.Add(password);
        }

        public void RemovePassword(string title)
        {
            PasswordEntry pw1 = GetPasswordEntry(title);
            if (pw1 == null)
                throw new Exception("Slaptažodis kurį norite ištrinti neegzistuoja.");
            passwords.Remove(pw1);
        }

        public PasswordEntry GetPasswordEntry(string title)
        {
            int index = passwords.FindIndex(x => x.Title == title);
            if (index == -1)
                return null;

            return passwords[index];
        }

        public override string ToString()
        {
            string data = $"{nickname}, {accountPassword}\n";
            foreach (PasswordEntry pe in passwords)
            {
                data += pe.ToString();
            }

            return data;
        }
    }
}

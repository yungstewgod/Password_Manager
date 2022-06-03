using Saugumo_4_praktinis.UserData;
using Saugumo_4_praktinis.Files;
using System;
using System.Windows.Forms;

namespace Saugumo_4_praktinis
{
    public partial class SlaptazodzioRedagavimas : Saugumo_4_praktinis.SlaptazodziuGeneravimas
    {
        private PasswordEntry passwordEntry;
        public SlaptazodzioRedagavimas()
        {
            InitializeComponent();
        }

        public SlaptazodzioRedagavimas(User user) : base(user)
        {
            InitializeComponent();
            DisableFields();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            try
            {
                passwordEntry = user.GetPasswordEntry(titleTextBox.Text);
                if (passwordEntry == null)
                    throw new Exception("Slaptažodis nerastas, bandykite dar kartą");

                passwordTextBox.Text = passwordEntry.Password;
                urlTextBox.Text = passwordEntry.Url;
                commentTextBox.Text = passwordEntry.Comment;
                EnableFields();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Encryption.Decrypt(passwordEntry.Password));
        }

        private void generationButton_Click(object sender, EventArgs e)
        {
            passwordTextBox.Text = Encryption.GeneratePassword();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                user.RemovePassword(passwordEntry.Title);
                MessageBox.Show("Slaptažodis sėkmingai pašalintas");

                ClearFields();
                DisableFields();
                passwordEntry = null;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (passwordEntry == null)
                    return;

                string decrypted = Encryption.Decrypt(passwordEntry.Password);
                string password = passwordEntry.Password;
                if (passwordTextBox.Text != password && passwordTextBox.Text != decrypted)
                {
                    password = passwordTextBox.Text;
                    password = Encryption.Encrypt(password);
                }


                PasswordEntry pentry = new PasswordEntry(
                    titleTextBox.Text, password, urlTextBox.Text, commentTextBox.Text);
                user.UpdatePassword(passwordEntry.Title, pentry);

                MessageBox.Show("Slaptažodis sėkmingai atnaujintas");
                ClearFields();
                DisableFields();
                passwordEntry = null;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void showButton_MouseEnter(object sender, EventArgs e)
        {
            if (passwordEntry == null)
                return;

            if (passwordEntry.Password != passwordTextBox.Text)
                return;

            string password = Encryption.Decrypt(passwordEntry.Password);
            passwordTextBox.Text = password;
        }

        private void showButton_MouseLeave(object sender, EventArgs e)
        {
            if (passwordEntry == null)
                return;

            string password = Encryption.Decrypt(passwordEntry.Password);
            if (passwordTextBox.Text != passwordEntry.Password && passwordTextBox.Text != password)
                return;

            passwordTextBox.Text = passwordEntry.Password;
        }

        private void DisableFields()
        {
            showButton.Enabled = false;
            copyButton.Enabled = false;
            generationButton.Enabled = false;
            deleteButton.Enabled = false;
            updateButton.Enabled = false;
            passwordTextBox.Enabled = false;
            urlTextBox.Enabled = false;
            commentTextBox.Enabled = false;
        }
        private void EnableFields()
        {
            showButton.Enabled = false;
            copyButton.Enabled = false;
            generationButton.Enabled = false;
            deleteButton.Enabled = false;
            updateButton.Enabled = false;
            passwordTextBox.Enabled = false;
            urlTextBox.Enabled = false;
            commentTextBox.Enabled = false;
        }

        private void addButton_Click(object sender, EventArgs e)
        {

        }
    }
}


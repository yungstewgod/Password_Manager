using Saugumo_4_praktinis.UserData;
using Saugumo_4_praktinis.Files;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Saugumo_4_praktinis
{
    public partial class SlaptazodziuGeneravimas : Form
    {
        protected User user;
        public SlaptazodziuGeneravimas()
        {
            InitializeComponent();
        }

        public SlaptazodziuGeneravimas(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                string password = Encryption.Encrypt(passwordTextBox.Text);
                user.AddPassword(new PasswordEntry(
                    titleTextBox.Text, password, urlTextBox.Text, commentTextBox.Text));

                MessageBox.Show("Slaptažodis sėkmingai pridėtas");
                ClearFields();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void generationButton_Click(object sender, EventArgs e)
        {
            passwordTextBox.Text = Encryption.GeneratePassword();
        }

        protected void ClearFields()
        {
            titleTextBox.Text = "";
            passwordTextBox.Text = "";
            urlTextBox.Text = "";
            commentTextBox.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
using Saugumo_4_praktinis.UserData;
using Saugumo_4_praktinis.Files;
using System;
using System.Windows.Forms;

namespace Saugumo_4_praktinis
{
    public partial class Registracija:Form
    {
        private User user;
        public Registracija()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                User user = new User(nameTextBox.Text, passwordTextBox.Text);
                user.SetAccountPassword(BCrypt.Net.BCrypt.HashPassword(passwordTextBox.Text)) ;
                Console.WriteLine(user.ToString());

                FileControl fileManager = new FileControl();
                fileManager.WriteAFile(FilePath.GetPathTxt(user.GetNickname()), user.ToString());
                FailuEncryption.EncryptCombo(FilePath.GetPathTxt(user.GetNickname()));

                MessageBox.Show("Sėkmingai prisijungėte.");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                MessageBox.Show(exc.Message);
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                FileControl fileManager = new FileControl();
                
                FailuEncryption.DecryptCombo(FilePath.GetPathAes(nameTextBox.Text));
                string[] data = fileManager.ReadFile(FilePath.GetPathTxt(nameTextBox.Text));

                user = ConverToString.GetStringToUser(data);
                
                if (!BCrypt.Net.BCrypt.Verify(passwordTextBox.Text, user.GetAccountPassword()))
                    throw new Exception("Slaptažodis nerastas.");

                Form form = new SlaptazodziuMeniu(user);
                form.ShowDialog();
            }
            catch (Exception exc)
            {
                if (user != null)
                   FailuEncryption.EncryptCombo(FilePath.GetPathTxt(user.GetNickname()));
                MessageBox.Show(exc.Message);
            }
        }

        private void Registracija_Load(object sender, EventArgs e)
        {

        }
    }
}
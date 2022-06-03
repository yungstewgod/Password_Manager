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
    public partial class SlaptazodziuMeniu : Form
    {
        private User user;
        public SlaptazodziuMeniu(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void ManagementForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            FileControl fileManager = new FileControl();
            string data = user.ToString();
            Console.WriteLine(data);
            fileManager.WriteAFile(FilePath.GetPathTxt(user.GetNickname()), data);
            FailuEncryption.EncryptCombo(FilePath.GetPathTxt(user.GetNickname()));
            user = null;
        }

        private void addPasswordButton_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            Form form = new SlaptazodziuGeneravimas(user) { TopLevel = false, FormBorderStyle = FormBorderStyle.None };
            flowLayoutPanel1.Controls.Add(form);
            form.Show();
        }

        private void manageButton_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            Form form = new SlaptazodzioRedagavimas(user) { TopLevel = false, FormBorderStyle = FormBorderStyle.None };
            flowLayoutPanel1.Controls.Add(form);
            form.Show();
        }

    }
}

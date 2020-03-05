using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace rad_dev
{
    public partial class FormGantiPassword : DevExpress.XtraEditors.XtraForm
    {
        String username, passLama, passBaru, confirmasi;
        public FormGantiPassword()
        {
            InitializeComponent();
            textUsername.Text = Class_login.Ses_username;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String cek = cekText();
            if (cek == "isi")
            {
                String idp = Class_login.Ses_id_peg;
                String valid = new ClassGantiPass().cekPassword(username, passLama, passBaru);

                if (valid == "Valid")
                {
                    passBaru = new Class_login().EnkripMD5(passBaru);
                    String simpan = new ClassGantiPass().updatePassword(username, passBaru);
                    if (simpan == "Password Berhasil Di Update" + "\nPengalihan aplikasi...")
                    {
                        MessageBox.Show(simpan);
                        String act = "Ganti Password" +username;
                        new Class_login().auditLogin(idp, username, act);
                    
                        Application.Restart();
                    }
                    else
                    {
                        MessageBox.Show("Password Gagal Di Update");
                    }
                }
                else
                {
                    MessageBox.Show("Password Lama Salah", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    kosongtext();
                }
            }
            
            else
            {
                MessageBox.Show(cek);
            }
            
        }

        public void setData()
        {
            username = textUsername.Text;
            passLama = textPassLama.Text;
            passBaru = textPassBaru.Text;
            confirmasi = textConfirmasi.Text;
        }

        public String cekText()
        {
            String a;
            setData();
            if (textUsername.Text.Length == 0)
            {
                a = "Text Username masih kosong";
                return a;
            }
            else if ((textPassLama.Text.Length == 0)  || (passLama[0] == ' '))
            {
                a = "Text Password Lama masih kosong";
                return a;
            }
            else if ((textPassBaru.Text.Length == 0)  || (passBaru[0] == ' '))
            {
                a = "Text Password Baru masih kosong";
                return a;
            }
            else if ((textConfirmasi.Text.Length == 0)  || (confirmasi[0] == ' '))
            {
                a = "Text Confirmasi Password masih kosong";
                return a;
            }
            else
            {
                a = "isi";
                return a;
            }
        }

        public void kosongtext()
        {
            textUsername.Clear();
            textPassLama.Clear();
            textPassBaru.Clear();
            textConfirmasi.Clear();
        }

        private void textPassLama_KeyPress(object sender, KeyPressEventArgs e)
        {
            const char Delete = (char)8;
            if ((!char.IsLetterOrDigit(e.KeyChar)) && (!char.IsWhiteSpace(e.KeyChar)) && (e.KeyChar != Delete))
            {
                e.Handled = true;
            }

        }
    }
}
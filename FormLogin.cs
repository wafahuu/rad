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
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {
        string username, pass;
        //public static String idp;
        //public static String user;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            cekLogin();
        }
       
        public void setData()
        {
            username = textUsername.Text;
            pass = textPass.Text;
        }

        public String cekText()
        {
            string a;
            setData();

            if(textUsername.Text.Length == 0)
            {
                a = "Text Username Tidak Boleh Kosong";
                return a;
            }
            else if (textPass.Text.Length == 0)
            {
                a = "Password tidak Boleh Kosong";
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
            textPass.Clear();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hubungi Administrator untuk Membuat Akun");
        }

        public void cekLogin()
        {
            String cek = cekText();
            if (cek == "isi")
            {
                String valid = new Class_login().cekLogin(textUsername.Text, textPass.Text);

                if (valid == "blokir")
                {
                    MessageBox.Show("Username dan Password Terblokir", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    kosongtext();
                }
                else
                    if (valid == "Tidak Aktif")
                    {
                        MessageBox.Show("Username dan Password Tidak Aktif", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        kosongtext();
                    }
                    else
                        if (valid == "Tidak Terdaftar")
                        {
                            MessageBox.Show("Username dan Password Tidak Terdaftar", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            kosongtext();
                        }
                        else
                            if (valid == "gagal")
                            {
                                MessageBox.Show("Username dan Password Tidak Valid", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                kosongtext();
                            }
                            else
                            {
                                MessageBox.Show("Selamat Datang " +textUsername.Text, "Berhasil Login Sebagai " +valid, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                if (valid == "Administrator")
                                {
                                    this.Hide();
                                    MenuAdmin admin = new MenuAdmin();
                                    admin.Show();
                                }
                                if (valid == "Staff Tata Usaha")
                                {
                                    this.Hide();
                                    MenuTU tu = new MenuTU();
                                    tu.Show();
                                }
                                if (valid == "Guru")
                                {
                                    this.Hide();
                                    MenuGuru guru = new MenuGuru();
                                    guru.Show();
                                }
                                if (valid == "Wali Kelas")
                                {
                                    this.Hide();
                                    MenuWali wali = new MenuWali();
                                    wali.Show();
                                }
                                if (valid == "Staff Kurikulum")
                                {
                                    this.Hide();
                                    MenuBK bk = new MenuBK();                                    
                                    bk.Show();
                                    
                                }
                                if (valid == "Kepala Sekolah")
                                {
                                    this.Hide();
                                    MenuKepSek ks = new MenuKepSek();
                                    ks.Show();
                                }
                            }
            }
            else
            {
                MessageBox.Show(cek);
            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
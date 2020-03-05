using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace rad_dev
{
    class Class_login
    {
        public static String Ses_id_peg; 
        public static String Ses_username;

        // Method untuk cek login
        public String cekLogin(String usr, String pwd)
        {
            String result = "";
            koneksirad.query = "Select * from tb_user where username = @username";
            koneksirad.param("@username", usr);
            koneksirad.execr();
            if (koneksirad.read())
            {
                String idp = koneksirad.dr["nip"].ToString();
                String user = koneksirad.dr["username"].ToString();
                String pass = koneksirad.dr["password"].ToString();
                String lev = koneksirad.dr["level"].ToString();
                String stat = koneksirad.dr["status"].ToString();
                String itung = koneksirad.dr["coba_pass"].ToString();
                int count = Convert.ToInt32(itung);

                Ses_id_peg = idp;
                Ses_username = user;

                String enc = EnkripMD5(pwd);
                String status = "Tidak Aktif";
                String status1 = "blokir";

                if (status.Equals(stat))
                {  //cek apakah akun tidak aktif
                    result = "Tidak Aktif";
                }
                else
                    if (status1.Equals(stat))
                    {  //cek apakah akun terblokir
                        result = "blokir";
                    }
                    else
                        if (enc.Equals(pass))
                        {
                            if (lev.Equals("Staff Tata Usaha"))
                            {
                                result = "Staff Tata Usaha";
                            }
                            if (lev.Equals("Guru"))
                            {
                                result = "Guru";
                            }
                            if (lev.Equals("Administrator"))
                            {
                                result = "Administrator";
                            }
                            if (lev.Equals("Staff Kurikulum"))
                            {
                                result = "Staff Kurikulum";
                            }
                            if (lev.Equals("Kepala Sekolah"))
                            {
                                result = "Kepala Sekolah";
                            }

                            //reset salah password
                            count = 3;
                            salah_password(count, usr);
                            //setSession(idp, usr);
                            auditLogin(idp, user, "Login Ke Sistem");
                        }
                        else
                        {
                            result = "gagal";
                            //hitung coba pass tersisa
                            count--;
                            salah_password(count, usr);
                            if (count >= 0)
                            {
                                MessageBox.Show("Maksimal salah password tersisa " + count + " kali lagi");
                            }
                        }
            }
            else
            {
                result = "Tidak Terdaftar";
            }
            return result;
        }

        // Method untuk enkripsi MD5
        public String EnkripMD5(String passen)
        {
            try
            {
                //Masukkan di kalkulasi ke md5 hash
                MD5 Md5 = MD5.Create();
                byte[] inputbytes = Encoding.ASCII.GetBytes(passen);

                byte[] hasbytes = Md5.ComputeHash(inputbytes);

                //convert bytes to hexadecimal

                StringBuilder strBuild = new StringBuilder();

                for (int i = 0; i < hasbytes.Length; i++)
                {
                    strBuild.Append(hasbytes[i].ToString("X2"));
                }

                return strBuild.ToString();
            }
            catch (CryptographicException e)
            {
                MessageBox.Show(e.ToString());
            }
            return null;
        }

        // Method Untuk Salah Input Password
        public String salah_password(int count, String usr)
        {

            String sts = "blokir";

            if (count > 0)
            {
                try
                {
                    koneksirad.query = "update tb_user set coba_pass = @cp where username = @username";
                    koneksirad.param("@cp", count);
                    koneksirad.param("@username", usr);
                    koneksirad.exec();
                    koneksirad.close();
                }

                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString()); ;
                }
            }
            else
            {
                try
                {
                    koneksirad.query = "Update tb_user set status = @status where username = @username";
                    koneksirad.param("@status", sts);
                    koneksirad.param("@username", usr);

                    int i = koneksirad.exec();

                    if (i != 0)
                    {
                        //audit
                        try
                        {
                            koneksirad.query = "select * from tb_user where username = @username";
                            koneksirad.param("@username", usr);
                            koneksirad.execr();
                            if (koneksirad.read())
                            {
                                String idp = koneksirad.dr["nip"].ToString();
                                String user = koneksirad.dr["username"].ToString();
                                new Class_login().auditLogin(idp, user, "Akun Terblokir");
                            }
                        }
                        catch (SqlException e)
                        {
                            MessageBox.Show(e.ToString());
                        }
                    }

                    koneksirad.close();
                }
                catch (SqlException e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
            return null;
        }

        // Method untuk audit login
        public String auditLogin(String idpeg, String usr, String act)
	    {
            string dateNow = DateTime.Now.ToShortDateString();
            
		    String result = "";
		    try
		    {	
		        koneksirad.query = "insert into tb_audit(nip, username, aktifitas, tanggal)values(@nip, @username, @aktif, @tgl)";
			    koneksirad.param("@nip", idpeg);
                koneksirad.param("@username", usr);
                koneksirad.param("@aktif", act);
                koneksirad.param("@tgl", dateNow);
			    
                int i = koneksirad.exec();
			    if(i!=0) 
			    {
  				    result = "Data Berhasil Disimpan";
  			    }
			    else 
			    {	
				    result = "Data Gagal Disimpan";			
  			    }
			    koneksirad.close();
		    }
		
		    catch(SqlException e){
			    MessageBox.Show(e.ToString());
		    }
		    //---------------------------------
		    //Copy to file.txt
            /*
		    try {
			    String str = idpeg+ " , " +usr+ " , " +act+ " , " +dateNow+ "System.out.println()";
			    FileWriter fout = new FileWriter ("audit_log/audit.txt");
			    fout.write(str, 0, str.length());
			    fout.write(str);
			    fout.close();
		    }
		    catch (FileNotFoundException fe) {
			    System.out.println("Exception : " +fe.toString());
		    }
		    catch (IOException e2) {
			    System.out.println("Exception : " +e2.toString());
		    }*/
		
		    return result;
	    }
        /*
        public void setSession(String idpegawai, String username)
        {
            this.Ses_id_peg = idpegawai;
            this.Ses_username = username;
        }

        // Method untuk get session id pegawai
        public String getSession_idpegawai()
        {
            return Ses_id_peg;
        }

        //Method untuk get session username
        public String getSession_username()
        {
            return Ses_username;
        }*/
    }
}

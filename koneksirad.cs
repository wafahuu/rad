using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace rad_dev
{
    class koneksirad
    {
        public static String data_source = "WAFA-PC";
        public static String initial_catalog = "raport";
        public static String user = "sa";
        public static String pass = "sa";
        public static String integrated_security = "false";

        public static SqlConnection con = new SqlConnection();
        public static SqlCommand cmd = new SqlCommand();
        public static SqlDataReader dr;
        public static SqlDataAdapter da;
        public static DataSet ds;
        public static String query = "";
        public static String pesan_error = "";
        
        public static void open()
        {
            try
            {
                if (dr.IsClosed == false)
                {
                    dr.Close();
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = "Data Source=" + data_source +
                            ";Initial Catalog=" + initial_catalog +
                            ";Integrated Security=" + integrated_security +
                            ";user=" + user +
                            ";password=" + pass + ";";
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                pesan_error = ex.ToString();
            }
        }
        public static int exec()
        {
            try
            {
                open();
                cmd.Connection = con;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return 1;
            }
            catch (Exception ex)
            {
                if (koneksirad.query == "insert into tb_user (nip, username, password, level, status, coba_pass) values (@nip, @user, @pass, @level, @status, @coba_pass)")
                {
                    pesan_error = "Username Harus Unik";
                    MessageBox.Show(pesan_error);
                    return 0;
                }
                else
                {
                    pesan_error = ex.ToString();
                    MessageBox.Show(pesan_error);
                    return 0;
                }
            }
        }
        public static int execr()
        {
            try
            {
                open();
                cmd.Connection = con;
                cmd.CommandText = query;
                dr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return 1;
            }
            catch (Exception ex)
            {
                pesan_error = ex.ToString();
                MessageBox.Show(pesan_error); 
                return 0;
            }
        }
        
        public static bool read()
        {
            return dr.Read();
        }
        public static void param(String parameter, Object value)
        {
            cmd.Parameters.AddWithValue(parameter, value);
        }
        public static void close()
        {
            con.Close();
        }
        public static void fill()
        {
            open();
            ds = new DataSet();
            da = null;
            da = new SqlDataAdapter(query, con);
            da.Fill(ds,"rad");
        }
        public static DataTable result()
        {
            return ds.Tables[0];
        }

        internal static void param()
        {
            throw new NotImplementedException();
        }

        internal static void fill(DataGridView dataGridEkstra)
        {
            throw new NotImplementedException();
        }
    }
}

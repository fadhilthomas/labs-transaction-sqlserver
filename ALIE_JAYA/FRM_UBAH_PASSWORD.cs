using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PROJECT_KARYA
{
    public partial class FRM_UBAH_PASSWORD : Form
    {
        SqlConnection con = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlConnection con2 = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlCommand cmd, cmd2;
        SqlDataAdapter adapt;


        public FRM_UBAH_PASSWORD()
        {
            InitializeComponent();
        }

        private void btUbah_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUser.Text != string.Empty)
                {
                    if (txtPass.Text != string.Empty)
                    {
                        if (txtPassBaru.Text != string.Empty)
                            {
                                if (txtPassBaru2.Text == txtPassBaru.Text)
                                {

                                    String query = "select * from tbl_hak_akses where username = '" + txtUser.Text + "' and password = '" + txtPass.Text + "'";
                                    cmd = new SqlCommand(query, con);
                                    SqlDataReader dbr;
                                    con.Open();
                                    dbr = cmd.ExecuteReader();
                                    int count = 0;
                                    while (dbr.Read())
                                    {
                                        count += 1;
                                    }
                                    if (count == 1)
                                    {
                                        cmd2 = new SqlCommand("update tbl_hak_akses set password=@password where username=@username", con2);
                                        cmd2.Parameters.AddWithValue("@password", txtPassBaru.Text);
                                        cmd2.Parameters.AddWithValue("@username", txtUser.Text);
                                        con2.Open();
                                        cmd2.ExecuteNonQuery();
                                        MessageBox.Show("Ubah Password Berhasil");
                                        txtUser.Text = "";
                                        txtPass.Text = "";
                                        txtPassBaru.Text = "";
                                        txtPassBaru2.Text = "";
                                        con2.Close();
                                    }
                                    else if (count > 1)
                                    {
                                        MessageBox.Show("Username dan Password tidak boleh sama.", "Login");
                                    }
                                    else
                                    {
                                        MessageBox.Show("Username, Password lama salah.", "Login");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Password baru tidak cocok.", "Login");
                                }
                        }
                        else
                        {
                            MessageBox.Show("Password baru tidak boleh kosong.", "Login");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Password tidak boleh kosong.", "Login");
                    }
                }
                else
                {
                    MessageBox.Show("Username tidak boleh kosong.", "Login");
                }
                con.Close();
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message);
            }
        }
    }
}

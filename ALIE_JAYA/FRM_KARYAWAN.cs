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
    public partial class FRM_KARYAWAN : Form
    {
        SqlConnection con = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;


        public FRM_KARYAWAN()
        {
            InitializeComponent();
            DisplayData();
            otomatis();
            lbJam.Text = DateTime.Now.ToLongTimeString();
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        private void otomatis()
        {
            long hitung;
            string urut;

            con.Open();
            cmd = new SqlCommand("select kode_karyawan from tbl_karyawan where kode_karyawan in(select max(kode_karyawan) from tbl_karyawan) order by kode_karyawan desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["kode_karyawan"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "KRY" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "KRY0001";
            }
            rd.Close();
            txtKode.Text = urut;
            con.Close();
        }

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_karyawan AS 'Kode Karyawan', nama_karyawan AS 'Nama Karyawan', no_telp AS 'Nomor Telepon', alamat_karyawan AS 'Alamat', status AS 'Status' from tbl_karyawan", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

        }

        private void CariData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_karyawan AS 'Kode Karyawan', nama_karyawan AS 'Nama Karyawan', no_telp AS 'Nomor Telepon', alamat_karyawan AS 'Alamat' from tbl_karyawan where kode_karyawan LIKE '%" + txtCari.Text + "%' OR nama_karyawan LIKE '%" + txtCari.Text + "%';", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void CleanText()
        {
            txtKode.Text = "";
            txtNama.Text = "";
            txtTelepon.Text = "";
            txtAlamat.Text = "";
            txtKode.Enabled = false;
        }

        private void btBaru_Click(object sender, EventArgs e)
        {
            CleanText();
            otomatis();
        }

        private void btSimpan_Click(object sender, EventArgs e)
        {
            if (txtKode.Text != "" && txtNama.Text != "" && txtTelepon.Text != "" && txtAlamat.Text != "" && cbStatus.SelectedIndex!=0)
            {
                cmd = new SqlCommand("insert into tbl_karyawan(kode_karyawan,nama_karyawan,no_telp,alamat_karyawan, status) values(@kode,@nama,@telepon,@alamat, @status)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode", txtKode.Text);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@telepon", txtTelepon.Text);
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                cmd.Parameters.AddWithValue("@status", cbStatus.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data berhasil disimpan", "Berhasil");
                DisplayData();
                CleanText();
            }
            else
            {
                MessageBox.Show("Gagal simpan", "Gagal");
            } 
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtKode.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtNama.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtTelepon.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAlamat.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtKode.Enabled = false;
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (txtKode.Text != "" && txtNama.Text != "" && txtTelepon.Text != "" && txtAlamat.Text != "" && cbStatus.SelectedIndex != 0)
            {
                cmd = new SqlCommand("update tbl_karyawan set nama_karyawan=@nama,no_telp=@telepon,alamat_karyawan=@alamat where kode_karyawan=@kode", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode", txtKode.Text);
                cmd.Parameters.AddWithValue("@nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@telepon", txtTelepon.Text);
                cmd.Parameters.AddWithValue("@alamat", txtAlamat.Text);
                cmd.Parameters.AddWithValue("@status", cbStatus.SelectedItem.ToString());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ubah berhasil", "Berhasil");
                con.Close();
                DisplayData();
                CleanText();
            }
            else
            {
                MessageBox.Show("Gagal ubah", "Gagal");
            }  
        }

        private void btHapus_Click(object sender, EventArgs e)
        {
            if (txtKode.Text != "")
            {
                cmd = new SqlCommand("delete tbl_karyawan where kode_karyawan=@kode", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode", txtKode.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Berhasil hapus", "Berhasil");
                DisplayData();
                CleanText();
            }
            else
            {
                MessageBox.Show("Gagal hapus", "Gagal");
            }  
        }

        private void btKeluar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            CariData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbJam.Text = DateTime.Now.ToLongTimeString();
        }
    }
}

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
    public partial class FRM_BARANG : Form
    {
        SqlConnection con = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        public FRM_BARANG()
        {
            InitializeComponent();
            DisplayData();
            CleanText();
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
            cmd = new SqlCommand("select kode_barang from tbl_barang where kode_barang in(select max(kode_barang) from tbl_barang) order by kode_barang desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["kode_barang"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "BRG" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "BRG0001";
            }
            rd.Close();
            txtKodeBarang.Text = urut;
            con.Close();
        }


        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', harga AS 'Harga', stock AS 'Stock' from tbl_barang", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();

        }

        private void CariData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', harga AS 'Harga', stock AS 'Stock' from tbl_barang where kode_barang LIKE '%" + txtCari.Text + "%' OR nama_barang LIKE '%" + txtCari.Text + "%';", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void CleanText()
        {
            txtKodeBarang.Text = "";
            txtNamaBarang.Text = "";
            txtHarga.Text = "0";
            txtStock.Text = "0";
            txtKodeBarang.Enabled = false;
            txtStock.Enabled = false;

        }

        private void FRM_BARANG_Load(object sender, EventArgs e)
        {
            
        }

        private void btSimpan_Click(object sender, EventArgs e)
        {
            if (txtKodeBarang.Text != "" && txtNamaBarang.Text != "" && txtHarga.Text != "" && txtStock.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_barang(kode_barang,nama_barang,harga,stock) values(@kode_barang,@nama_barang,@harga,@stock)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                cmd.Parameters.AddWithValue("@nama_barang", txtNamaBarang.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@stock", txtStock.Text);
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
            txtKodeBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtNamaBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtHarga.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtStock.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtKodeBarang.Enabled = false;
        }

        private void btKeluar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            if (txtKodeBarang.Text != "" && txtNamaBarang.Text != "" && txtHarga.Text != "" && txtStock.Text != "")
            {
                cmd = new SqlCommand("update tbl_barang set nama_barang=@nama_barang,harga=@harga,stock=@stock where kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                cmd.Parameters.AddWithValue("@nama_barang", txtNamaBarang.Text);
                cmd.Parameters.AddWithValue("@harga", txtHarga.Text);
                cmd.Parameters.AddWithValue("@stock", txtStock.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Ubah berhasil","Berhasil");
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
            if (txtKodeBarang.Text != "")
            {
                cmd = new SqlCommand("delete tbl_barang where kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
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

        private void btBaru_Click(object sender, EventArgs e)
        {
            CleanText();
            otomatis();
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            CariData();
        }

        private void txtHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbJam.Text = DateTime.Now.ToLongTimeString();
        }  
    }
}

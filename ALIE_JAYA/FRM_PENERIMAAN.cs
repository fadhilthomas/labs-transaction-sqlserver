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
    public partial class FRM_PENERIMAAN : Form
    {
        SqlConnection con = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        public FRM_PENERIMAAN()
        {
            InitializeComponent();
            DisplayData();
            otomatis();
            DisplayDataBar();
            total_terbilang();
        }

        private void CleanText()
        {
            txtKodeBarang.Text = "";
            txtNamaBarang.Text = "";
            txtStock.Text = "0";
            txtHarga.Text = "0";
            txtJumlah.Text = "";
            txtKet.Text = "";
        }

        private void CleanAll()
        {
            txtKodeBarang.Text = "";
            txtNamaBarang.Text = "";
            txtStock.Text = "";
            txtHarga.Text = "";
            txtJumlah.Text = "";
            txtKet.Text = "";
        }

        private void total_terbilang()
        {
            con.Close();
            cmd = new SqlCommand("select sub_total from tbl_penerimaan where no_penerimaan = '" + txtNoPenerimaan.Text + "'", con);
            SqlDataReader dbr;
            con.Open();
            dbr = cmd.ExecuteReader();
            int count = 0;
            while (dbr.Read())
            {
                count += 1;
            }
            if (count != 0)
            {
                txtTotal.Text = total("sub_total", "tbl_penerimaan").Rows[0][0].ToString();
                lblTerbilang.Text = Terbilang(int.Parse(txtTotal.Text)) + "rupiah";
                lblTerbilang.Text = lblTerbilang.Text.Replace("  ", " ");
            }
            else
            {
                txtTotal.Text = "0";
                lblTerbilang.Text = "nol rupiah";
            }
            con.Close();
        }

        public static string Terbilang(int x)
        {
            string[] bilangan = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas" };
            string temp = "";

            if (x < 12)
            {
                temp = " " + bilangan[x];
            }
            else if (x < 20)
            {
                temp = Terbilang(x - 10).ToString() + " belas";
            }
            else if (x < 100)
            {
                temp = Terbilang(x / 10) + " puluh" + Terbilang(x % 10);
            }
            else if (x < 200)
            {
                temp = " seratus" + Terbilang(x - 100);
            }
            else if (x < 1000)
            {
                temp = Terbilang(x / 100) + " ratus" + Terbilang(x % 100);
            }
            else if (x < 2000)
            {
                temp = " seribu" + Terbilang(x - 1000);
            }
            else if (x < 1000000)
            {
                temp = Terbilang(x / 1000) + " ribu" + Terbilang(x % 1000);
            }
            else if (x < 1000000000)
            {
                temp = Terbilang(x / 1000000) + " juta" + Terbilang(x % 1000000);
            }

            return temp;
        }

        private void otomatis()
        {
            long hitung;
            string urut;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select no_penerimaan from tbl_penerimaan_detail where no_penerimaan in(select max(no_penerimaan) from tbl_penerimaan_detail) order by no_penerimaan desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["no_penerimaan"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "PNR" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "PNR0001";
            }
            rd.Close();
            txtNoPenerimaan.Text = urut;
            con.Close();
        }

        private void DisplayData()
        {
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select no_penerimaan AS 'Nomor Penerimaan', tgl_penerimaan AS 'Tanggal Penerimaan', kode_barang AS 'Kode Barang', jumlah_terima AS 'Jumlah Penerimaan', sub_total AS 'Sub Total', keterangan AS 'Keterangan' from tbl_penerimaan", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void DisplayDataBar()
        {
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', harga AS 'Harga', stock AS 'Stock' from tbl_barang", con);
            adapt.Fill(dt);
            dGBar.DataSource = dt;
            con.Close();
        }

        private void CariDataBar()
        {
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', satuan AS 'Satuan', harga AS 'Harga', stock AS 'Stock' from tbl_barang where kode_barang LIKE '%" + txtCariBarang.Text + "%' OR nama_barang LIKE '%" + txtCariBarang.Text + "%';", con);
            adapt.Fill(dt);
            dGBar.DataSource = dt;
            con.Close();
        }

        
        private DataTable edit(string field, string tabel, string kriteria, string syarat)
        {
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select " + field + " from " + tabel + " where " + kriteria + " = '" + syarat + "'", con);
            adapt.Fill(dt);
            con.Close();
            return dt;
        }

        private DataTable total(string field, string tabel)
        {
            con.Close();
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select sum(" + field + ") from " + tabel, con);
            adapt.Fill(dt);
            con.Close();
            return dt;
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            DisplayDataBar();
            pnBarang.Visible = true;
            pnBarang.BringToFront();
        }

        private void btBaru_Click(object sender, EventArgs e)
        {
            otomatis();
            txtNoPenerimaan.Enabled = false;
            CleanAll();
            dateTimePicker1.Value = DateTime.Now;
        }

        
        private void dGBar_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtKodeBarang.Text = dGBar.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtNamaBarang.Text = dGBar.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtHarga.Text = dGBar.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtStock.Text = dGBar.Rows[e.RowIndex].Cells[3].Value.ToString();
            pnBarang.Visible = false;
        }


        private void btTambah_Click(object sender, EventArgs e)
        {

            if (txtKodeBarang.Text != "" && txtNoPenerimaan.Text != "" && txtJumlah.Text != "" && txtKet.Text != "" && int.Parse(txtJumlah.Text) > 0)
            {
                con.Close();
                cmd = new SqlCommand("select no_penerimaan, kode_barang from tbl_penerimaan where no_penerimaan = '" + txtNoPenerimaan.Text + "' and kode_barang = '" + txtKodeBarang.Text + "'", con);
                SqlDataReader dbr;
                con.Open();
                dbr = cmd.ExecuteReader();
                int count = 0;
                while (dbr.Read())
                {
                    count += 1;
                }
                if (count == 0)
                {
                    cmd = new SqlCommand("insert into tbl_penerimaan(no_penerimaan, tgl_penerimaan, kode_barang, jumlah_terima, sub_total, keterangan) values(@no_penerimaan, @tgl_penerimaan, @kode_barang, @jumlah_terima, @sub_total, @keterangan)", con);
                    con.Close();
                    con.Open();
                    cmd.Parameters.AddWithValue("@no_penerimaan", txtNoPenerimaan.Text);
                    cmd.Parameters.Add("@tgl_penerimaan", SqlDbType.Date).Value = dateTimePicker1.Value.Date;
                    cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                    cmd.Parameters.AddWithValue("@jumlah_terima", txtJumlah.Text);
                    cmd.Parameters.AddWithValue("@sub_total", txtSubTotal.Text);
                    cmd.Parameters.AddWithValue("@keterangan", txtKet.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    cmd = new SqlCommand("update tbl_barang set stock=@stock_akhir where kode_barang=@kode_barang", con);
                    con.Open();
                    String stock_akhir = (int.Parse(txtJumlah.Text) + int.Parse(txtStock.Text)).ToString();
                    cmd.Parameters.AddWithValue("@stock_akhir", stock_akhir);
                    cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Data berhasil disimpan", "Berhasil");
                    total_terbilang();
                    DisplayData();
                    CleanText();
                }
                else
                {
                    MessageBox.Show("Transaksi sudah ada", "Gagal");
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Gagal simpan", "Gagal");
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNoPenerimaan.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtKodeBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtKet.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtNamaBarang.Text = edit("nama_barang", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtHarga.Text = edit("harga", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtStock.Text = edit("stock", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtJumlah.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSubTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void btHapus_Click(object sender, EventArgs e)
        {
            con.Close();
            if (txtNamaBarang.Text != "")
            {
                cmd = new SqlCommand("delete tbl_penerimaan where no_penerimaan=@no_penerimaan AND kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_penerimaan", txtNoPenerimaan.Text);
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                cmd = new SqlCommand("update tbl_barang set stock=@stock_akhir where kode_barang=@kode_barang", con);
                con.Open();
                String stock_akhir = (int.Parse(txtStock.Text) - int.Parse(txtJumlah.Text)).ToString();
                cmd.Parameters.AddWithValue("@stock_akhir", stock_akhir);
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Berhasil hapus", "Berhasil");
                DisplayData();
                CleanText();
                total_terbilang();
            }
            else
            {
                MessageBox.Show("Gagal hapus", "Gagal");
            }
        }

        private void btSimpan_Click(object sender, EventArgs e)
        {
            con.Close();
            if (txtNoPenerimaan.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_penerimaan_detail(no_penerimaan, tgl_penerimaan, kode_barang, jumlah_terima, sub_total, keterangan) select no_penerimaan, tgl_penerimaan, kode_barang, jumlah_terima, sub_total, keterangan from tbl_penerimaan ", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                cmd = new SqlCommand("delete tbl_penerimaan where no_penerimaan=@no_penerimaan", con);
                cmd.Parameters.AddWithValue("@no_penerimaan", txtNoPenerimaan.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Transaksi tersimpan", "Berhasil");
                DisplayData();
                total_terbilang();
                CleanAll();
                otomatis();
            }
            else
            {
                MessageBox.Show("Transaksi gagal tersimpan", "Gagal");
            }
        }

        private void txtCariBarang_TextChanged(object sender, EventArgs e)
        {
            CariDataBar();
        }

        private void txtJumlah_TextChanged(object sender, EventArgs e)
        {
            if (txtJumlah.Text != "") txtSubTotal.Text = (int.Parse(txtHarga.Text) * int.Parse(txtJumlah.Text)).ToString();
            else txtSubTotal.Text = "0";
        }

        private void txtJumlah_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }
    }
}

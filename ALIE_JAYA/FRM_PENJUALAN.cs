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
    public partial class FRM_PENJUALAN : Form
    {
        SqlConnection con = new SqlConnection("Server=KP-002\\SQLEXPRESS;Database=ALIE_JAYA;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        public FRM_PENJUALAN()
        {
            InitializeComponent();
            DisplayData();
            otomatis();
            DisplayDataBar();
            DisplayDataPel();
            total_terbilang();
        }

        private void CleanText()
        {
            txtKodeBarang.Text = "";
            txtNamaBarang.Text = "";
            txtStock.Text = "";
            txtHarga.Text = "0";
            txtSubTotal.Text = "0";
            txtKet.Text = "";
            txtJumlahBeli.Text = "0";
        }

        private void CleanAll()
        {
            txtKodeBarang.Text = "";
            txtNamaBarang.Text = "";
            txtStock.Text = "";
            txtHarga.Text = "0";
            txtKode.Text = "";
            txtAlamat.Text = "";
            txtTelepon.Text = "";
            txtNama.Text = "";
            txtSubTotal.Text = "0";
            txtKet.Text = "";
            txtJumlahBeli.Text = "0";
        }

        private void total_terbilang()
        {
                con.Close();
                cmd = new SqlCommand("select sub_total from tbl_penjualan where no_penjualan = '" + txtNoOrder.Text + "'", con);
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
                    txtTotal.Text = total("sub_total", "tbl_penjualan").Rows[0][0].ToString();
                    lblTerbilang.Text = Terbilang(int.Parse(txtTotal.Text)) + "rupiah";
                    lblTerbilang.Text = lblTerbilang.Text.Replace("  "," ");
                }
                else
                {
                    txtTotal.Text = "0";
                    lblTerbilang.Text = "nol rupiah";
                }
                con.Close();
        }

        private void otomatis()
        {
            long hitung;
            string urut;

            con.Open();
            cmd = new SqlCommand("select no_penjualan from tbl_penjualan_detail where no_penjualan in(select max(no_penjualan) from tbl_penjualan_detail) order by no_penjualan desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["no_penjualan"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "PJL" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "PJL0001";
            }
            rd.Close();
            txtNoOrder.Text = urut;
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


        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select no_penjualan AS 'Nomor Penjualan', tgl_penjualan AS 'Tanggal Penjualan', kode_pelanggan AS 'Kode Pelanggan', kode_barang AS 'Kode Barang', jumlah_beli AS 'Jumlah Beli', sub_total AS 'Sub Total', keterangan AS 'Keterangan' from tbl_penjualan", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void DisplayDataPel()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_pelanggan AS 'Kode Pelanggan', nama_pelanggan AS 'Nama Pelanggan', no_telp AS 'Nomor Telepon', alamat_pelanggan AS 'Alamat' from tbl_pelanggan", con);
            adapt.Fill(dt);
            dGPel.DataSource = dt;
            con.Close();
        }

        private void DisplayDataBar()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', harga AS 'Harga', stock AS 'Stock' from tbl_barang", con);
            adapt.Fill(dt);
            dGBar.DataSource = dt;
            con.Close();
        }

        private void CariDataBar()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_barang AS 'Kode Barang', nama_barang AS 'Nama Barang', harga AS 'Harga', stock AS 'Stock' from tbl_barang where kode_barang LIKE '%" + txtCari.Text + "%' OR nama_barang LIKE '%" + txtCari.Text + "%';", con);
            adapt.Fill(dt);
            dGBar.DataSource = dt;
            con.Close();
        }

        private void CariDataPel()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select kode_pelanggan AS 'Kode Pelanggan', nama_pelanggan AS 'Nama Pelanggan', no_telp AS 'Nomor Telepon', alamat_pelanggan AS 'Alamat' from tbl_pelanggan where kode_pelanggan LIKE '%" + txtCari.Text + "%' OR nama_pelanggan LIKE '%" + txtCari.Text + "%';", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private DataTable edit(string field, string tabel, string kriteria, string syarat)
        {
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

        private void btPilihPelanggan_Click(object sender, EventArgs e)
        {
            if (pnPelanggan.Visible == false)
            {
                pnPelanggan.Visible = true;
                pnPelanggan.BringToFront();
                DisplayDataPel();
            }
            else
            {
                pnPelanggan.Visible = false;
            }
        }

        private void btPilihBarang_Click(object sender, EventArgs e)
        {
            if (pnBarang.Visible == false)
            {
                pnBarang.Visible = true;
                DisplayDataBar();
                pnBarang.BringToFront();
            }
            else
            {
                pnBarang.Visible = false;
            }
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
            if (txtKode.Text != "" && txtKodeBarang.Text != "" && txtNoOrder.Text != "" && txtJumlahBeli.Text != "" && txtKet.Text != "" && txtSubTotal.Text != "" && int.Parse(txtJumlahBeli.Text)>0)
            {
                con.Close();
                cmd = new SqlCommand("select no_penjualan, kode_pelanggan, kode_barang from tbl_penjualan where no_penjualan = '" + txtNoOrder.Text + "' and kode_pelanggan = '" + txtKode.Text + "' and kode_barang = '" + txtKodeBarang.Text + "'", con);
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
                    if (int.Parse(txtJumlahBeli.Text) < int.Parse(txtStock.Text))
                    {
                        con.Close();
                        cmd = new SqlCommand("insert into tbl_penjualan(no_penjualan, tgl_penjualan, kode_pelanggan, kode_barang, jumlah_beli, sub_total, keterangan) values(@no_penjualan, @tgl_penjualan, @kode_pelanggan, @kode_barang, @jumlah_beli, @sub_total, @keterangan)", con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@no_penjualan", txtNoOrder.Text);
                        cmd.Parameters.Add("@tgl_penjualan", SqlDbType.Date).Value = dateTimePicker1.Value.Date;
                        cmd.Parameters.AddWithValue("@kode_pelanggan", txtKode.Text);
                        cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                        cmd.Parameters.AddWithValue("@jumlah_beli", txtJumlahBeli.Text);
                        cmd.Parameters.AddWithValue("@sub_total", txtSubTotal.Text);
                        cmd.Parameters.AddWithValue("@keterangan", txtKet.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();

                        cmd = new SqlCommand("update tbl_barang set stock=@stock_akhir where kode_barang=@kode_barang", con);
                        con.Open();
                        String stock_akhir = (int.Parse(txtStock.Text) - int.Parse(txtJumlahBeli.Text)).ToString();
                        cmd.Parameters.AddWithValue("@stock_akhir", stock_akhir);
                        cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        total_terbilang();
                        MessageBox.Show("Data berhasil disimpan", "Berhasil");
                        DisplayData();
                        CleanText();
                    }
                    else
                    {
                        MessageBox.Show("Stock harus lebih besar dari jumlah beli", "Gagal");
                    }
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
            txtNoOrder.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            dateTimePicker1.Value = new DateTime(int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Substring(6, 4)), int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Substring(3, 2)), int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString().Substring(0, 2)));
            txtKode.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtKodeBarang.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtNama.Text = edit("nama_pelanggan", "tbl_pelanggan", "kode_pelanggan", txtKode.Text).Rows[0][0].ToString();
            txtTelepon.Text = edit("no_telp", "tbl_pelanggan", "kode_pelanggan", txtKode.Text).Rows[0][0].ToString();
            txtAlamat.Text = edit("alamat_pelanggan", "tbl_pelanggan", "kode_pelanggan", txtKode.Text).Rows[0][0].ToString();
            txtNamaBarang.Text = edit("nama_barang", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtHarga.Text = edit("harga", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtStock.Text = edit("stock", "tbl_barang", "kode_barang", txtKodeBarang.Text).Rows[0][0].ToString();
            txtJumlahBeli.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtSubTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            txtKet.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
        }

        private void btHapus_Click(object sender, EventArgs e)
        {
            if (txtNamaBarang.Text != "")
            {
                cmd = new SqlCommand("delete tbl_penjualan where no_penjualan=@no_penjualan AND kode_pelanggan=@kode_pelanggan AND kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_penjualan", txtNoOrder.Text);
                cmd.Parameters.AddWithValue("@kode_pelanggan", txtKode.Text);
                cmd.Parameters.AddWithValue("@kode_barang", txtKodeBarang.Text);
                cmd.ExecuteNonQuery();
                con.Close();

                cmd = new SqlCommand("update tbl_barang set stock=@stock_akhir where kode_barang=@kode_barang", con);
                con.Open();
                String stock_akhir = (int.Parse(txtStock.Text) + int.Parse(txtJumlahBeli.Text)).ToString();
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
            if (txtNoOrder.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_penjualan_detail(no_penjualan, tgl_penjualan, kode_pelanggan, kode_barang, jumlah_beli, sub_total, keterangan) select no_penjualan, tgl_penjualan, kode_pelanggan, kode_barang, jumlah_beli, sub_total, keterangan from tbl_penjualan ", con);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                cmd = new SqlCommand("delete tbl_penjualan where no_penjualan=@no_penjualan", con);
                cmd.Parameters.AddWithValue("@no_penjualan", txtNoOrder.Text);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Transaksi tersimpan", "Berhasil");
                txtTotal.Text = total("sub_total", "tbl_penjualan").Rows[0][0].ToString();
                DisplayData();
                CleanAll();
                otomatis();
                total_terbilang();
            }
            else
            {
                MessageBox.Show("Transaksi gagal tersimpan","Gagal");
            }
        }

        private void btBaru_Click(object sender, EventArgs e)
        {
            otomatis();
            CleanAll();
        }

        private void txtJumlahBeli_TextChanged(object sender, EventArgs e)
        {
            if (txtJumlahBeli.Text != "") txtSubTotal.Text = (int.Parse(txtHarga.Text) * int.Parse(txtJumlahBeli.Text)).ToString();
            else txtSubTotal.Text = "0";
        }

        private void txtJumlahBeli_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void dGPel_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtKode.Text = dGPel.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtNama.Text = dGPel.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtTelepon.Text = dGPel.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtAlamat.Text = dGPel.Rows[e.RowIndex].Cells[3].Value.ToString();
            pnPelanggan.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CariDataPel();
        }

        
    }
}

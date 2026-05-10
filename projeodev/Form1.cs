using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace projeodev
{
    public partial class telefonrehberi : Form
    {
        public telefonrehberi()
        {
            InitializeComponent();
        }
        SqlConnection conn;
        SqlCommand cmd; 
        SqlDataAdapter adapter; 
        DataTable tablo;

        private bool alanlarkontrol()
        {
            if (txtAd.Text == "" || txtSoyad.Text == "" || txtTel.Text == "")
            {
                MessageBox.Show("giris aygıtları bos bırakılamaz");
                return false;

            }
            return true;
        }
        void kisilerisay()
        {
            lblgoster.Text = "telefon rehberinde" + dgvKisiler.Rows.Count + " kisi var!";

        }
  

        void KisiGetir()
        {
            conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=dbRehber;Integrated Security=True");
            adapter=new SqlDataAdapter("SELECT *FROM dbo.kisilerr",conn);   
            tablo = new DataTable();
            conn.Open();
            adapter.Fill(tablo);
            dgvKisiler.DataSource = tablo;
            conn.Close();
        }
        private void telefonrehberi_Load(object sender, EventArgs e)
        {
            KisiGetir();  
            kisilerisay();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAra_Click(object sender, EventArgs e)
        {

            try
            {
                DataView dv = tablo.DefaultView;
                dv.RowFilter = "Ad Like'" + txtAra.Text + "%'";
                dgvKisiler.DataSource = dv;

            }
            catch (Exception)
            {

                MessageBox.Show("kisi bulunamadı");
            }




        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (!alanlarkontrol()) return;
            


            string sorgu = "Insert into kisilerr(Ad,Soyad,Tel) values (@Ad,@Soyad,@Tel)";
            cmd = new SqlCommand(sorgu,conn);
            cmd.Parameters.AddWithValue("@Ad", txtAd.Text);
            cmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            cmd.Parameters.AddWithValue("@Tel", txtTel.Text);
            conn.Open();
            cmd.ExecuteNonQuery();  
            conn.Close();
            KisiGetir();
            kisilerisay();
            MessageBox.Show(txtAd.Text + "" + txtSoyad.Text + "Adlı kisi eklendi!");

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (!alanlarkontrol()) return;
            string sorgu = "Update kisilerr set ad=@Ad,soyad=@Soyad,tel=@Tel where id=@id";
            cmd= new SqlCommand(sorgu,conn);
            cmd.Parameters.AddWithValue("@Ad", txtAd.Text);
            cmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            cmd.Parameters.AddWithValue("@Tel", txtTel.Text);
            cmd.Parameters.AddWithValue("@id", dgvKisiler.CurrentRow.Cells["id"].Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            KisiGetir();
            kisilerisay();
            MessageBox.Show(txtAd.Text +""+ txtSoyad.Text + "Adlı kisi guncellendi!");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string ad=dgvKisiler.CurrentRow.Cells[1].Value.ToString();
            string soyad = dgvKisiler.CurrentRow.Cells[2].Value.ToString();

            string sorgu = "Delete kisilerr where id=@id";
            cmd = new SqlCommand(sorgu, conn);
            cmd.Parameters.AddWithValue("@id", dgvKisiler.CurrentRow.Cells["id"].Value);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            KisiGetir();
            kisilerisay();
            MessageBox.Show(ad + "" + soyad + "Adlı kisi silindi!");


        }
        private void dgvKisiler_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtAd.Text = dgvKisiler.CurrentRow.Cells[1].Value.ToString();
            txtSoyad.Text = dgvKisiler.CurrentRow.Cells[1].Value.ToString();
            txtTel.Text = dgvKisiler.CurrentRow.Cells[1].Value.ToString();

        }
    }
}

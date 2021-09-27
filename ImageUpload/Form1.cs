using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageUpload
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Archivos jpg (*.jpg| *.jpg|Archivos png(*png| *.png";
            openFileDialog1.RestoreDirectory = true;

            if(openFileDialog1.ShowDialog() == DialogResult.OK)

            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }
        private void Refresh()
        {
            using (Modelos.pruebaEntities db =new Modelos.pruebaEntities())
            {
                var lst = from d in db.imagenes
                          select new { d.id, d.nombre };
                dvgLista.DataSource = lst.ToList();

                textBox2.Text = "";
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals("")|| textBox2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Nombre y archivo son obligatorios");
                return;
            }

            byte[] file = null;
            Stream myStream = openFileDialog1.OpenFile();
            using (MemoryStream ms = new MemoryStream())
            {
                myStream.CopyTo(ms);
                file = ms.ToArray();
            }
            
            using (Modelos.pruebaEntities db =new Modelos.pruebaEntities())
            {
                Modelos.imagenes oImage = new Modelos.imagenes();
                oImage.nombre = textBox1.Text.Trim();
                oImage.img = file;

                db.imagenes.Add(oImage);
                db.SaveChanges();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ( dvgLista.Rows.Count > 0)
            {
                int id = int.Parse(dvgLista.Rows[dvgLista.CurrentRow.Index].Cells[0].Value.ToString());

                using (Modelos.pruebaEntities db =new Modelos.pruebaEntities())
                {
                    var oImage = db.imagenes.Find(id);

                    MemoryStream ms = new MemoryStream(oImage.img);
                    Bitmap bmp = new Bitmap(ms);
                    pb.Image = bmp;
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Refresh();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Matt
{
    public partial class Matt : Form
    {
        public Matt()
        {
            InitializeComponent();
        }

        private void openImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "All Images|*.bmp;*.png;*.jpg;*.jpeg;*.gif";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            pictureBox1.Image = Bitmap.FromFile(ofd.FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = pictureBox1.Image as Bitmap;
            if(bmp == null)
            {
                pictureBox2.Image = null;
                return;
            }

            pictureBox2.Image = bmp;
        }

        Smith.Colormap GetCurrentColormap()
        {
            Smith.Colormap cmp = gobColormap.SelectedItem as Smith.Colormap;
            if (cmp != null)
                return cmp;

            if (!File.Exists(cmpOrGobPath.Text))
                return null;

            cmp = new Smith.Colormap(Path.GetFileName(cmpOrGobPath.Text), File.OpenRead(cmpOrGobPath.Text));

            return cmp;
        }

        private void openMat_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Material File (*.mat)|*.mat";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            Smith.Material mat = new Smith.Material(Path.GetFileName(ofd.FileName), File.OpenRead(ofd.FileName));
            Smith.Colormap cmp = GetCurrentColormap();

            if (mat.ColorBits == 8 && cmp == null)
            {
                MessageBox.Show($"{Path.GetFileName(ofd.FileName)} is 8-bit but no colormap has been selected");
                return;
            }

            Bitmap bmp;
            mat.GenerateBitmap(out bmp, cmp);

            pictureBox1.Image = bmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string cmpFile = (sender as TextBox).Text;

            if (!File.Exists(cmpFile))
                return;

            if(Path.GetExtension(cmpFile).Equals(".gob", StringComparison.InvariantCultureIgnoreCase))
            {
                using (Smith.GOB gob = new Smith.GOB(Path.GetFileName(cmpFile), File.OpenRead(cmpFile)))
                    foreach (string s in gob.GetFilesWithExtension("cmp"))
                    {
                        Smith.Colormap cmp = new Smith.Colormap(Path.GetFileName(s), gob[s]);

                        gobColormap.Items.Add(cmp);
                    }

                gobColormap.Enabled = true;

            } else
            {
                gobColormap.Items.Clear();
                gobColormap.Enabled = false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Colormap Sources (*.cmp, *.gob)|*.cmp;*.gob";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            cmpOrGobPath.Text = ofd.FileName;
        }
    }
}

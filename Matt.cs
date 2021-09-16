using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

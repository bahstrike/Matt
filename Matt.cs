using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Matt
{
    public partial class Matt : Form
    {
        private string INIFile
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), "Matt.ini");
            }
        }

        public Matt()
        {
            InitializeComponent();
        }

        Smith.Colormap GetCurrentColormap()
        {
            Smith.Colormap cmp = gobColormap.SelectedItem as Smith.Colormap;
            if (cmp != null)
                return cmp;

            if (!File.Exists(cmpOrGobPath.Text))
                return null;

            if (!Path.GetExtension(cmpOrGobPath.Text).Equals(".cmp", StringComparison.InvariantCultureIgnoreCase))
                return null;

            cmp = new Smith.Colormap(Path.GetFileName(cmpOrGobPath.Text), File.OpenRead(cmpOrGobPath.Text));

            return cmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string cmpFile = (sender as TextBox).Text;

            if(File.Exists(cmpFile) && Path.GetExtension(cmpFile).Equals(".gob", StringComparison.InvariantCultureIgnoreCase))
            {
                using (Smith.GOB gob = new Smith.GOB(Path.GetFileName(cmpFile), File.OpenRead(cmpFile)))
                    foreach (string s in gob.GetFilesWithExtension("cmp"))
                    {
                        Smith.Colormap cmp = new Smith.Colormap(Path.GetFileName(s), gob[s]);

                        gobColormap.Items.Add(cmp);
                    }

                gobColormap.Enabled = true;

                if(gobColormap.Items.Count > 0)
                    gobColormap.SelectedIndex = 0;

            } else
            {
                // file may not exist,  or might be a valid *.cmp;  but we're gonna zilch out the GOB dropdown regardless

                gobColormap.Items.Clear();
                gobColormap.Enabled = false;

            }

            UpdateCMP();
            Reprocess();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "Colormap Sources (*.cmp, *.gob)|*.cmp;*.gob";
            ofd.RestoreDirectory = true;

            try
            {
                ofd.InitialDirectory = Path.GetDirectoryName(cmpOrGobPath.Text);
            }
            catch
            {

            }

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            cmpOrGobPath.Text = ofd.FileName;
        }

        private void Matt_Load(object sender, EventArgs e)
        {
            // load config
            using (Smith.INIFile ini = new Smith.INIFile(INIFile))
            {
                cmpOrGobPath.Text = ini.GetKey("General", "CMPorGOB", string.Empty);

                int cmpIndex;
                if(int.TryParse(ini.GetKey("General", "GOBCMPIndex", string.Empty), out cmpIndex))
                {
                    if (cmpIndex >= 0 && cmpIndex < gobColormap.Items.Count)
                        gobColormap.SelectedIndex = cmpIndex;
                }
            }   
        }

        private void Matt_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save config
            using (Smith.INIFile ini = new Smith.INIFile(INIFile))
            {
                ini.WriteKey("General", "CMPorGOB", cmpOrGobPath.Text);
                ini.WriteKey("General", "GOBCMPIndex", gobColormap.SelectedIndex.ToString());
            }
        }

        private void gobColormap_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCMP();
            Reprocess();
        }

        void UpdateCMP()
        {
            Smith.Colormap cmp = GetCurrentColormap();
            if(cmp == null)
            {
                pictureBox3.Image = null;
                return;
            }

            int width = pictureBox3.ClientRectangle.Width;
            int height = pictureBox3.ClientRectangle.Height;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(0, 0, 0), Color.FromArgb(60, 60, 60)), 0, 0, width, height);

                for(int i=0; i<256; i++)
                {
                    Color clr = cmp.Palette[i].Color;

                    const int clrsPerRow = 25;
                    float clrSize = (float)width / (float)clrsPerRow;
                    int iX = i % clrsPerRow;
                    int iY = i / clrsPerRow;

                    float fX = (float)iX * clrSize;
                    float fY = (float)iY * clrSize;

                    RectangleF clrRc = new RectangleF(fX, fY, clrSize, clrSize);
                    gfx.FillRectangle(new SolidBrush(clr), clrRc);
                }

            }

            pictureBox3.Image = bmp;
        }

        public string OpenedImageFilePath = string.Empty;
        public bool OpenedMAT
        {
            get
            {
                return OpenedImageFilePath.EndsWith(".mat", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public Smith.Material LoadOriginalAsMaterial()
        {
            if (!OpenedMAT)
                return null;

            return new Smith.Material(Path.GetFileName(OpenedImageFilePath), File.OpenRead(OpenedImageFilePath));
        }

        public Bitmap GenerateBitmap(Smith.Colormap cmpOverride=null)
        {
            try
            {
                if (!OpenedMAT)
                    return (Bitmap)Bitmap.FromFile(OpenedImageFilePath);

                Smith.Material mat = LoadOriginalAsMaterial();
                Smith.Colormap cmp = cmpOverride ?? GetCurrentColormap();

                Bitmap bmp;
                mat.GenerateBitmap(out bmp, cmp);

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public void ReloadOriginal(bool autoChangeOptions=false)
        {
            try
            {
                if (!OpenedMAT)
                {
                    

                    if(autoChangeOptions)
                    {

                        if (OpenedImageFilePath.EndsWith(".bmp", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // dunno if .NET Image.FromFile()  has been fixed to support 32bit ARGB yet, so we'll
                            // have to check and maybe add a custom loader to support bitmaps with alpha..  but for
                            // now we'll just "assume" it works

                            Bitmap bmp = (Bitmap)Image.FromFile(OpenedImageFilePath);// yea we're temp-loading the whole image just for properties..  who cares; computers are fast now
                            switch(bmp.PixelFormat)
                            {
                                case PixelFormat.Format8bppIndexed:
                                    bitdepth8.Checked = true;
                                    break;

                                case PixelFormat.Format24bppRgb:
                                    bitdepth565.Checked = true;
                                    break;

                                case PixelFormat.Format32bppArgb:
                                    bitdepth1555.Checked = true;
                                    break;
                            }

                        }
                        else if (OpenedImageFilePath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                            // i dunno maybe select 1555 for PNGs cause they might default to having transparency
                            bitdepth1555.Checked = true;
                        else if (OpenedImageFilePath.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                            // GIF is by nature only 256 color  so if someones using that, they prolly want to make an 8bit tex
                            bitdepth8.Checked = true;
                        else
                            // everything else (jpgs basically) should default to 565..  cause most ppl prolly just want to do 16bit color mats
                            bitdepth565.Checked = true;
                    }
                }
                else
                {
                    Smith.Material mat = LoadOriginalAsMaterial();

                    if(autoChangeOptions)
                    {
                        if (mat.IsSingleColor(0))
                            bitdepthSolid.Checked = true;
                        else if (mat.ColorBits == 8)
                            bitdepth8.Checked = true;
                        else
                        {
                            if (mat.GreenBits == 6)
                                bitdepth565.Checked = true;
                            else if (mat.GreenBits == 5)
                                bitdepth1555.Checked = true;
                            else if (mat.GreenBits == 4)
                                bitdepth4444.Checked = true;
                        }
                    }
                }


                pictureBox1.Image = GenerateBitmap(forceOriginalColormap);


                Reprocess(true);
            }
            catch
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
            }
        }

        public void Reprocess(bool fromReloadOriginal=false)
        {
            // if not from reloadoriginal,  maybe we just want to run that instead.
            // could be optimized if only changing output settings.. but who cares PCs are fast now
            if(!fromReloadOriginal)
            {
                ReloadOriginal();
                return;
            }

            pictureBox2.Image = GenerateBitmap(null/*no override*/);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        int? openLastFilterIndex = null;
        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "All Images|*.mat;*.bmp;*.png;*.jpg;*.jpeg;*.gif|Material Files (*.mat)|*.mat|Image Files (*.bmp;*.png;*.jpg;*.jpeg;*.gif)|*.bmp;*.png;*.jpg;*.jpeg;*.gif";
            ofd.FilterIndex = openLastFilterIndex ?? 0;
            try
            {
                ofd.FileName = OpenedImageFilePath;
            }
            catch
            {

            }

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            openLastFilterIndex = ofd.FilterIndex;
            OpenedImageFilePath = ofd.FileName;

            ReloadOriginal(true);
        }

        private void format_CheckedChanged(object sender, EventArgs e)
        {
            // if 16bit disable the colormap group
            

            Reprocess();
        }

        Smith.Colormap forceOriginalColormap = null;
        private void originalKeepColormap_Click(object sender, EventArgs e)
        {
            if(forceOriginalColormap == null)
            {
                forceOriginalColormap = GetCurrentColormap();
                if (forceOriginalColormap == null)
                    return;

                originalKeepColormap.Text = "Clear Saved Colormap";
            } else
            {
                originalKeepColormap.Text = "Keep Current Colormap";

                // if clearing saved colormap then reprocess immediately using whatever is selected
                forceOriginalColormap = null;
                ReloadOriginal(false);
            }
        }
    }
}

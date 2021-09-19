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
using Smith;

namespace Matt
{
    public partial class Matt : Form
    {
        public enum Format
        {
            Solid,
            Paletted,
            RGB565,
            ARGB1555,
            ARGB4444,
        }

        public int CurrentColorIndex = 0;

        public Format CurrentFormat
        {
            get
            {
                if (bitdepthSolid.Checked)
                    return Format.Solid;

                if (bitdepth8.Checked)
                    return Format.Paletted;

                if (bitdepth565.Checked)
                    return Format.RGB565;

                if (bitdepth1555.Checked)
                    return Format.ARGB1555;

#if SUPPORTINDY
                if (bitdepth4444.Checked)
                    return Format.ARGB4444;
#endif

                // donno
                return Format.RGB565;
            }

            set
            {
                switch(value)
                {
                    case Format.Solid:
                        bitdepthSolid.Checked = true;
                        break;

                    case Format.Paletted:
                        bitdepth8.Checked = true;
                        break;

                    case Format.RGB565:
                        bitdepth565.Checked = true;
                        break;

                    case Format.ARGB1555:
                        bitdepth1555.Checked = true;
                        break;

#if SUPPORTINDY
                    case Format.ARGB4444:
                        bitdepth4444.Checked = true;
                        break;
#endif
                }
            }
        }

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

#if !SUPPORTINDY
            bitdepth4444.Visible = false;
#endif
        }

        Colormap GetCurrentColormap()
        {
            Colormap cmp = gobColormap.SelectedItem as Colormap;
            if (cmp != null)
                return cmp;

            if (!File.Exists(cmpOrGobPath.Text))
                return null;

            if (!Path.GetExtension(cmpOrGobPath.Text).Equals(".cmp", StringComparison.InvariantCultureIgnoreCase))
                return null;

            cmp = new Colormap(Path.GetFileName(cmpOrGobPath.Text), File.OpenRead(cmpOrGobPath.Text));

            return cmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string cmpFile = (sender as TextBox).Text;

            if(File.Exists(cmpFile) && Path.GetExtension(cmpFile).Equals(".gob", StringComparison.InvariantCultureIgnoreCase))
            {
                using (GOB gob = new GOB(Path.GetFileName(cmpFile), File.OpenRead(cmpFile)))
                    foreach (string s in gob.GetFilesWithExtension("cmp"))
                    {
                        Colormap cmp = new Colormap(Path.GetFileName(s), gob[s]);

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
            using (INIFile ini = new INIFile(INIFile))
            {
                cmpOrGobPath.Text = ini.GetKey("General", "CMPorGOB", string.Empty);

                int cmpIndex;
                if(int.TryParse(ini.GetKey("General", "GOBCMPIndex", string.Empty), out cmpIndex))
                {
                    if (cmpIndex >= 0 && cmpIndex < gobColormap.Items.Count)
                        gobColormap.SelectedIndex = cmpIndex;
                }
            }


            // trigger some dumb UI stuff
            format_CheckedChanged(null, new EventArgs());
        }

        private void Matt_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save config
            using (INIFile ini = new INIFile(INIFile))
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
            Colormap cmp = GetCurrentColormap();
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
                FillRectEmpty(gfx, new Rectangle(0, 0, width, height));

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

                    if (i == CurrentColorIndex)
                        gfx.DrawRectangle(new Pen(Color.FromArgb(255 - clr.R, 255 - clr.G, 255 - clr.B), 1.0f), clrRc.X, clrRc.Y, clrRc.Width, clrRc.Height);
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

        public Material LoadOriginalAsMaterial()
        {
            if (!OpenedMAT)
                return null;

            using(Stream s = File.OpenRead(OpenedImageFilePath))
                return new Material(Path.GetFileName(OpenedImageFilePath), s);
        }

        public Bitmap GenerateBitmap(out bool failedToNoColormap, bool fillBGForTransparent, Colormap cmpOverride=null, Material matOverride=null, int index=0)
        {
            failedToNoColormap = false;

            try
            {
                if (!OpenedMAT && matOverride == null)
                    return (Bitmap)Bitmap.FromFile(OpenedImageFilePath);

                Material mat = matOverride ?? LoadOriginalAsMaterial();
                Colormap cmp = cmpOverride ?? GetCurrentColormap();

                Bitmap bmp;
                mat.GenerateBitmap(out bmp, cmp, out failedToNoColormap, fillBGForTransparent, index);

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
                if (autoChangeOptions)
                {
                    if(!OpenedMAT)
                    {
                        if (OpenedImageFilePath.EndsWith(".bmp", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // dunno if .NET Image.FromFile()  has been fixed to support 32bit ARGB yet, so we'll
                            // have to check and maybe add a custom loader to support bitmaps with alpha..  but for
                            // now we'll just "assume" it works

                            Bitmap bmp = (Bitmap)Image.FromFile(OpenedImageFilePath);// yea we're temp-loading the whole image just for properties..  who cares; computers are fast now
                            switch (bmp.PixelFormat)
                            {
                                case PixelFormat.Format8bppIndexed:
                                    CurrentFormat = Format.Paletted;
                                    break;

                                case PixelFormat.Format24bppRgb:
                                    CurrentFormat = Format.RGB565;
                                    break;

                                case PixelFormat.Format32bppArgb:
                                    CurrentFormat = Format.ARGB1555;
                                    break;
                            }

                        }
                        else if (OpenedImageFilePath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                            // i dunno maybe select 1555 for PNGs cause they might default to having transparency
                            CurrentFormat = Format.ARGB1555;
                        else if (OpenedImageFilePath.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                            // GIF is by nature only 256 color  so if someones using that, they prolly want to make an 8bit tex
                            CurrentFormat = Format.Paletted;
                        else
                            // everything else (jpgs basically) should default to 565..  cause most ppl prolly just want to do 16bit color mats
                            CurrentFormat = Format.RGB565;
                    } else
                    {
                        Material mat = LoadOriginalAsMaterial();

                        if (autoChangeOptions)
                        {
                            if (mat.Materials.Count > 0)
                                CurrentColorIndex = mat.Materials[0].colorIndex;

                            if (mat.IsSingleColor(0))
                                CurrentFormat = Format.Solid;
                            else if (mat.ColorBits == 8)
                                CurrentFormat = Format.Paletted;
                            else
                            {
                                if (mat.GreenBits == 6)
                                    CurrentFormat = Format.RGB565;
                                else if (mat.GreenBits == 5)
                                    CurrentFormat = Format.ARGB1555;
                                else if (mat.GreenBits == 4)
                                    CurrentFormat = Format.ARGB4444;
                            }
                        }
                    }
                }


                bool needColormap;
                pictureBox1.Image = GenerateBitmap(out needColormap, true, forceOriginalColormap);
                originalNeedColormap.Visible = needColormap;


                // we are baking the "colorindex" rectangle into the colormap bitmap for now..  so regenerate it
                UpdateCMP();

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


            Material mat = GenerateOutputMat();

            bool needColormap;
            pictureBox2.Image = GenerateBitmap(out needColormap, true, null/*no override*/, mat);
            previewNeedColormap.Visible = needColormap;
        }

        unsafe Material GenerateOutputMat()
        {
            bool needColormap;
            Bitmap bmp = GenerateBitmap(out needColormap, false, forceOriginalColormap);
            if (bmp == null)
                return null;

            Format fmt = CurrentFormat;
            Colormap cmp = GetCurrentColormap();
            int clrIndex = CurrentColorIndex;

            bool supportsTransparency = (fmt == Format.Paletted || fmt == Format.ARGB1555 || fmt == Format.ARGB4444);

            // if 8bit we need a colormap
            if (cmp == null && (fmt == Format.Solid || fmt == Format.Paletted))
                return null;


            Material mat = new Material();

            switch(fmt)
            {
                case Format.Solid:
                    // not supported yet
                    break;

                case Format.Paletted:
                    mat.ColorBits = 8;
                    break;

                case Format.RGB565:
                    mat.ColorBits = 16;
                    mat.RedBits = 5;
                    mat.GreenBits = 6;
                    mat.BlueBits = 5;
                    break;

                case Format.ARGB1555:
                    mat.ColorBits = 16;
                    mat.RedBits = 5;
                    mat.GreenBits = 5;
                    mat.BlueBits = 5;
                    break;

                case Format.ARGB4444:
                    mat.ColorBits = 16;
                    mat.RedBits = 4;
                    mat.GreenBits = 4;
                    mat.BlueBits = 4;
                    break;
            }


            Material.MaterialHeader mh = new Material.MaterialHeader();
            mat.Materials.Add(mh);

            mh.colorIndex = clrIndex;
            mh.textureId = 0;




            Material.TextureHeader th = new Material.TextureHeader();
            mat.Textures.Add(th);

            // precache dimensions since the property accessor is horrendously slow
            int bmpWidth = bmp.Width;
            int bmpHeight = bmp.Height;


            // one mipmap for now
            th.Width = bmpWidth;
            th.Height = bmpHeight;
            th.Transparent = supportsTransparency;// donno

            byte[] mipData = new byte[th.Width * th.Height * (mat.ColorBits / 8)];
            fixed(byte* _dst = mipData)
            {
                byte* dst = _dst;

                BitmapData bmdat = bmp.LockBits(new Rectangle(0, 0, bmpWidth, bmpHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // would be faster to have loops for each format rather than embedding a switch() in deepest loop..  but lazy
                for(int y=0; y<bmpHeight; y++)
                {
                    byte* src = (byte*)((long)bmdat.Scan0 + (long)(y * bmdat.Stride));

                    for (int x=0; x<bmpWidth; x++)
                    {
                        switch(fmt)
                        {
                            case Format.Paletted:
                                {
                                    if (src[3] < 255)   // if transparent (whats a good threshold?)  write a 0
                                        *(dst++) = 0;
                                    else
                                        *(dst++) = (byte)cmp.FindClosestColor(src[2], src[1], src[0], /*mh.colorIndex*/ 0/*is zero always transparent? if so, never select it for a valid color*/);
                                }
                                break;

                            case Format.RGB565:
                                {
                                    ushort pixelword = 0;

                                    pixelword |= (ushort)(src[0] * 0x1F / 255);//blue
                                    pixelword |= (ushort)((src[1] * 0x3F / 255) << 5);//green
                                    pixelword |= (ushort)((src[2] * 0x1F / 255) << 11);//red

                                    *((ushort*)dst) = pixelword;
                                    dst += 2;
                                }
                                break;

                            case Format.ARGB1555:
                                {
                                    ushort pixelword = 0;

                                    pixelword |= (ushort)(src[0] * 0x1F / 255);//blue
                                    pixelword |= (ushort)((src[1] * 0x1F / 255) << 5);//green
                                    pixelword |= (ushort)((src[2] * 0x1F / 255) << 10);//red

                                    // first bit is trans(0)/opaque(1)..  shift everything left and then encode it
                                    pixelword <<= 1;
                                    if (src[3] > 128)// lol some arbitrary alpha threshold
                                        pixelword |= 1;// opaque

                                    *((ushort*)dst) = pixelword;
                                    dst += 2;
								}
                                break;
                        }

                        src += 4;
                    }
                }


                bmp.UnlockBits(bmdat);
            }
            th.MipmapData.Add(mipData);



            return mat;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


        const string filterAllImages = "All Images|*.mat;*.bmp;*.png;*.jpg;*.jpeg;*.gif";
        const string filterMatFiles = "Material Files (*.mat)|*.mat";
        const string filterImageFiles = "Image Files (*.bmp;*.png;*.jpg;*.jpeg;*.gif)|*.bmp;*.png;*.jpg;*.jpeg;*.gif";


        int? openLastFilterIndex = null;
        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = $"{filterAllImages}|{filterMatFiles}|{filterImageFiles}";
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

            OpenOriginal(ofd.FileName);
        }

        void OpenOriginal(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return;

            if (!File.Exists(filename))
                return;

            bool match = false;
            foreach(string ext in new string[] { ".mat", ".bmp", ".png", ".jpg", ".jpeg", ".gif" })
                if(filename.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase))
                {
                    match = true;
                    break;
                }

            if (!match)
                return;

            OpenedImageFilePath = filename;

            ReloadOriginal(true);
        }

        private void format_CheckedChanged(object sender, EventArgs e)
        {
            bool is8bit = (CurrentFormat == Format.Solid) || (CurrentFormat == Format.Paletted);

            colormapGroup.Enabled = is8bit;
            originalKeepColormap.Enabled = is8bit;

            Reprocess();
        }

        Colormap forceOriginalColormap = null;
        private void originalKeepColormap_Click(object sender, EventArgs e)
        {
            if(forceOriginalColormap == null)
            {
                forceOriginalColormap = GetCurrentColormap();
                if (forceOriginalColormap == null)
                    return;

                originalKeepColormap.Text = $"Clear: {forceOriginalColormap.Name}";
            } else
            {
                originalKeepColormap.Text = "Keep Current Colormap";

                // if clearing saved colormap then reprocess immediately using whatever is selected
                forceOriginalColormap = null;
                ReloadOriginal(false);
            }
        }

        private void Matt_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            e.Effect = DragDropEffects.Link;
        }

        private void Matt_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files.Length < 1)
                return;

            OpenOriginal(files[0]);
        }

        public static void FillRectEmpty(Graphics gfx, Rectangle rc)
        {
            gfx.FillRectangle(new HatchBrush(HatchStyle.LightDownwardDiagonal, Color.FromArgb(0, 0, 0), Color.FromArgb(60, 60, 60)), rc);
        }

        private void colormap_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb == null)
                return;

            // if we have an image to draw, do nothing
            if (pb.Image != null)
                return;

            // no image.. fill in some "nothing here" background
            FillRectEmpty(e.Graphics, pb.ClientRectangle);
        }

        int? saveLastFilterIndex = null;
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = $"{filterMatFiles}|{filterImageFiles}";
            sfd.FilterIndex = saveLastFilterIndex ?? 0;
            sfd.InitialDirectory = Path.GetDirectoryName(OpenedImageFilePath);

            /*try
            {
                sfd.FileName = OpenedImageFilePath;
            }
            catch
            {

            }*/

            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            saveLastFilterIndex = sfd.FilterIndex;


            if (sfd.FileName.EndsWith(".mat", StringComparison.InvariantCultureIgnoreCase))
            {
                Material mat = GenerateOutputMat();
                mat.Save(sfd.FileName);
            } else
            {
                // whatever random image file from bitmap..   just save "original" bitmap
                bool needColormap;
                Bitmap bmp = GenerateBitmap(out needColormap, false, forceOriginalColormap);
                if (bmp == null)
                    return;

                bmp.Save(sfd.FileName, ImageFormatFromExtension(Path.GetExtension(sfd.FileName)));
            }
        }

        ImageFormat ImageFormatFromExtension(string ext)
        {
            if (ext[0] != '.')
                ext = '.' + ext;

            switch(ext.ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;

                case ".png":
                    return ImageFormat.Png;

                case ".gif":
                    return ImageFormat.Gif;

                case ".bmp":
                    return ImageFormat.Bmp;

                default:
                    MessageBox.Show($"No idea what extension {ext} is supposed to be");
                    return ImageFormat.Bmp;
            }
        }
    }
}

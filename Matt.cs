using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Smith;

namespace Matt
{ 
    public partial class Matt : Form
    {
        public static Matt inst;

        public enum Format
        {
            Solid,
            Paletted,
            BGR565,
            ABGR1555,
            ABGR4444,
            BGR24,
            ABGR32
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
                if (bgr565.Checked)
                    return Format.BGR565;
                if (abgr1555.Checked)
                    return Format.ABGR1555;
                if (abgr4444.Checked)
                    return Format.ABGR4444;
                if (bgr24.Checked)
                    return Format.BGR24;
                return Format.ABGR32;
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

                    case Format.BGR565:
                        bgr565.Checked = true;
                        break;

                    case Format.ABGR1555:
                        abgr1555.Checked = true;
                        break;

                    case Format.ABGR4444:
                        abgr4444.Checked = true;
                        break;

                    case Format.BGR24:
                        bgr24.Checked = true;
                        break;

                    case Format.ABGR32:
                        abgr32.Checked = true;
                        break;
                }
            }
        }

        public Matt()
        {
            InitializeComponent();

            inst = this;



            // lets begin
            Log.Print("-------------------------------------");
            Log.Print("             Matt Startup");
            Log.Print("-------------------------------------");

#if !DEBUG
            logList.Visible = false;
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

            using (Stream filehandle = File.OpenRead(cmpOrGobPath.Text))
                cmp = new Colormap(Path.GetFileName(cmpOrGobPath.Text), filehandle);

            return cmp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string cmpFile = (sender as TextBox).Text;

            if(File.Exists(cmpFile) && Path.GetExtension(cmpFile).Equals(".gob", StringComparison.InvariantCultureIgnoreCase))
            {
                gobColormap.Items.Clear();

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

            Reprocess();


            // whatever repaint everything NOW if colormap data has changed
            Update();
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

        bool initialFormLoad = false;
        private void Matt_Load(object sender, EventArgs e)
        {
            initialFormLoad = true;

            // load config
            cmpOrGobPath.Text = Properties.Settings.Default.CMPorGOB;

            int cmpIndex = Properties.Settings.Default.GOBCMPIndex;
            if (cmpIndex >= 0 && cmpIndex < gobColormap.Items.Count)
                gobColormap.SelectedIndex = cmpIndex;



            // trigger some dumb UI stuff
            format_CheckedChanged(null, new EventArgs());

            initialFormLoad = false;


            Reprocess();
        }

        private void Matt_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save config
            Properties.Settings.Default.CMPorGOB = cmpOrGobPath.Text;
            Properties.Settings.Default.GOBCMPIndex = gobColormap.SelectedIndex;
        }

        private void gobColormap_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reprocess();

            // enforce redrawing everything now
            Update();
        }

        void UpdateCMP(List<int> usedPaletteIndices)
        {
            Log.Print("UpdateCMP");

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
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(16, 16, 16)), new Rectangle(0, 0, width, height));
                //FillRectEmpty(gfx, new Rectangle(0, 0, width, height));

                for(int i=0; i<256; i++)
                {
                    Color clr = cmp.Palette[i].Color;

                    const int clrsPerRow = 21;
                    float clrSize = (float)width / (float)clrsPerRow;
                    int iX = i % clrsPerRow;
                    int iY = i / clrsPerRow;

                    float fX = (float)iX * clrSize;
                    float fY = (float)iY * clrSize;

                    RectangleF clrRc = new RectangleF(fX, fY, clrSize, clrSize);
                    clrRc.Inflate(-2.0f, -2.0f);
                    gfx.FillRectangle(new SolidBrush(clr), clrRc);


                    // figure out a good visible inverse color.  if too similar, blow out color channels to force visible.
                    int iR = 255 - clr.R;
                    int iG = 255 - clr.G;
                    int iB = 255 - clr.B;

                    if (Math.Abs(iR - clr.R) < 30)
                        iR = clr.R < 128 ? 255 : 0;
                    if (Math.Abs(iG - clr.G) < 30)
                        iG = clr.G < 128 ? 255 : 0;
                    if (Math.Abs(iB - clr.B) < 30)
                        iB = clr.B < 128 ? 255 : 0;

                    Color inverseColor = Color.FromArgb(iR, iG, iB);



                    if (usedPaletteIndices.Contains(i))
                        gfx.DrawRectangle(new Pen(inverseColor, 1.0f), clrRc.X, clrRc.Y, clrRc.Width, clrRc.Height);

                    if (i == CurrentColorIndex)
                        gfx.DrawLine(new Pen(inverseColor, 1.0f), clrRc.X, clrRc.Y+clrRc.Height, clrRc.X+clrRc.Width, clrRc.Y);
                    //    gfx.DrawRectangle(new Pen(Color.FromArgb(255 - clr.R, 255 - clr.G, 255 - clr.B), 1.0f), clrRc.X, clrRc.Y, clrRc.Width, clrRc.Height);
                }

            }

            pictureBox3.Image = bmp;
        }

        public string OpenedImageFilePath = string.Empty;
        public bool OpenedBMP_ARGB = false;//flag to force using custom BMP load routine;  .NET doesnt support loading alpha channel, even though Bitmap does support it.. :P
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
                {
                    Bitmap imgBmp;

                    if (OpenedBMP_ARGB)
                        imgBmp = LoadBMP_ARGB(OpenedImageFilePath);

                    imgBmp = (Bitmap)Bitmap.FromFile(OpenedImageFilePath);

                    // if we want to fill BG then do an extra compositing step;  otherwise just directly return our loaded bmp
                    if (!fillBGForTransparent)
                        return imgBmp;

                    Bitmap cbmp = new Bitmap(imgBmp.Width, imgBmp.Height, PixelFormat.Format32bppArgb);
                    cbmp.SetResolution(imgBmp.HorizontalResolution, imgBmp.VerticalResolution);// force same DPI so Graphics.DrawImage doesnt try to be friggin "smart"
                    using (Graphics gfx = Graphics.FromImage(cbmp))
                    {
                        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 255)), new Rectangle(0, 0, cbmp.Width, cbmp.Height));
                        gfx.DrawImage(imgBmp, new Point(0, 0));
                    }
                    return cbmp;
                }

                Material mat = matOverride ?? LoadOriginalAsMaterial();
                Colormap cmp = cmpOverride ?? GetCurrentColormap();

                mat.GenerateBitmap(out Bitmap bmp, cmp, out failedToNoColormap, fillBGForTransparent, index);

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2, CharSet = CharSet.Ansi)]
        private struct BITMAPHEADER
        {
            public UInt16 bfType;
            public UInt32 bfSize;
            public UInt16 bfReserved1;
            public UInt16 bfReserved2;
            public UInt32 bfOffBits;
            public UInt32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public UInt16 biPlanes;
            public UInt16 biBitCount;
            public UInt32 biCompression;
            public UInt32 biSizeImage;
            public Int32 biXPelsPerMeter;
            public Int32 biYPelsPerMeter;
            public UInt32 biClrUsed;
            public UInt32 biClrImportant;
        }

        unsafe static Bitmap LoadBMP_ARGB(string filename)
        {
            byte[] bmpFileBytes = File.ReadAllBytes(filename);

            BITMAPHEADER* pBH = stackalloc BITMAPHEADER[1];
            Marshal.Copy(bmpFileBytes, 0, (IntPtr)(pBH), Marshal.SizeOf(typeof(BITMAPHEADER)));

            if (pBH->biBitCount != 32)
                return null;

            fixed (byte* pbmpFileBytes = bmpFileBytes)
            {
                int w = pBH->biWidth;
                int h;
                bool inv;

                if (pBH->biHeight < 0)
                {
                    h = -pBH->biHeight;
                    inv = false;
                }
                else
                {
                    h = pBH->biHeight;
                    inv = true;
                }

                Bitmap bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
                BitmapData bmdat = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                byte* pSrc = (byte*)((ulong)pbmpFileBytes + (ulong)pBH->bfOffBits);
                int stride = (w * 4 + 7) & ~7;

                for (int y = 0; y < h; y++)
                {
                    byte* pSrcRow = (byte*)((ulong)pSrc + (ulong)(stride * (inv ? (h - y - 1) : y)));
                    byte* pDstRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(y * bmdat.Stride));

                    for (int x = 0; x < w; x++)
                    {
                        pDstRow[0] = pSrcRow[0];
                        pDstRow[1] = pSrcRow[1];
                        pDstRow[2] = pSrcRow[2];
                        pDstRow[3] = pSrcRow[3];

                        pDstRow += 4;
                        pSrcRow += 4;
                    }
                }

                bmp.UnlockBits(bmdat);

                return bmp;
            }
        }

        public void ReloadOriginal(bool autoChangeOptions=false)
        {
            // force-disable autoChangeOptions   if checkbox is off
            autoChangeOptions &= autoselectFormat.Checked;

            Log.Print($"ReloadOriginal({autoChangeOptions})");

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

                                case PixelFormat.Format16bppRgb555:
                                case PixelFormat.Format16bppRgb565:
                                    CurrentFormat = Format.BGR565;
                                    break;

                                case PixelFormat.Format24bppRgb:
                                    CurrentFormat = Format.BGR24;
                                    break;

                                /*case PixelFormat.Format32bppArgb:
                                    CurrentFormat = Format.ARGB1555;
                                    break;*/

                                // .NET is dumb and loads 32bit ARGB  *.bmp  as  32bit RGB  and doesnt keep the alpha channel.. set flag to use our own routine
                                case PixelFormat.Format32bppRgb:
                                    CurrentFormat = Format.ABGR32;
                                    OpenedBMP_ARGB = true;
                                    break;
                            }

                        }
                        else if (OpenedImageFilePath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase))
                        {
                            Bitmap bmp = (Bitmap)Image.FromFile(OpenedImageFilePath);// yea we're temp-loading the whole image just for properties..  who cares; computers are fast now

                            if ((bmp.Flags & 0x02) != 0)// check transparency flag?
                                CurrentFormat = Format.ABGR4444;
                            else
                                CurrentFormat = Format.BGR565;
                        }
                        else if (OpenedImageFilePath.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase))
                            // GIF is by nature only 256 color  so if someones using that, they prolly want to make an 8bit tex
                            CurrentFormat = Format.Paletted;
                        else
                            // everything else (jpgs basically) should default to 565...  cause most ppl prolly just want to do 16bit color mats
                            CurrentFormat = Format.BGR565;
                    } 
                    else
                    {
                        Material mat = LoadOriginalAsMaterial();

                        if (autoChangeOptions)
                        {
                            if (mat.Records.Count > 0)
                                CurrentColorIndex = mat.Records[0].colorIndex;

                            if (mat.IsSingleColor(0))
                                CurrentFormat = Format.Solid;
                            else if (mat.format.Mode == MatColorMode.Indexed)
                                CurrentFormat = Format.Paletted;
                            else if (mat.format == ColorFormat.BGR565)
                                CurrentFormat = Format.BGR565;
                            else if (mat.format == ColorFormat.ABGR1555)
                                CurrentFormat = Format.ABGR1555;
                            else if (mat.format == ColorFormat.ABGR4444)
                                CurrentFormat = Format.ABGR4444;
                            else if (mat.format == ColorFormat.BGR24)
                                CurrentFormat = Format.BGR24;
                            else if (mat.format == ColorFormat.ABGR32)
                                CurrentFormat = Format.ABGR32;
                        }
                    }
                }

                bool needColormap;
                pictureBox1.Image = GenerateBitmap(out needColormap, fillTransparent.Checked, forceOriginalColormap);
                originalNeedColormap.Visible = needColormap;

                Reprocess(true);
            }
            catch
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
            }
        }

        int reprocNum = 0;

        public void Reprocess(bool fromReloadOriginal=false)
        {
            if (initialFormLoad)
                return;

            // if not from reloadoriginal,  maybe we just want to run that instead.
            // could be optimized if only changing output settings.. but who cares PCs are fast now
            if (!fromReloadOriginal)
            {
                ReloadOriginal();
                return;
            }

            Log.Print($"Reprocess #{++reprocNum}");

            List<int> usedPaletteIndices;
            Material mat = GenerateOutputMat(out usedPaletteIndices);

            bool needColormap;
            pictureBox2.Image = GenerateBitmap(out needColormap, fillTransparent.Checked, null/*no override*/, mat);
            previewNeedColormap.Visible = needColormap;

            UpdateCMP(usedPaletteIndices);
        }

        unsafe Material GenerateOutputMat(out List<int> usedPaletteIndices)
        {
            usedPaletteIndices = new List<int>();

            bool needColormap;
            Bitmap bmp = GenerateBitmap(out needColormap, false, forceOriginalColormap);
            if (bmp == null)
                return null;

            // powers-of-2 automatic rescale
            int newWidth = (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bmp.Width) / Math.Log(2.0)));
            int newHeight = (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)bmp.Height) / Math.Log(2.0)));

            if (newWidth != bmp.Width || newHeight != bmp.Height)
            {
                Bitmap newBmp = new Bitmap(newWidth, newHeight, PixelFormat.Format32bppArgb);
                newBmp.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

                using (Graphics gfx = Graphics.FromImage(newBmp))
                {
                    gfx.CompositingMode = CompositingMode.SourceCopy;
                    gfx.InterpolationMode = InterpolationMode.NearestNeighbor;
                    gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (ImageAttributes wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        gfx.DrawImage(bmp, new Rectangle(0, 0, newWidth, newHeight), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                bmp.Dispose();
                bmp = newBmp;
            }

            Format fmt = CurrentFormat;
            Colormap cmp = GetCurrentColormap();
            int clrIndex = CurrentColorIndex;

            bool supportsTransparency = (fmt == Format.Paletted || fmt == Format.ABGR1555 || fmt == Format.ABGR4444 || fmt == Format.ABGR32);

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
                    mat.format = ColorFormat.Indexed;
                    break;

                case Format.BGR565:
                    mat.format = ColorFormat.BGR565;
                    break;

                case Format.ABGR1555:
                    mat.format = ColorFormat.ABGR1555;
                    break;

                case Format.ABGR4444:
                    mat.format = ColorFormat.ABGR4444;
                    break;

                case Format.BGR24:
                    mat.format = ColorFormat.BGR24;
                    break;

                case Format.ABGR32:
                    mat.format = ColorFormat.ABGR32;
                    break;
            }

            Material.Record mh = new Material.Record();
            mat.Records.Add(mh);

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
            th.TransparentColorNum = 0;

            int epixSize = (int)mat.format.Bpp / 8;
            byte[] mipData = new byte[th.Width * th.Height * epixSize];
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
                            case Format.Solid: break;
                            case Format.Paletted:
                                {
                                    byte val;
                                    if (src[3] == 0)   // if transparent (whats a good threshold?)  write a 0
                                        val = (byte)th.TransparentColorNum;
                                    else
                                        val = (byte)cmp.FindClosestColor(src[2], src[1], src[0], excludeSelfIlluminated.Checked, /*mh.colorIndex*/ 0/*is zero always transparent? if so, never select it for a valid color*/);

                                    *(dst++) = val;

                                    if (!usedPaletteIndices.Contains(val))
                                        usedPaletteIndices.Add(val);
                                }
                                break;
                            default: // RGB(A)
                                {
                                    var pix = Color.FromArgb(src[3], src[2], src[1], src[0]);
                                    var epix = Material.EncodePixel(pix, mat.format);
                                    if (mat.format.Bpp == 16)
                                        *((ushort*)dst) = (ushort)epix;
                                    if (mat.format.Bpp == 24)
                                    {
                                        dst[0] = (byte)(epix & 0xFF);
                                        dst[1] = (byte)((epix >> 8) & 0xFF);
                                        dst[2] = (byte)((epix >> 16) & 0xFF);
                                    }
                                    else
                                        *((uint*)dst) = (uint)epix;

                                    dst += epixSize;
                                } break;
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
            ofd.Multiselect = true;
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

            OpenOriginal(ofd.FileNames);
        }

        void OpenOriginal(string[] filenames)
        {
            if (filenames == null || filenames.Length < 1)
                return;


            // default to batch convert?
            if(filenames.Length > 1)
            {
                if(MessageBox.Show("Multiple files selected..\nProceed with batch or cancel?", "Initiate Batch Convert?", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;

                BatchConvertInputs(filenames);

                return;
            }


            // single file
            OpenOriginal(filenames[0]);
        }

        void ClearOriginal()
        {
            OpenedImageFilePath = string.Empty;
            OpenedBMP_ARGB = false;

            // flush changes
            ReloadOriginal();
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
            OpenedBMP_ARGB = false;//reset flag.. we handle this within ReloadOriginal

            ReloadOriginal(true);
        }

        private void format_CheckedChanged(object sender, EventArgs e)
        {
            // only trigger if enabling check so we dont reprocess twice
            if (sender != null && !(sender as RadioButton).Checked)
                return;

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
            OpenOriginal(files);
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


            SaveCurrent(sfd.FileName);
        }

        void SaveCurrent(string filename)
        {
            if (filename.EndsWith(".mat", StringComparison.InvariantCultureIgnoreCase))
            {
                List<int> dummy;
                Material mat = GenerateOutputMat(out dummy);
                mat.Save(filename);
            }
            else
            {
                // whatever random image file from bitmap..   just save "original" bitmap
                bool needColormap;
                Bitmap bmp = GenerateBitmap(out needColormap, false, forceOriginalColormap);
                if (bmp == null)
                    return;

                bmp.Save(filename, ImageFormatFromExtension(Path.GetExtension(filename)));
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

        private void fillTransparent_CheckedChanged(object sender, EventArgs e)
        {
            Reprocess();
        }

        private void Matt_FormClosed(object sender, FormClosedEventArgs e)
        {


            Log.Print("  ****  GRACEFUL FORM CLOSE  ****");
            inst = null;
        }

        void BatchConvertInputs(string[] filenames)
        {
            if (filenames == null || filenames.Length <= 0)
                return;

            // need a dump folder
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.SelectedPath = Path.GetDirectoryName(filenames[0]);

            if (fbd.ShowDialog() != DialogResult.OK)
                return;


            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = fbd.SelectedPath;
            sfd.RestoreDirectory = true;
            sfd.CheckFileExists = false;
            sfd.Filter = filterAllImages;
            sfd.Title = "MAKE ANY file.XYZ WHERE .XYZ IS WUT U WANT";
            sfd.FileName = "RENAME EXTENSION (filename doesnt matter).png";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;

            string desiredExt = Path.GetExtension(sfd.FileName);

            foreach(string inputFile in filenames)
            {
                // whatever lets just do thru UI
                OpenOriginal(inputFile);


                string outputFile = Path.Combine(fbd.SelectedPath, Path.GetFileNameWithoutExtension(inputFile) + desiredExt);
                SaveCurrent(outputFile);
            }

            ExplorerOpenFolder(fbd.SelectedPath);
        }


        void ExplorerOpenFolder(string path)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void excludeSelfIlluminated_CheckedChanged(object sender, EventArgs e)
        {
            Reprocess();

            // enforce redrawing everything now
            Update();
        }
    }

    public static class Log
    {
        public const string Filename = "Matt.log";

        public static void Print(string s)
        {
#if !DEBUG
            return;
#endif

            File.AppendAllText(Filename, s + "\n");

            if (Matt.inst != null)
            {
                ListBox loglb = Matt.inst.logList;

                while (loglb.Items.Count > 100)
                    loglb.Items.RemoveAt(0);

                int i = loglb.Items.Add(s);
                loglb.TopIndex = i;

                loglb.Update();
            }
        }
    }
}

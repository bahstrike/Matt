using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Smith
{
    public enum MatColorMode : UInt32
    {
        Indexed = 0,
        RGB     = 1,
        RGBA    = 2
    }

    public class ColorFormat
    {
        public MatColorMode Mode;
        public uint Bpp; // bits per pixel

        public uint RedBpp;
        public uint GreenBpp;
        public uint BlueBpp;

        public uint RedShl;
        public uint GreenShl;
        public uint BlueShl;

        public uint RedShr;
        public uint GreenShr;
        public uint BlueShr;

        public uint AlphaBpp;
        public uint AlphaShl;
        public uint AlphaShr;

        public ColorFormat() { }

        public ColorFormat(ref BinaryReader br)
        {
            Mode = (MatColorMode)br.ReadInt32();
            Bpp  = br.ReadUInt32();

            RedBpp   = br.ReadUInt32();
            GreenBpp = br.ReadUInt32();
            BlueBpp  = br.ReadUInt32();

            RedShl   = br.ReadUInt32();
            GreenShl = br.ReadUInt32();
            BlueShl  = br.ReadUInt32();

            RedShr   = br.ReadUInt32();
            GreenShr = br.ReadUInt32();
            BlueShr  = br.ReadUInt32();

            AlphaBpp = br.ReadUInt32();
            AlphaShl = br.ReadUInt32();
            AlphaShr = br.ReadUInt32();
        }

        public void Save(ref BinaryWriter bw)
        {
            bw.Write((uint)Mode);
            bw.Write(Bpp);

            bw.Write(RedBpp);
            bw.Write(GreenBpp);
            bw.Write(BlueBpp);

            bw.Write(RedShl);
            bw.Write(GreenShl);
            bw.Write(BlueShl);

            bw.Write(RedShr);
            bw.Write(GreenShr);
            bw.Write(BlueShr);

            bw.Write(AlphaBpp);
            bw.Write(AlphaShl);
            bw.Write(AlphaShr);
        }

        public static readonly ColorFormat Indexed = new ColorFormat()
        { 
            Mode = MatColorMode.Indexed,
            Bpp  = 8,

            RedBpp = 0, GreenBpp = 0, BlueBpp  = 0,
            RedShl = 0, GreenShl = 0, BlueShl = 0,
            RedShr = 0, GreenShr = 0, BlueShr = 0,
            AlphaBpp = 0, AlphaShl = 0, AlphaShr = 0 
        };

        public static readonly ColorFormat ABGR32 = new ColorFormat()
        { 
            Mode = MatColorMode.RGBA,
            Bpp  = 32,

            RedBpp = 8, GreenBpp = 8, BlueBpp  = 8,
            RedShl = 24, GreenShl = 16, BlueShl = 8,
            RedShr = 0, GreenShr = 0, BlueShr = 0,
            AlphaBpp = 8, AlphaShl = 0, AlphaShr = 0 
        };

        public static readonly ColorFormat BGR24 = new ColorFormat()
        {
            Mode = MatColorMode.RGB,
            Bpp = 24,

            RedBpp = 8, GreenBpp = 8, BlueBpp = 8,
            RedShl = 16, GreenShl = 8, BlueShl = 0,
            RedShr = 0, GreenShr = 0, BlueShr = 0,
            AlphaBpp = 0, AlphaShl = 0, AlphaShr = 0
        };

        public static readonly ColorFormat ABGR1555 = new ColorFormat()
        { 
            Mode = MatColorMode.RGBA,
            Bpp  = 16,

            RedBpp = 5, GreenBpp = 5, BlueBpp  = 5,
            RedShl = 11, GreenShl = 6, BlueShl = 1,
            RedShr = 3, GreenShr = 3, BlueShr = 3,
            AlphaBpp = 1, AlphaShl = 0, AlphaShr = 7 
        };

        public static readonly ColorFormat ABGR4444 = new ColorFormat()
        { 
            Mode = MatColorMode.RGBA,
            Bpp  = 16,

            RedBpp = 4, GreenBpp = 4, BlueBpp  = 4,
            RedShl = 12, GreenShl = 8, BlueShl = 4,
            RedShr = 4, GreenShr = 4, BlueShr = 4,
            AlphaBpp = 4, AlphaShl = 0, AlphaShr = 4 
        };

        public static readonly ColorFormat BGR565 = new ColorFormat()
        {
            Mode = MatColorMode.RGB,
            Bpp = 16,

            RedBpp = 5, GreenBpp = 6, BlueBpp = 5,
            RedShl = 11, GreenShl = 5, BlueShl = 0,
            RedShr = 3, GreenShr = 2, BlueShr = 3,
            AlphaBpp = 0, AlphaShl = 0, AlphaShr = 0
        };
    }

    public class Material
    {
        public readonly string Name;

        public const int ColorMaterialSize = 32;

        public class Record
        {
            public int colorIndex;
            public int textureId;
        }

        public class TextureHeader
        {
            public int Width;
            public int Height;
            public bool Transparent;
            public int TransparentColorNum;
            public List<byte[]> MipmapData = new List<byte[]>();
        }

        //public int TransparentColor = 0;

        public ColorFormat format;

        public List<Record> Records = new List<Record>();
        public List<TextureHeader> Textures = new List<TextureHeader>();

        public int Width
        {
            get
            {
                if (Textures.Count == 0)
                    return ColorMaterialSize;

                return Textures[0].Width;
            }
        }

        public int Height
        {
            get
            {
                if (Textures.Count == 0)
                    return ColorMaterialSize;

                return Textures[0].Height;
            }
        }

        public bool IsSingleColor(int index = 0)
        {
            if (index < 0 || index >= Records.Count)
                return false;// whatev

            Record mh = Records[index];

            if (mh.textureId != -1)
                return false;

            return true;
        }

        public unsafe void GenerateBitmap(out Bitmap bmp, Colormap cmp, out bool failedToNoColormap, bool fillBGForTransparent, int index = 0)
        {
            failedToNoColormap = false;
            bmp = null;

            //if (cmp == null)
            //    return;

            if (index < 0 || index >= Records.Count)
                return;

            Record mh = Records[index];

            if (mh.textureId == -1)
            {
                // single color mat
                if (mh.colorIndex < 0 || mh.colorIndex > 255)
                    return;

                if (cmp == null)
                {
                    failedToNoColormap = true;
                    return;
                }

                Colormap.RGB rgb = cmp.Palette[mh.colorIndex];
                bmp = new Bitmap(ColorMaterialSize, ColorMaterialSize, PixelFormat.Format24bppRgb);
                using (Graphics gfx = Graphics.FromImage(bmp))
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(rgb.R, rgb.G, rgb.B)), new Rectangle(0, 0, bmp.Width, bmp.Height));

                return;
            }

            if (mh.textureId >= Textures.Count)
                return;

            TextureHeader th = Textures[mh.textureId];

            byte[] mipmap = th.MipmapData[0];   // assuming first mipmap for now
            if (mipmap == null || mipmap.Length == 0)
                return;

            bool hasSelfIlluminatedPixels = false;
            if (format.Mode == MatColorMode.Indexed)
            {
                if (cmp == null)
                {
                    failedToNoColormap = true;
                    return;
                }

                if (mipmap.Length < (th.Width * th.Height))
                    return;

                fixed (byte* pMipmap = mipmap)
                {
                    for (int y = 0; y < th.Height; y++)
                    {
                        byte* pMipmapRow = pMipmap + y * (th.Width * 1);
                        for (int x = 0; x < th.Width; x++)
                        {
                            byte b = pMipmapRow[x];

                            Colormap.RGB rgb = cmp.Palette[b];

                            if (rgb.SelfIlluminated)
                            {
                                hasSelfIlluminatedPixels = true;
                                break;
                            }
                        }
                    }
                }
            }

            bmp = new Bitmap(th.Width, th.Height, PixelFormat.Format32bppArgb);   // assuming no transparency for now

            Color transColor = Color.FromArgb(255, 0, 255);

            // fill background with 0,0,0,0  or  255,0,255 (or whatever) based on render preference
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                if (fillBGForTransparent)
                    // transparent as magenta?
                    gfx.FillRectangle(new SolidBrush(transColor), new Rectangle(0, 0, bmp.Width, bmp.Height));
                else
                    // transparent as 0,0,0,0
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), new Rectangle(0, 0, bmp.Width, bmp.Height));
            }

            // now we lock-down and fill valid pixels
            BitmapData bmdat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            fixed (byte* pMipmap = mipmap)
            {
                if (format.Mode == MatColorMode.Indexed && cmp != null)
                {
                    // 8-bit palette;  use colormap lookup
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                        byte* pMipmapRow = pMipmap + y * (th.Width * 1);

                        for (int x = 0; x < bmp.Width; x++)
                        {
                            byte b = pMipmapRow[x];

                            // if transparent pixel;  we have already filled the background
                            if (th.Transparent && b == th.TransparentColorNum) // guess based on OpenJKDF2 source code
                                continue;

                            Colormap.RGB rgb = cmp.Palette[b];
                            pBMPRow[x * 4 + 0] = rgb.B;     // blue
                            pBMPRow[x * 4 + 1] = rgb.G;     // green
                            pBMPRow[x * 4 + 2] = rgb.R;     // red
                            pBMPRow[x * 4 + 3] = 255;       // alpha
                        }
                    }
                }
                else if (format.Mode == MatColorMode.RGB || format.Mode == MatColorMode.RGBA)
                {
                    int pixsize = (int)format.Bpp / 8;
                    int stride  = th.Width * pixsize;
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                        byte* pRow    = pMipmap + y * stride;

                        for (int x = 0; x < bmp.Width; x++)
                        {
                            int epix = 0;
                            switch (pixsize)
                            {
                                case 2: epix = *(ushort*)&pRow[x * pixsize]; break;
                                case 3: epix = (pRow[x * pixsize] | (pRow[x * pixsize + 1] << 8) | (pRow[x * pixsize + 2] << 16)); break;
                                case 4: epix = *(int*)&pRow[x * pixsize]; break;
                            }

                            var pix = DecodePixel(epix, format);
                            pBMPRow[x * 4 + 0] = pix.B;
                            pBMPRow[x * 4 + 1] = pix.G;
                            pBMPRow[x * 4 + 2] = pix.R;
                            pBMPRow[x * 4 + 3] = pix.A;
                        }
                    }
                }
                else
                {
                    //Log.Error("Unsupported mat bit depth: " + ColorBits.ToString());
                }
            }
            bmp.UnlockBits(bmdat);

            return;
        }

        public Material()
        {

        }

        public void Save(string filepath)
        {
            using(Stream s = File.Create(filepath))
                Save(s);
        }

        public void Save(Stream s)
        {
            BinaryWriter bw = new BinaryWriter(s);

            bw.Write((byte)'M');
            bw.Write((byte)'A');
            bw.Write((byte)'T');
            bw.Write((byte)' ');
            bw.Write(0x32); // ver
            bw.Write(2);    // type
            bw.Write(Records.Count);    // nummaterials
            bw.Write(Textures.Count);    // numtextures

            format.Save(ref bw);

            int texIndex = 0;
            foreach(Record mh in Records)
            {
                bw.Write(8);    // texture

                // unknown "colornum"
                /*if (ColorBits == 8)
                    bw.Write(0xFF); // always 255 for 8bit?
                else
                    bw.Write(0);    // is this 0 for 16bit?*/
                bw.Write(mh.colorIndex);

                for (int x = 0; x < 4; x++)
                    bw.Write((int)0x3F800000);
#if false
                // matmaster seems to have these swapped;  use this for matmaster "compatibility"
                bw.Write(0);    // dunno
                bw.Write((uint)0xBFF78482);  // dunno
                bw.Write(0);    // dunno
#else
                bw.Write(0);    // dunno
                bw.Write(0);    // dunno
                bw.Write((uint)0xBFF78482);  // dunno
#endif
                bw.Write(texIndex);    // texture index

                texIndex++;
            }

            foreach(TextureHeader th in Textures)
            {
                bw.Write(th.Width);
                bw.Write(th.Height);
                bw.Write(th.Transparent?1:0/*should be color value?*/);    // transparent
                bw.Write(0);    // pad
                bw.Write(th.TransparentColorNum);
                bw.Write(th.MipmapData.Count);    // mipmaps

                // dump mips
                foreach (byte[] mip in th.MipmapData)
                    bw.Write(mip);

            }
        }
        

        public Material(string n, Stream s)
        {
            Name = n;

            BinaryReader br = new BinaryReader(s);

            br.ReadBytes(8);

            int type = br.ReadInt32();
            int numRecords  = br.ReadInt32();
            int numTextures = br.ReadInt32();
            format = new ColorFormat(ref br);

            for (int x = 0; x < numRecords; x++)
            {
                Record m = new Record();

                int mtype = br.ReadInt32();
                m.colorIndex = br.ReadInt32();
                br.ReadBytes(16);

                if (mtype == 8)
                {
                    br.ReadBytes(12);
                    m.textureId = br.ReadInt32();
                }
                else
                    m.textureId = -1;

                Records.Add(m);
            }

            for (int x = 0; x < numTextures; x++)
            {
                TextureHeader t = new TextureHeader();

                t.Width = br.ReadInt32();
                t.Height = br.ReadInt32();
                t.Transparent = (br.ReadInt32() != 0);
                br.ReadBytes(4);
                t.TransparentColorNum = br.ReadInt32();

                int mipmapBufSize = (t.Width * t.Height * (int)format.Bpp) / 8;
                int numMipmaps    = br.ReadInt32();
                for (int y = 0; y < numMipmaps; y++)
                {
                    t.MipmapData.Add(br.ReadBytes(mipmapBufSize));
                    mipmapBufSize /= 4;
                }

                Textures.Add(t);
            }
        }
        static uint GetColorMask(uint bpc)
        {
            return 0xFFFFFFFF >> (32 - (int)bpc);
        }

        public static Color DecodePixel(int p, ColorFormat cf)
        {
            int r = ((p >> (int)cf.RedShl) & (int)GetColorMask(cf.RedBpp)) << (int)cf.RedShr;
            int g = ((p >> (int)cf.GreenShl) & (int)GetColorMask(cf.GreenBpp)) << (int)cf.GreenShr;
            int b = ((p >> (int)cf.BlueShl) & (int)GetColorMask(cf.BlueBpp)) << (int)cf.BlueShr;
            int a = 255;
            if (cf.AlphaBpp > 0)
            {
                a = ((p >> (int)cf.AlphaShl) & (int)GetColorMask(cf.AlphaBpp)) << (int)cf.AlphaShr;
                if (cf.AlphaBpp == 1) // RGB5551
                {
                    a = a > 0 ? 255 : 0;
                }
            }
            return Color.FromArgb(a, r, g, b);
        }

        public static int EncodePixel(Color p, ColorFormat cf)
        {
            int ep =
            ((p.R >> (int)cf.RedShr) << (int)cf.RedShl) |
            ((p.G >> (int)cf.GreenShr) << (int)cf.GreenShl) |
            ((p.B >> (int)cf.BlueShr) << (int)cf.BlueShl);

            if (cf.AlphaBpp != 0)
            {
                ep |= (p.A >> (int)cf.AlphaShr) << (int)cf.AlphaShl;
            }
            return ep;
        }
    }
}

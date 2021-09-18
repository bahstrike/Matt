using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Smith
{
    public class Material
    {
        public readonly string Name;

        public const int ColorMaterialSize = 64;

        public class MaterialHeader
        {
            public int colorIndex;
            public int textureId;
        }

        public class TextureHeader
        {
            public int Width;
            public int Height;
            public bool Transparent;
            public List<byte[]> MipmapData = new List<byte[]>();
        }

        public int TransparentColor = 0;
        public int ColorBits = 0;
        public int BlueBits = 0;
        public int GreenBits = 0;
        public int RedBits = 0;

        public List<MaterialHeader> Materials = new List<MaterialHeader>();
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
            if (index < 0 || index >= Materials.Count)
                return false;// whatev

            MaterialHeader mh = Materials[index];

            if (mh.textureId != -1)
                return false;

            return true;
        }

        public unsafe void GenerateBitmap(out Bitmap bmp, Colormap cmp, out bool failedToNoColormap, int index = 0)
        {
            failedToNoColormap = false;

            bmp = null;

            //if (cmp == null)
            //    return;

            if (index < 0 || index >= Materials.Count)
                return;

            MaterialHeader mh = Materials[index];

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
            if (ColorBits == 8)
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
            BitmapData bmdat = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            fixed (byte* pMipmap = mipmap)
            {
                if (ColorBits == 8 && cmp != null)
                {
                    // 8-bit palette;  use colormap lookup
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                        byte* pMipmapRow = pMipmap + y * (th.Width * 1);

                        for (int x = 0; x < bmp.Width; x++)
                        {
                            byte b = pMipmapRow[x];

                            Colormap.RGB rgb = cmp.Palette[b];

                            pBMPRow[x * 4 + 0] = rgb.B;                                                              // blue
                            pBMPRow[x * 4 + 1] = rgb.G;                                                              // green
                            pBMPRow[x * 4 + 2] = rgb.R;                                                              // red
                            pBMPRow[x * 4 + 3] = (byte)((th.Transparent && b == TransparentColor) ? 0 : 255);        // alpha
                        }
                    }
                }
                else if (ColorBits == 16)
                {
                    // 16-bit

                    if (GreenBits == 5)
                    {
                        // 1555 ARGB
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                            ushort* pMipmapRow = (ushort*)(pMipmap + y * (th.Width * 2));

                            for (int x = 0; x < bmp.Width; x++)
                            {
                                ushort pixelword = pMipmapRow[x];

#if true
                                // NEW METHOD;  works for indy

                                if ((pixelword & 0x1) != 0)
                                {
                                    // opaque
                                    pBMPRow[x * 4 + 0] = (byte)((double)((pixelword>>1) & 0x1F) / (double)0x1F * 255.0);		// blue
                                    pBMPRow[x * 4 + 1] = (byte)((double)((pixelword>>6) & 0x1F) / (double)0x1F * 255.0);	// green
                                    pBMPRow[x * 4 + 2] = (byte)((double)((pixelword>>11) & 0x1F) / (double)0x1F * 255.0);	// red
                                    pBMPRow[x * 4 + 3] = 255;                                                           // alpha
                                }
                                else
                                {
                                    // transparent
                                    pBMPRow[x * 4 + 0] = 0;     // blue
                                    pBMPRow[x * 4 + 1] = 0;     // green
                                    pBMPRow[x * 4 + 2] = 0;     // red
                                    pBMPRow[x * 4 + 3] = 0;     // alpha
                                }
#else
                                // OLD METHOD;  was in use for JK..  not totally confirmed if correct

                                if ((pixelword & 0x8000) != 0)
                                {
                                    // opaque
                                    pBMPRow[x * 4 + 0] = (byte)((double)(pixelword & 0x1F) / (double)0x1F * 255.0);		// blue
                                    pBMPRow[x * 4 + 1] = (byte)((double)(pixelword & 0x3E0) / (double)0x3E0 * 255.0);	// green
                                    pBMPRow[x * 4 + 2] = (byte)((double)(pixelword & 0x7C00) / (double)0x7C00 * 255.0);	// red
                                    pBMPRow[x * 4 + 3] = 255;                                                           // alpha
                                }
                                else
                                {
                                    // transparent
                                    pBMPRow[x * 4 + 0] = 0;     // blue
                                    pBMPRow[x * 4 + 1] = 0;     // green
                                    pBMPRow[x * 4 + 2] = 0;     // red
                                    pBMPRow[x * 4 + 3] = 0;     // alpha
                                }
#endif
                            }
                        }
                    }
                    else if (GreenBits == 6)
                    {
                        // 565 RGB
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                            ushort* pMipmapRow = (ushort*)(pMipmap + y * (th.Width * 2));

                            for (int x = 0; x < bmp.Width; x++)
                            {
                                ushort pixelword = pMipmapRow[x];

#if true
                                pBMPRow[x * 4 + 0] = (byte)((pixelword & 0x1F) * 255 / 0x1F);	        // blue
                                pBMPRow[x * 4 + 1] = (byte)(((pixelword >> 5) & 0x3F) * 255 / 0x3F);	// green
                                pBMPRow[x * 4 + 2] = (byte)(((pixelword >> 11) & 0x1F) * 255 / 0x1F); 	// red
                                pBMPRow[x * 4 + 3] = 255;                                               // alpha
#else

                                pBMPRow[x * 4 + 0] = (byte)((double)(pixelword & 0x1F) / (double)0x1F * 255.0);		// blue
                                pBMPRow[x * 4 + 1] = (byte)((double)(pixelword & 0x7E0) / (double)0x7E0 * 255.0);	// green
                                pBMPRow[x * 4 + 2] = (byte)((double)(pixelword & 0xF800) / (double)0xF800 * 255.0);	// red
                                pBMPRow[x * 4 + 3] = 255;                                                           // alpha
#endif
                            }
                        }
                    }
                    else if (GreenBits == 4)
                    {
                        // 4444 RGBA
                        for (int y = 0; y < bmp.Height; y++)
                        {
                            byte* pBMPRow = (byte*)((ulong)bmdat.Scan0 + (ulong)(bmdat.Stride * y));
                            ushort* pMipmapRow = (ushort*)(pMipmap + y * (th.Width * 2));

                            for (int x = 0; x < bmp.Width; x++)
                            {
                                ushort pixelword = pMipmapRow[x];

                                pBMPRow[x * 4 + 0] = (byte)(((pixelword>>4) & 0xF) * 255 / 0xF);	    // blue
                                pBMPRow[x * 4 + 1] = (byte)(((pixelword >> 8) & 0xF) * 255 / 0xF);	    // green
                                pBMPRow[x * 4 + 2] = (byte)(((pixelword >> 12) & 0xF) * 255 / 0xF); 	// red
                                pBMPRow[x * 4 + 3] = (byte)((pixelword & 0xF) * 255 / 0xF);             // alpha
                            }
                        }
                    }
                    else
                    {
                        //Log.Error($"Unsupported 16-bit mat channel configuration {RedBits}{GreenBits}{BlueBits}");
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
            Save(File.Create(filepath));
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
            bw.Write(Materials.Count);    // nummaterials
            bw.Write(Textures.Count);    // numtextures
            bw.Write(TransparentColor);    // transparency value
            bw.Write(ColorBits);    // colorbits
            bw.Write(BlueBits);    // bluebits
            bw.Write(GreenBits);    // greenbits
            bw.Write(RedBits);    // redbits

            bw.Write(0x0B);// unknown
            bw.Write(0x05);//unknown
            bw.Write(0);//unknown
            bw.Write(0x03);//unknown
            bw.Write(0x02);//unknown
            bw.Write(0x03);//unknown
            for (int x = 0; x < 3 * 4; x++)
                bw.Write((byte)0);

            int texIndex = 0;
            foreach(MaterialHeader mh in Materials)
            {
                bw.Write(8);    // texture
                bw.Write(0);    // unknown
                for (int x = 0; x < 4; x++)
                    bw.Write((int)0x3F800000);
                bw.Write(0);    // dunno
                bw.Write(0);    // dunno
                bw.Write((uint)0xBFF78482);  // dunno
                bw.Write(texIndex);    // texture index

                texIndex++;
            }

            foreach(TextureHeader th in Textures)
            {
                bw.Write(th.Width);
                bw.Write(th.Height);
                bw.Write(th.Transparent);    // transparent
                bw.Write(0);    // pad
                bw.Write(0);    // pad
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
            int numMaterials = br.ReadInt32();
            int numTextures = br.ReadInt32();
            TransparentColor = br.ReadInt32();
            ColorBits = br.ReadInt32();
            BlueBits = br.ReadInt32();
            GreenBits = br.ReadInt32();
            RedBits = br.ReadInt32();
            br.ReadBytes(36);

            // JKPaint support
            if (ColorBits == 16 && RedBits == 0 && GreenBits == 0 && BlueBits == 0)
            {
                if (TransparentColor == 1)
                {
                    RedBits = 5;
                    GreenBits = 6;
                    BlueBits = 5;
                }
                else
                {
                    RedBits = 5;
                    GreenBits = 5;
                    BlueBits = 5;
                }
            }

            for (int x = 0; x < numMaterials; x++)
            {
                MaterialHeader m = new MaterialHeader();

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

                Materials.Add(m);
            }

            for (int x = 0; x < numTextures; x++)
            {
                TextureHeader t = new TextureHeader();

                t.Width = br.ReadInt32();
                t.Height = br.ReadInt32();
                t.Transparent = (br.ReadInt32() != 0);
                br.ReadBytes(8);

                int mipmapBufSize = (t.Width * t.Height * ColorBits) / 8;
                int numMipmaps = br.ReadInt32();
                for (int y = 0; y < numMipmaps; y++)
                {
                    t.MipmapData.Add(br.ReadBytes(mipmapBufSize));
                    mipmapBufSize /= 4;
                }

                Textures.Add(t);
            }
        }
    }
}

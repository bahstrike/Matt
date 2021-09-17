using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Smith
{
    public class Colormap
    {
        public readonly string Name;

        public struct RGB
        {
            public byte R;
            public byte G;
            public byte B;
            public bool SelfIlluminated;

            public System.Drawing.Color Color
            {
                get
                {
                    return System.Drawing.Color.FromArgb(R, G, B);
                }
            }
        }

        public float TintR;
        public float TintG;
        public float TintB;
        public RGB[] Palette = new RGB[256];

        public override string ToString()
        {
            return Name;
        }

        public int FindClosestColor(int r, int g, int b)
        {
            int smallestDiffIndex = 0;

            for(int i=1; i<256; i++)
            {
                int bestDiff = DetermineColorDiff(smallestDiffIndex, r, g, b);
                int curDiff = DetermineColorDiff(i, r, g, b);

                if (curDiff < bestDiff)
                    smallestDiffIndex = i;

                // if diff is 0 then dont bother checking more
                if (curDiff == 0)
                    break;
            }

            return smallestDiffIndex;
        }

        private int DetermineColorDiff(int i, int r, int g, int b)
        {
            RGB p = Palette[i];

            int diff = 0;
            diff += Math.Abs((int)p.R - r);
            diff += Math.Abs((int)p.G - g);
            diff += Math.Abs((int)p.B - b);

            return diff;
        }

        public Colormap(string n, Stream s)
        {
            Name = n;

            BinaryReader br = new BinaryReader(s);

            br.ReadBytes(8);
            bool transparency = (br.ReadInt32()!=0);
            TintR = br.ReadSingle();
            TintG = br.ReadSingle();
            TintB = br.ReadSingle();
            br.ReadBytes(40);

            /*if (Tint != Color4.white() && Tint != Color4.black())
            {
                Log.Info("Using colormap with tint (" + Name + ")");
            }*/

            for (int x = 0; x < 256; x++)
            {
                RGB rgb = new RGB();

                rgb.R = br.ReadByte();
                rgb.G = br.ReadByte();
                rgb.B = br.ReadByte();

                Palette[x] = rgb;
            }


            // maybe this cmp doesnt include anything else?
            if (s.Position >= s.Length)
                return;

            // light table is stored as full palette  source color -> dest color
            // for each light level

            byte[][] lightTables = new byte[64][];
            for (int x = 0; x < 64; x++)
                lightTables[x] = br.ReadBytes(256);

            // for now lets just see if lightlevel 0 and 63 are the same and store that as a flag in the palette
            for (int x = 0; x < 256; x++)
            {
                // dont make black pixels self illuminated
                if (Palette[x].R == 0 && Palette[x].G == 0 && Palette[x].B == 0)
                    continue;

                Palette[x].SelfIlluminated = (lightTables[0][x] == lightTables[63][x]);

                if (Palette[x].SelfIlluminated)
                {
                    //x = x;
                }
            }

            // probably should care about lighting data but i dunno how to use lol
            //br.ReadBytes(16384);

#if false
            if (transparency)
            {
                byte[][] transtable = new byte[256][];

                // care about transparency table?
                for (int x = 0; x < 256; x++)
                {
                    transtable[x] = br.ReadBytes(256);
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(258, 258, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                bmp.SetPixel(0, 0, System.Drawing.Color.White);
                bmp.SetPixel(1, 0, System.Drawing.Color.White);
                bmp.SetPixel(0, 1, System.Drawing.Color.White);
                bmp.SetPixel(1, 1, System.Drawing.Color.White);

                for(int y=0; y<256; y++)
                    for (int x = 0; x < 256; x++)
                    {
                        byte b = transtable[y][x];

                        RGB rgb = Palette[b];

                        bmp.SetPixel(x+2, y+2, System.Drawing.Color.FromArgb(rgb.R, rgb.G, rgb.B));
                    }


                // what in the hell is this
                bmp.Save(@"c:\smith\tmp\transtable.bmp");
            }
#endif
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Smith;

namespace MattControls
{
    public partial class PaletteControl : PictureBox
    {
        public PaletteControl()
        {
            InitializeComponent();
        }
        
        public void UpdateCMP(Colormap cmp, int CurrentColorIndex, List<int> usedPaletteIndices)
        {
            Matt.Log.Print("UpdateCMP");

            if (cmp == null)
            {
                Image = null;
                return;
            }

            int width = ClientRectangle.Width;
            int height = ClientRectangle.Height;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(16, 16, 16)), new Rectangle(0, 0, width, height));
                //FillRectEmpty(gfx, new Rectangle(0, 0, width, height));

                for (int i = 0; i < 256; i++)
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
                        gfx.DrawLine(new Pen(inverseColor, 1.0f), clrRc.X, clrRc.Y + clrRc.Height, clrRc.X + clrRc.Width, clrRc.Y);
                    //    gfx.DrawRectangle(new Pen(Color.FromArgb(255 - clr.R, 255 - clr.G, 255 - clr.B), 1.0f), clrRc.X, clrRc.Y, clrRc.Width, clrRc.Height);
                }

            }

            Image = bmp;
        }
    }
}

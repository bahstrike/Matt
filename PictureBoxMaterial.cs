using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Matt
{
    public class PictureBoxMaterial : PictureBox
    {
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            MattMain.FillRectEmpty(paintEventArgs.Graphics, ClientRectangle);

            if (Image == null)
                return;

            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.OnPaint(paintEventArgs);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Matt
{
    public struct WorkColor
    {
        public double a;
        public double r;
        public double g;
        public double b;

        public bool Transparent
        {
            get
            {
                return (a < 0.001);
            }
        }

        public static implicit operator WorkColor(Color clr)
        {
            WorkColor r = new WorkColor();
            r.a = (double)clr.A;
            r.r = (double)clr.R;
            r.g = (double)clr.G;
            r.b = (double)clr.B;
            return r;
        }

        public static implicit operator Color(WorkColor clr)
        {
            return Color.FromArgb(
                    Math.Max(0, Math.Min(255, (int)Math.Round(clr.a))),
                    Math.Max(0, Math.Min(255, (int)Math.Round(clr.r))),
                    Math.Max(0, Math.Min(255, (int)Math.Round(clr.g))),
                    Math.Max(0, Math.Min(255, (int)Math.Round(clr.b)))
                );
        }

        public static implicit operator WorkColor(Smith.Colormap.RGB rgb)
        {
            WorkColor r = new WorkColor();
            r.a = 255.0;
            r.r = (double)rgb.R;
            r.g = (double)rgb.G;
            r.b = (double)rgb.B;
            return r;
        }

        public static WorkColor operator +(WorkColor a, WorkColor b)
        {
            WorkColor r = new WorkColor();
            r.a = a.a + b.a;
            r.r = a.r + b.r;
            r.g = a.g + b.g;
            r.b = a.b + b.b;
            return r;
        }

        public static WorkColor operator -(WorkColor a, WorkColor b)
        {
            WorkColor r = new WorkColor();
            r.a = a.a - b.a;
            r.r = a.r - b.r;
            r.g = a.g - b.g;
            r.b = a.b - b.b;
            return r;
        }

        public static WorkColor operator *(WorkColor a, double s)
        {
            WorkColor r = new WorkColor();
            r.a = a.a * s;
            r.r = a.r * s;
            r.g = a.g * s;
            r.b = a.b * s;
            return r;
        }

        public static WorkColor operator /(WorkColor a, double s)
        {
            WorkColor r = new WorkColor();
            r.a = a.a / s;
            r.r = a.r / s;
            r.g = a.g / s;
            r.b = a.b / s;
            return r;
        }
    }
}

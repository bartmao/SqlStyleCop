using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSTReportConfigurationTool
{
    public static class FontSizeRuler
    {
        public static SizeF GetFontSize(Font font, string str)
        {
            var path = new GraphicsPath();
            path.AddString(str,font.FontFamily,(int)font.Style,font.Size,Point.Empty,new StringFormat());
            var rect = path.GetBounds();
           
            return rect.Size;
        }
        public static double GetFontSizeInch(Font font, string str)
        {
            return GetFontSize(font, str).Width / 72.0;
        }


        public static SizeF GetWWidth(Font font)
        {
            return GetFontSize(font, "W");
        }
    }
}

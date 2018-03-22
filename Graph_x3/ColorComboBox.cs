using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Graph_x3
{
    public class ColorComboBox : ComboBox
    {
        public ColorComboBox()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            foreach (string s in Enum.GetNames(typeof(System.Drawing.KnownColor)))
                Items.Add(s);
            SelectedIndex = 0;
            DropDownStyle = ComboBoxStyle.DropDownList;
            this.DrawItem += new DrawItemEventHandler(ColorComboBox_DrawItem);
        }
        void ColorComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index < 0) return;
            Color c = Color.FromName(Items[e.Index] as string);
            Rectangle rect = new Rectangle
                     (4, e.Bounds.Top + 2, e.Bounds.Height, e.Bounds.Height - 4);
            e.Graphics.FillRectangle(new SolidBrush(c), rect);
            e.Graphics.DrawRectangle(Pens.Black, rect);
            e.Graphics.DrawString(Items[e.Index].ToString(), Font, Brushes.Black,
                     e.Bounds.Height + 5,
                     ((e.Bounds.Height - Font.Height) / 2) + e.Bounds.Top);
        }

    }

}

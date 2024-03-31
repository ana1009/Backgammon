using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Drawing;

namespace Table.view.grafica
{
    class PiesaCreare
    {
        public Ellipse piesaJoc { get; }

        public PiesaCreare(Color culoare, int dimensiune)
        {
            //cream piesa
            piesaJoc = new Ellipse();
            piesaJoc.Width = dimensiune;
            piesaJoc.Height = dimensiune;

            //coloram piesa
            SolidColorBrush brush = new SolidColorBrush(culoare);
            piesaJoc.Fill = brush;


        }
    }
}

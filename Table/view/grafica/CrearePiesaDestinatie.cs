using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Table.view.grafica
{
    class CrearePiesaDestinatie
    {
        public Rectangle piesa { get; }

        public CrearePiesaDestinatie(int marime, Color culoare)
        {

            //cream piesa
            piesa = new Rectangle();
            piesa.Height = marime/3;
            piesa.Width = marime;

            //bordura
            SolidColorBrush pensulaBordura = new SolidColorBrush();
            pensulaBordura.Color = Colors.DarkGray;
            piesa.StrokeThickness = 1;
            piesa.Stroke = pensulaBordura;

            //coloram piesa
            SolidColorBrush brush = new SolidColorBrush(culoare);
            piesa.Fill = brush;

        }

    }
}
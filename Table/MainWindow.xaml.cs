using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Table.control;
using Table.model.logic;
using Table.model.logic.campuri;
using Table.view.grafica;

namespace Table
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MotorulJocului motorulJocului;
        Grid selectieGrid1,selectieGrid2;
        Mutare mutare = new Mutare();
        public MainWindow()
        {
            // Seteaza culoare jucator
            String j1Culoare = "White";
            String j2Culoare = "Black";

            // Creare Jucatori
            Jucator jucator1 = new Jucator(1, "Player1", j1Culoare);
            Jucator jucator2 = new Jucator(2, "Player2", j2Culoare);

            // Creare piese
            Piesa[] J1Piese = new Piesa[15];
            for (int i = 0; i < 15; i++)
            {
                J1Piese[i] = new Piesa(i, jucator1);
            }
            Piesa[] J2Piese = new Piesa[15];
            for (int i = 0; i < 15; i++)
            {
                J2Piese[i] = new Piesa(i + 15, jucator2);
            }

            // Creare zaruri
            Zar z1 = new Zar();
            Zar z2 = new Zar();

            AruncareZaruri aruncareZaruri = new AruncareZaruri(z1, z2);

            //Creare campuri tabla
            CampDePeTabla[] campuriDePeTabla=new CampDePeTabla[24];
            for(int i = 0; i < 24; i++)
            {
                campuriDePeTabla[i] = new CampDePeTabla(new LinkedList<Piesa>(), i);
            }

            //creare camp eliminare
            CampEliminat campEliminat = new CampEliminat();

            //creare campuri destinatie
            CampDestinatie campDestinatieJ1 = new CampDestinatie(jucator1, 27);
            CampDestinatie campDestinatieJ2 = new CampDestinatie(jucator2, 26);

            //creare reguli
            Reguli reguli = new Reguli(campuriDePeTabla, campEliminat, campDestinatieJ1, campDestinatieJ2, aruncareZaruri, jucator1, jucator2);

            //creare tabla de joc
            TablaDeJoc tablaDeJoc = new TablaDeJoc(campuriDePeTabla,campEliminat,campDestinatieJ1,campDestinatieJ2,J1Piese,J2Piese,aruncareZaruri,reguli,jucator1,jucator2);

            //selectie -1 =nici un camp selectat
            mutare.from = -1;
            mutare.to = -1;

            motorulJocului = new MotorulJocului(tablaDeJoc);

            InitializeComponent();
        }

        //incepe jocul
        private void startJoc(object sender, RoutedEventArgs e)
        {
            //piese in pozitia de start
        //campuriDePeTabla    0  1  2  3  4  5  6  7  8  9  10 11 12 13 14 15 16 17 18 19 20 21 22 23 -  E D2 D1
            int[] j1Array = { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 0, 3, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] j2Array = { 0, 0, 0, 0, 0, 5, 0, 3, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0 };

            //pune piesele in pozitia de start
            motorulJocului.initializareSituatieStart(j1Array, j2Array);

            motorulJocului.startJoc();


            exceptionMessage.Text = "";
            zar1Imagine.Source = null;
            zar2Imagine.Source = null;
            startGrid.Visibility = Visibility.Hidden;
            printeazatabla();
            btnZar.IsEnabled = true;
        }

        //printeaza piesele pe tabla
        public void printeazatabla()
        {
            afiseazaJucatorulActiv();

            //loop pt fiecare camp de pe tabla
            for(int i = 0; i <= 23; i++)
            {
                string stackPanelName = "f" + i;
                StackPanel panel = (StackPanel)this.FindName(stackPanelName);
                panel.Children.Clear();

                //deseneaza fiecare piesa in camp
                for(int j=0;j< motorulJocului.obtineNumarulPieselorInCamp(i); j++)
                {
                    //pt jumatatea de jos
                    if (i > 11)
                    {
                        panel.VerticalAlignment= VerticalAlignment.Bottom;
                    }
                    
                    Color culoare = (Color)ColorConverter.ConvertFromString(motorulJocului.obtineCuloarea(i));
                    PiesaCreare piesa = new PiesaCreare(culoare, 30);

                    //ajusteaza marginile daca in stackpanel sunt mai mult de 5 piese
                    Thickness margine = new Thickness();
                    if (motorulJocului.obtineNumarulPieselorInCamp(i) > 5)
                    {
                        if (j > 0)
                        {
                            double inaltimePiesa = piesa.piesaJoc.Height;
                            int numarPiese = motorulJocului.obtineNumarulPieselorInCamp(i);
                            double x = ((numarPiese - 5) * inaltimePiesa) / numarPiese;
                            margine = new Thickness(0, -x, 0, 0);
                        }
                    }

                    piesa.piesaJoc.Margin = margine;
                    panel.Children.Add(piesa.piesaJoc);

                }
            }

            //printeaza piesele eliminate in campul de eliminare
            campEliminat.Children.Clear();
            for (int i = 0; i < motorulJocului.obtineNumarulPieselorEliminate(); i++)
            {
                Color culoare = (Color)ColorConverter.ConvertFromString(motorulJocului.obtineCuloareaPieseiEliminate(i));
                PiesaCreare piesa = new PiesaCreare(culoare, 30);
                campEliminat.Children.Add(piesa.piesaJoc);
            }

            //printeaza piesele in camp destinatie 1
            f27.Children.Clear();
            for (int i = 0; i < motorulJocului.obtineNumarulPieselorInCampDestinatieJ1(); i++)
            {
                Color culoare = (Color)ColorConverter.ConvertFromString(motorulJocului.obtineCuloareaDestinatieJ1());
                CrearePiesaDestinatie piesa = new CrearePiesaDestinatie(30, culoare);
                f27.Children.Add(piesa.piesa);
            }

            //printeaza piesele in camp destinatie 2
            f26.Children.Clear();
            for (int i = 0; i < motorulJocului.obtineNumarulPieselorInCampDestinatieJ2(); i++)
            {
                Color culoare = (Color)ColorConverter.ConvertFromString(motorulJocului.obtineCuloareaDestinatieJ2());
                CrearePiesaDestinatie piesa = new CrearePiesaDestinatie(30, culoare);
                f26.Children.Add(piesa.piesa);
            }

            //printeaza info jucator
            numeJ1.Text = motorulJocului.obtineNumeleJucator1();
            numeJ2.Text = motorulJocului.obtineNumeleJucator2();
            SolidColorBrush b1 = new SolidColorBrush();
            b1.Color = (Color)ColorConverter.ConvertFromString(motorulJocului.culoareJucator1());
            piesaJ1.Fill = b1;

            SolidColorBrush b2 = new SolidColorBrush();
            b2.Color = (Color)ColorConverter.ConvertFromString(motorulJocului.culoareJucator2());
            piesaJ2.Fill = b2;
        }

        private void aruncareZaruri(object sender, RoutedEventArgs e)
        {
            exceptionMessage.Text = "";
            motorulJocului.aruncareZaruri();
            afiseazaZaruri();
            btnZar.IsEnabled = false;

            if (!motorulJocului.maiMulteMutari())
            {
                exceptionMessage.Text = "Nu au existat mutari valide pentru " + motorulJocului.obtineNumeleJucatoruluiInactiv();
                btnZar.IsEnabled = true;

                afiseazaJucatorulActiv();
            }
        }

        //Afiseaza valorile zarurilor pe tabla
        public void afiseazaZaruri()
        {
            List<int> mutari = motorulJocului.obtineZaruri();

            if(mutari.Count > 0)
            {
                BitmapImage Img = new BitmapImage(new Uri(@"view\imaginiZar\dice"+ mutari.ElementAt(0)+".png",UriKind.Relative));
                zar1Imagine.Source = Img;
            }
            else
            {
                zar1Imagine.Source = null;
            }
            if (mutari.Count > 1)
            {
                BitmapImage Img = new BitmapImage(new Uri(@"view\imaginiZar\dice" + mutari.ElementAt(1) + ".png", UriKind.Relative));
                zar2Imagine.Source = Img;
            }
            else
            {
                zar2Imagine.Source = null;
            }
        }

        //Evidentiaza mutarile posibile din pozitia selectata
        public bool evidentiazaMutariPosibile(int index)
        {
            List<int> list = motorulJocului.obtineMutariPosibileDeLaPozitie(index);
            for(int i = 0; i < list.Count; i++)
            {
                StackPanel panel = (StackPanel)this.FindName("f" + list.ElementAt(i));
                Grid grid = (Grid)panel.Parent;
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Colors.SandyBrown;
                brush.Opacity = 50;
                grid.Background = brush;
            }
            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //elimina evidentieri
        public void eliminaEvidentiere(int index)
        {
            List<int> list = motorulJocului.obtineMutariPosibileDeLaPozitie(index);
            for(int i = 0; i < list.Count; i++)
            {
                StackPanel panel=(StackPanel)this.FindName("f"+list.ElementAt(i));
                Grid grid=(Grid)panel.Parent;
                grid.Background = null;
            }
        }


        //afiseaza pe tabla care jucator e activ
        public void afiseazaJucatorulActiv()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color=Colors.SandyBrown;
            brush.Opacity = 50;
            if (motorulJocului.obtineNumeleJucatoruluiActiv().Equals(motorulJocului.obtineNumeleJucator1()))
            {
                J1Info.Background = brush;
                J2Info.Background = null;
            }
            else
            {
                J1Info.Background = null;
                J2Info.Background = brush;
            }
        }


        //Cand se face clic pe tabla
        private void onClick(object sender, MouseButtonEventArgs e)
        {
            exceptionMessage.Text = "";
            DependencyObject dep = (DependencyObject)e.OriginalSource;

            //cautare in ierarhia elementelor pentru a gssi grila
            while((dep!=null)&&!(dep is Grid))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null)
            {
                return;
            }

            //dacă obiectul este o grila si grila de start este ascunsa
            if(dep is Grid && !dep.Equals(exceptionOutput) && (startGrid.Visibility == Visibility.Hidden))
            {
                DropShadowEffect effect = new DropShadowEffect();
                effect.Color = Colors.Brown;
                effect.BlurRadius = 7;
                effect.Direction = 380;

                Grid componentaApasata = dep as Grid;

                int selectie = int.Parse(componentaApasata.Tag.ToString());

                if((mutare.from>=0) && (selectie == mutare.from))
                {
                    //resetare selectie 1
                    componentaApasata.Effect = null;
                    eliminaEvidentiere(mutare.from);
                    mutare.from = -1;
                }
                //prima selectie si nu este niciun camp destinatie
                else if((mutare.from<0) && (selectie>=0) && (selectie!=26) && (selectie != 27)){
                    //face prima selectie
                    if (evidentiazaMutariPosibile(selectie))
                    {
                        componentaApasata.Effect = effect;
                        mutare.from=selectie;
                        selectieGrid1=componentaApasata;
                    }
                }
                //face a doua selectie
                else if((mutare.from >= 0) && (mutare.to < 0))
                {
                    mutare.to = selectie;
                    selectieGrid2 = componentaApasata;
                }
                //daca selecteaza 1 si 2, face mutarea
                if((mutare.from >= 0) && (mutare.to >= 0))
                {
                    eliminaEvidentiere(mutare.from);
                    try
                    {
                        motorulJocului.faMutare(mutare);
                    }
                    catch(Table.control.Exceptii.MutareInvalida info)
                    {
                        //Afiseaza un mesaj de exceptie
                        exceptionMessage.Text = info.Message;
                    }

                    //daca jucatorul nu mai are mutari valide
                    if (!motorulJocului.maiMulteMutari())
                    {
                        if (motorulJocului.obtineZaruri().Count >= 1)
                        {
                            exceptionMessage.Text = "Nu mai sunt mutari valide. Trec la" + motorulJocului.obtineNumeleJucatoruluiActiv();
                        }

                        btnZar.IsEnabled = true;
                    }
                    //daca jucatorul castiga
                    if (motorulJocului.avemCastigator())
                    {
                        startGrid.Visibility=Visibility.Visible;
                        castigatorText.Text = motorulJocului.returneazaCastigator() + " a castigat jocul";
                        exceptionMessage.Text = "";
                    }

                    //Reseteaza selectii
                    selectieGrid1.Effect = null;
                    printeazatabla();
                    mutare.from = -1;
                    mutare.to = -1;


                }

                afiseazaZaruri();
            }

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Table.control.Exceptii;
using Table.model.logic;
using Table.model.logic.campuri;

namespace Table.control
{
    class MotorulJocului
    {
        private TablaDeJoc tablaDeJoc;

        public MotorulJocului(TablaDeJoc tablaDeJoc)
        {
            this.tablaDeJoc = tablaDeJoc;
        }

        //start joc nou
        public void startJoc()
        {
            //jucatorul 1 incepe mereu
            tablaDeJoc.jucatorActiv = tablaDeJoc.jucator1;
            tablaDeJoc.aruncareZar.reseteazaAruncareZaruri();
        }

        public void initializareSituatieStart(int[] J1Pozitii, int[] J2Pozitii)
        {
            tablaDeJoc.initializareSituatieStart(J1Pozitii, J2Pozitii);
        }

        public int obtineNumarulPieselorInCamp(int index)
        {
            return tablaDeJoc.campuriDePeTabla[index].obtineNumarulPieselor();
        }

        public int obtineNumarulPieselorInCampDestinatieJ1()
        {
            return tablaDeJoc.campDestinatieJ1.obtineNumarulPieselor();
        }

        public int obtineNumarulPieselorInCampDestinatieJ2()
        {
            return tablaDeJoc.campDestinatieJ2.obtineNumarulPieselor();
        }

        public  int obtineNumarulPieselorEliminate()
        {
            return tablaDeJoc.campEliminat.obtineNumarulPieselor();
        }

        public string obtineCuloareaPieseiEliminate(int index)
        {
            return tablaDeJoc.campEliminat.obtinePiesaLa(index).jucator.culoare;
        }

        public string obtineCuloarea(int index)
        {
            return tablaDeJoc.campuriDePeTabla[index].obtineJucatorulInCamp().culoare;
        }

        public string obtineCuloareaDestinatieJ1()
        {
            return tablaDeJoc.jucator1.culoare;
        }

        public string obtineCuloareaDestinatieJ2()
        {
            return tablaDeJoc.jucator2.culoare;
        }

        public void aruncareZaruri()
        {
            tablaDeJoc.aruncareZar.reseteazaAruncareZaruri();
            tablaDeJoc.aruncareZar.aruncareZaruri();
            tablaDeJoc.mutaPiesa = new List<MutaPiesa>();
            tablaDeJoc.mutaPiesa = tablaDeJoc.reguli.verificaMutariPosibile(tablaDeJoc.jucatorActiv);
            verificaRandulJucatorului();
        }

        public List<int> obtineZaruri()
        {
            return tablaDeJoc.aruncareZar.obtineMutari();
        }

        public string obtineNumeleJucatoruluiActiv()
        {
            return tablaDeJoc.jucatorActiv.nume;
        }

        public string obtineNumeleJucatoruluiInactiv()
        {
            if (tablaDeJoc.jucator1.Equals(tablaDeJoc.jucatorActiv))
            {
                return tablaDeJoc.jucator2.nume;
            }
            else
            {
                return tablaDeJoc.jucator1.nume;
            }
        }

        public string obtineNumeleJucator1()
        {
            return tablaDeJoc.jucator1.nume;
        }
        public string obtineNumeleJucator2()
        {
            return tablaDeJoc.jucator2.nume;
        }

        public string culoareJucator1()
        {
            return tablaDeJoc.jucator1.culoare;
        }
        public string culoareJucator2()
        {
            return tablaDeJoc.jucator2.culoare;
        }

        public void faMutare(Mutare mutare)
        {
            CampBaza deLaCamp;
            CampBaza laCamp;
            /*
             0-23 -> triunghiuri 
             25 -> camp eliminare
             26 -> camp destinatie jucator 2
             27 -> camp destinatie jucator 1
            */
            //verifica daca piesa pleaca din campul de eliminare/campul destinatie/triunghi
            if (mutare.from == 25)
            {
                deLaCamp = tablaDeJoc.campEliminat;
            }
            else if (mutare.from == 26 || mutare.from == 27)
            {
                throw new MutareInvalida("Mutarea nu este permisa");
            }
            else
            {
                deLaCamp = tablaDeJoc.campuriDePeTabla[mutare.from];
            }

            //verifica daca piesa se muta in campul destinatie/campul de eliminare/triunghi
            if (mutare.to == 26)
            {
                laCamp = tablaDeJoc.campDestinatieJ2;
            }
            else if (mutare.to == 27)
            {
                laCamp = tablaDeJoc.campDestinatieJ1;
            }
            else if (mutare.to == 25)
            {
                throw new MutareInvalida("Mutarea nu este permisa");
            }
            else
            {
                laCamp = tablaDeJoc.campuriDePeTabla[mutare.to];
            }

            //muta piesa
            try
            {
                tablaDeJoc.mutarePiesa(tablaDeJoc.jucatorActiv, deLaCamp, laCamp, tablaDeJoc.mutaPiesa);
                tablaDeJoc.mutaPiesa = tablaDeJoc.reguli.verificaMutariPosibile(tablaDeJoc.jucatorActiv);
                verificaRandulJucatorului();
            }
            catch (MutareInvalida info)
            {
                throw new MutareInvalida(info.Message);
            }
        }

        //Schimba jucatorul activ daca nu exista mutari posibile
        public void verificaRandulJucatorului()
        {
            if (tablaDeJoc.jucatorActiv.Equals(tablaDeJoc.jucator1) && !maiMulteMutari())
            {
                tablaDeJoc.jucatorActiv = tablaDeJoc.jucator2;
            }
            else if(tablaDeJoc.jucatorActiv.Equals(tablaDeJoc.jucator2) && !maiMulteMutari())
            {
                tablaDeJoc.jucatorActiv = tablaDeJoc.jucator1;
            }
        }

        //verifica daca este vreun castigator
        public bool avemCastigator()
        {
            if(tablaDeJoc.campDestinatieJ1 != null && tablaDeJoc.campDestinatieJ2 != null)
            {
                if ((tablaDeJoc.campDestinatieJ1.obtineNumarulPieselor() >= 15) || (tablaDeJoc.campDestinatieJ2.obtineNumarulPieselor() >= 15))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        //returneaza numele castigatorului
        public string returneazaCastigator()
        {
            string castigator = "";
            if (tablaDeJoc.campDestinatieJ1.obtineNumarulPieselor() >= 15)
            {
                castigator = obtineNumeleJucator1();
            }
            else if(tablaDeJoc.campDestinatieJ2.obtineNumarulPieselor() >= 15)
            {
                castigator=obtineNumeleJucator2();
            }
            return castigator;
        }

        //Verifică daca mai sunt mutari posibile
        public bool maiMulteMutari()
        {
            if((tablaDeJoc.aruncareZar.obtineMutari().Count<=0) || (tablaDeJoc.mutaPiesa.Count <= 0))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Returneaza toate mutarile posibile dintr-o anumita pozitie
        public List<int> obtineMutariPosibileDeLaPozitie(int index)
        {
            List<int> list = new List<int>();
            foreach(MutaPiesa mutari in tablaDeJoc.mutaPiesa)
            {
                if (mutari.from.obtinePozitia() == index)
                {
                    list.Add(mutari.to.obtinePozitia());
                }
            }
            return list;
        }


    }
}

using System;
using System.Collections.Generic;
using Table.control;
using Table.control.Exceptii;
using Table.model.logic.campuri;

namespace Table.model.logic
{
    class TablaDeJoc
    {
        public Jucator jucator1 { get; set; }
        public Jucator jucator2 { get; set; }
        public CampDePeTabla[] campuriDePeTabla { get; }
        public AruncareZaruri aruncareZar { get; set; }
        public CampEliminat campEliminat { get; }
        public CampDestinatie campDestinatieJ1 { get; }
        public CampDestinatie campDestinatieJ2 { get; }
        public List<MutaPiesa> mutaPiesa { get; set; }
        public Jucator jucatorActiv { get; set; }
        public Piesa[] J1Piese { get; }
        public Piesa[] J2Piese { get; }
        public Reguli reguli { get; }

        public TablaDeJoc(CampDePeTabla[] campuriDePeTabla, CampEliminat campEliminat, CampDestinatie campDestinatieJ1, CampDestinatie campDestinatieJ2, Piesa[] j1Piese, Piesa[] j2Piese, AruncareZaruri aruncareZar, Reguli reguli,Jucator jucator1, Jucator jucator2)
        {
            this.jucator1 = jucator1;
            this.jucator2 = jucator2;
            this.campuriDePeTabla = campuriDePeTabla;
            this.aruncareZar = aruncareZar;
            this.campEliminat = campEliminat;
            this.campDestinatieJ1 = campDestinatieJ1;
            this.campDestinatieJ2 = campDestinatieJ2;
            this.J1Piese = j1Piese;
            this.J2Piese = j2Piese;
            this.reguli = reguli;
        }

        //Muta o piesa dintr un camp in altul
        public void mutarePiesa(Jucator jucatorActiv,CampBaza deLaCamp,CampBaza laCamp, List<MutaPiesa> mutaPiesa)
        {
            try
            {
                if (reguli.valideazaMutare(deLaCamp, laCamp, mutaPiesa)){
                    //daca oponentul are o piesa in camp
                    if(laCamp.obtineNumarulPieselor()==1 && !laCamp.obtineJucatorulInCamp().Equals(jucatorActiv))
                    {
                        //muta piesa oponentului in campul de eliminare
                        Piesa piesaoponent = laCamp.eliminaPiesa();
                        campEliminat.adaugaPiesa(piesaoponent);
                    }

                    //muta piesa jucatorului activ
                    Piesa piesa = null;
                    if (campEliminat.arePiesaDeLa(jucatorActiv))
                    {
                        piesa=campEliminat.eliminaPiesa(jucatorActiv);
                    }
                    else
                    {
                        piesa = deLaCamp.eliminaPiesa();
                    }
                    laCamp.adaugaPiesa(piesa);

                    //Elimina mutarea făcută din lista de mutari
                    foreach(MutaPiesa mutare in mutaPiesa)
                    {
                        if(mutare.from.Equals(deLaCamp) && mutare.to.Equals(laCamp))
                        {
                            aruncareZar.eliminaMutare(mutare.mutare);
                            break;
                        }
                    }
                }
            }
            catch(MutareInvalida e)
            {
                throw new MutareInvalida(e.Message);
            }

        }

        //Pune piesele in campurile de pe tabla
        public void initializareSituatieStart(int[] J1Pozitii, int[] J2Pozitii)
        {
            //reseteaza toate campurile
            for(int i = 0; i < 24; i++)
            {
                campuriDePeTabla[i].sterge();
            }
            campEliminat.sterge();
            campDestinatieJ1.sterge();
            campDestinatieJ2.sterge();

            //contoarele
            int j1Pozitie = 0;
            int j2Pozitie = 0;
            for(int i = 0; i < 24; i++)
            {
                //jucator1
                for(int j = 0; j < J1Pozitii[i]; j++)
                {
                    campuriDePeTabla[i].adaugaPiesa(J1Piese[j1Pozitie]);
                    j1Pozitie++;
                }
                //jucator2
                for (int j = 0; j < J2Pozitii[i]; j++)
                {
                    campuriDePeTabla[i].adaugaPiesa(J2Piese[j2Pozitie]);
                    j2Pozitie++;
                }
                //campul de eliminare
                for(int j = 0; j < J1Pozitii[25]; j++)
                {
                    campEliminat.adaugaPiesa(J1Piese[j1Pozitie]);
                    j1Pozitie++;
                }
                for (int j = 0; j < J2Pozitii[25]; j++)
                {
                    campEliminat.adaugaPiesa(J2Piese[j2Pozitie]);
                    j2Pozitie++;
                }
                //camp destinatie jucator1
                for (int j = 0; j < J1Pozitii[27]; j++)
                {
                    campDestinatieJ1.adaugaPiesa(J1Piese[j1Pozitie]);
                    j1Pozitie++;
                }
                //camp destinatie jucator2
                for (int j = 0; j < J2Pozitii[27]; j++)
                {
                    campDestinatieJ2.adaugaPiesa(J2Piese[j2Pozitie]);
                    j2Pozitie++;
                }
            }
        }

    }
}

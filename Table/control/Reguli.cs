using Table.model.logic;
using Table.model.logic.campuri;
using Table.control.Exceptii;
using Table.view.grafica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Table.control
{
    class Reguli
    {
        CampEliminat campEliminat;
        CampBaza campDestinatieJ1, campDestinatieJ2;
        CampDePeTabla[] campuriDePeTabla;
        AruncareZaruri aruncareZaruri;
        Jucator jucator1, jucator2;

        public Reguli(CampDePeTabla[] campuriDePeTabla, CampEliminat campEliminat, CampBaza campDestinatieJ1, CampBaza campDestinatieJ2, AruncareZaruri aruncareZaruri, Jucator jucator1, Jucator jucator2)
        {
            this.campEliminat = campEliminat;
            this.campDestinatieJ1 = campDestinatieJ1;
            this.campDestinatieJ2 = campDestinatieJ2;
            this.aruncareZaruri = aruncareZaruri;
            this.jucator1 = jucator1;
            this.jucator2 = jucator2;
            this.campuriDePeTabla = campuriDePeTabla;
        }

        //Validează mutarea față de lista de mutari valide
        public bool valideazaMutare(CampBaza deLaCamp, CampBaza laCamp, List<MutaPiesa> mutaPiesele)
        {
            bool returnValue = false;
            foreach (MutaPiesa mutare in mutaPiesele)
            {
                if (mutare.from.Equals(deLaCamp) && mutare.to.Equals(laCamp))
                {
                    returnValue = true;
                    break;
                }
            }
            if (!returnValue)
            {
                throw new MutareInvalida("Nu este o mutare valida");
            }
            return returnValue;
        }

        //Verifica daca o mutare este valida
        public bool esteMutareValida(CampBaza deLaCamp, CampBaza laCamp, Jucator jucatorActiv)
        {
            //La mutarea din câmpul de eliminat
            if (deLaCamp.Equals(campEliminat))
            {
                if (!okPentruMutareLaCamp(laCamp, jucatorActiv))
                {
                    //intra exceptia de mutare invalida;nu se poate muta piesa in camp
                    return false;
                }
                else
                {
                    //În funcție de direcția de mutare
                    int d;
                    if (jucatorActiv.Equals(jucator1))
                    {
                        d = -1;
                    }
                    else
                    {
                        d = 24;
                    }
                    int to = laCamp.obtinePozitia();
                    if (!mutareaCorespundeZaruri(aruncareZaruri, d, to))
                    {
                        return false;
                    }
                    return true;
                }
            }

            //Când jucătorul 1 încearcă să se deplaseze pe campul destinatie
            if (laCamp.Equals(campDestinatieJ1) && jucatorActiv.Equals(jucator1))
            {
                //Dacă deLaCamp nu conține piese de la jucătorul activ
                if (!okPentruMutareDeLaCamp(deLaCamp, jucatorActiv))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //Când jucătorul 2 încearcă să se deplaseze pe campul destinatie
            if (laCamp.Equals(campDestinatieJ2) && jucatorActiv.Equals(jucator2))
            {
                //Dacă deLaCamp nu conține piese de la jucătorul activ
                if (!okPentruMutareDeLaCamp(deLaCamp, jucatorActiv))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            //Daca mutarea nu corespunde cu nici un zar
            if (!mutareaCorespundeZaruri(aruncareZaruri, deLaCamp.obtinePozitia(), laCamp.obtinePozitia()))
            {
                return false;
            }

            //daca nu e ok sa muti de la deLaCamp
            if (!okPentruMutareDeLaCamp(deLaCamp, jucatorActiv))
            {
                return false;
            }

            //daca nu e ok sa muti la laCamp
            if (!okPentruMutareLaCamp(laCamp, jucatorActiv))
            {
                return false;
            }

            //Mutarea în direcția corectă jucătorul1
            if (jucatorActiv.Equals(jucator1) &&
                !deLaCamp.Equals(campEliminat) &&
                !laCamp.Equals(campDestinatieJ1) &&
                deLaCamp.obtinePozitia() >= laCamp.obtinePozitia())
            {
                return false;
            }

            //Mutarea în direcția corectă jucătorul2
            if (jucatorActiv.Equals(jucator2) &&
                !deLaCamp.Equals(campEliminat) &&
                !laCamp.Equals(campDestinatieJ2) &&
                deLaCamp.obtinePozitia() <= laCamp.obtinePozitia())
            {
                return false;
            }

            //Dacă jucătorul are piese în câmpul de eliminare, dar încearcă să mute o altă piesa
            if (campEliminat.arePiesaDeLa(jucatorActiv) && !deLaCamp.Equals(campEliminat))
            {
                return false;
            }

            return true;
        }

        //Verifică dacă câmpul țintă este valid
        public bool okPentruMutareLaCamp(CampBaza camp, Jucator jucatorActiv)
        {
            //Dacă există mai multe piese în câmp
            if (camp.obtineNumarulPieselor() > 1)
            {
                //daca piesa nu e a jucatorului activ
                if (!camp.obtineJucatorulInCamp().Equals(jucatorActiv))
                {
                    return false;
                }
            }
            return true;
        }

        //verifica daca campul sursa este valid
        public bool okPentruMutareDeLaCamp(CampBaza camp, Jucator jucatorActiv)
        {
            //Trebuie să existe cel puțin o piesă care să fie mutată și trebuie să fie deținută de jucătorul activ
            if (camp.obtineNumarulPieselor() > 0 && camp.obtineJucatorulInCamp().Equals(jucatorActiv))
            {
                return true;
            }
            return false;

        }

        //Verifica daca mutarea corespunde cu vreo fata a zarului
        public bool mutareaCorespundeZaruri(AruncareZaruri aruncareZaruri, int from, int to)
        {
            int distantaMutare = 0;
            distantaMutare = to - from;
            //daca distantaMutare<0 inverseaza
            if (distantaMutare < 0)
            {
                distantaMutare = -distantaMutare;
            }

            //daca numarul de pasi nu coespunde cu zarul
            if (!aruncareZaruri.obtineMutari().Contains<int>(distantaMutare))
            {
                return false;
            }
            return true;
        }

        //Verifică dacă toate piesele jucătorului sunt in casă
        public bool toatePieseleInCasa(Jucator jucator,List<CampBaza> campuriDeVerificat)
        {
            int numarPiese = 0;
            foreach(CampBaza camp in campuriDeVerificat)
            {
                //daca piesele din camp sunt detinute de jucator
                if ((camp.obtineNumarulPieselor() > 0) && (camp.obtineJucatorulInCamp().Equals(jucator)))
                {
                    numarPiese = numarPiese + camp.obtineNumarulPieselor();
                }
            }
            //daca toate piesele sunt in casa si se pot scoate
            if(numarPiese >= 15)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //verifica toate mutarile posibile
        public List<MutaPiesa> verificaMutariPosibile(Jucator jucator)
        {
            Jucator jucatorActiv = jucator;
            List<MutaPiesa> mutariPosibile =new List<MutaPiesa>();
            foreach (int mutare in aruncareZaruri.obtineMutari())
            {
                //daca jucatorul are piesa eliminata
                if (campEliminat.arePiesaDeLa(jucatorActiv))
                {
                    //Setare pt jucătorul 1
                    int to = mutare - 1;
                    //pt jucatorl 2
                    if (jucatorActiv.Equals(jucator2))
                    {
                        to = 24 - mutare;
                    }
                    try
                    {
                        if (esteMutareValida(campEliminat, campuriDePeTabla[to], jucatorActiv))
                        {
                            mutariPosibile.Add(new MutaPiesa(campEliminat, campuriDePeTabla[to], mutare));
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    //alege directia in care se muta in functie de jucator
                    //jucator 1
                    if (jucatorActiv.Equals(jucator1))
                    {
                        //repeta pt fiecare camp de pe tabla sa verifice mutarile posibile
                        for (int i=0; i < 24; i++){
                            int to = i + mutare;
                            if (to <= 23)
                            {
                                try
                                {
                                    //verifica daca mutarea e valida
                                    if (esteMutareValida(campuriDePeTabla[i], campuriDePeTabla[to], jucatorActiv))
                                    {
                                        mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[i], campuriDePeTabla[to], mutare));
                                    }
                                }
                                catch
                                {
                                    // Nu face nimic pt ca aceasta creeaza doar o listă cu posibile mutari
                                }
                            }
                        }

                        //verifica daca se poate muta in campul destinatie
                        //creaza o lista cu campurile de verificat
                        List<CampBaza> campuriDeVerificat = new List<CampBaza>();
                        for(int i = 18; i < 24; i++)
                        {
                            campuriDeVerificat.Add(campuriDePeTabla[i]);
                        }
                        campuriDeVerificat.Add(campDestinatieJ1);
                        if (toatePieseleInCasa(jucatorActiv, campuriDeVerificat))
                        {
                            for(int i = 18; i < 24; i++)
                            {
                                //Daca campul care corespunde cu mutarea are o piesa detinuta de jucatorul activ
                                if ((campuriDePeTabla[24 - mutare].obtineNumarulPieselor() > 0) && (campuriDePeTabla[24 - mutare].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //posibilitate de a muta piesa in campul destinatie
                                    mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[24 - mutare], campDestinatieJ1, mutare));
                                    break;
                                }
                                //Dacă exista vreo piesa in camp mai mare decât valoarea mutarii.Nu se poate trece la campul destinatie
                                if (((24 - campuriDePeTabla[i].obtinePozitia()) > mutare) &&
                                    (campuriDePeTabla[i].obtineNumarulPieselor()>0)&&
                                    (campuriDePeTabla[i].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //Nu este posibil sa folosesti aceasta mutare pentru a pune piesa in campul destinatie
                                    break;
                                }
                                //Dacă piesa de pe camp este egala sau mai mica decat valoarea mutarii. Posibil sa treci la campul destinatie
                                if (((24 - campuriDePeTabla[i].obtinePozitia()) <= mutare) && (campuriDePeTabla[i].obtineNumarulPieselor() > 0) && (campuriDePeTabla[i].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //posibil sa muti piesa in campl destinatie
                                    mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[i], campDestinatieJ1, mutare));
                                    break;
                                }
                            }
                        }

                    }
                    //jucator 2
                    else
                    {
                        for (int i = 23; i >= 0; i--)
                        {
                            int to = i - mutare;
                            if (to >= 0)
                            {
                                try
                                {
                                    if (esteMutareValida(campuriDePeTabla[i], campuriDePeTabla[to], jucatorActiv))
                                    {
                                        mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[i], campuriDePeTabla[to], mutare));
                                    }
                                }
                                catch
                                {
                                    // Nu face nimic pt ca aceasta creeaza doar o listă cu posibile mutari
                                }
                            }
                        }

                        //verifica daca este posibi sa muti la campul destinatie
                        //creaza o lista cu campuri de verificat
                        List<CampBaza> campuriDeVerificat = new List<CampBaza>();
                        for (int i = 5; i >= 0; i--)
                        {
                            campuriDeVerificat.Add(campuriDePeTabla[i]);
                        }
                        campuriDeVerificat.Add(campDestinatieJ2);
                        if (toatePieseleInCasa(jucatorActiv, campuriDeVerificat))
                        {
                            for (int i = 5; i >=0; i--)
                            {
                                //Daca campul care corespunde cu mutarea are o piesa detinuta de jucatorul activ
                                if ((campuriDePeTabla[mutare-1].obtineNumarulPieselor() > 0) && (campuriDePeTabla[mutare - 1].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //posibilitate de a muta piesa in campul destinatie
                                    mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[mutare - 1], campDestinatieJ2, mutare));
                                    break;
                                }
                                //Dacă exista vreo piesa in camp mai mare decât valoarea mutarii.Nu se poate trece la campul destinatie
                                if (((campuriDePeTabla[i].obtinePozitia()) > (mutare-1)) &&
                                    (campuriDePeTabla[i].obtineNumarulPieselor() > 0) &&
                                    (campuriDePeTabla[i].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //Nu este posibil sa folosesti aceasta mutare pentru a pune piesa in campul destinatie
                                    break;
                                }
                                //Dacă piesa de pe camp este egala sau mai mica decat valoarea mutarii. Posibil sa treci la campul destinatie
                                if (((campuriDePeTabla[i].obtinePozitia()) <= (mutare-1)) && (campuriDePeTabla[i].obtineNumarulPieselor() > 0) && (campuriDePeTabla[i].obtineJucatorulInCamp().Equals(jucatorActiv)))
                                {
                                    //posibil sa muti piesa in campl destinatie
                                    mutariPosibile.Add(new MutaPiesa(campuriDePeTabla[i], campDestinatieJ2, mutare));
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            return mutariPosibile;
        }

    }
}


using System.Collections.Generic;
using System.Linq;

namespace Table.model.logic.campuri
{
    class CampBaza
    {
        protected LinkedList<Piesa> piese = null;
        protected int pozitie;

        public CampBaza(LinkedList<Piesa> piese, int pozitie)
        {
            this.piese = piese;
            this.pozitie = pozitie;
        }

        public CampBaza() { }

        public int obtinePozitia()
        {
            return this.pozitie;
        }

        //Returneaza numarul de piese din camp
        public int obtineNumarulPieselor()
        {
            return piese.Count;
        }


        //Returneaza jucatorul care detine prima piesa in lista
        public Jucator obtineJucatorulInCamp()
        {
            return piese.First.Value.jucator;
        }

        //Elimina o piesa din camp si o returneaza
        public Piesa eliminaPiesa()
        {
            Piesa primaPiesa = piese.First();
            piese.RemoveFirst();
            return primaPiesa;
        }

        //Adauga o piesa in lista
        public void adaugaPiesa(Piesa piesa)
        {
            piese.AddFirst(piesa);
        }

        //Returneaza o piesa specifica din lista;folosita pt a imprima tabla
        public Piesa obtinePiesaLa(int p)
        {
            return piese.ElementAt(p);
        }

        public void sterge()
        {
            this.piese.Clear();
        }

    }
}


using System;
using System.Collections.Generic;
using Table.model.logic.campuri;
using Table.model.logic;

namespace Table.model.logic.campuri
{
    class CampEliminat : CampBaza
    {
        public CampEliminat()
        {
            this.piese = new LinkedList<Piesa>();
            this.pozitie = 25;
        }

        //Verifică dacă jucătorul are vreo piesa în câmpul de eliminare
        public bool arePiesaDeLa(Jucator jucator)
        {
            foreach (Piesa piesa in piese)
            {
                if (piesa.jucator.Equals(jucator))
                {
                    return true;
                }
            }
            return false;
        }

        public Piesa eliminaPiesa(Jucator jucator)
        {
            Piesa piesaTemp = null;
            foreach (Piesa piesa in piese)
            {
                if (piesa.jucator.Equals(jucator))
                {
                    piesaTemp = piesa;
                    piese.Remove(piesa);
                    break;
                }
            }
            return piesaTemp;
        }
    }
}





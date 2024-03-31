using System.Collections.Generic;

namespace Table.model.logic.campuri
{
    class CampDestinatie : CampBaza
    {
        private Jucator jucator;

        public CampDestinatie(Jucator jucator, int poz)
        {
            this.jucator = jucator;
            this.piese = new LinkedList<Piesa>();
            this.pozitie = poz;
        }

    }
}



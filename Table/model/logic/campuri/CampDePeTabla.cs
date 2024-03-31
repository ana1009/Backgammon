using System.Collections.Generic;

namespace Table.model.logic.campuri
{
    class CampDePeTabla : CampBaza
    {
        private int pozitie;

        public CampDePeTabla(LinkedList<Piesa> piese, int pozitie)
            : base(piese, pozitie) { }
    }
}



using Table.model.logic.campuri;
using System;

namespace Table.model.logic
{
    class MutaPiesa : CampBaza
    {
        public CampBaza from { get; }
        public CampBaza to { get; }
        public int mutare { get; }

        public MutaPiesa(CampBaza from, CampBaza to, int mutare)
        {
            this.from = from;
            this.to = to;
            this.mutare = mutare;
        }
    }
}


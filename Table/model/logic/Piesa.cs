using System;

namespace Table.model.logic
{

    class Piesa
    {
        public int piesaId;
        public Jucator jucator { get; }

        public Piesa(int piesaId, Jucator jucator)
        {
            this.piesaId = piesaId;
            this.jucator = jucator;
        }

    }
}

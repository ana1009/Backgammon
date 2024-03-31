using System;

namespace Table.model.logic
{
    class Jucator
    {
        public int id { get; }
        public string nume { get; set; }
        public string culoare { get; set; }

        public Jucator() { }

        public Jucator(int id, string nume, string culoare)
        {
            this.id = id;
            this.nume = nume;
            this.culoare = culoare;
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using Table.model.logic;

namespace Table.model.logic
{
    class AruncareZaruri
    {
        private Zar z1;
        private Zar z2;
        private List<int> mutari = new List<int>();


        public AruncareZaruri(Zar z1, Zar z2)
        {
            this.z1 = z1;
            this.z2 = z2;
        }
       
        //Aruncarea Zarurilor
        public void aruncareZaruri()
        {
            mutari.Clear();
            Random random = new Random();
            z1.valoareZar = random.Next(1, 7);
            z2.valoareZar = random.Next(1, 7);

            if (z1.valoareZar == z2.valoareZar)
            {
                for (int i = 0; i < 4; i++)
                {
                    mutari.Add(z1.valoareZar);
                }
            }
            else
            {
                mutari.Add(z1.valoareZar);
                mutari.Add(z2.valoareZar);
            }
        }

        //Returnează o listă de mutari
        public List<int> obtineMutari()
        {
            return mutari;
        }

        //Șterge o mutare din lista de mutari posibile
        public void eliminaMutare(int mutare)
        {
            for (int i = 0; i < mutari.Count(); i++)
            {
                if (mutari[i] == mutare)
                {
                    mutari.RemoveAt(i);
                    break;
                }
            }
        }

        //Resetează AruncareaZarurilor
        public void reseteazaAruncareZaruri()
        {
            mutari.Clear();
        }

    }
}

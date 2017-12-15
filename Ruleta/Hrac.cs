using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruleta
{
    class Hrac
    {
        public int zostavajuciKredit;
        private ArrayList historiaTahov = new ArrayList();
        private int cisloTahu = 1;

        public Hrac(int zostavajuciKredit=1000)
        {
            this.zostavajuciKredit = zostavajuciKredit;
        }

        public void zapisTah(int vyskaStavky, string prelozenyVysledokSpinu, string nazovStavky, int vyhra, string vasaStavka)
        {
            //Console.WriteLine("Zapisujem Ťah {0}",cisloTahu);
            historiaTahov.Add(new Tah(cisloTahu, vyskaStavky, prelozenyVysledokSpinu, nazovStavky, vyhra, vasaStavka));
            cisloTahu++;
        }

        public void VypisHistoriu()
        {
            foreach (Tah jedenTah in historiaTahov)
            {
                Console.WriteLine("------------ Číslo ťahu {0} -------------", jedenTah.cisloTahu);
                Console.WriteLine("Výška stávky = {0}", jedenTah.vyskaStavky);
                Console.WriteLine("Názov stávky = {0}", jedenTah.nazovStavky);
                Console.WriteLine("Číslo stávky = {0}", jedenTah.vasaStavka);
                Console.WriteLine("Výsledok spinu = {0}", jedenTah.prelozenyVysledokSpinu);
                Console.WriteLine("Výhra        = {0}", jedenTah.vyhra);
            }
        }

    }
}

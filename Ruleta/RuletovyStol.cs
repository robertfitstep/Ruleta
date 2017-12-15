using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruleta
{
    class RuletovyStol
    {
        string nespravnyPrikaz = "Zadali ste nesprávny príkaz.";
        string zadTypStavky = "Zadajte typ stávky.\n1 = na číslo\n2 = na farbu\n3 = Párna/Nepárna\n4 = Stávka na radu\nReplay = zobraziť historiu\nKoniec = ukončiť hru";
        string zadVasuStavku = "Zadajte vašu stávku";
        string zadVyskuStavky = "Zadajte výšku stávky v EUR";
        string msg_prehra = "Prehrali ste";
        string msg_vyhra = "Vyhrali ste";
        string msg_nespravna_stavka = "Zadali ste nesprávnu stávku";
        string msg_prehra_kreditu = "Prehrali ste všetok kredit.\n\nZačína sa nová hra.";
        string msg_zostavajuci_kredit = "Váš zostávajúci kredit je";

        Random generatorCisiel = new Random();

        Hrac Hrac1 = new Hrac(1000);

        readonly public int[] ruletoveCisla = new int[37] {

            //nakreslený stôl rulety 

            //farba čísla                  čísla na stole

                    3,                  //        0
             1,     0,      1,          //  1     2       3
             0,     1,      0,          //  4     5       6
             1,     0,      1,          //  7     8       9
             0,     0,      1,          //  10    11      12
             0,     1,      0,          //  13    14      15
             1,     0,      1,          //  16    17      18
             1,     0,      1,          //  19    20      21
             0,     1,      0,          //  22    23      24     
             1,     0,      1,          //  25    26      27
             0,     0,      1,          //  28    29      30
             0,     1,      0,          //  31    32      33
             1,     0,      1           //  34    35      36

        };

        readonly string[] farby = new string[2] { "čierna", "červená" };

        

        readonly Dictionary<int, string> nazovStavky = new Dictionary<int, string>()
        {
            {1,"Stávka na číslo" },
            {2, "Stávka na farbu" },
            {3, "Stávka na párnu/nepárnu" },
            {4, "Stávka na radu" }
        };

        public RuletovyStol()
        {
            ZadajPrikaz();
        }

        public void ZadajPrikaz()
        {
            //rieši vstup od užívateľa

            string typStavky;
            Console.WriteLine(zadTypStavky);
            typStavky = Console.ReadLine();

            switch (typStavky)
            {
                case "1":  //číslo stávky                    
                case "2":
                case "3":
                case "4":
                    sprocesujStavku(typStavky);
                    break;
                case "Replay":
                    Hrac1.VypisHistoriu();
                    ZadajPrikaz();
                    break;
                case "Koniec":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine(nespravnyPrikaz);
                    ZadajPrikaz();
                    break;
            }
        }



        private int[] Spin()
        {
            int randomCislo = generatorCisiel.Next(37);
            return new int[2] { randomCislo, ruletoveCisla[randomCislo] };
        }

        private void VypisVysledok(int[] vysledok)
        {
            Console.WriteLine("Padla : {0}", PrelozVysledok(vysledok));
        }

        private string PrelozVysledok(int[] vysledok)
        {
            //vráti výsledok v čitateľnom tvare "číslo (farba)"

            string farba;

            if (vysledok[1] == 0)
            {
                farba = farby[0]; //čierna
            }
            else if (vysledok[1] == 1)
            {
                farba = farby[1]; //červená
            }
            else
            {
                farba = "N/A";
            }

            return String.Format("{0} ({1})", vysledok[0], farba);
        }

        private int VratFarbu(string vasaStavka)
        {
            if(vasaStavka == farby[0])
            {
                return 0;
            }
            else if (vasaStavka == farby[1])
            {
                return 1;
            }
            else
            {
                return 3;
            }
        }

        private int VyhodnotVysledok(int vyskaStavky, int[] vysledokSpinu, string vasaStavka, int typStavky)
        {
            //vracia výšku výhry (+) alebo výšku prehry (-)

            switch (typStavky)
            {
                case 1:
                    if (Convert.ToString(vysledokSpinu[0]) == vasaStavka)
                    {
                        return 35 * vyskaStavky;
                    }
                    else
                    {
                        return -vyskaStavky;
                    }
                case 2:
                    if (vysledokSpinu[1] == VratFarbu(vasaStavka))
                    {
                        return vyskaStavky;
                    }
                    else
                    {
                        return -vyskaStavky;
                    }
                case 3:
                    if (vasaStavka == "Párna" && vysledokSpinu[0] % 2 == 0)
                    {
                        return vyskaStavky;
                    }
                    else if (vasaStavka == "Nepárna" && vysledokSpinu[0] % 2 != 0)
                    {
                        return vyskaStavky;
                    }
                    else
                    {
                        return -vyskaStavky;
                    }
                case 4: //stávka na radu
                    if (vasaStavka == "1" && (vysledokSpinu[0] - Int32.Parse(vasaStavka)) % 3 == 0)
                    {
                        return vyskaStavky * 2;
                    }
                    else if (vasaStavka == "2" && (vysledokSpinu[0] - Int32.Parse(vasaStavka)) % 3 == 0)
                    {
                        return vyskaStavky * 2;
                    }
                    else if (vasaStavka == "3" && (vysledokSpinu[0] - Int32.Parse(vasaStavka)) % 3 == 0)
                    {
                        return vyskaStavky * 2;
                    }
                    else
                    {
                        return -vyskaStavky * 2;
                    }
                default:
                    return 0;
            }

        }

        private bool ValidujStavku(string vasaStavka, string typStavky)
        {
            // validácia stávky (správne číslo, farba...), v rámci konkrétneho typu stávky

            switch (typStavky) {
                case "1": //stávka na číslo
                    int parsovanaStavka;
                    if (Int32.TryParse(vasaStavka, out parsovanaStavka) && parsovanaStavka >= 0 && parsovanaStavka <= 36)
                    {
                        return true;
                    }
                        else
                    {
                        return false;
                    }
                case "2":
                    if (vasaStavka == farby[0] || vasaStavka == farby[1])
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "3":
                    if (vasaStavka == "Párna" || vasaStavka == "Nepárna")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "4": //stávka na radu
                    if (vasaStavka == "1" || vasaStavka == "2" || vasaStavka == "3")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                default:
                    return false;
        }
    }

        void sprocesujStavku(string typStavky)
        {
            int[] vysledokSpinu;
            string vasaStavka;
            int vyskaStavky;
            int vyhra;

            Console.WriteLine(zadVasuStavku);
            vasaStavka = Console.ReadLine();
            Console.WriteLine(zadVyskuStavky);
            vyskaStavky = Int32.Parse(Console.ReadLine());
            if (ValidujStavku(vasaStavka, typStavky))
            {
                vysledokSpinu = Spin();
                VypisVysledok(vysledokSpinu);
                vyhra = VyhodnotVysledok(vyskaStavky, vysledokSpinu, vasaStavka, Int32.Parse(typStavky));
                if (vyhra > 0)
                {
                    Console.WriteLine("{0} {1}EUR", msg_vyhra, vyhra);
                    Hrac1.zostavajuciKredit = Hrac1.zostavajuciKredit + vyhra;
                    Hrac1.zapisTah(vyskaStavky, PrelozVysledok(vysledokSpinu), nazovStavky[Int32.Parse(typStavky)],vyhra,vasaStavka);
                }
                else
                {
                    Console.WriteLine("{0} {1}EUR", msg_prehra, -vyhra);
                    if (Hrac1.zostavajuciKredit + vyhra <= 0)
                    {
                        Console.WriteLine(msg_prehra_kreditu);
                        Hrac1 = new Hrac(1000);
                        ZadajPrikaz();
                    }
                    else
                    {
                        Hrac1.zostavajuciKredit = Hrac1.zostavajuciKredit + vyhra;
                        Hrac1.zapisTah(vyskaStavky, PrelozVysledok(vysledokSpinu), nazovStavky[Int32.Parse(typStavky)], vyhra, vasaStavka);
                    }
                    
                }
                Console.WriteLine("{0} {1}EUR", msg_zostavajuci_kredit, Hrac1.zostavajuciKredit);
                ZadajPrikaz();
            }
            else
            {
                Console.WriteLine(msg_nespravna_stavka);
                ZadajPrikaz();
            }
        }
    }
}

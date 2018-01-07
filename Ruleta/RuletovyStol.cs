using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruleta
{
    class RuletovyStol
    {
        private Policko[] ruletovyStol;
        private Hrac Hrac1;
        private Random generatorCisiel = new Random();
        private int vysledokSpinu;
        private string zadanaStavkaUzivatelom;
        private int vyskaStavky;
        private int vyhra;
        private Farba[] rozmiestnenieFarieb = new Farba[37]
            {
                                Farba.Zelená,                   //      0
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  1   2   3
                Farba.Čierna,   Farba.červená,  Farba.Čierna,   //  4   5   6 
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  7   8   9 
                Farba.Čierna,   Farba.Čierna,   Farba.červená,  //  10  11  12
                Farba.Čierna,   Farba.červená,  Farba.Čierna,   //  13  14  15
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  16  17  18
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  19  20  21
                Farba.Čierna,   Farba.červená,  Farba.Čierna,   //  22  23  24
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  25  26  27
                Farba.Čierna,   Farba.Čierna,   Farba.červená,  //  28  29  30
                Farba.Čierna,   Farba.červená,  Farba.Čierna,   //  31  32  33
                Farba.červená,  Farba.Čierna,   Farba.červená,  //  34  35  36
            };

        public RuletovyStol()
        {
            this.ruletovyStol = vratRuletovyStol();
            this.Hrac1 = new Hrac(1000);
            precitajPrikazUzivatela();
        }

        private void precitajPrikazUzivatela()
        {
            Console.WriteLine(Messages.zadPrikaz);
            validujAVykonajPrikaz(Console.ReadLine());
            
        }

        private void validujAVykonajPrikaz(string prikazUzivatela)
        {
            if (prikazUzivatela == Prikazy.Farba.ToString() || prikazUzivatela == Prikazy.Číslo.ToString() ||
                prikazUzivatela == Prikazy.Párnosť.ToString() || prikazUzivatela == Prikazy.Rada.ToString() ||
                prikazUzivatela == Prikazy.Rozsah.ToString())
            {
                precitajStavku(prikazUzivatela);
            }
            else if (prikazUzivatela == Prikazy.Replay.ToString())
            {
                Hrac1.VypisHistoriu();
                precitajPrikazUzivatela();
            }
            else if (prikazUzivatela == Prikazy.Koniec.ToString()) Environment.Exit(0);
            else if (prikazUzivatela == Prikazy.Pomoc.ToString()) vypisOznamAPresmeruj(vratZoznamPrikazov(),precitajPrikazUzivatela);
            else vypisOznamAPresmeruj(Messages.nespravnyPrikaz, precitajPrikazUzivatela);   
        }

        private void precitajStavku(string typStavky)
        {
            Console.WriteLine(Messages.zadVasuStavku);
            zadanaStavkaUzivatelom = ValidujStavku(Console.ReadLine(), typStavky);
            Console.WriteLine(Messages.zadVyskuStavky);
            vyskaStavky = validujVyskuStavky(Console.ReadLine(), typStavky);
            roztocRuletuASprocesujStavku(typStavky);
        }

        private string ValidujStavku(string zadanaStavkaUzivatelom, string typStavky)
        {
            // validácia stávky (správne číslo, farba...), v rámci konkrétneho typu stávky
            int parsovanaStavka;

            if (zadanaStavkaUzivatelom == Prikazy.Pomoc.ToString())
            {
                vypisOznamAPresmeruj(vratVolbyPodlaTypuStavky(typStavky), precitajStavku, typStavky);
                return zadanaStavkaUzivatelom;
            }
            else if (typStavky == Prikazy.Číslo.ToString() && Int32.TryParse(zadanaStavkaUzivatelom, out parsovanaStavka) && parsovanaStavka >= 0 && parsovanaStavka <= 36)
                return zadanaStavkaUzivatelom;

            else if (typStavky == Prikazy.Farba.ToString() && nachadzaSaVoFarbach(zadanaStavkaUzivatelom)) 
                return zadanaStavkaUzivatelom;

            else if (typStavky == Prikazy.Párnosť.ToString() && nachadzaSaVParnostiach(zadanaStavkaUzivatelom))
                return zadanaStavkaUzivatelom;

            else if (typStavky == Prikazy.Rada.ToString() && nachadzaSaVRadach(zadanaStavkaUzivatelom)) return zadanaStavkaUzivatelom;
            else if (typStavky == Prikazy.Rozsah.ToString() &&nachadzaSaVRozsahu(zadanaStavkaUzivatelom)) return zadanaStavkaUzivatelom;
            else
            {
                vypisOznamAPresmeruj(Messages.msg_nespravna_stavka, precitajStavku, typStavky);
                return zadanaStavkaUzivatelom;
            }
        }

        private int validujVyskuStavky(string zadanaVyskaStavkyUzivatelom, string typStavky)
        {
            int vyskaStavky;
            bool stavkaJeCislo = Int32.TryParse(zadanaVyskaStavkyUzivatelom, out vyskaStavky);

            if ( stavkaJeCislo && Hrac1.zostavajuciKredit >= vyskaStavky && vyskaStavky >= 0 )
            {
                return vyskaStavky;
            }
            else if (stavkaJeCislo && Hrac1.zostavajuciKredit < vyskaStavky && vyskaStavky >= 0)
            {
                vypisOznamAPresmeruj(Messages.msg_nedostatok_kreditu, precitajStavku, typStavky);
                return 0;
            }
            else if (stavkaJeCislo && 0 >= vyskaStavky)
            {
                vypisOznamAPresmeruj(Messages.msg_nul_alebo_minus_stavka, precitajStavku, typStavky);
                return 0;
            }
            else
            {
                vypisOznamAPresmeruj(Messages.msg_nepravny_format_kreditu, precitajStavku, typStavky);
                return 0;
            }
        }

        private void roztocRuletuASprocesujStavku(string typStavky)
        {

            vysledokSpinu = Spin();
            VypisVysledok(vysledokSpinu);
            vyhra = vratVyskuVyhry(vyskaStavky, vysledokSpinu, zadanaStavkaUzivatelom, typStavky);
            zapisVyhru(typStavky);
            vypisZostavajuciKredit();
            precitajPrikazUzivatela();
        }

        private Párnosť vratParnost(int i)
        {
            if (i == 0) return Párnosť.Žiadna;
            else if (i % 2 == 0) return Párnosť.Párna;
            return Párnosť.Nepárna;
        }

        private Rada vratRadu(int i)
        {
            if (i == 0) return Rada.Žiadna;
            if (i == 1 || (i - 1) % 3 == 0) return Rada.Prvá;
            else if (i == 2 || (i - 2) % 3 == 0) return Rada.Druhá;
            return Rada.Tretia;
        }

        private bool nachadzaSaCisloVRozsahu(int i, string stavkaUzivatela)
        {
            if (stavkaUzivatela == Rozsah.R13_24.ToString() && i >= 13 && i <= 24) return true;
            else if (stavkaUzivatela == Rozsah.R19_36.ToString() && i >= 19 && i <= 36) return true;
            else if (stavkaUzivatela == Rozsah.R1_12.ToString() && i >= 1 && i <= 12) return true;
            else if (stavkaUzivatela == Rozsah.R1_18.ToString() && i >= 1 && i <= 18) return true;
            else if (stavkaUzivatela == Rozsah.R25_36.ToString() && i >= 25 && i <= 36) return true;
            else if (stavkaUzivatela == Rozsah.Žiadny.ToString() && i == 0) return true;
            else return false;
        }

        private Policko[] vratRuletovyStol()
        {
            Policko[] ruletovyStol = new Policko[37];

            for (int i = 0; i < ruletovyStol.Length; i++)
            {
                ruletovyStol[i].Farba = rozmiestnenieFarieb[i];
                ruletovyStol[i].Cislo = i;
                ruletovyStol[i].Parnost = vratParnost(i);
                ruletovyStol[i].Rada = vratRadu(i);
            }
            return ruletovyStol;
        }

        private int Spin()
        {
            return generatorCisiel.Next(37);
        }

        private string vratCitatelnyVysledok(int vysledok)
        {
            //vráti výsledok v čitateľnom tvare "číslo (farba)"
            return String.Format("{0} ({1})", vysledok, rozmiestnenieFarieb[vysledok]);
        }

        private int vratVyskuVyhry(int vyskaStavky, int vysledokSpinu, string vasaStavka, string typStavky)
        {
            //vracia výšku výhry (+) alebo výšku prehry (-)
            if (typStavky == Prikazy.Číslo.ToString() && Convert.ToString(vysledokSpinu) == vasaStavka) return 35 * vyskaStavky;
            else if (typStavky == Prikazy.Farba.ToString() && rozmiestnenieFarieb[vysledokSpinu].ToString() == vasaStavka) return vyskaStavky;
            else if (typStavky == Prikazy.Párnosť.ToString() && vratParnost(vysledokSpinu).ToString() == vasaStavka) return vyskaStavky;
            else if (typStavky == Prikazy.Rada.ToString() && vasaStavka == vratRadu(vysledokSpinu).ToString()) return vyskaStavky * 2;
            else if (typStavky == Prikazy.Rozsah.ToString() && nachadzaSaCisloVRozsahu(vysledokSpinu, vasaStavka)) return vyskaStavky * 2;
            else return -vyskaStavky;

        }

        private void VypisVysledok(int vysledok)
        {
            Console.WriteLine("Padla : {0} ({1})", vysledok, rozmiestnenieFarieb[vysledok]);
        }

        private void vypisOznamAPresmeruj(string message, Action<string> redirect, string argument)
        {
            Console.WriteLine(message);
            redirect(argument);
        }

        private void vypisOznamAPresmeruj(string message, Action redirect)
        {
            Console.WriteLine(message);
            redirect();
        }

        private void zapisVyhru(string typStavky)
        {
            if (vyhra > 0)
            {
                Console.WriteLine("{0} {1}EUR", Messages.msg_vyhra, vyhra);
                zapisHracovTah(typStavky);
            }
            else
            {
                Console.WriteLine("{0} {1}EUR", Messages.msg_prehra, -vyhra);
                if (Hrac1.zostavajuciKredit + vyhra <= 0)
                {
                    Console.WriteLine(Messages.msg_prehra_kreditu);
                    Hrac1 = new Hrac(1000);
                    precitajPrikazUzivatela();
                }
                else zapisHracovTah(typStavky);

            }
        }

        private void vypisZostavajuciKredit()
        {
            Console.WriteLine("{0} {1}EUR", Messages.msg_zostavajuci_kredit, Hrac1.zostavajuciKredit);
        }

        private void zapisHracovTah(string typStavky)
        {
            Hrac1.zostavajuciKredit = Hrac1.zostavajuciKredit + vyhra;
            Hrac1.zapisTah(vyskaStavky, vratCitatelnyVysledok(vysledokSpinu), typStavky, vyhra, zadanaStavkaUzivatelom);
        }

        private string vratZoznamPrikazov()
        {
            string stavky = Messages.msg_zvolte;
            foreach (string meno in Enum.GetNames(typeof(Prikazy))) stavky = stavky + meno + "\n";
            return stavky;
        }
        
        private string vratZoznamFarieb()
        {
            string farby = Messages.msg_zvolte;
            foreach (string meno in Enum.GetNames(typeof(Farba))) farby = farby + meno + "\n";
            return farby;
        }

        private string vratZoznamParnosti()
        {
            string parnosti = Messages.msg_zvolte;
            foreach (string meno in Enum.GetNames(typeof(Párnosť))) parnosti = parnosti + meno + "\n";
            return parnosti;
        }

        private string vratZoznamRad()
        {
            string rady = Messages.msg_zvolte;
            foreach (string meno in Enum.GetNames(typeof(Rada))) rady = rady + meno + "\n";
            return rady;
        }

        private string vratZoznamRozsahov()
        {
            string rozsahy = Messages.msg_zvolte;
            foreach (string meno in Enum.GetNames(typeof(Rozsah))) rozsahy = rozsahy + meno + "\n";
            return rozsahy;
        }

        private string vratVolbyPodlaTypuStavky(string typStavky)
        {
            if (typStavky == Prikazy.Farba.ToString()) return vratZoznamFarieb();
            else if (typStavky == Prikazy.Číslo.ToString()) return Messages.msg_zadaj_0_36;
            else if (typStavky == Prikazy.Párnosť.ToString()) return vratZoznamParnosti();
            else if (typStavky == Prikazy.Rada.ToString()) return vratZoznamRad();
            else if (typStavky == Prikazy.Rozsah.ToString()) return vratZoznamRozsahov();
            else return "";
        }

        private bool nachadzaSaVRozsahu(string zadanaStavkaUzivatelom)
        {
            foreach (string meno in Enum.GetNames(typeof(Rozsah)))
            {
                if (zadanaStavkaUzivatelom == meno) return true;
            }
            return false;
        }

        private bool nachadzaSaVoFarbach(string zadanaStavkaUzivatelom)
        {
            foreach (string meno in Enum.GetNames(typeof(Farba)))
            {
                if (zadanaStavkaUzivatelom == meno) return true;
            }
            return false;
        }

        private bool nachadzaSaVRadach(string zadanaStavkaUzivatelom)
        {
            foreach (string meno in Enum.GetNames(typeof(Rada)))
            {
                if (zadanaStavkaUzivatelom == meno) return true;
            }
            return false;
        }

        private bool nachadzaSaVParnostiach(string zadanaStavkaUzivatelom)
        {
            foreach (string meno in Enum.GetNames(typeof(Párnosť)))
            {
                if (zadanaStavkaUzivatelom == meno) return true;
            }
            return false;
        }
    }
}

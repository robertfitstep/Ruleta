﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruleta
{
    struct Tah
    {
        public int vyskaStavky;
        public string prelozenyVysledokSpinu;
        public int cisloTahu;
        public string TypStavky;
        public int vyhra;
        public string vasaStavka;

        public Tah(int cisloTahu, int vyskaStavky, string prelozenyVysledokSpinu, string TypStavky, int vyhra, string vasaStavka)
        {
            this.vyskaStavky = vyskaStavky;
            this.prelozenyVysledokSpinu = prelozenyVysledokSpinu;
            this.TypStavky = TypStavky;
            this.cisloTahu = cisloTahu;
            this.vyhra = vyhra;
            this.vasaStavka = vasaStavka;
        }
    }
}

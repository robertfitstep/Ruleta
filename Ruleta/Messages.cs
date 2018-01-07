using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruleta
{
    public static class Messages
    {
        public static readonly string nespravnyPrikaz = "Zadali ste nesprávny príkaz.";
        public static readonly string zadPrikaz = "Zadajte príkaz alebo Pomoc pre nápovedu";
        public static readonly string zadVasuStavku = "Zadajte vašu stávku alebo Pomoc pre nápovedu";
        public static readonly string zadVyskuStavky = "Zadajte výšku stávky v EUR";
        public static readonly string msg_prehra = "Prehrali ste";
        public static readonly string msg_vyhra = "Vyhrali ste";
        public static readonly string msg_nespravna_stavka = "Zadali ste nesprávnu stávku";
        public static readonly string msg_nepravny_format_kreditu = "Zadali ste nesprávny formát kreditu";
        public static readonly string msg_nedostatok_kreditu = "Na stávku nemáte dostatok kreditu";
        public static readonly string msg_nul_alebo_minus_stavka = "Vaša stávka musí byť vyžšia ako 0";
        public static readonly string msg_prehra_kreditu = "Prehrali ste všetok kredit.\n\nZačína sa nová hra.";
        public static readonly string msg_zostavajuci_kredit = "Váš zostávajúci kredit je";
        public static readonly string msg_zadaj_0_36 = "Zadajte číslo od 0-36";
        public static readonly string msg_zvolte = "Zvoľte si z nástedujúcich volieb:\n";
    }
}

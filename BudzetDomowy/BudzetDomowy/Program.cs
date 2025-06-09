using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BudzetDomowy
{
    class Program
    {
        static void Main(string[]args)
        {
            string sciezka = @"C:\Users\admin\Desktop\BudzetDomowy.txt";
            Budzet budzet = new Budzet(sciezka);
            budzet.WczytajBudzet();
            budzet.PokazDane();
            budzet.PokazDaneKoncowe();
            budzet.ZapiszDoPliku(sciezka);
        }


    }
}
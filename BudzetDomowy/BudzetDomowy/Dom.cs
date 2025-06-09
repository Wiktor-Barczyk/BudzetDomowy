using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BudzetDomowy
{
    class Budzet
    {
       public decimal Przychod { get; set; }
       public decimal Kredyt { get; set; }
       public decimal Rachunki { get; set; }
       public decimal Zywnosc { get; set; }
       public decimal Transport { get; set; }
       public decimal Zwierzeta { get; set; }
       public decimal ChemiaLeki { get; set; }
       public decimal LiczbaOsob { get; set; }

       public string sciezkaPliku;

        public Budzet(string sciezka)
        {
            sciezkaPliku = sciezka;
        }

        public void WczytajBudzet()
        {
            Console.Write("Czy wczytać poprzedni budzet? (T/N): ");
            string odpowiedzP = Console.ReadLine();
            if (odpowiedzP.ToUpper() == "T")
            {
                Console.Clear();
                WczytajZPliku(sciezkaPliku);
                return;
            }
            else
            {
                Console.Clear();
            }

                Console.Write("Podaj budzet domowy: ");
            Przychod = WczytajDane();
            Console.Clear();
            if (Przychod <= 0)
            {
                Console.WriteLine("Idz do pracy");
                Environment.Exit(0);
            }

            Console.Write("Czy mieszkasz sam? (T/N): ");
            string odpowiedzL = Console.ReadLine();
            if (odpowiedzL.ToUpper() == "N")
            {
                Console.Clear();
                Console.Write("Ile osób mieszka z tobą?: ");
                LiczbaOsob = WczytajDane() + 1;
                Console.Clear();
            }
            else
            {
                LiczbaOsob = 1;
                Console.Clear();
            }

            Console.Write("Czy masz/macie kredyt w banku? (T/N): ");
            string odpowiedzK = Console.ReadLine();
            if (odpowiedzK.ToUpper() == "T")
            {
                Console.Clear();
                Console.Write("Szacowane wydatki na spłatę raty w banku: ");
                Kredyt = WczytajDane();
                Console.Clear();
                if (Kredyt < 0)
                {
                    Console.WriteLine("Złe dane!");
                    Environment.Exit(0);
                }
            }
            else
            {
                Kredyt = 0;
                Console.Clear();
            }

            Console.Write("Czy masz/macie zwierzęta? (T/N): ");
            string odpowiedzZw = Console.ReadLine();
            if (odpowiedzZw.ToUpper() == "T")
            {
                Console.Clear(); 
                Console.Write("Szacowane wydatki na zwierzęta: ");
                Zwierzeta = WczytajDane();
                Console.Clear();
            }
            else
            {
                Zwierzeta = 0;
                Console.Clear();
            }

            Console.Write("Szacowane wydatki na rachunki(media, internet, telefon): ");
            Rachunki = WczytajDane();
            Console.Clear();

            Console.Write("Szacowane wydatki na jedzenie: ");
            Zywnosc = WczytajDane();
            Console.Clear();

            Console.Write("Szacowane wydatki na transport: ");
            Transport = WczytajDane();
            Console.Clear();

            Console.Write("Szacowane wydatki na Chemie/Leki: ");
            ChemiaLeki = WczytajDane();
            Console.Clear();

            if (Rachunki < 0 || Zywnosc < 0 || Transport < 0 || ChemiaLeki < 0)
            {
                Console.WriteLine("Złe dane!");
                Environment.Exit(0);
            }

        }

        public void ZapiszDoPliku(string sciezkaPliku)
        {
            
                using (StreamWriter writer = new StreamWriter(sciezkaPliku))
                {
                    writer.WriteLine($"Budżet domowy: {Przychod}zł");
                    writer.WriteLine($"Liczba osób: {LiczbaOsob}");
                    writer.WriteLine($"Kredyt: {Kredyt}zł");
                    writer.WriteLine($"Rachunki: {Rachunki}zł");
                    writer.WriteLine($"Żywność: {Zywnosc}zł");
                    writer.WriteLine($"Transport: {Transport}zł");
                    writer.WriteLine($"Zwierzęta: {Zwierzeta}zł");
                    writer.WriteLine($"Chemia/Leki: {ChemiaLeki}zł");

                    decimal suma = Kredyt + Rachunki + Zywnosc + Transport + Zwierzeta + ChemiaLeki;
                    decimal koncowyBudzet = Przychod - suma;
                    decimal wydatkiNaOsobe = Math.Round(suma / LiczbaOsob, 2);

                    writer.WriteLine($"Wydatki w miesiącu: {suma}zł");
                    writer.WriteLine($"Pozostałe pieniądze: {koncowyBudzet}zł");
                    writer.WriteLine($"Wydatki na osobę: {wydatkiNaOsobe}zł");
                }
        }

        public void WczytajZPliku(string sciezkaPliku)
        {
            if (!File.Exists(sciezkaPliku))
            {
                Console.WriteLine("Plik nie istnieje.");
                Environment.Exit(0);
            }

            string[] linie = File.ReadAllLines(sciezkaPliku);

            foreach (string linia in linie)
            {
                if (linia.StartsWith("Budżet domowy:"))
                {
                    Przychod = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Liczba osób:"))
                {
                    LiczbaOsob = decimal.Parse(linia.Split(':')[1].Trim());
                }
                else if (linia.StartsWith("Kredyt:"))
                {
                    Kredyt = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Rachunki:"))
                {
                    Rachunki = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Żywność:"))
                {
                    Zywnosc = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Transport:"))
                {
                    Transport = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Zwierzęta:"))
                {
                    Zwierzeta = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
                else if (linia.StartsWith("Chemia/Leki:"))
                {
                    ChemiaLeki = decimal.Parse(linia.Split(':')[1].Replace("zł", "").Trim());
                }
            }
        }

        public decimal WczytajDane()
        {
            decimal wynik;
            while(!decimal.TryParse(Console.ReadLine(), out wynik))
            {
                Console.WriteLine("Podaj poprawną wartość");
            }
            return wynik;
        }

        public void PokazDane()
        {
            
            Console.WriteLine($"* Budzet domowy: {Przychod}zł");
            Console.WriteLine("*");
            if (Kredyt > 0)
            {
                Console.WriteLine($"* Rata: -{Kredyt}zł");
            }
            Console.WriteLine($"* Rachunki: -{Rachunki}zł");
            Console.WriteLine($"* Żywność: -{Zywnosc}zł");
            Console.WriteLine($"* Transport: -{Transport}zł");
            if (Zwierzeta > 0)
            {
                Console.WriteLine($"* Zwierzęta: -{Zwierzeta}zł");
            }
            Console.WriteLine($"* Chemia/Leki: -{ChemiaLeki}zł");
            Console.WriteLine("*");
            decimal Suma = Kredyt + Rachunki + Zywnosc + Transport + Zwierzeta + ChemiaLeki;
            Console.WriteLine($"* Wydatki w danym miesiącu: {Suma}zł");
            Console.WriteLine("*");
        }

        public void PokazDaneKoncowe()
        {
            decimal KoncowyBudzet = Przychod - (Kredyt + Rachunki + Zywnosc + Transport + Zwierzeta + ChemiaLeki);
            decimal Wydatki = Math.Round((Kredyt + Rachunki + Zywnosc + Transport + Zwierzeta + ChemiaLeki)/LiczbaOsob,2);
            Console.WriteLine($"* Pozostałe pieniądze do zagospodarowania: {KoncowyBudzet}zł");
            if (LiczbaOsob > 1)
            {
                Console.WriteLine($"* Wydatki zostały podzielone na {LiczbaOsob}" );
                Console.WriteLine($"* Wydatki na osobe: {Wydatki}zł");
            }
            
        }
       
    }
}

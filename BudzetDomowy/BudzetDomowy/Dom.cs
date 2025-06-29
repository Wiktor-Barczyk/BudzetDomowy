using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudzetDomowy
{
    class Wydatek
    {
        public string Kategoria { get; set; }
        public decimal Kwota { get; set; }

        public Wydatek(string kategoria, decimal kwota)
        {
            Kategoria = kategoria;
            Kwota = kwota;
        }
    }

    abstract class BudzetBase
    {
        public decimal Przychod { get; set; }
        public decimal LiczbaOsob { get; set; }
        public string sciezkaPliku;

        public BudzetBase(string sciezka)
        {
            sciezkaPliku = sciezka;
        }

        public abstract void WczytajBudzet();
        public abstract void ZapiszDoPliku(string sciezkaPliku);
        public abstract void PokazDane();
        public abstract void PokazDaneKoncowe();
    }

    class Budzet : BudzetBase
    {
        public List<Wydatek> Wydatki { get; set; } = new List<Wydatek>();

        public Budzet(string sciezka) : base(sciezka) { }

        public override void WczytajBudzet()
        {
            Console.Write("Czy wczytać poprzedni budżet? (T/N): ");
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

            Console.Write("Podaj budżet domowy: ");
            Przychod = WczytajDane();
            Console.Clear();
            if (Przychod <= 0)
            {
                Console.WriteLine("Nieprawidłowy budzet");
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

            while (true)
            {
                Console.WriteLine("Dodawanie wydatków. Wpisz 'koniec' jako kategorię, aby zakończyć.");
                Console.Write("Kategoria wydatku: ");
                string kategoria = Console.ReadLine();
                if (kategoria.Trim().ToLower() == "koniec")
                {
                    Console.Clear();
                    break;
                }

                Console.Write("Kwota: ");
                decimal kwota = WczytajDane();
                if (kwota < 0)
                {
                    Console.WriteLine("Kwota nie może być ujemna.");
                    continue;
                }

                Wydatki.Add(new Wydatek(kategoria, kwota));
                Console.Clear();
            }
        }

        public override void ZapiszDoPliku(string sciezkaPliku)
        {
            using (StreamWriter writer = new StreamWriter(sciezkaPliku))
            {
                writer.WriteLine($"Budżet domowy: {Przychod}zł");
                writer.WriteLine($"Liczba osób: {LiczbaOsob}");
                foreach (var w in Wydatki)
                {
                    writer.WriteLine($"{w.Kategoria}: {w.Kwota}zł");
                }
                decimal suma = Wydatki.Sum(w => w.Kwota);
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
                else if (linia.Contains(":") && linia.Contains("zł"))
                {
                    string[] parts = linia.Split(':');
                    if (parts.Length >= 2)
                    {
                        string kategoria = parts[0].Trim();
                        decimal kwota = decimal.Parse(parts[1].Replace("zł", "").Trim());
                        Wydatki.Add(new Wydatek(kategoria, kwota));
                    }
                }
            }
        }

        public decimal WczytajDane()
        {
            decimal wynik;
            while (!decimal.TryParse(Console.ReadLine(), out wynik))
            {
                Console.WriteLine("Podaj poprawną wartość");
            }
            return wynik;
        }

        public override void PokazDane()
        {
            Console.WriteLine($"* Budżet domowy: {Przychod}zł");
            foreach (var w in Wydatki)
            {
                Console.WriteLine($"* {w.Kategoria}: -{w.Kwota}zł");
            }
            decimal suma = Wydatki.Sum(w => w.Kwota);
            Console.WriteLine("*");
            Console.WriteLine($"* Wydatki w danym miesiącu: {suma}zł");
            Console.WriteLine("*");
        }

        public override void PokazDaneKoncowe()
        {
            decimal suma = Wydatki.Sum(w => w.Kwota);
            decimal koncowyBudzet = Przychod - suma;
            decimal wydatkiNaOsobe = Math.Round(suma / LiczbaOsob, 2);
            Console.WriteLine($"* Pozostałe pieniądze do zagospodarowania: {koncowyBudzet}zł");
            if (LiczbaOsob > 1)
            {
                Console.WriteLine($"* Wydatki zostały podzielone na {LiczbaOsob}");
                Console.WriteLine($"* Wydatki na osobę: {wydatkiNaOsobe}zł");
                Console.WriteLine("* " + DateTime.Now.ToString("d/MM/yyyy"));
            }
        }
    }
}
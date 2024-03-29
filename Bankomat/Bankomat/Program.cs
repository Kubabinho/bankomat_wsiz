using System;
using System.Collections.Generic;
using System.IO;

namespace Bankomat
{
    class Bankomat
    {
        private Dictionary<string, Tuple<string, decimal>> account;
        private string FileNameAcc = "konta.txt";

        public Bankomat()
        {
            account = new Dictionary<string, Tuple<string, decimal>>();
            if (!File.Exists(FileNameAcc))
            {
                File.Create(FileNameAcc).Close();
            }
            LoadAccFromFile(FileNameAcc);
        }

        private void LoadAccFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] linie = File.ReadAllLines(fileName);
                foreach (string linia in linie)
                {
                    string[] dane = linia.Split(';');
                    if (dane.Length == 3)
                    {
                        string CardNumber = dane[0];
                        string pin = dane[1];
                        decimal cash = decimal.Parse(dane[2]);
                        account[CardNumber] = new Tuple<string, decimal>(pin, cash);
                    }
                }
            }
            else
            {
                Console.WriteLine("nie ma pliku.");
            }
        }

        private bool verifyPin(string CardNumber, string pin)
        {
            if (account.ContainsKey(CardNumber))
            {
                return account[CardNumber].Item1 == pin;
            }
            return false;
        }

        public void takeCash(string CardNumber, string pin, decimal amount)
        {
            if (verifyPin(CardNumber, pin))
            {
                if (amount <= account[CardNumber].Item2)
                {
                    account[CardNumber] = new Tuple<string, decimal>(account[CardNumber].Item1, account[CardNumber].Item2 - amount);
                    Console.WriteLine($"karta {CardNumber}. wyplacam {amount} zł. Stan: {account[CardNumber].Item2}");
                }
                else
                {
                    Console.WriteLine("nie masz wystarczajaco na konice.");
                }
            }
            else
            {
                Console.WriteLine("zly numer karty lub pin.");
            }
        }

        public void insertCash(string CardNumber, decimal amount)
        {
            account[CardNumber] = new Tuple<string, decimal>(account[CardNumber].Item1, account[CardNumber].Item2 + amount);
            Console.WriteLine($"wplacasz {amount} zł na konto {CardNumber}. Nowy stan: {account[CardNumber].Item2}");
        }

        public void checkCash(string CardNumber, string pin)
        {
            if (verifyPin(CardNumber, pin))
            {
                Console.WriteLine($"Stan dla {CardNumber}: {account[CardNumber].Item2}");
            }
            else
            {
                Console.WriteLine("zly numer karty lub pin.");
            }
        }

        public void addAcount(string CardNumber, string pin, decimal cash)
        {
            account[CardNumber] = new Tuple<string, decimal>(pin, cash);
            using (StreamWriter writer = File.AppendText(FileNameAcc))
            {
                writer.WriteLine($"{CardNumber};{pin};{cash}");
            }
            Console.WriteLine($"dodano nowe konto {CardNumber} do bazy");
        }

        public void showMenu()
        {
            Console.WriteLine("witamy w bakomacie");
            Console.WriteLine("1. wypłata");
            Console.WriteLine("2. wpłata");
            Console.WriteLine("3. sprawdzenie stanu konta");
            Console.WriteLine("4. dodanie nowego konta");
            Console.WriteLine("5. wyjście");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bankomat bankomat = new Bankomat();

            bool @continue = true;
            while (@continue)
            {
                bankomat.showMenu();
                Console.Write("co chcesz robic: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        Console.Write("podaj numer karty: ");
                        string CardNumber1 = Console.ReadLine();
                        Console.Write("podaj pin: ");
                        string pin1 = Console.ReadLine();
                        Console.Write("ile do wypłaty: ");
                        decimal amount1 = decimal.Parse(Console.ReadLine());
                        bankomat.takeCash(CardNumber1, pin1, amount1);
                        break;
                    case "2":
                        Console.Write("podaj numer karty: ");
                        string CardNumber2 = Console.ReadLine();
                        Console.Write("ile do wyplaty: ");
                        decimal amount2 = decimal.Parse(Console.ReadLine());
                        bankomat.insertCash(CardNumber2, amount2);
                        break;
                    case "3":
                        Console.Write("podaj numer karty: ");
                        string CardNumber3 = Console.ReadLine();
                        Console.Write("podaj pin: ");
                        string pin3 = Console.ReadLine();
                        bankomat.checkCash(CardNumber3, pin3);
                        break;
                    case "4":
                        Console.Write("podaj numer karty: ");
                        string CardNumber4 = Console.ReadLine();
                        Console.Write("podaj pin: ");
                        string pin4 = Console.ReadLine();
                        Console.Write("podaj stan konta: ");
                        decimal balance = decimal.Parse(Console.ReadLine());
                        bankomat.addAcount(CardNumber4, pin4, balance);
                        break;
                    case "5":
                        @continue = false;
                        Console.WriteLine("dzienki za skorzystanie z bankomatu");
                        break;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BrickGame
{
    class Program
    {
        static string[] plansza;
        const string GRACZ = "^";
        const string PRZESZKODA = "#";
        const string NITRO = "N";
        //zmienna do zliczania punktów
        static int punkty = 0;
        static void Main(string[] args)
        {
            //zmienne dla naszej gry

            // 0 -> po lewej  1 -> na środku  2 -> po prawej
            int pozycjaGracza = 1;

            // dodajemy funkcjonalność nitro - czyli przyspieszenia 
            int czasNitro = -1;
            int wartoscNitro = 0;
            bool czyPaliwoNitro = false;

            bool czyUderzony = false;

            //generator licz losowych - do losowania pozycji przeszkody
            Random random = new Random();

            //Generuje nam pustą planszę - przekazujemy długość planszy czyli ile przeszkód będzie widoczne wcześniej
            NowaPlansza(10);
            UstawGracza(pozycjaGracza);
            PokazPlansze();

            while (!czyUderzony)
            {

                if (Console.KeyAvailable)
                {
                    //zczytanie jaki klawisz na klawiaturze został wciśnięty
                    ConsoleKeyInfo nacisnietyKlawisz = Console.ReadKey(true);
                    if (nacisnietyKlawisz.Key == ConsoleKey.RightArrow)
                    {
                        // pozacyja  gracza   0   1   2
                        if (pozycjaGracza < 2)
                        {
                            pozycjaGracza++;
                        }
                    }
                    if (nacisnietyKlawisz.Key == ConsoleKey.LeftArrow)
                    {
                        // pozacyja  gracza   0   1   2
                        if (pozycjaGracza > 0)
                        {
                            pozycjaGracza--;
                        }
                    }
                    if(nacisnietyKlawisz.Key == ConsoleKey.Spacebar && czyPaliwoNitro)
                    {
                        if(czasNitro == -1)
                        {
                            czasNitro = 10;
                            czyPaliwoNitro = false;
                        }
                    }
                    //czyszczenie buforu - usuwanie zakolejkowanych ruchów
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(false);
                    }
                }

                //sprawdzenie uderzenia
                int pozycjaNajblizszejPrzeszkody = plansza[plansza.Length - 2].IndexOf(PRZESZKODA);
                if(pozycjaGracza == pozycjaNajblizszejPrzeszkody)
                {
                    czyUderzony = true;
                }
                else
                {
                    punkty++;
                }

                //sprawdzenie czy zdobyto nitro
                int pozycjaNajblizszegoNitro = plansza[plansza.Length - 2].IndexOf(NITRO);
                if(pozycjaGracza == pozycjaNajblizszegoNitro)
                {
                    czyPaliwoNitro = true;
                }

                //nowa przeszkoda
                int pozycjaPrzeszkody = random.Next(0,3);
                string przeszkoda = UstawPrzeszkode(pozycjaPrzeszkody);

                if(random.Next(20) == 0)
                {
                    int pozycjaNitro = random.Next(3);
                    przeszkoda = UstawNitro(pozycjaNitro, przeszkoda);
                }


                //przesunięcie planszy w dół + dodanie do niej przeszkody
                for(int i = plansza.Length - 2; i > 0; i--)
                {
                    plansza[i] = plansza[i - 1];
                }
                plansza[0] = przeszkoda;

                UstawGracza(pozycjaGracza);
                PokazPlansze();
                Console.WriteLine();
                Console.WriteLine($"Punkty: {punkty}");
                Console.WriteLine($"Nitro: {czyPaliwoNitro}");
                //wprowadzenie mechanizmu nitro
                if(czasNitro > 0)
                {
                    wartoscNitro = 100;
                    czasNitro--;
                }
                else if(czasNitro == 0)
                {
                    wartoscNitro = 0;
                    czasNitro = -1;
                }
                // zwiększenie prędkości co 1 ruch przeszkody 
                if (punkty + wartoscNitro < 600)
                {
                    Thread.Sleep(600 - punkty - wartoscNitro);
                }
            }

            Console.Clear();
            Console.WriteLine("GAME OVER");
            Console.WriteLine($"Zdobyłeś {punkty} punktów");

            Thread.Sleep(2000);
            //TO JEST KONIEC PROGRAMU
            Console.ReadKey();
        }
        private static void NowaPlansza(int rozmiarPlanszy)
        {
            plansza = new string[rozmiarPlanszy];
            for(int i = 0; i < plansza.Length; i++)
            {
                plansza[i] = "";
            }
        }

        private static void UstawGracza(int pozycja)
        {
            string linia = "   "; // w cudzysłowie 3 spacje
            linia = linia.Insert(pozycja, GRACZ);  // metoda insert do wsadzania danego znaku do stringa 
            plansza[plansza.Length - 1] = linia;
        }

        private static void PokazPlansze()
        {
            Console.Clear();
            for (int i = 0; i < plansza.Length; i++)
            {
                Console.WriteLine(plansza[i]);
            }
        }

        private static string UstawPrzeszkode(int pozycja)
        {
            string linia = "   "; //3 spacje
            linia = linia.Insert(pozycja, PRZESZKODA);
            return linia;
        }

        //funkcja do ustawiania Nitro na podstawie pozycji i lini w której ma być umieszczone
        private static string UstawNitro(int pozycja, string linia)
        {
            linia = linia.Remove(pozycja,1).Insert(pozycja, NITRO);
            return linia;
        }
    }
}

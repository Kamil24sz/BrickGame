using System;
using System.Collections.Generic;
using System.Linq;
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
        static void Main(string[] args)
        {
            //zmienne dla naszej gry

            // 0 -> po lewej  1 -> na środku  2 -> po prawej
            int pozycjaGracza = 1;

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
                }

                //sprawdzenie uderzenia
                int pozycjaNajblizszejPrzeszkody = plansza[plansza.Length - 2].IndexOf(PRZESZKODA);
                if(pozycjaGracza == pozycjaNajblizszejPrzeszkody)
                {
                    czyUderzony = true;
                }

                //nowa przeszkoda
                int pozycjaPrzeszkody = random.Next(0,3);
                string przeszkoda = UstawPrzeszkode(pozycjaPrzeszkody);

                //przesunięcie planszy w dół + dodanie do niej przeszkody
                for(int i = plansza.Length - 2; i > 0; i--)
                {
                    plansza[i] = plansza[i - 1];
                }
                plansza[0] = przeszkoda;

                UstawGracza(pozycjaGracza);
                PokazPlansze();
                Thread.Sleep(600);
            }

            Console.Clear();
            Console.WriteLine("GAME OVER");

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
    }
}

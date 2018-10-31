#if DEBUG_PROGRAM
using System;

using Fifteen;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game(2, 2);
        game.Shuffle();

        ConsoleKeyInfo key;
        do
        {
            Console.Clear();
            Paint(game);
            Console.WriteLine("Number is " + game.Number() + ", solvability is " + game.CheckSolvability());
            if (game.CheckVictory()) Console.WriteLine("VICTORY!!!");

            key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    game.Play(Direction.UP);
                    break;
                case ConsoleKey.DownArrow:
                    game.Play(Direction.DOWN);
                    break;
                case ConsoleKey.LeftArrow:
                    game.Play(Direction.LEFT);
                    break;
                case ConsoleKey.RightArrow:
                    game.Play(Direction.RIGHT);
                    break;
            }
        }
        while (key.Key != ConsoleKey.Escape);
    }

    static void Paint(Game game)
    {
        int max = game.Height * game.Width;
        for (int j = 0; j < game.Height; j++)
        {
            string line = "";
            for (int i = 0; i < game.Width; i++)
            {
                int val = game[i, j];
                if (val == max) line += " []";
                else line += Format(game[i, j], 3);
            }
            Console.WriteLine(line);
        }
    }
    static string Format(int val, int d)
    {
        int num = (int)Math.Log10(val) + 1;
        string s = "";
        for (int i = num; i < d; i++) s += " ";
        return s + val;
    }
}
#endif
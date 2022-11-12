namespace ZoneRpg.UserInterface
{
    public class ConsoleUtils
    {
        public static void DrawBox(int width, int height)
        {
            string c1 = "╔";
            string c2 = "╗";
            string c3 = "╝";
            string c4 = "╚";
            string space = "═";
            string I = "║";
            Console.Write(c1);
            for (int i = 1; i < width - 1; i++)
            {
                Console.Write(space);
            }
            Console.WriteLine(c2);

            for (int i = 2; i < height; i++)
            {
                Console.Write(I);
                for (int j = 2; j < width; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(I + "  ");  // Extra spaces om utf-8 råkar förstöra
            }
            Console.Write(c4);

            for (int i = 2; i < width; i++)
            {
                Console.Write(space);

            }
            Console.WriteLine(c3);

        }
    }
}
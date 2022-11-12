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
                Console.WriteLine(I);
            }
            Console.Write(c4);

            for (int i = 2; i < width; i++)
            {
                Console.Write(space);

            }
            Console.WriteLine(c3);
        }

        //
        // Samma sak som ovan, men använder inte WriteLine så vi kan ha en box var som helst på skärmen
        //
        public static void DrawBox(int x, int y, int width, int height)
        {
            Console.SetCursorPosition(x, y);
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
            Console.Write(c2);
            Console.SetCursorPosition(x, y + 1);

            for (int i = 2; i < height; i++)
            {
                Console.Write(I);
                for (int j = 2; j < width; j++)
                {
                    Console.Write(" ");
                }
                Console.Write(I);
                Console.SetCursorPosition(x + 1, y + 1 + i);
            }
            Console.Write(c4);

            for (int i = 2; i < width; i++)
            {
                Console.Write(space);

            }
            Console.Write(c3);

        }
    }
}
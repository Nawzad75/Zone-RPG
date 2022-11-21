namespace ZoneRpg.UserInterface
{
    public class ConsoleUtils
    {
        //  Ritar en ascii-box
        public static void DrawBox(int x, int y, int width, int height)
        {
            
            string cornerTL = "╔";
            string cornerTR = "╗";
            string cornerBR = "╝";
            string cornerBL = "╚";
            string vertical = "║";
            string horizont = "═";

            Console.SetCursorPosition(x, y);
            Console.Write(cornerTL);
            for (int i = 0; i < width; i++)
            {
                Console.Write(horizont);
            }
            Console.Write(cornerTR);
            Console.SetCursorPosition(x, y + 1);

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i + 1);
                Console.Write(vertical);
                for (int j = 0; j < width; j++)
                {                    
                    Console.Write(" ");
                }
                Console.Write(vertical + " "); // last space repairs utf8 bug
            }

            Console.SetCursorPosition(x, y + height + 1);
            Console.Write(cornerBL);
            for (int i = 0; i < width; i++)
            {
                Console.Write(horizont);

            }
            Console.Write(cornerBR);

        }
    }
}
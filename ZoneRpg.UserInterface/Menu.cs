namespace ZoneRpg.UserInterface
{
    public class Menu
    {

        private int _choice;
        private string[] Options;
        private string Prompt;

        public Menu(string prompt, string[] options)
        {
            Prompt = prompt;
            Options = options;
            _choice = 0;
        }

        public void DisplayOptions()

        {
            Console.Clear();
            Console.WriteLine(Prompt);
            for (int i = 0; i < Options.Length; i++)
            {
                string currentOption = Options[i];
                string prefix;
                if (i == _choice)
                {
                    prefix = ">";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(prefix + currentOption);

            }
            Console.ResetColor();
        }


        public int Run()
        {
            ConsoleKey keyPressed;
            do
            {

                DisplayOptions();

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _choice--;
                    if (_choice < 0)
                    {
                        _choice = Options.Length - 1;
                    }
                    Console.Clear();
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _choice++;
                    if (_choice >= Options.Length)
                    {
                        _choice = 0;
                    }
                    Console.Clear();
                }
            } while (keyPressed != ConsoleKey.Enter);
            return _choice;

        }
    }
}
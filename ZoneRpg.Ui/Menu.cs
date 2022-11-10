namespace ZoneRpg.Ui
{
    public class Menu
    {

        private int _choice;
        private string[] _options;
        private string _prompt;

        public Menu(string prompt, string[] options)
        {
            _prompt = prompt;
            _options = options;
            _choice = 0;
        }

        //
        // Displays all the options in the menu
        //
        public void DisplayOptions()
        {
            Console.WriteLine(_prompt);

            for (int i = 0; i < _options.Length; i++)
            {
                string currentOption = _options[i];
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

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;
                if (keyPressed == ConsoleKey.UpArrow)
                {
                    _choice--;
                    if (_choice < 0)
                    {
                        _choice = _options.Length - 1;
                    }
                    Console.Clear();
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    _choice++;
                    if (_choice >= _options.Length)
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
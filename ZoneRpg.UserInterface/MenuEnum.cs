namespace ZoneRpg.UserInterface
{
    //
    // Menu which can: 
    //      * use string[] options and return index as int
    //      * use enum and return enum value
    //
    public class MenuEnum<T>
    {
        private int _choice = 0;
        private string[] _options;
        private string _prompt;

        // Constructor where we pass in the options and the prompt
        public MenuEnum(string prompt, string[] options)
        {
            _prompt = prompt;
            _options = options;
        }

        // Constructor where figure out the options from the enum
        public MenuEnum(string prompt) : this(prompt, Enum.GetNames(typeof(T)))
        {
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

        //
        // Get the user's choice
        //
        public T Run()
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


            // If T is an int, return the index of the choice
            if (typeof(T) == typeof(int))
            {
                return (T)Convert.ChangeType(_choice, typeof(T));
            }

            // If T is an enum, return the enum value of the choice
            else if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), _options[_choice]);
            }
            // Else throw an error
            else
            {
                throw new Exception("Menu<T> only supports int and enums!");
            }


        }

    }

}
using ZoneRpg.Game;
using ZoneRpg.Database;
using ZoneRpg.Ui;

namespace ZoneRpg.Game
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
        
    }
}
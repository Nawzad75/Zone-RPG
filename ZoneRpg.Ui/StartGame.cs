using ZoneRpg.Shared;

namespace ZoneRpg.Ui
{

    public class StartGame
    {
        public void RunMainMenu()
        {


            string prompt = @"███████  ██████  ███    ██ ███████ ██████  ██████   ██████  
   ███  ██    ██ ████   ██ ██      ██   ██ ██   ██ ██       
  ███   ██    ██ ██ ██  ██ █████   ██████  ██████  ██   ███ 
 ███    ██    ██ ██  ██ ██ ██      ██   ██ ██      ██    ██ 
███████  ██████  ██   ████ ███████ ██   ██ ██       ██████  
                                                            
                                                            ";
            // string[] options = { "Start", "About", "Exit" };
            // Menu mainMenu = new Menu(prompt, options);
            // int _choice = mainMenu.Run();

            StartMenu _choice = new Menu<StartMenu>(prompt).Run();

            switch (_choice)
            {
                case StartMenu.Start:
                    Start();
                    Console.Clear();
                    break;

                case StartMenu.About:
                    About();
                    break;

                case StartMenu.Exit:
                    ExitGame();
                    break;
            }

        }

        //Här skickar vi in funktionen som skapar spelaren
        public void Start()
        {






        }
        public void ExitGame()
        {
            Console.WriteLine("Exit Game");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        private void About()
        {
            Console.Clear();
            Console.WriteLine("This is a game about a guy who is trying to find his way home, but he is lost in a strange world. He has to find his way home, but he has to fight monsters and find items to help him on his way. ");
            Console.ReadKey();
            Console.Clear();
            RunMainMenu();
        }
    }

}

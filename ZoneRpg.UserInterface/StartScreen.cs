namespace ZoneRpg.UserInterface
{

    public class StartMenu
    {
        public void RunMainMenu()
        {


            string prompt = @"
███████  ██████  ███    ██ ███████ ██████  ██████   ██████  
   ███  ██    ██ ████   ██ ██      ██   ██ ██   ██ ██       
  ███   ██    ██ ██ ██  ██ █████   ██████  ██████  ██   ███ 
 ███    ██    ██ ██  ██ ██ ██      ██   ██ ██      ██    ██ 
███████  ██████  ██   ████ ███████ ██   ██ ██       ██████  
                                                            
                                                            ";
            string[] options = { "Start", "About", "Exit" };
            
            int _choice = new Menu(prompt, options).Run();

            switch (_choice)
            {
                case 0:
                    Start();
                    Console.Clear();
                    break;

                case 1:
                    About();
                    break;

                case 2:
                    ExitGame();
                    break;
            }

        }
        public void Start()
        {
        }

        public void ExitGame()
        {
            Console.WriteLine("Exit Game");
            Console.ReadKey();
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

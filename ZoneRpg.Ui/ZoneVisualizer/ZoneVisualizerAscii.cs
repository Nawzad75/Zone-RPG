using ZoneRpg.Shared;

namespace ZoneRpg.Ui
{
    internal class ZoneVisualizerAscii : IZoneVisualizer
    {

        //
        // Draws a Zone
        //
        public void DrawZone(Zone zone)
        {

            string c1 = "╔";
            string c2 = "╗";
            string c3 = "╝";
            string c4 = "╚";
            string space = "═";
            string I = "║";
            Console.SetCursorPosition(0, 0);
            Console.Write(c1);
            for (int i = 1; i < zone.Width - 1; i++)
            {
                Console.Write(space);
            }
            Console.WriteLine(c2);

            for (int i = 2; i < zone.Height; i++)
            {
                Console.Write(I);
                for (int j = 2; j < zone.Width; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(I + "  ");  // Extra spaces om utf-8 råkar förstöra
            }
            Console.Write(c4);

            for (int i = 2; i < zone.Width; i++)
            {
                Console.Write(space);

            }
            Console.WriteLine(c3);
            Console.WriteLine("                Zone: " + zone.Name);
        }

        //
        // Draws all entities in the zone
        //
        public void DrawEntities(List<Entity> entities)
        {
            foreach (var item in entities)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.WriteLine(item.Symbol);

            }
        }

        //
        // Draw the player
        //
        public void DrawPlayerEntity(Entity playerEntity)
        {
            Console.SetCursorPosition(playerEntity.X, playerEntity.Y);
            Console.WriteLine(playerEntity.Symbol);
        }

        //
        //
        //
        public void DrawBattle(BattleStatus battleStatus)
        {
            switch (battleStatus.State)
            {
                case BattleState.NotInBattle:
                    Console.WriteLine("Not in battle");
                    break;


                case BattleState.InBattle:
                    Console.WriteLine("You are in a battle with <????> !");
                    foreach (var item in battleStatus.GetMessages())
                    {
                        Console.WriteLine(item);
                    }
                    break;

                case BattleState.Won:
                    Console.WriteLine("You won the battle!");
                    break;

                case BattleState.Lost:
                    Console.WriteLine("You lost the battle!");
                    break;
            }



        }
    }
}
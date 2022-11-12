using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class ZoneRendererAscii : IZoneRenderer
    {

        //
        // Draws a Zone
        //
        public void DrawZone(Zone zone)
        {
            Console.SetCursorPosition(0, 0);
            ConsoleUtils.DrawBox(0,0, zone.Width, zone.Height);
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
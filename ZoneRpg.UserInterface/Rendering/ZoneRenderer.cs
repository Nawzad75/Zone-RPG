using ZoneRpg.GameLogic;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class ZoneRenderer
    {

        // Draws a Zone
        public void DrawZone(Zone zone)
        {
            Console.SetCursorPosition(0, 0);
            ConsoleUtils.DrawBox(0, 0, zone.Width, zone.Height);
            Console.Write("\nZone: " );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(zone.Name);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Help: " );
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[T] "); 
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Send message  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[I] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Inventory  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("[Q] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Quit");
            Console.ResetColor();

            Console.WriteLine();
        }
        
        // Draws all entities in the zone
        public void DrawEntities(List<Entity> entities)
        {
            foreach (var item in entities)
            {
                Console.SetCursorPosition(item.X, item.Y);
                Console.WriteLine(item.Symbol);

            }
        }

        // Draw the player
        public void DrawPlayerEntity(Entity playerEntity)
        {
            Console.SetCursorPosition(playerEntity.X, playerEntity.Y);
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine(playerEntity.Symbol);
            Console.ResetColor();
        }


    }
}
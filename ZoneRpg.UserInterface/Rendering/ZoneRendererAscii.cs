using ZoneRpg.GameLogic;
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
            ConsoleUtils.DrawBox(0, 0, zone.Width, zone.Height);
            Console.WriteLine("\n                Zone: " + zone.Name);
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

      
    }
}
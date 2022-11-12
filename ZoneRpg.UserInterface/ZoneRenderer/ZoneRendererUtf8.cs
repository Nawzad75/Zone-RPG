using ZoneRpg.GameLogic;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class ZoneRendererUtf8 : IZoneRenderer
    {

        //
        // Draws a Zone
        //
        public void DrawZone(Zone zone)
        {

            string c1 = "🧱";
            string c2 = "🧱";
            string c3 = "🧱";
            string c4 = "🧱";
            string space = "🧱";
            string I = "🧱";
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
                    Console.Write("🏴");
                }
                Console.WriteLine(I);
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
                Console.SetCursorPosition(item.X * 2, item.Y);
                Console.WriteLine(
                    item.Symbol.ToString()
                        .Replace("P", "🏃")
                        .Replace("M", "👻")
                        .Replace("D", "🚪")
                        .Replace("K", "📦"));

            }
        }

        //
        // Draw the player
        //
        public void DrawPlayerEntity(Entity playerEntity)
        {
            Console.SetCursorPosition(playerEntity.X * 2, playerEntity.Y);
            Console.WriteLine(playerEntity.Symbol.ToString().Replace("P", "🏃"));
        }

        public void DrawBattle(BattleStatus v)
        {
            Console.WriteLine("Draw fight not implemented in ZoneVisualizerUtf8");
        }
    }
}
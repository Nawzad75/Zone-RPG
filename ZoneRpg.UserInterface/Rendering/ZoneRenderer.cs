using ZoneRpg.GameLogic;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal class ZoneRenderer
    {

        //
        // Draws a Zone
        //
        public void DrawZone(Zone zone)
        {
            Console.SetCursorPosition(0, 0);
            ConsoleUtils.DrawBox(0, 0, zone.Width, zone.Height);
            Console.WriteLine("\n Zone: " + zone.Name + "    [T] Send message  [I] Inventory  [Q] Quit");
            Console.WriteLine();
        }
        public void DrawChatBox(ChatBox chatBox, Game game)
        {
            int y = 0;

            foreach (var message in game.ChatBox.Messages.TakeLast(12))
            {
                Console.SetCursorPosition(60, y++);
                Console.Write(message.character.Name + " " + message.Text);
            }

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
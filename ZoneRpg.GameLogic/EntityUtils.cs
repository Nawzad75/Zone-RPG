using ZoneRpg.Models;
using ZoneRpg.Shared;

namespace ZoneRpg.GameLogic
{
    public class EntityUtils
    {
        // Kollar om spelaren kan kollidera med något
        public static CollisionMap GetCollisions(IFighter player, List<Entity> entities)
        {
            CollisionMap collisionMap = new();
            foreach (var entity in entities)
            {
                if (entity.EntityType == EntityType.Player
                   || entity.EntityType == EntityType.Monster
                   || entity.EntityType == EntityType.Stone)
                {
                    if ((player.GetX() - entity.X) == 1 && (player!.GetY() - entity.Y) == 0)
                    {
                        collisionMap.Left = true;
                    }
                    if ((player!.GetX() - entity.X) == -1 && (player!.GetY() - entity.Y) == 0)
                    {
                        collisionMap.Right = true;
                    }
                    if ((player!.GetY() - entity.Y) == 1 && (player!.GetX() - entity.X) == 0)
                    {
                        collisionMap.Up = true;
                    }
                    if ((player!.GetY() - entity.Y) == -1 && (player!.GetX() - entity.X) == 0)
                    {
                        collisionMap.Down = true;
                    }
                }
            }
            return collisionMap;
        }
    }
}
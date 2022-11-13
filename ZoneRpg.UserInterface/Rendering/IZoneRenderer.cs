using ZoneRpg.GameLogic;
using ZoneRpg.Shared;

namespace ZoneRpg.UserInterface
{
    internal interface IZoneRenderer
    {
        public void DrawZone(Zone zone);

        public void DrawEntities(List<Entity> entities);

        public void DrawPlayerEntity(Entity playerEntity);
    }
}
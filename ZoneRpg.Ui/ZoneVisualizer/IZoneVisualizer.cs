using ZoneRpg.Shared;

namespace ZoneRpg.Ui
{
    internal interface IZoneVisualizer
    {
        public void DrawZone(Zone zone);

        public void DrawEntities(List<Entity> entities);

        public void DrawPlayerEntity(Entity playerEntity);
    }
}
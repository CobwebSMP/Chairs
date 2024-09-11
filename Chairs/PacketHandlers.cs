using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;

namespace Chairs
{
    public class PacketHandlers : Plugin
    {
        [PacketHandler]
        public void EntityCheck(McpeInteract packet, Player source)
        {
            var action = (McpeInteract.Actions)packet.actionId;

            if (action == McpeInteract.Actions.LeaveVehicle && Chairs.ChairEntities.TryGetValue(source.EntityId, out long ChairID))
            {
                long id = packet.targetRuntimeEntityId;

                if (ChairID == id)
                {
                    if (source.Level.TryGetEntity(id, out Entity entity))
                    {
                        source.Level.RemoveEntity(entity);
                        Chairs.ChairEntities.Remove(source.EntityId);
                    }
                }
            }
        }
    }
}

using MiNET;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils.Vectors;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace Chairs
{
    public class Events
    {
        private static Dictionary<Player, DateTime> lastExecutionTimes = new Dictionary<Player, DateTime>();

        public static void PlayerOnLeave(object? sender, PlayerEventArgs e)
        {
            Player player = e.Player;

            lastExecutionTimes.Remove(player);
        }

        public static void LevelOnBlockInteract(object? sender, BlockInteractEventArgs e)
        {
            Block block = e.Block;
            Level level = e.Level;
            Player player = e.Player;

            if (block is BlockStairs stair)
            {
                if (!canExecute(player)) { return; }

                if (!stair.UpsideDownBit)
                {
                    var pitch = 90;
                    if (stair.WeirdoDirection == 1)
                    {
                        pitch = 270;
                    }
                    else if (stair.WeirdoDirection == 2)
                    {
                        pitch = 180;
                    }
                    else if (stair.WeirdoDirection == 3)
                    {
                        pitch = 360;
                    }

                    var entity = new Entity("minecraft:chicken", level);
                    entity.KnownPosition = new PlayerLocation(stair.Coordinates.X + 0.5f, stair.Coordinates.Y, stair.Coordinates.Z + 0.5f, pitch, pitch, 0);
                    entity.SpawnEntity();
                    entity.RiderRotationLocked = true;
                    entity.RiderMaxRotation = -90.0f;
                    entity.RiderMaxRotation = 90.0f;
                    entity.IsInvisible = true;
                    entity.BroadcastSetEntityData();

                    player.Vehicle = entity.EntityId;
                    Chairs.ChairEntities.Add(player.EntityId, player.Vehicle);

                    McpeSetEntityLink link = McpeSetEntityLink.CreateObject();
                    link.linkType = (byte)McpeSetEntityLink.LinkActions.Ride;
                    link.riderId = player.EntityId;
                    link.riddenId = entity.EntityId;
                    level.RelayBroadcast(link);

                    player.IsRiding = true;
                    MetadataDictionary metadata = player.GetMetadata();
                    metadata[56] = new MetadataVector3(0, 1.50f, 0);
                    metadata[57] = new MetadataByte(1);
                    metadata[58] = new MetadataFloat(90f);
                    metadata[59] = new MetadataFloat(-90f);
                    player.BroadcastSetEntityData(metadata);
                }
            }
        }

        public static bool canExecute(Player player)
        {
            DateTime currentTime = DateTime.Now;

            if (lastExecutionTimes.TryGetValue(player, out DateTime lastExecutionTime))
            {
                if ((currentTime - lastExecutionTime).TotalSeconds < 1)
                {
                    return false;
                }
            }

            lastExecutionTimes[player] = currentTime;
            return true;
        }
    }
}

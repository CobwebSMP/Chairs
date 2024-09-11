using log4net;
using MiNET;
using MiNET.Plugins;
using MiNET.Worlds;

namespace Chairs
{
    public class Chairs : Plugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Chairs));
        public static Dictionary<long, long> ChairEntities = new Dictionary<long, long>();

        protected override void OnEnable()
        {
            Log.Warn("Chairs 1.0 (Stair sitting plugin) Enabled.");

            var server = Context.Server;

            server.LevelManager.LevelCreated += (sender, args) =>
            {
                Level level = args.Level;
                level.BlockInteract += Events.LevelOnBlockInteract;
            };

            server.PlayerFactory.PlayerCreated += (sender, args) =>
            {
                Player player = args.Player;
                player.PlayerLeave += Events.PlayerOnLeave;
            };
        }
    }
}

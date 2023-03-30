using ConnectIt.Gameplay.Model;
using ConnectIt.Infrastructure.Registrators;

namespace ConnectIt.Gameplay
{
    public class GameStateObserver
    {
        private readonly Tilemaps _tilemaps;
        private readonly IRegistrator<Port> _portRegistrator;

        public GameStateObserver(Tilemaps tilemaps,
            IRegistrator<Port> portRegistrator)
        {
            _tilemaps = tilemaps;
            _portRegistrator = portRegistrator;
        }
    }
}

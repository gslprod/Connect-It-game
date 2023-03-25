using ConnectIt.Model;
using Zenject;

namespace ConnectIt.View
{
    public class PortView
    {
        private Port _portModel;

        public PortView(Port portModel)
        {
            _portModel = portModel;
        }

        public class Factory : PlaceholderFactory<Port, PortView> { }
    }
}

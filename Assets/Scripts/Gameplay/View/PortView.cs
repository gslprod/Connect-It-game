using ConnectIt.Gameplay.Model;
using System;
using Zenject;

namespace ConnectIt.Gameplay.View
{
    public class PortView : IInitializable, IDisposable
    {
        private Port _portModel;

        public PortView(Port portModel)
        {
            _portModel = portModel;
        }

        public void Initialize()
        {
            _portModel.Disposing += OnModeDisposing;
        }

        public void Dispose()
        {
            _portModel.Disposing -= OnModeDisposing;
        }

        private void OnModeDisposing(Port obj)
        {
            Dispose();
        }

        public class Factory : PlaceholderFactory<Port, PortView> { }
    }
}

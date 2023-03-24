using ConnectIt.Infrastructure.ModelAndView;
using ConnectIt.Model;
using Zenject;

namespace ConnectIt.View
{
    public class PortView : IView<Port>
    {
        public Port Model => _portModel;

        private Port _portModel;

        public void Init(Port model)
        {
            _portModel = model;
        }

        public class Factory : PlaceholderFactory<Port, PortView> { }
    }
}

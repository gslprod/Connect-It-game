﻿using ConnectIt.Infrastructure.ModelAndView;
using ConnectIt.Model;

namespace ConnectIt.View
{
    public class PortView : MonoBehaviourView<Port>
    {
        public override Port Model => _portModel;

        private Port _portModel;

        public override void Init(Port model)
        {
            _portModel = model;

            print(_portModel.Connectable.CompatibilityIndex);
        }
    }
}

using System;

namespace WGetNET
{
    internal class ActionUsedAttribute : Attribute
    {
        public MainAction[] MainActions { get { return _mainActions; } }

        private readonly MainAction[] _mainActions;

        public ActionUsedAttribute(params MainAction[] mainActions)
        {
            _mainActions = mainActions;
        }
    }
}

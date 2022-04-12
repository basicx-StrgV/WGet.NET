using System;
using System.Collections.Generic;
using System.Text;

namespace WGetNET
{
    internal class ActionBuilder
    {
        public string BuildActionArguments(MainAction mainAction, ActionParameter actionParameter)
        {
            StringBuilder argumentBuilder = new StringBuilder();

            argumentBuilder.Append(
                GetMainActionArgument(mainAction));

            return argumentBuilder.ToString();
        }

        private string GetMainActionArgument(MainAction mainAction)
        {
            return mainAction switch
            {
                MainAction.NONE => string.Empty,
                MainAction.INSTALL => "install",
                MainAction.SEARCH => "search",
                MainAction.LIST => "list",
                MainAction.UPGRADE => "upgrade",
                MainAction.UNINSTALL => "uninstall",
                MainAction.EXPORT => "export",
                MainAction.IMPORT => "import",
                MainAction.SOURCE_ADD => "source add",
                MainAction.SOURCE_LIST => "source list",
                MainAction.SOURCE_UPDATE => "source update",
                MainAction.SOURCE_REMOVE => "source remove",
                MainAction.SOURCE_RESET => "source reset",
                MainAction.SOURCE_EXPORT => "source export",
                _ => string.Empty
            };
        }
    }
}

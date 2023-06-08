using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.Constants
{
    public static class RecieveFunctionNames
    {
        //Method names to be used by clients.
        //They will get action through SignalR through below given method name
        public static string ProductAddedMessage = "recieveProductAddedMessage";
        public static string NewOrderMessage = "recieveOrderAddedMessage";
    }
}

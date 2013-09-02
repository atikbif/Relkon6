using System;
using System.Collections.Generic;
using System.Text;

namespace Kontel.Relkon
{
    public sealed class EthernetChannel : AbstractChannel
    {
        public EthernetChannel(ProtocolType protocol)          
        {
            _protocol = protocol;
            _minimalTimeout = 10000;
        }
    }
}

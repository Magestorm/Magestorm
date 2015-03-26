using System;
using System.Net;

namespace Helper.Network
{
    public static class NetHelper
    {
        public static Int16 FlipBytes(Int16 num)
        {
            return IPAddress.HostToNetworkOrder(num);
        }
        public static UInt16 FlipBytes(UInt16 num)
        {
            return (UInt16)IPAddress.HostToNetworkOrder((Int16)num);
        }

        public static Int32 FlipBytes(Int32 num)
        {
            return IPAddress.HostToNetworkOrder(num);
        }

        public static UInt32 FlipBytes(UInt32 num)
        {
            return (UInt32)IPAddress.HostToNetworkOrder((Int32)num);
        }
        
        public static Int64 FlipBytes(Int64 num)
        {
            return IPAddress.HostToNetworkOrder(num);
        }

        public static UInt64 FlipBytes(UInt64 num)
        {
            return (UInt64)IPAddress.HostToNetworkOrder((Int64)num);
        }
    }
}

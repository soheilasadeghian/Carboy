using CarBoyWebservice.MBProto.AuthObjectCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.MBProto
{
    public static class Contractors
    {
        public static Dictionary<uint, Type> constructors = new Dictionary<uint, Type>() {

            {0x2da9ea, typeof (LogoutConstructor)},
            {0x33526a, typeof (registerUserLocationConstructor)},
            {0x6d3f25, typeof (getUserProductListConstructor)},
            {0x81349b, typeof (getRequiredDistributorProductConstructor)},
            {0x4a9316, typeof (geDistributorStartLocationConstructor)},
            {0xada3e7, typeof (getCarboyPathConstructor)},
            {0xd7e58d, typeof (userRequestPassProductConstructor)},
            {0x6ea16b, typeof (userPassProductConstructor)},
            {0x58ee1c, typeof (carboyRequestServiceCompleteConstructor)},
            {0xf632a1, typeof (carboyServiceCompleteConstructor)},
            {0x7cb2c8, typeof (carboyRequestCancelConstructor)},
            {0x388d22, typeof (distributorRequestCancelConstructor)},
            {0x212c3c, typeof (carboyMoveToCustomerConstructor)},
            {0x1374c7, typeof (carboyStartServiceConstructor)},
            {0x761241, typeof (carboyServiceProductDeliveredConstructor)},
            {0x2fabf6, typeof (getCarboyServiceListConstructor)},
            {0x85a4e4, typeof (getCarboyServiceDetailConstructor)},
            {0x229a99, typeof (editCarboyProfileConstructor)},
            {0x1db7ba, typeof (getCarboyServiceLocationConstructor)},
            {0x27fced, typeof (getUserMessageListConstructor)},
            {0xf59af7, typeof (GetUserUnreadMessageCountConstructor)},
            {0x914774, typeof (registerUserPushConstructor)},
            {0x4e4c88, typeof (EditCustomerCarConstructor)},
            {0x368b26, typeof (getCarboyHistoryServiceListConstructor)},
        };
    }
}
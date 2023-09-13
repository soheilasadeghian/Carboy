using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarBoyWebservice.ClassCollection
{
    public enum ServiceOrderProductStatus
    {
        Orginal,
        New,
        Deleted
    }
    public enum ServiceOrderServiceStatus
    {
        Orginal,
        New,
        Deleted
    }
    public enum DeleteStatus
    {
        Active = 0,
        Block = 1,
        Deleted = 2
    }
    public enum RepairmanTimeSheetStatus
    {
        Available = 1,
        Reserved = 2,
        Vacation = 3,
        TempReserved = 4
    }
    public enum GiftType
    {
        ByOperator = 0, // کد ایجاد شده توسط اپراتور
        ByPresenter = 1,//کد ایجاد شده یک کد معرف است (یا همان کد مشتری) یا به عبارتی کد ایجاد شده جایزه ای برای یک معرفی شونده است
        ByRepresenter = 2, //کد ایجاد شده جایزه ای برای یک معرفی کننده است
        Periodica = 3, //نوع کارت هدیه ای از نوع دوره ای میباشد
        Mellat = 4, //نوع کارت هدیه ای از نوع باشگاه مشتریان بانک ملت میباشد
        OrderCount = 5//کارت هدیه جایزه برای تعدد سفارشات //AddOrderCount
    }
    public enum ServiceOrderPaymentStatus
    {
        No_Pay = 0,
        Pay_Online = 1,
        Pay_Cash = 2
    }
    public enum ServiceOrderState
    {
        Idle = 0,
        MoveToCustomer = 1,
        Proccessing = 2,
        Done = 3,
        CanceledByOperator = 4,
        CanceledByCustomer = 5
    }
    public enum CustomerMessageType
    {
        Canceled = 0,
        System = 1,
        CarmanChange = 2
    }
}
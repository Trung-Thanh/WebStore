using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Enums
{
    public enum OrderStatus
    {
        InProgress, // dang dat hang
        Confirmed,  // da xac nhan dat hang
        Shipping,   // dang ship
        Success,    // ship thanh cong
        Canceled    // da huy
    }
}

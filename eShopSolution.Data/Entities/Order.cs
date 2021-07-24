using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Order
    {
        // id luôn khác nhau - nên id này sẽ là khóa chính
        public int Id { set; get; }
        public DateTime OrderDate { set; get; }

        // user id là khóa ngoại
        public Guid UserId { set; get; }

        // thong tin người nhận
        public string ShipName { set; get; }
        public string ShipAddress { set; get; }
        public string ShipEmail { set; get; }
        public string ShipPhoneNumber { set; get; }
        public OrderStatus Status { set; get; }

        // the hien rang mot oder co mot list nhung order detail
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

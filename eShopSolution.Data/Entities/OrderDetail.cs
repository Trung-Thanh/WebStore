using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class OrderDetail
    {
        // order id nay se la khoa ngoai den bang order
        // tuong tu voi product id, ca 2 khoa ngoai nay se tao len 1 khoa chinh
        public int OrderId { set; get; }

        // 1 truong thong tin san pham
        public int ProductId { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }

        // order nay la dinh danh lay them thong tin ngay thang cua no
        public Order Order { get; set; }

        // bo sung cho 3 thuoc tinh co ban cua product tren
        public Product Product { get; set; }
    }
}

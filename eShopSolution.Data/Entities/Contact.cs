using eShopSolution.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Contact
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }

        // bình luật của khách hàng để lại
        // tại sao không sủ dụng user luôn ???
        // giống như cho toàn bộ người vào tuy cập trang web đều có thể để lại message - comments
        public string Message { set; get; }
        public Status Status { set; get; }

    }
}

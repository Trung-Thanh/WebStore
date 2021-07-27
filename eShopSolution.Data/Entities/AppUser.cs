using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class AppUser : IdentityUser<Guid> // <theo interface co <TKey>> // mặc định lớp cha gần nó nhất đang là string, ta có thể đổi lại
    {
        public DateTime Dob { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }

        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
        public List<Transaction> transactions { get; set; }


    }
}

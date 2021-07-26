using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace eShopSolution.Data.Entities
{
    public class Transaction // thong tin thanh toan
    {
        public int Id { set; get; }
        public DateTime TransactionDate { set; get; }
        public string ExternalTransactionId { set; get; } // id ben nhan
        public decimal Amount { set; get; }
        public decimal Fee { set; get; }
        public string Result { set; get; }
        public string Message { set; get; }
        public TransactionStatus Status { set; get; }
        public string Provider { set; get; } // nha cung cap

        public Guid UserId { get; set; } // nguoi thanht toan -- co nghia la dang ky thi moi duoc thanh toan

        public AppUser AppUser { get; set; }
    }
}

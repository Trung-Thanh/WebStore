using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Appication.DataTransferObject
{
    // dung de cac tham so xem xan pham, the loai ... ke thua, luon co 2 thuoc tinh page size va 
    public class PagingRequestBase
    {
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
    }
}

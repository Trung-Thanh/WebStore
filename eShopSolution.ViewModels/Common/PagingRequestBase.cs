using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    // dung de cac tham so xem xan pham, the loai ... ke thua, luon co 2 thuoc tinh page size va 
    public class PagingRequestBase
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}

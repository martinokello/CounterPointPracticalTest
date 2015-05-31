using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterPointDAL.MusicStoreDomainModel
{
    public class SalesLine
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Date Sold required")]
        [DataType(DataType.Date, ErrorMessage = "Date should be in the right format")]
        public DateTime? DateSold { get; set; }
        [Required(ErrorMessage = "Units Sold required")]
        public int UnitsSold { get; set; }
        public int salesLineId { get; set; }
    }
}

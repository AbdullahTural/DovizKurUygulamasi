using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doviz.Entities
{
    public class ParaBirimi
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Tanim { get; set; }
        public decimal UyariLimit { get; set; }

        public string Email { get; set; }
        public string AdSoyad { get; set; }
    }
}

using Insurance.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class SurchargeRate : IDtoEntity
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        public float Rate { get; set; }
    }
}

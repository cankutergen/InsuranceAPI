using Insurance.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Entities.Concrete
{
    public class ProductType : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool CanBeInsured { get; set; }
    }
}

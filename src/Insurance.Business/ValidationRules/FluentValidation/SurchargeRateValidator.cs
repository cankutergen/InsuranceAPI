using FluentValidation;
using Insurance.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insurance.Business.ValidationRules.FluentValidation
{
    public class SurchargeRateValidator : AbstractValidator<SurchargeRate>
    {
        public SurchargeRateValidator()
        {
            RuleFor(x => x.Rate)
                .NotNull()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ProductTypeId)
                .NotNull()
                .GreaterThan(0);
        }
    }
}

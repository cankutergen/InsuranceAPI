using System.Text.Json.Serialization;

namespace Insurance.Api.Models
{
    public class InsuranceApiModel
    {
        public int ProductId { get; set; }

        public float InsuranceValue { get; set; }
    }
}

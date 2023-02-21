namespace Insurance.Api.Models.SurchargeRate
{
    public class SurchargeResponseModel
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        public float Rate { get; set; }
    }
}

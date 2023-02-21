namespace Insurance.Api.Models.SurchargeRate
{
    public class SurchargePostRequestModel
    {
        public int ProductTypeId { get; set; }

        public float Rate { get; set; }
    }
}

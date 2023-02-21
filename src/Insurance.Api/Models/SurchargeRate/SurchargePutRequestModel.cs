namespace Insurance.Api.Models.SurchargeRate
{
    public class SurchargePutRequestModel
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        public float Rate { get; set; }
    }
}

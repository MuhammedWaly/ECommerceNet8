namespace ECommerceNet8.Core.DTOS.ProductDtos
{
    public class Model_BaseImageCustom
    {
        public long Id { get; set; }
        public DateTime AddedOn { get; set; }
        public int BaseProductId { get; set; }
        public string staticPath { get; set; }
    }
}

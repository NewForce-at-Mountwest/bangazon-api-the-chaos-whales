namespace BangazonAPI.Models
{
    public class Products
    {
        public int Id { get; set; }

        public int ProductTypeId { get; set; }

        ProductTypes productType { get; set; }

        public int Price { get; set; }

        public int Quantity { get; set; }

        public int CustomerId { get; set; }

        Customers customer { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}

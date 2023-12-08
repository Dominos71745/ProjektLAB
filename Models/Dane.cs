using System.ComponentModel.DataAnnotations;

namespace ProjektLAB.Models
{
    public class Dane
    {
        public class Categories 
        {
            [Key]
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }

            public ICollection<Cars> Cars { get; set; }
        }

        public class Cars
        {
            [Key]
            public int CarId { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }

            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public Categories Category { get; set; }

            public ICollection<Categories> Categories { get; set; }

            public ICollection<Orders> Orders { get; set; }
        }

        public class Clients
        {
            [Key]
            public int ClientId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Street { get; set; }
            public string PostCode { get; set; }
            public string City { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }

            public ICollection<Orders> Orders { get; set; }
        }

        public class Orders
        {
            [Key]
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }

            public int ClientId { get; set; }
            public Clients Client { get; set; }

            public int CarId { get; set; }
            public Cars Car {  get; set; }
        }
    }
}

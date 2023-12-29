using ProjektLAB.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektLAB.Models
{
    public class Dane
    {
        public class Categories 
        {
            [Key]
            public int CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public ICollection<Cars> Cars { get; set; }
        }

        public class Cars
        {
            [Key]
            public int CarId { get; set; }
            public string? Brand { get; set; }
            public string? Model { get; set; }

            public int CategoryId { get; set; }
            public string? CategoryName { get; set; }
            public Categories? Category { get; set; }
            public ICollection<Clients>? Clients { get; set; }
            public ICollection<Orders>? Orders { get; set; }
        }

        public class Clients
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string? ClientId { get; set; }
            [Required(ErrorMessage = "Please enter your first name")]
            public string FirstName { get; set; }
            [Required(ErrorMessage = "Please enter your last name")]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Please enter your street")]
            public string Street { get; set; }
            [Required(ErrorMessage = "Please enter your postal code")]
            public string PostCode { get; set; }
            [Required(ErrorMessage = "Please enter your city")]
            public string City { get; set; }
            [Required(ErrorMessage = "Please enter your phone number")]
            [Phone(ErrorMessage = "Please provide a valid phone number")]
            public string PhoneNumber { get; set; }
            [Required(ErrorMessage = "Please enter your email")]
            [RegularExpression(".+\\@.+\\.[a-z]{2,3}", ErrorMessage = "Please provide a valid email address")]
            [EmailAddress]
            public string Email { get; set; }
            public int CarId { get; set; }
            public Cars? Car { get; set; }  
            public ICollection<Orders>? Orders { get; set; }
        }

        public class Orders
        {
            [Key]
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public string? UserId { get; set; }
            public ApplicationUser? User { get; set; }
            public string? ClientId { get; set; }
            public Clients? Client { get; set; }
            public int CarId { get; set; }
            public Cars? Car {  get; set; }
            [Required(ErrorMessage = "Please enter a pickup date")]
            [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
            public DateTime PickupDate { get; set; }
            [Required(ErrorMessage = "Please enter return date")]
            [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
            public DateTime ReturnDate { get; set; }
        }
    }
}

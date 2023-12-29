using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjektLAB.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using static ProjektLAB.Models.Dane;

namespace ProjektLAB.Models.ViewModels
{
    public class ReservationViewModel
    {
        public Clients? Client { get; set; }
        public Cars? Car { get; set; }
        public Orders? Order { get; set; }
        public ApplicationUser? User { get; set; }
    }
}

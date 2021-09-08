using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Core.Domain.Models
{
    public class AccountForCreationDto
    {
        [Required]
        public string AccountId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

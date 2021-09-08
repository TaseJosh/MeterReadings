using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Core.Domain.Entities
{
    public class Account
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(50)] public int AccountId { get; set; }
        [Required] [MaxLength(100)] public string FirstName { get; set; }
        [Required] [MaxLength(100)] public string LastName { get; set; }
        public ICollection<MeterReading> MeterReadings { get; set; } = new List<MeterReading>();
    }
}

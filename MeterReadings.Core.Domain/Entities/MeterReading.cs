using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeterReadings.Core.Domain.Entities
{
    public class MeterReading
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(50)] public DateTime MeterReadingDateTime { get; set; }
        [Required] [MaxLength(5)] public int MeterReadValue { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")] public Account Account { get; set; }
    }
}
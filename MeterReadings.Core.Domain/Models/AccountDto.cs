using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Core.Domain.Models

{
    public class AccountDto
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Fees.Dto
{
    public class FeeDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsRecurring { get; set; } = false;
    }
}
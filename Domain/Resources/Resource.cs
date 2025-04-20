using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrefingreGymControl.Api.Domain.Resources
{
    public class Resource
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = default!;
    }
}
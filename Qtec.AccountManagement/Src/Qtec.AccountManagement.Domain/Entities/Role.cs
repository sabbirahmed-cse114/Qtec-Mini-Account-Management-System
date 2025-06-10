using System;

namespace Qtec.AccountManagement.Domain.Entities
{
    public class Role : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

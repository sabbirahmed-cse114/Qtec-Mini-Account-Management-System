

namespace Qtec.AccountManagement.Domain.Entities
{
    public class Account : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? ParentId { get; set; }
    }
}

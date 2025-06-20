namespace Qtec.AccountManagement.Domain.Dtos
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Guid? ParentId { get; set; }
        public string ParentName { get; set; }
        public List<AccountDto> Children { get; set; } = new List<AccountDto>();
        public int Level { get; set; } = 0;
    }
}
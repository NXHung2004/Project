namespace Project.Dto
{
    public class CrudUserDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public Guid? RoleId { get; set; }
        public bool Status { get; set; }
    }
}

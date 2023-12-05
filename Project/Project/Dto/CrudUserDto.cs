namespace Project.Dto
{
    public class CrudUserDto
    {
        public Guid? Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public Guid? RoleId { get; set; }
        public bool Status { get; set; }
        public string? UserNew { get; set; }
        public DateTime? TimeNew { get; set; }
        public string? UserEdit { get; set; }
        public DateTime? TimeEdit { get; set; }
    }
}

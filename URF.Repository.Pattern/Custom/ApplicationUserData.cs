namespace Repository.Pattern
{
    public class ApplicationUserData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        //public string SessionId { get; set; }
        public string[] Roles { get; set; }

        public string JobRole { get; set; }
        public string Department { get; set; }
        public int? DepartmentId { get; set; }
    }
}

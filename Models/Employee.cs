namespace EmployeeMgmnt.Models
{
    public class Employee
    {
        public int EmpId { get; set; }
        public string? EmpName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int ReportingManager { get; set; }

    }
}

namespace EmployeeMgmnt.Models
{
    public class Leave
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public DateTime LeaveDate { get; set; }
        public string? status { get; set; }
    }
}

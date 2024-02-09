using EmployeeManagementAPI.DTOS;
using EmployeeMgmnt.DTOS;
using EmployeeMgmnt.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Diagnostics.Eventing.Reader;

namespace EmployeeManagementAPI.Data
{
    public class DataManager
    {
        private static DataManager _instance;

        private static List<Employee> _employees;

        private static List<Leave> _leaves;
        
        private DataManager()
        {

            _employees = new List<Employee>();
            _leaves = new List<Leave>();

            var superAdmin = new Employee
            {
                EmpId = 1,
                EmpName = "admin",
                Password = "string",
                Role = "admin",
                ReportingManager = 0
            };
            _employees.Add(superAdmin);
        }

        public static DataManager Instance => _instance ?? (_instance = new DataManager());

        public Employee AddEmployee(EmployeeDTO employeeDto, int empId)
        {

            var loggedInEmployee = DataManager.Instance.GetEmployeeById(empId);

            if (loggedInEmployee.Role == "admin" || loggedInEmployee.Role == "manager")
            {
                

                Employee newEmp = new Employee
                {
                    EmpId = _employees.Count + 1,
                    EmpName = employeeDto.EmpName,
                    Password = employeeDto.Password,
                    Role = employeeDto.Role,
                    ReportingManager = loggedInEmployee.EmpId

                };

                _employees.Add(newEmp);
                return newEmp;
            }

            else
            {
                throw new UnauthorizedAccessException("Only managers and admin can add employees");
            }
            
            
        }

        public Employee GetEmployeeById(int empId)
        {
            return _employees.FirstOrDefault(e => e.EmpId == empId);
        }


        public IEnumerable<Employee> GetAllEmployees(int loggedEmpId)
        {
            return _employees.Where(e => e.ReportingManager == loggedEmpId);
        }



        public Leave AddLeave(LeaveDTO leaveDto, int empId)
        {

            Leave leave = new Leave
            {
                LeaveId = _leaves.Count + 1,
                EmpId = empId,
                LeaveDate = leaveDto.LeaveDate,
                status = "Pending"
            };

            _leaves.Add(leave);
            return leave;
        }
        public void ApproveLeave(int empId, int leaveId)
        {
            Leave userLeaveRequest = _leaves.Find(leave => leave.LeaveId == leaveId && leave.EmpId == empId);
            if (userLeaveRequest == null)
            {

                throw new ArgumentException("Leave request not found", nameof(leaveId));
            }
            userLeaveRequest.status = "Approved";
        }

        public Leave LeaveStatus(int empId, int leaveId)
        {
            return _leaves.Find(leave => leave.LeaveId == leaveId && leave.EmpId == empId);
        }
    }
}

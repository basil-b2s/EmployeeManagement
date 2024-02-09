using EmployeeManagementAPI.DTOS;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeManagementAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using EmployeeMgmnt.DTOS;
using EmployeeMgmnt.Models;

namespace EmployeeMgmnt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        [HttpPost("/employee")]
        [Authorize(Roles = "admin, manager")]
        public IActionResult AddEmployee([FromBody] EmployeeDTO employeeDto)

        {
            
            var empId = int.Parse(HttpContext.Request.Cookies["empId"]);
           // var empId = GetCurrentUserId();

            Console.WriteLine($"{empId}");


            try
            {
                
                Employee employee = DataManager.Instance.AddEmployee(employeeDto, empId);
                var response = new
                {
                    Employee = employee,
                    Message = "Employee added successfully"
                };
                return Ok(response);
            }catch (Exception ex) 
            {
                
                return StatusCode(404, "You are not authorized to perform this action");
            }
        }

        [HttpGet("/employee/{id}")]
        [Authorize(Roles ="admin,manager")]
        public IActionResult GetEmployee(int id)
        {
            var employee = DataManager.Instance.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }
            return Ok(employee);
        }

        [HttpGet("/employess")]
        [Authorize(Roles = "admin, manager")]
        public IActionResult GetAllEmployees()
        {
            var loggedInEmployeeId = HttpContext.Request.Cookies["empId"];
            //var employees = DataManager.Instance.GetAllEmployees(int.Parse(loggedInEmployeeId));
            //return Ok(employees);
            try
            {
                var employees = DataManager.Instance.GetAllEmployees(int.Parse(loggedInEmployeeId));
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return Forbid("Authenticatoin error occured", ex.Message);
            }

        }



        [HttpPost("/leave")]
        public IActionResult ApplyForLeave([FromBody] LeaveDTO leaveDto)
        {
            Console.WriteLine(leaveDto);
            var empId = int.Parse(HttpContext.Request.Cookies["empId"]);
            Console.WriteLine(empId);
            Leave leave = DataManager.Instance.AddLeave(leaveDto, empId);
            var response = new
            {
                Leave = leave,
                Message = "Leave Requested... Please wait for the manager's approval"
            };
            return Ok(response);
        }



        [HttpPost("/employee/{empId}/leave/{leaveId}")]
        [Authorize(Roles = "manager, admin")]
        public IActionResult ApproveLeaveRequest(int empId, int leaveId)
        {
            DataManager.Instance.ApproveLeave(empId, leaveId);
            return Ok("Leave request for the employee has been approved");
        }



        [HttpPost("/leavestatus/{leaveId}")]
        public IActionResult LeaveStatus(int leaveId)
        {

            int empId = int.Parse(HttpContext.Request.Cookies["empId"]);
            try
            {
                Leave empLeaveStatus = DataManager.Instance.LeaveStatus(empId, leaveId);
                return Ok(empLeaveStatus);
            }catch(Exception ex)
            {
                return Forbid("Error Occured");
            }
            
            
        }

/*        private int GetCurrentUserId()
        {
            Console.WriteLine("hii");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            
            return 0;
        }*/
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication2MVC.Models;

namespace WebApplication2MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeController
        IConfiguration _configuration;
        SqlConnection _Connection;
        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
            _Connection = new SqlConnection(_configuration.GetConnectionString("EmployeeDB"));
        }


        public List<EmployeeModel> GetEmployee()
        {
            //Console.WriteLine("Inside Get Employee");
            List<EmployeeModel> employee = new List<EmployeeModel>();
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("FETCH_EMPLOYEE", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                EmployeeModel emp = new();
                emp.empID = (int)reader["EmpId"];
                emp.empName = (string)reader["empName"];
                emp.empDOB = (DateTime)reader["dateOfBirth"];
                emp.empDept = (string)reader["department"];
                emp.empTec = (string)reader["technology"];
                emp.empLoc = (string)reader["baseLocation"];
                emp.empSalary = (int)(double)reader["salary"];

                //Console.WriteLine("Loaded!");
                employee.Add(emp);
            }

            //Console.WriteLine(employee);
            reader.Close();

            _Connection.Close();
            return employee;
        }
        public ActionResult Index()
        {
            return View(GetEmployee());
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        EmployeeModel GetEmployeeDetail(int id)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("GetEmployeeDetail", _Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empId", id);
            SqlDataReader reader = cmd.ExecuteReader();
            EmployeeModel emp = new();
            while (reader.Read())
            {
                emp.empID = (int)reader["EmpId"];
                emp.empName = (string)reader["empName"];
                emp.empDOB = (DateTime)reader["dateOfBirth"];
                emp.empDept = (string)reader["department"];
                emp.empTec = (string)reader["technology"];
                emp.empLoc = (string)reader["baseLocation"];
                emp.empSalary = (int)(double)reader["salary"];
            }
            return emp;

        }

        // GET: EmployeeController/Edit/5


        public ActionResult Edit(int id)
        {
            
            return View(GetEmployeeDetail(id));
        }



        void UpdateEmployee(int id, EmployeeModel emp)
        {
            _Connection.Open();
            SqlCommand cmd = new SqlCommand("EDIT_EMPLOYEE", _Connection);


            Console.WriteLine(emp.empID + emp.empDept, emp.empLoc, emp.empSalary) ;
            //cmd.Parameters.AddWithValue("@empId", id);

            cmd.Parameters.AddWithValue("@empDept", emp.empDept);
            cmd.Parameters.AddWithValue("@empTec", emp.empTec);
            cmd.Parameters.AddWithValue("@empSalary", emp.empSalary);
            cmd.Parameters.AddWithValue("@empLoc", emp.empLoc);
            cmd.Parameters.AddWithValue("@empId", emp.empID);

            Console.WriteLine("IN UPDATE EMPLOYEE!");

            cmd.ExecuteNonQuery();
            _Connection.Close();
        }
        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeModel emp)
        {

            Console.WriteLine("Inside Post menthid");
            try
            {
                UpdateEmployee(id,emp);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return View();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

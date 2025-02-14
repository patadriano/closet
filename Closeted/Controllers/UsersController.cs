using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Closeted.Data;
using Closeted.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Closeted.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Users
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.User.ToListAsync());
        //}

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendLine("INSERT INTO [User] (Username, Password) VALUES (@Username, @Password);");


                try
                {
                    using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                    {
                        await oConnection.OpenAsync(); // Open connection asynchronously

                        using (SqlCommand oCommand = new SqlCommand())
                        {
                            oCommand.Connection = oConnection;
                            oCommand.CommandText = sQuery.ToString();
                            oCommand.CommandType = CommandType.Text;

                            // Add parameters to prevent SQL injection
                            oCommand.Parameters.AddWithValue("@Username", user.Username);
                            oCommand.Parameters.AddWithValue("@Password", user.Password);

                            int rowsAffected = await oCommand.ExecuteNonQueryAsync(); // Execute the query

                            if (rowsAffected > 0)
                            {
                               return RedirectToAction("Index1");
                            }
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

              

            }
              return View();
        }

        // GET: Users/Index1
        public IActionResult Index1()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index1([Bind("Id,Username,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sQuery = new StringBuilder();
                sQuery.AppendLine("SELECT Username, Password FROM [User] WHERE Username = @Username AND Password = @Password;");

                try
                {
                    using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                    {
                        await oConnection.OpenAsync(); // Open connection asynchronously

                        using (SqlCommand oCommand = new SqlCommand())
                        {
                            oCommand.Connection = oConnection;
                            oCommand.CommandText = sQuery.ToString();
                            oCommand.CommandType = CommandType.Text;

                            // Add parameters to prevent SQL injection
                            oCommand.Parameters.AddWithValue("@Username", user.Username);
                            oCommand.Parameters.AddWithValue("@Password", user.Password);

                            using (SqlDataReader reader = await oCommand.ExecuteReaderAsync()) // Execute the query
                            {
                                if (await reader.ReadAsync()) // Check if a record was returned
                                {
                                    Console.WriteLine("Login successful!"); 
                                    return RedirectToAction("Create");

                                    // You can also set some session or cookie here to track the user
                                }
                                //else
                                //{
                                //    Console.WriteLine("Invalid username or password.");
                                //}
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}

using Closeted.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace Closeted.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            //Full full = new Full();
            //full.top = GetTop();
            //full.bottom = GetBottom();
            //full.headwear = GetHeadwear();
            //return View(full);
            ViewData["Top"] = GetTop();
            ViewData["Bottom"] = GetBottom();
            ViewData["Headwear"] = GetHeadwear();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public List<Top> GetTop()
        {
            
            List<Top> topList = new List<Top>();

            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("SELECT Name FROM [Top];");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;

                        
                        using (SqlDataReader reader = oCommand.ExecuteReader())
                        {
                            // Check if there are any results
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Top top = new Top();

                                    
                                    top.Name = reader["Name"].ToString();

                                    
                                    topList.Add(top);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return topList;
        }

        public List<Bottom> GetBottom()
        {
          
            List<Bottom> bottomList = new List<Bottom>();

            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("SELECT Name FROM [Bottom];");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;


                        using (SqlDataReader reader = oCommand.ExecuteReader())
                        {
                            // Check if there are any results
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {  Bottom bottom = new Bottom();
                                    
                                    bottom.Name = reader["Name"].ToString();
                                    bottomList.Add(bottom);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return bottomList;
        }

        public List<Headwear> GetHeadwear()
        {
            
            List<Headwear> headwearList = new List<Headwear>();

            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("SELECT Name FROM [Headwear];");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;


                        using (SqlDataReader reader = oCommand.ExecuteReader())
                        {
                            // Check if there are any results
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {Headwear headwear = new Headwear();
                                    // Read data for each row (example: retrieve the "Name" column)
                                    headwear.Name = reader["Name"].ToString();
                                    headwearList.Add(headwear);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No rows found.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return headwearList;
        }
        [HttpPost]
        public ActionResult Top(Top top)
        {
           
            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("INSERT INTO [Top] (Name) VALUES (@Name);");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                     oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;

                        // Add parameters to prevent SQL injection
                        oCommand.Parameters.AddWithValue("@Name", top.Name);


                        /*int rowsAffected = oCommand.ExecuteNonQuery();*/ // Execute the query
                        oCommand.ExecuteNonQuery();

                      

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return RedirectToAction("Privacy");
        }
        [HttpPost]
        public ActionResult Bottom(Bottom bottom)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("INSERT INTO [Bottom] (Name) VALUES (@Name);");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;

                        // Add parameters to prevent SQL injection
                        oCommand.Parameters.AddWithValue("@Name", bottom.Name);


                        /*int rowsAffected = oCommand.ExecuteNonQuery();*/ // Execute the query
                        oCommand.ExecuteNonQuery();

                        //if (rowsAffected > 0)
                        //{
                        //    return RedirectToAction("Index1");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return RedirectToAction("Privacy");
        }
        [HttpPost]
        public ActionResult Headwear(Headwear headwear)
        {
            StringBuilder sQuery = new StringBuilder();
            sQuery.AppendLine("INSERT INTO [Headwear] (Name) VALUES (@Name);");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open(); // Open connection asynchronously

                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;

                        // Add parameters to prevent SQL injection
                        oCommand.Parameters.AddWithValue("@Name", headwear.Name);


                        /*int rowsAffected = oCommand.ExecuteNonQuery();*/ // Execute the query
                        oCommand.ExecuteNonQuery();

                        //if (rowsAffected > 0)
                        //{
                        //    return RedirectToAction("Index1");
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return RedirectToAction("Privacy");
        }
        
    }
}

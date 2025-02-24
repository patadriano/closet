using Closeted.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
            sQuery.AppendLine("SELECT Image FROM [Headwear];");

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
                {
                    oConnection.Open();
                    using (SqlCommand oCommand = new SqlCommand())
                    {
                        oCommand.Connection = oConnection;
                        oCommand.CommandText = sQuery.ToString();
                        oCommand.CommandType = CommandType.Text;

                        using (SqlDataReader reader = oCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var headwear = new Headwear
                                    {
                                        
                                        Image = reader["Image"] as byte[]  // Cast to byte[]
                                    };
                                    headwearList.Add(headwear);
                                }
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
        public IActionResult Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Convert the uploaded image into a byte array
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                // Save the image to the database
                SaveImageToDatabase( imageData);

                return Content("Image uploaded successfully.");
            }
            else
            {
                return Content("No file selected.");
            }
        }

        private void SaveImageToDatabase(byte[] imageData)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
            {
                string query = "INSERT INTO Headwear (Image) VALUES (@Image)";
                SqlCommand cmd = new SqlCommand(query, conn);
                
                cmd.Parameters.AddWithValue("@Image", imageData);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine(ex.Message);
                }
            }
        }


    }
}

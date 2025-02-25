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
            
            ViewData["Top"] = GetTop();
            ViewData["Bottom"] = GetBottom();
            ViewData["Headwear"] = GetHeadwear();
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Top"] = GetTop();
            ViewData["Bottom"] = GetBottom();
            ViewData["Headwear"] = GetHeadwear();
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
            sQuery.AppendLine("SELECT Image FROM [Top];");

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
                            // Check if there are any results
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Top top = new Top();


                                    top.Image = reader["Image"] as byte[];


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
            sQuery.AppendLine("SELECT Image FROM [Bottom];");

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

                                    bottom.Image = reader["Image"] as byte[];
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
        public IActionResult Top(IFormFile file)
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
                SaveImageToDatabase3(imageData);

                return RedirectToAction("Index");
            }
            else
            {
                return Content("No file selected.");
            }
        }
        [HttpPost]
        public IActionResult Bottom(IFormFile file)
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
                SaveImageToDatabase2(imageData);

                return RedirectToAction("Index");
            }
            else
            {
                return Content("No file selected.");
            }
        }
        [HttpPost]
        public IActionResult Headwear(IFormFile file)
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
                SaveImageToDatabase1( imageData);

                return RedirectToAction("Index");
            }
            else
            {
                return Content("No file selected.");
            }
        }

        private void SaveImageToDatabase1(byte[] imageData)
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
        private void SaveImageToDatabase2(byte[] imageData)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
            {
                string query = "INSERT INTO Bottom (Image) VALUES (@Image)";
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
        private void SaveImageToDatabase3(byte[] imageData)
        {
            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("MVCCRUD")))
            {
                string query = "INSERT INTO [Top] (Image) VALUES (@Image)";
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

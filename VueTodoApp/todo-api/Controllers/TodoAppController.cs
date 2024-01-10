using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace todo_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoAppController : ControllerBase
    {
        private IConfiguration _configuration;

        public TodoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
          
        }

        /// <summary>
        /// This is all bad practice but for the sake of testing Vue it will do for now.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNotes")]
        public JsonResult GetNotes()
        {
            string query = "select * from dbo.notes";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader reader;

            using (SqlConnection conn = new(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new(query, conn))
                {
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    conn.Close();
                }           
            }

            return new JsonResult(table);
        }

        [HttpPost]
        [Route("AddNote")]
        public JsonResult AddNote([FromForm] string newNotes)
        {
            string query = "insert into dbo.notes values (@newNotes)";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader reader;

            using (SqlConnection conn = new(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@newNotes", newNotes);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    conn.Close();
                }
            }

            return new JsonResult("New note added");
        }


        [HttpDelete]
        [Route("DeleteNote")]
        public JsonResult DeleteNote(int id)
        {
            string query = "delete from dbo.notes where id = @id";
            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("todoAppDBCon");
            SqlDataReader reader;

            using (SqlConnection conn = new(sqlDataSource))
            {
                conn.Open();
                using (SqlCommand cmd = new(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    reader = cmd.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    conn.Close();
                }
            }

            return new JsonResult("Deleted note");
        }
    }
}

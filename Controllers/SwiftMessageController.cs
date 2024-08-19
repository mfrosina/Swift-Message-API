namespace SwiftMessageAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Data.Sqlite;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using SwiftMessageAPI.Models;
    using System.IO;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class SwiftMessageController : ControllerBase
    {
        private readonly ILogger<SwiftMessageController> _logger;
        private readonly IConfiguration _configuration;

        public SwiftMessageController(ILogger<SwiftMessageController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadSwiftMessage([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not provided or empty.");

            using (var streamReader = new StreamReader(file.OpenReadStream()))
            {
                var content = await streamReader.ReadToEndAsync();
                var swiftMessage = ParseSwiftMessage(content);

                _logger.LogInformation("Parsed Swift Message: {@SwiftMessage}", swiftMessage);

                SaveToDatabase(swiftMessage);

                return Ok(swiftMessage);
            }
        }

        private SwiftMessage ParseSwiftMessage(string content)
        {
            var swiftMessage = new SwiftMessage
            {
                Block1 = ExtractBlock(content, "{1:", "}"),
                Block2 = ExtractBlock(content, "{2:", "}"),
                Block4_20 = ExtractBlock(content, ":20:", " "),
                Block4_21 = ExtractBlock(content, ":21:", " "),
                Block4_79 = ExtractBlock(content, ":79:", "-}")
            };

            return swiftMessage;
        }

        private string ExtractBlock(string content, string start, string end)
        {
            var startIndex = content.IndexOf(start) + start.Length;
            var endIndex = content.IndexOf(end, startIndex);
            return content.Substring(startIndex, endIndex - startIndex);
        }

        private void SaveToDatabase(SwiftMessage message)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS SwiftMessages (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Block1 TEXT,
                        Block2 TEXT,
                        Block4_20 TEXT,
                        Block4_21 TEXT,
                        Block4_79 TEXT
                    )";
                    command.ExecuteNonQuery();

                    command.CommandText = @"
                    INSERT INTO SwiftMessages (Block1, Block2, Block4_20, Block4_21, Block4_79)
                    VALUES (@Block1, @Block2, @Block4_20, @Block4_21, @Block4_79)";
                    command.Parameters.AddWithValue("@Block1", message.Block1);
                    command.Parameters.AddWithValue("@Block2", message.Block2);
                    command.Parameters.AddWithValue("@Block4_20", message.Block4_20);
                    command.Parameters.AddWithValue("@Block4_21", message.Block4_21);
                    command.Parameters.AddWithValue("@Block4_79", message.Block4_79);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving to database");
                throw;
            }
        }
    }
}

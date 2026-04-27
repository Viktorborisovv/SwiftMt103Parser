namespace SwiftMt103Parser.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SwiftMt103Parser.Api.DTOs;
    using SwiftMt103Parser.Api.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class SwiftMessagesController : ControllerBase
    {
        private readonly SwiftMessageService swiftMessageService;

        public SwiftMessagesController(SwiftMessageService swiftMessageService)
        {
            this.swiftMessageService = swiftMessageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid MT103 file.");
            }

            using StreamReader reader = new StreamReader(file.OpenReadStream());
            string rawMessage = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(rawMessage))
            {
                return BadRequest("The uploaded file is empty.");
            }

            SwiftMessageResponse response = await swiftMessageService.CreateFromTextAsync(rawMessage);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPost("text")]
        public async Task<IActionResult> CreateFromText(CreateSwiftMessageRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.RawMessage))
            {
                return BadRequest("RawMessage is required.");
            }

            SwiftMessageResponse response = await swiftMessageService.CreateFromTextAsync(request.RawMessage);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<SwiftMessageResponse> messages = await swiftMessageService.GetAllAsync();

            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            SwiftMessageResponse? messageResponse = await swiftMessageService.GetByIdAsync(id);

            if (messageResponse == null)
            {
                return NotFound();
            }

            return Ok(messageResponse);
        }
    }
}

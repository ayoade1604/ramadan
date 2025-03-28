using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BirthdayCardGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private static Dictionary<string, CardData> _cards = new Dictionary<string, CardData>();

        [HttpPost("save")]
        public IActionResult SaveCard([FromBody] CardData cardData)
        {
            if (cardData == null)
            {
                return BadRequest("Invalid card data");
            }

            // Generate a unique ID for the card
            string cardId = Guid.NewGuid().ToString();

            // Store the card data
            _cards[cardId] = cardData;

            // Return the card ID
            return Ok(new { id = cardId });
        }

        [HttpGet("{id}")]
        public IActionResult GetCard(string id)
        {
            if (_cards.TryGetValue(id, out var cardData))
            {
                return Ok(cardData);
            }

            return NotFound("Card not found");
        }
    }

    public class CardData
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string From { get; set; }
        public string BgColor { get; set; }
        public string TextColor { get; set; }
        public string FontFamily { get; set; }
        public string FontSize { get; set; }
        public string Template { get; set; }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Fallback to index.html for SPA routing
app.MapFallbackToFile("index.html");

app.Run();
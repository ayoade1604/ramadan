 namespace RamadanGreetingsAPI.Models
{
    public class Greeting
    {
        public string Template { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Signature { get; set; }
        public string ColorScheme { get; set; }
    }
}
using Microsoft.AspNetCore.Mvc;
using RamadanGreetingsAPI.Models;
using System.Collections.Generic;

namespace RamadanGreetingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingsController : ControllerBase
    {
        private static List<Greeting> greetings = new List<Greeting>();

        [HttpPost]
        public IActionResult SaveGreeting([FromBody] Greeting greeting)
        {
            if (greeting == null)
            {
                return BadRequest("Greeting cannot be null.");
            }

            greetings.Add(greeting);
            return Ok(new { Message = "Greeting saved successfully!" });
        }

        [HttpGet]
        public IActionResult GetGreetings()
        {
            return Ok(greetings);
        }
    }
}
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
saveBtn.addEventListener('click', async function() {
  // Create URL parameters
  const params = {
      template: currentTemplate,
      title: titleInput.value,
      message: messageInput.value,
      signature: signatureInput.value,
      colorScheme: colorSchemeSelect.value
  };

  // Send POST request to save the greeting
  const response = await fetch('/api/greetings', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(params)
  });

  if (response.ok) {
      const data = await response.json();
      // Generate URL
      const url = `${window.location.origin}${window.location.pathname}?template=${currentTemplate}&title=${encodeURIComponent(titleInput.value)}&message=${encodeURIComponent(messageInput.value)}&signature=${encodeURIComponent(signatureInput.value)}&colorScheme=${colorSchemeSelect.value}`;
      
      // Show share section
      shareSection.style.display = 'block';
      shareUrlInput.value = url;

      // Scroll to share section
      shareSection.scrollIntoView({ behavior: 'smooth' });
  } else {
      alert('Failed to save greeting.');
  }
});
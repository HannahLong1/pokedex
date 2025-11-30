using Microsoft.AspNetCore.Mvc;
using PokeApiDemo.Models;
using System.Text.Json;

namespace PokeApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public PokemonController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetPokemon(string name)
        {
            var url = $"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound($"Pokemon '{name}' not found.");
            }

            var jsonString = await response.Content.ReadAsStringAsync();

            var pokemon = JsonSerializer.Deserialize<PokemonResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return Ok(pokemon);
        }
    }
}
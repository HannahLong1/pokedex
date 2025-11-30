namespace PokeApiDemo.Models;

public class PokemonResponse {
    public int id { get; set; }
    public string name { get; set; }
    public int weight { get; set; }
    public int height { get; set; }
    public Sprites sprites { get; set; }
}

public class Sprites {
    public string front_default { get; set; }
}
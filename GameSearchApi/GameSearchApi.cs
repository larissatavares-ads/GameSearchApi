using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace GameSearchApi
{
    public static class GameSearchApi 
    {

        static HttpClient client = new HttpClient();

        public static void MapHttpRoutes(this IEndpointRouteBuilder app)
        {
            app.MapGet ("api/games/getall", async ([AsParameters]Game game) =>
            {
                Console.WriteLine($"Id: {game.Id}\tName: {game.Name}\tDescription: " +
                    $"{game.Description}\tGenre: {game.Genre}\tDate of creation: {game.DateOfCreation}");
            });

            app.MapGet("api/games/get", async () =>            
            {
                try
                {
                    using (StreamReader reader = new StreamReader("games.json"))
                    {
                        var json = reader.ReadToEnd();
                        //var itens = File.ReadAllText(json);
                        var games = JsonSerializer.Deserialize<List<Game>>(json)!;

                        return Results.Ok(games);
                    }
                }
                catch (Exception ex) 
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPost("api/games/post", async (Game game) => 
            {
                try
                {
                    List<Game> listGames = new List<Game>();

                    using (StreamReader reader = new StreamReader("games.json")) 
                    {
                        var json = reader.ReadToEnd();
                        var games = JsonSerializer.Deserialize<List<Game>>(json)!;

                        listGames.AddRange(games);

                        var last = games[games.Count - 1].Id + 1;
                        game.Id = last;

                        listGames.Add(game);

                        reader.Close();
                    }

                    using (StreamWriter writer = new StreamWriter("games.json"))
                    {
                        writer.WriteLine(JsonSerializer.Serialize(listGames));
                        writer.Close();
                    }                        

                    return Results.Ok("");
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPut($"api/games/update", async (int id) => {

            });

            app.MapDelete("api/games/delete", async (int id) =>
            {

            });

            app.MapPatch("api/games/patch", async (int id) =>
            {

            });

            static void ShowGame(Game game)
            {
                Console.WriteLine($"Id: {game.Id}\tName: {game.Name}\tDescription: " +
                    $"{game.Description}\tGenre: {game.Genre}\tDate of creation: {game.DateOfCreation}");
            }

            static async Task<Game> GetGameAsync(string path, CancellationToken cancellationToken)
            {
                Game game = null;

                HttpResponseMessage response = await client.GetAsync(path, cancellationToken: cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    game = await response.Content.ReadFromJsonAsync<Game>();
                }
                return game;
            }

            static async Task<Game> UpdateGameAsync(Game game, CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync($"api/games/{game.Id}", game, cancellationToken: cancellationToken);
                response.EnsureSuccessStatusCode();

                game = await response.Content.ReadFromJsonAsync<Game>();
                return game;
            }

            static async Task<HttpStatusCode> DeleteGameAsync(string id, CancellationToken cancellationToken)
            {
                HttpResponseMessage response = await client.DeleteAsync($"api/games/{id}", cancellationToken: cancellationToken);
                return response.StatusCode;
            }
        }
        
    }
}

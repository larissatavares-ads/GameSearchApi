using GameSearchApi.Model;
using System.Net;
using System.Text.Json;

namespace GameSearchApi.Endpoint
{
    public static class GameSearchApi
    {

        static HttpClient client = new HttpClient();

        public static void MapHttpRoutes(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/games/get", async () =>
            {
                try
                {
                    using (StreamReader reader = new StreamReader("games.json"))
                    {
                        var json = reader.ReadToEnd();
                        var games = JsonSerializer.Deserialize<List<Game>>(json)!;

                        return Results.Ok(games);
                    }
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPost("api/games/post", async (GameRequest gameRequest) =>
            {
                try
                {
                    Game game = new Game();
                    List<Game> listGames = new List<Game>();

                    using (StreamReader reader = new StreamReader("games.json"))
                    {
                        var json = reader.ReadToEnd();
                        var games = JsonSerializer.Deserialize<List<Game>>(json)!;

                        listGames.AddRange(games);

                        var last = games[games.Count - 1].Id + 1;
                        game.Id = last;
                        game.Name = gameRequest.Name;
                        game.Description = gameRequest.Description;
                        game.Genre = gameRequest.Genre;
                        game.DateOfCreation = gameRequest.DateOfCreation;

                        listGames.Add(game);

                        reader.Close();
                    }

                    using (StreamWriter writer = new StreamWriter("games.json"))
                    {
                        writer.WriteLine(JsonSerializer.Serialize(listGames));
                        writer.Close();
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPut("api/games/update/{id}", async (int id, GameRequest gameRequest) =>
            {
                try
                {
                    List<Game> gamesUpdated = new List<Game>();
                    Game gameToUpdate = new Game();

                    using (StreamReader reader = new StreamReader("games.json"))
                    {
                        var json = reader.ReadToEnd();
                        gamesUpdated = JsonSerializer.Deserialize<List<Game>>(json)!;

                        gameToUpdate = gamesUpdated.Find(x => x.Id == id);

                        gameToUpdate.Name = !string.IsNullOrWhiteSpace(gameRequest.Name) ? gameRequest.Name : gameToUpdate.Name;
                        gameToUpdate.Description = !string.IsNullOrWhiteSpace(gameRequest.Description) ? gameRequest.Description : gameToUpdate.Description;
                        gameToUpdate.Genre = !string.IsNullOrWhiteSpace(gameRequest.Genre) ? gameRequest.Genre : gameToUpdate.Genre;
                        gameToUpdate.DateOfCreation = !string.IsNullOrWhiteSpace(gameRequest.DateOfCreation) ? gameRequest.DateOfCreation : gameToUpdate.DateOfCreation;

                        reader.Close();
                    }

                    using (StreamWriter writer = new StreamWriter("games.json"))
                    {
                        writer.WriteLine("");
                        writer.WriteLine(JsonSerializer.Serialize(gamesUpdated));
                        writer.Close();
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapDelete("api/games/delete/{id}", async (int id) =>
            {
                try
                {
                    List<Game> listGames = new List<Game>();

                    using (StreamReader reader = new StreamReader("games.json"))
                    {
                        var json = reader.ReadToEnd();
                        var games = JsonSerializer.Deserialize<List<Game>>(json)!;

                        listGames.AddRange(games);
                        var gameToRemove = listGames.Find(x => x.Id == id);
                        listGames.Remove(gameToRemove);
                        reader.Close();
                    }

                    using (StreamWriter writer = new StreamWriter("games.json"))
                    {
                        writer.WriteLine("");
                        writer.WriteLine(JsonSerializer.Serialize(listGames));
                        writer.Close();
                    }

                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });

            app.MapPatch("api/games/patch", async (int id) =>
            {

            });
        }

    }
}

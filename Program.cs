using System.Security.Claims;

Dictionary<string, List<string>> gamesMap = new()
{
    {"player1", new List<string>() {"Street Fighter II", "Minecraft" }},
    {"player2", new List<string>() {"Forza Horizon 5", "Final Fantasy XIV", "FIFA 23" }},
};
Dictionary<string, List<string>> subscriptionMap = new()
{
    {"silver", new List<string>() {"Street Fighter II", "Minecraft" }},
    {"gold", new List<string>() {"Street Fighter II", "Minecraft" ,"Forza Horizon 5", "Final Fantasy XIV", "FIFA 23" }},
};


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();


app.MapGet("/playergames", () => gamesMap)
    .RequireAuthorization(policy =>
    {
        policy.RequireRole("admin");
    });

app.MapGet("/mygames", (ClaimsPrincipal user)  =>
{
    var hasClaim = user.HasClaim(c => c.Type == "subscription");
    if (!hasClaim)
    {
        return Results.BadRequest("No subscription claim found.");
    }
    if (hasClaim)
    {
        var subscription = user.FindFirst("subscription")?.Value;
        if (subscription != null && subscriptionMap.ContainsKey(subscription))
        {
            return Results.Ok(subscriptionMap[subscription]);
        }
        return Results.NotFound("No games found for the subscription.");
    }

    var username = user.Identity?.Name;
    if (username != null && gamesMap.ContainsKey(username))
    {
        return Results.Ok(gamesMap[username]);
    }
    return Results.NotFound("No games found for the user.");
})
.RequireAuthorization(policy =>
{
    policy.RequireRole("player");
}); 
 
app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using pitchamon.Api.Data;
using Microsoft.AspNetCore.Authentication;
using pitchamon.Api.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, BearerAuthService>("Bearer", null);

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "151 Pokémon API",
            Version = "v1",
            Description = "API for the original 151 Pokémon cry sounds"
        });
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "Token",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your token here}"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    SeedPokemon(db);
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void SeedPokemon(AppDbContext db)
{
    if (db.Pokemon.Any())
    {
        return;
    }

    var pokemon = new List<pitchamon.Api.Models.Pokemon>
    {
        new() { Id = 1, Name = "Bulbasaur", Cry = "001.wav" },
        new() { Id = 2, Name = "Ivysaur", Cry = "002.wav" },
        new() { Id = 3, Name = "Venusaur", Cry = "003.wav" },
        new() { Id = 4, Name = "Charmander", Cry = "004.wav" },
        new() { Id = 5, Name = "Charmeleon", Cry = "005.wav" },
        new() { Id = 6, Name = "Charizard", Cry = "006.wav" },
        new() { Id = 7, Name = "Squirtle", Cry = "007.wav" },
        new() { Id = 8, Name = "Wartortle", Cry = "008.wav" },
        new() { Id = 9, Name = "Blastoise", Cry = "009.wav" },
        new() { Id = 10, Name = "Caterpie", Cry = "010.wav" },
        new() { Id = 11, Name = "Metapod", Cry = "011.wav" },
        new() { Id = 12, Name = "Butterfree", Cry = "012.wav" },
        new() { Id = 13, Name = "Weedle", Cry = "013.wav" },
        new() { Id = 14, Name = "Kakuna", Cry = "014.wav" },
        new() { Id = 15, Name = "Beedrill", Cry = "015.wav" },
        new() { Id = 16, Name = "Pidgey", Cry = "016.wav" },
        new() { Id = 17, Name = "Pidgeotto", Cry = "017.wav" },
        new() { Id = 18, Name = "Pidgeot", Cry = "018.wav" },
        new() { Id = 19, Name = "Rattata", Cry = "019.wav" },
        new() { Id = 20, Name = "Raticate", Cry = "020.wav" },
        new() { Id = 21, Name = "Spearow", Cry = "021.wav" },
        new() { Id = 22, Name = "Fearow", Cry = "022.wav" },
        new() { Id = 23, Name = "Ekans", Cry = "023.wav" },
        new() { Id = 24, Name = "Arbok", Cry = "024.wav" },
        new() { Id = 25, Name = "Pikachu", Cry = "025.wav" },
        new() { Id = 26, Name = "Raichu", Cry = "026.wav" },
        new() { Id = 27, Name = "Sandshrew", Cry = "027.wav" },
        new() { Id = 28, Name = "Sandslash", Cry = "028.wav" },
        new() { Id = 29, Name = "NidoranF", Cry = "029.wav" },
        new() { Id = 30, Name = "Nidorina", Cry = "030.wav" },
        new() { Id = 31, Name = "Nidoqueen", Cry = "031.wav" },
        new() { Id = 32, Name = "NidoranM", Cry = "032.wav" },
        new() { Id = 33, Name = "Nidorino", Cry = "033.wav" },
        new() { Id = 34, Name = "Nidoking", Cry = "034.wav" },
        new() { Id = 35, Name = "Clefairy", Cry = "035.wav" },
        new() { Id = 36, Name = "Clefable", Cry = "036.wav" },
        new() { Id = 37, Name = "Vulpix", Cry = "037.wav" },
        new() { Id = 38, Name = "Ninetales", Cry = "038.wav" },
        new() { Id = 39, Name = "Jigglypuff", Cry = "039.wav" },
        new() { Id = 40, Name = "Wigglytuff", Cry = "040.wav" },
        new() { Id = 41, Name = "Zubat", Cry = "041.wav" },
        new() { Id = 42, Name = "Golbat", Cry = "042.wav" },
        new() { Id = 43, Name = "Oddish", Cry = "043.wav" },
        new() { Id = 44, Name = "Gloom", Cry = "044.wav" },
        new() { Id = 45, Name = "Vileplume", Cry = "045.wav" },
        new() { Id = 46, Name = "Paras", Cry = "046.wav" },
        new() { Id = 47, Name = "Parasect", Cry = "047.wav" },
        new() { Id = 48, Name = "Venonat", Cry = "048.wav" },
        new() { Id = 49, Name = "Venomoth", Cry = "049.wav" },
        new() { Id = 50, Name = "Diglett", Cry = "050.wav" },
        new() { Id = 51, Name = "Dugtrio", Cry = "051.wav" },
        new() { Id = 52, Name = "Meowth", Cry = "052.wav" },
        new() { Id = 53, Name = "Persian", Cry = "053.wav" },
        new() { Id = 54, Name = "Psyduck", Cry = "054.wav" },
        new() { Id = 55, Name = "Golduck", Cry = "055.wav" },
        new() { Id = 56, Name = "Mankey", Cry = "056.wav" },
        new() { Id = 57, Name = "Primeape", Cry = "057.wav" },
        new() { Id = 58, Name = "Growlithe", Cry = "058.wav" },
        new() { Id = 59, Name = "Arcanine", Cry = "059.wav" },
        new() { Id = 60, Name = "Poliwag", Cry = "060.wav" },
        new() { Id = 61, Name = "Poliwhirl", Cry = "061.wav" },
        new() { Id = 62, Name = "Poliwrath", Cry = "062.wav" },
        new() { Id = 63, Name = "Abra", Cry = "063.wav" },
        new() { Id = 64, Name = "Kadabra", Cry = "064.wav" },
        new() { Id = 65, Name = "Alakazam", Cry = "065.wav" },
        new() { Id = 66, Name = "Machop", Cry = "066.wav" },
        new() { Id = 67, Name = "Machoke", Cry = "067.wav" },
        new() { Id = 68, Name = "Machamp", Cry = "068.wav" },
        new() { Id = 69, Name = "Bellsprout", Cry = "069.wav" },
        new() { Id = 70, Name = "Weepinbell", Cry = "070.wav" },
        new() { Id = 71, Name = "Victreebel", Cry = "071.wav" },
        new() { Id = 72, Name = "Tentacool", Cry = "072.wav" },
        new() { Id = 73, Name = "Tentacruel", Cry = "073.wav" },
        new() { Id = 74, Name = "Geodude", Cry = "074.wav" },
        new() { Id = 75, Name = "Graveler", Cry = "075.wav" },
        new() { Id = 76, Name = "Golem", Cry = "076.wav" },
        new() { Id = 77, Name = "Ponyta", Cry = "077.wav" },
        new() { Id = 78, Name = "Rapidash", Cry = "078.wav" },
        new() { Id = 79, Name = "Slowpoke", Cry = "079.wav" },
        new() { Id = 80, Name = "Slowbro", Cry = "080.wav" },
        new() { Id = 81, Name = "Magnemite", Cry = "081.wav" },
        new() { Id = 82, Name = "Magneton", Cry = "082.wav" },
        new() { Id = 83, Name = "Farfetchd", Cry = "083.wav" },
        new() { Id = 84, Name = "Doduo", Cry = "084.wav" },
        new() { Id = 85, Name = "Dodrio", Cry = "085.wav" },
        new() { Id = 86, Name = "Seel", Cry = "086.wav" },
        new() { Id = 87, Name = "Dewgong", Cry = "087.wav" },
        new() { Id = 88, Name = "Grimer", Cry = "088.wav" },
        new() { Id = 89, Name = "Muk", Cry = "089.wav" },
        new() { Id = 90, Name = "Shellder", Cry = "090.wav" },
        new() { Id = 91, Name = "Cloyster", Cry = "091.wav" },
        new() { Id = 92, Name = "Gastly", Cry = "092.wav" },
        new() { Id = 93, Name = "Haunter", Cry = "093.wav" },
        new() { Id = 94, Name = "Gengar", Cry = "094.wav" },
        new() { Id = 95, Name = "Onix", Cry = "095.wav" },
        new() { Id = 96, Name = "Drowzee", Cry = "096.wav" },
        new() { Id = 97, Name = "Hypno", Cry = "097.wav" },
        new() { Id = 98, Name = "Krabby", Cry = "098.wav" },
        new() { Id = 99, Name = "Kingler", Cry = "099.wav" },
        new() { Id = 100, Name = "Voltorb", Cry = "100.wav" },
        new() { Id = 101, Name = "Electrode", Cry = "101.wav" },
        new() { Id = 102, Name = "Exeggcute", Cry = "102.wav" },
        new() { Id = 103, Name = "Exeggutor", Cry = "103.wav" },
        new() { Id = 104, Name = "Cubone", Cry = "104.wav" },
        new() { Id = 105, Name = "Marowak", Cry = "105.wav" },
        new() { Id = 106, Name = "Hitmonlee", Cry = "106.wav" },
        new() { Id = 107, Name = "Hitmonchan", Cry = "107.wav" },
        new() { Id = 108, Name = "Lickitung", Cry = "108.wav" },
        new() { Id = 109, Name = "Koffing", Cry = "109.wav" },
        new() { Id = 110, Name = "Weezing", Cry = "110.wav" },
        new() { Id = 111, Name = "Rhyhorn", Cry = "111.wav" },
        new() { Id = 112, Name = "Rhydon", Cry = "112.wav" },
        new() { Id = 113, Name = "Chansey", Cry = "113.wav" },
        new() { Id = 114, Name = "Tangela", Cry = "114.wav" },
        new() { Id = 115, Name = "Kangaskhan", Cry = "115.wav" },
        new() { Id = 116, Name = "Horsea", Cry = "116.wav" },
        new() { Id = 117, Name = "Seadra", Cry = "117.wav" },
        new() { Id = 118, Name = "Goldeen", Cry = "118.wav" },
        new() { Id = 119, Name = "Seaking", Cry = "119.wav" },
        new() { Id = 120, Name = "Staryu", Cry = "120.wav" },
        new() { Id = 121, Name = "Starmie", Cry = "121.wav" },
        new() { Id = 122, Name = "MrMime", Cry = "122.wav" },
        new() { Id = 123, Name = "Scyther", Cry = "123.wav" },
        new() { Id = 124, Name = "Jynx", Cry = "124.wav" },
        new() { Id = 125, Name = "Electabuzz", Cry = "125.wav" },
        new() { Id = 126, Name = "Magmar", Cry = "126.wav" },
        new() { Id = 127, Name = "Pinsir", Cry = "127.wav" },
        new() { Id = 128, Name = "Tauros", Cry = "128.wav" },
        new() { Id = 129, Name = "Magikarp", Cry = "129.wav" },
        new() { Id = 130, Name = "Gyarados", Cry = "130.wav" },
        new() { Id = 131, Name = "Lapras", Cry = "131.wav" },
        new() { Id = 132, Name = "Ditto", Cry = "132.wav" },
        new() { Id = 133, Name = "Eevee", Cry = "133.wav" },
        new() { Id = 134, Name = "Vaporeon", Cry = "134.wav" },
        new() { Id = 135, Name = "Jolteon", Cry = "135.wav" },
        new() { Id = 136, Name = "Flareon", Cry = "136.wav" },
        new() { Id = 137, Name = "Porygon", Cry = "137.wav" },
        new() { Id = 138, Name = "Omanyte", Cry = "138.wav" },
        new() { Id = 139, Name = "Omastar", Cry = "139.wav" },
        new() { Id = 140, Name = "Kabuto", Cry = "140.wav" },
        new() { Id = 141, Name = "Kabutops", Cry = "141.wav" },
        new() { Id = 142, Name = "Aerodactyl", Cry = "142.wav" },
        new() { Id = 143, Name = "Snorlax", Cry = "143.wav" },
        new() { Id = 144, Name = "Articuno", Cry = "144.wav" },
        new() { Id = 145, Name = "Zapdos", Cry = "145.wav" },
        new() { Id = 146, Name = "Moltres", Cry = "146.wav" },
        new() { Id = 147, Name = "Dratini", Cry = "147.wav" },
        new() { Id = 148, Name = "Dragonair", Cry = "148.wav" },
        new() { Id = 149, Name = "Dragonite", Cry = "149.wav" },
        new() { Id = 150, Name = "Mewtwo", Cry = "150.wav" },
        new() { Id = 151, Name = "Mew", Cry = "151.wav" }
    };
    
    db.Pokemon.AddRange(pokemon);
    db.SaveChanges();
}
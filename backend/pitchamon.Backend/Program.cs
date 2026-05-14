using pitchamon.Backend.Services;
using Microsoft.EntityFrameworkCore;
using pitchamon.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BackendDbContext>(options =>
	options.UseNpgsql(connectionString));

builder.Services.AddSingleton<TemporaryFileService>();
builder.Services.AddHttpClient<PokemonApiClient>();
builder.Services.AddHttpClient<LotrApiClient>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders(
                  "Lotr-Class-Id", 
                  "Lotr-Class-Name", 
                  "Lotr-Class-Description", 
                  "Pokemon-Id");
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("FrontendPolicy");
app.MapControllers();

app.Run();
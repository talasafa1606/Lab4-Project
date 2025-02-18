using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext for PostgreSQL
builder.Services.AddDbContext<MyUniversityDBContext>(options =>
    options.UseNpgsql("Host=localhost;Database=uni;Username=talasafa16;Password=mypassword"));

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add Authorization
builder.Services.AddAuthorization();

// Add Controllers (required for API routes)
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Library API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();

// Map Controllers (handles API requests)
app.MapControllers();

app.Run();

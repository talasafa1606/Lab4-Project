using Lab4.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.Edm;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql("Host=localhost;Database=postgres;Username=talasafa16;Password=mypassword"));

static IEdmModel GetEdmModel()
{
    var modelBuilder = new ODataConventionModelBuilder();

    modelBuilder.EntitySet<Book>("Books");
    modelBuilder.EntitySet<Author>("Authors");
    modelBuilder.EntitySet<Borrower>("Borrowers");
    modelBuilder.EntitySet<Loan>("Loans");

    modelBuilder.EntityType<Book>();
    modelBuilder.EntityType<Author>();
    modelBuilder.EntityType<Borrower>();
    modelBuilder.EntityType<Loan>();

    return modelBuilder.GetEdmModel();
}


builder.Services
    .AddControllers()
    .AddOData(options => options
        .AddRouteComponents("api", GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .SetMaxTop(100)
        .Count()
        .Expand()
    );

builder.Services.Configure<ODataOptions>(options =>
{
    options.QuerySettings.EnableSelect = true;
    options.QuerySettings.EnableFilter = true;
    options.QuerySettings.EnableOrderBy = true;
    options.QuerySettings.EnableExpand = true;
    options.QuerySettings.EnableCount = true;
    options.QuerySettings.MaxTop = 100;
});

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
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


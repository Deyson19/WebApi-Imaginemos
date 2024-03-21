using Microsoft.EntityFrameworkCore;
using WebApi_Imaginemos.DataAccess;
using WebApi_Imaginemos.Helpers;
using WebApi_Imaginemos.Services.Contract;
using WebApi_Imaginemos.Services.Implementation;

var builder = WebApplication.CreateBuilder(args);


//connecion string
var connectionString = builder.Configuration.GetConnectionString("Postgres");


//database
builder.Services.AddDbContext<ImaginemosDbContext>(op =>
{
    op.UseNpgsql(connectionString);
});

//automapper
builder.Services.AddAutoMapper(typeof(Program), typeof(AutoMapperProfile));

//services
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<IVentasService, VentasService>();
builder.Services.AddScoped<IProductosService, ProductosService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

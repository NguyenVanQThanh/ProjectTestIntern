using API.Data;
using API.Data.Repository;
using API.Interface;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IEmailRegisterRepository,EmailRegisterRepository>();
builder.Services.AddHttpClient<WeatherService>();
builder.Services.AddHostedService<DailyEmailService>();
builder.Configuration.AddJsonFile("secret.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowCredentials()
.AllowAnyMethod().WithOrigins("https://localhost:4200"));
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseDefaultFiles();

app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToController("Index","Fallback");

app.Run();

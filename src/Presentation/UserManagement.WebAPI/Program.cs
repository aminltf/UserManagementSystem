using UserManagement.Application;
using UserManagement.Infrastructure.Identity;
using UserManagement.Infrastructure.Identity.Seeds;
using UserManagement.Shared.Kernel;
using UserManagement.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterApplication()
    .RegisterKernel(builder.Configuration)
    .RegisterApiVersioningExtension()
    .RegisterIdentity(builder.Configuration);

var app = builder.Build();

// Run Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDbInitializer.SeedAsync(services);
}

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

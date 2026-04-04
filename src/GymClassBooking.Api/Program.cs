using GymClassBooking.Infrastructure;
using GymClassBooking.Infrastructure.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((doc, _, _) =>
    {
        doc.Info.Title = "GymClassBooking API";
        doc.Info.Description = "REST API for managing gym class bookings.";
        doc.Info.Version = "v1";
        return Task.CompletedTask;
    });
});
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "GymClassBooking API";
        options.Theme = ScalarTheme.DeepSpace;
    });

    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    DatabaseInitializer.Seed(context);
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
using EzraTodoApi.Handler;
using EzraTodoApi.Manager;
using EzraTodoApi.Models;
using EzraTodoApi.Repository;
using EzraTodoApi.Routes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext with SQLite (adjust connection string as needed)
builder.Services.AddDbContext<EzraTodoDbContext>(options =>
    options.UseSqlite("Data Source=ezra_todo.db"));

// Register handlers
builder.Services.AddScoped<TodoListHandler>();
builder.Services.AddScoped<TodoItemHandler>();

// Register managers
builder.Services.AddScoped<ITodoListManager, TodoListManager>();
builder.Services.AddScoped<ITodoItemManager, TodoItemManager>();

// Register repositories
builder.Services.AddScoped<ITodoListRepository, TodoListRepository>();
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        var allowedOrigins = builder.Environment.IsDevelopment()
            ? new[] { "http://localhost:5173" } // Development frontend URL
            : new[] { "https://your-production-frontend.com" }; // Production frontend URL

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Map endpoints
var app = builder.Build();

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
        c.RoutePrefix = string.Empty; // serve Swagger UI at root
    });
}

// Use the CORS policy
app.UseCors("AllowSpecificOrigins");

app.MapTodoListRoutes();
app.MapTodoItemRoutes();

app.Run();

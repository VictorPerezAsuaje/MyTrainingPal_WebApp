using MyTrainingPal.API.Services;
using MyTrainingPal.Infrastructure;
using MyTrainingPal.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IWorkoutMapper, WorkoutMapper>();
builder.Services.AddScoped<IExerciseMapper, ExerciseMapper>();
builder.Services.AddScoped<ISetRepository, SetRepository>();

builder.Services.AddPersistenceServices(builder.Configuration);

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

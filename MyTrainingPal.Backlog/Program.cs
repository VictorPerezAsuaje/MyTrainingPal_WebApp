using MyTrainingPal.Infrastructure;
using MyTrainingPal.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWorkoutMapper, WorkoutMapper>();
builder.Services.AddScoped<IExerciseMapper, ExerciseMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

builder.Services.AddPersistenceServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Workout/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workout}/{action=Index}/{id?}");

app.Run();

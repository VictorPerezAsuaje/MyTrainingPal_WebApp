using MyTrainingPal.Infrastructure;
using MyTrainingPal.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IWorkoutMapper, WorkoutMapper>();
builder.Services.AddScoped<IExerciseMapper, ExerciseMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();

// Dependency for authorization purposes
builder.Services.AddAuthentication("AuthCookie").AddCookie("AuthCookie", opts =>
{
    opts.Cookie.Name = "AuthCookie";
    opts.Cookie.MaxAge = new TimeSpan(00, 30, 00);
});

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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workout}/{action=Index}/{id?}");

app.Run();

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Infrastructure.Interfaces;
using MyTrainingPal.Infrastructure.Repositories;

namespace MyTrainingPal.Infrastructure
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IExerciseRepository, ExerciseRepository>();

            return services;
        }
    }
}

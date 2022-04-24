using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using System.Linq.Expressions;

namespace MyTrainingPal.Infrastructure.Entities
{
    /* Specification para filtrar por dificultad, por tipo de fuerza, por equipamiento y por
    grupos musculares. Sirve para la pantalla de creación de rutinas. */

    public sealed class DifficultySpecification : Specification<Exercise>
    {
        private readonly DifficultyLevel _difficultyLevel;
        public DifficultySpecification(DifficultyLevel difficultyLevel)
        {
            _difficultyLevel = difficultyLevel;
        }

        public override Expression<Func<Exercise, bool>> ToExpression()
        {
            return exercise => exercise.Level == _difficultyLevel;
        }
    }
}

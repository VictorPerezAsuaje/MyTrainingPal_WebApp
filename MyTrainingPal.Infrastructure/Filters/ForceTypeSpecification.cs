using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Domain.Enums;
using System.Linq.Expressions;

namespace MyTrainingPal.Infrastructure.Entities
{
    public sealed class ForceTypeSpecification : Specification<Exercise>
    {
        private readonly ForceType _forceType;
        public ForceTypeSpecification(ForceType forceType)
        {
            _forceType = forceType;
        }

        public override Expression<Func<Exercise, bool>> ToExpression()
        {
            return exercise => exercise.ForceType == _forceType;
        }
    }
}

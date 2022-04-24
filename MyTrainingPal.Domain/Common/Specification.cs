using System.Linq.Expressions;

namespace MyTrainingPal.Domain.Common
{
    internal sealed class IdentitySpecification<T> : Specification<T>
    {
        // Allows us to instanciate the specification without falling into null pointer exception
        public override Expression<Func<T, bool>> ToExpression()
            => x => true;
    }
    public abstract class Specification<T>
    {
        // If only this specification is passed, then it will return all entities without filter
        public static readonly Specification<T> All = new IdentitySpecification<T>();

        /* The expression lives and is encapsulated in the inheritant classes 
         * and not provided by the client code. */
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool SatisfiesFilter(T entity)
        {
            Func<T, bool> predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public Specification<T> And(Specification<T> specification)
        {
            // Check if either my left or right operant is the x => true, if so, ignore it
            if (this == All) return specification;
            if (specification == All) return this;

            return new AndSpecification<T>(this, specification);
        }
        public Specification<T> Or(Specification<T> specification)
        {
            // Check if either my left or right operant is the x => true, if so, ignore it
            if (this == All) return specification;
            if (specification == All) return this;

            return new OrSpecification<T>(this, specification);
        }

        public Specification<T> Not()
            => new NotSpecification<T>(this);
    }

    internal sealed class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            BinaryExpression and = Expression.AndAlso(leftExpression, rightExpression);
            return Expression.Lambda<Func<T, bool>>(and, leftExpression.Parameters.Single());
        }
    }

    internal sealed class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            Expression<Func<T, bool>> rightExpression = _right.ToExpression();

            BinaryExpression or = Expression.OrElse(leftExpression, rightExpression);
            return Expression.Lambda<Func<T, bool>>(or, leftExpression.Parameters.Single());
        }
    }

    internal sealed class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> expression = _specification.ToExpression();

            UnaryExpression not = Expression.Not(expression);
            return Expression.Lambda<Func<T, bool>>(not, expression.Parameters.Single());
        }
    }
}

using System.Linq.Expressions;

namespace Muradov.Domain
{
    public static class ExpressionBuilder
    {
        public static Expression<Func<MuseumExhibit, bool>> Build(FilterData filter)
        {
            var parameter = Expression.Parameter(typeof(MuseumExhibit), "exhibit");
            Expression? combinedExpression = null;

            if (filter.Century.HasValue)
            {
                var property = Expression.Property(parameter, nameof(MuseumExhibit.Century));
                var value = Expression.Constant(filter.Century.Value);
                var equalExpression = Expression.Equal(property, value);
                combinedExpression = Combine(combinedExpression, equalExpression);
            }

            if (filter.MinEstimatedValue.HasValue)
            {
                var property = Expression.Property(parameter, nameof(MuseumExhibit.EstimatedValue));
                var value = Expression.Constant(filter.MinEstimatedValue.Value);
                var greaterThanOrEqualExpression = Expression.GreaterThanOrEqual(property, value);
                combinedExpression = Combine(combinedExpression, greaterThanOrEqualExpression);
            }

            if (filter.MaxEstimatedValue.HasValue)
            {
                var property = Expression.Property(parameter, nameof(MuseumExhibit.EstimatedValue));
                var value = Expression.Constant(filter.MaxEstimatedValue.Value);
                var lessThanOrEqualExpression = Expression.LessThanOrEqual(property, value);
                combinedExpression = Combine(combinedExpression, lessThanOrEqualExpression);
            }

            if (filter.RequiresRestoration.HasValue)
            {
                var property = Expression.Property(parameter, nameof(MuseumExhibit.RequiresRestoration));
                var value = Expression.Constant(filter.RequiresRestoration.Value);
                var equalExpression = Expression.Equal(property, value);
                combinedExpression = Combine(combinedExpression, equalExpression);
            }

            if (combinedExpression == null)
            {
                return p => true;
            }

            return Expression.Lambda<Func<MuseumExhibit, bool>>(combinedExpression, parameter);
        }

        private static Expression Combine(Expression? existing, Expression newExpression)
        {
            if (existing == null)
            {
                return newExpression;
            }
            return Expression.AndAlso(existing, newExpression);
        }
    }
}

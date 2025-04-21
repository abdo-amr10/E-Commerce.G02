using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class Specifications<T>  where T : class
    {
        public Expression<Func<T, bool>>? Criteria { get; set; }
        public List<Expression<Func<T, object>>> IncludeExpressions { get; set; } = new();
        protected Specifications(Expression<Func<T, bool>>? criteria)
        {
            Criteria = criteria;
        }
        protected void AddInclude(Expression<Func<T, object>> expression) => IncludeExpressions.Add(expression);
    }
}

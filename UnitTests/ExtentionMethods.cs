using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace UnitTests
{
    internal static class ExtensionMethods
    {
        //https://stackoverflow.com/questions/21069986/nsubstitute-dbset-iqueryablet
        //plus https://stackoverflow.com/questions/39488256/how-do-i-mock-dbcontext-using-nsubstitute-and-then-add-remove-data
        internal static DbSet<T> SubstituteDbSet<T>(this DbSet<T> dbSet, IQueryable<T> data) where T : class
        {
            var mockSetTribes = Substitute.For<DbSet<T>, IQueryable<T>>();
            ((IQueryable<T>)mockSetTribes).Provider.Returns(data.Provider);
            ((IQueryable<T>)mockSetTribes).Expression.Returns(data.Expression);
            ((IQueryable<T>)mockSetTribes).ElementType.Returns(data.ElementType);
            ((IQueryable<T>)mockSetTribes).GetEnumerator().Returns(data.GetEnumerator());
            return dbSet;
        }
    }
}

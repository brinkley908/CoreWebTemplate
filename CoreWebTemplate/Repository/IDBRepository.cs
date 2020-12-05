using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreWebTemplate.Repository
{
    public interface IDBRepository<TEntity> where TEntity : class
    {

        void Delete( TEntity entityToDelete );

        void Delete( object id );

        IQueryable<TEntity> GetAll( string include = null );

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>>
            filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "" );

        IQueryable<TEntity> Query(
           Expression<Func<TEntity, bool>>
           filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "" );

        TEntity GetByID( object id );

        // IEnumerable<TEntity> Get( string query, params object[] parameters );

        void Insert( TEntity entity );

        void Update( TEntity entityToUpdate );

    }
}

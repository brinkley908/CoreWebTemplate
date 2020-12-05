using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreWebTemplate.Repository
{
    public class DBRepository<TEntity> : IDBRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        internal DbSet<TEntity> dbSet;

        public DBRepository( DbContext context )
        {
            _context = context;
        }

        //public virtual IEnumerable<TEntity> Get( string query,
        //    params object[] parameters )
        //{
        //    return null dbSet..SqlQuery( query, parameters ).ToList();
        //}

        public IQueryable<TEntity> GetAll( string include = null )
        {
            if ( string.IsNullOrEmpty( include ) )
                return _context.Set<TEntity>();

            else
                return _context.Set<TEntity>().Include( include );
        }

        public virtual IQueryable<TEntity> Query(
           Expression<Func<TEntity, bool>>
           filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "" )
        {
            IQueryable<TEntity> query = dbSet;

            if ( filter != null )
            {
                query = query.Where( filter );
            }

            if ( includeProperties != null )
            {
                foreach ( var includeProperty in includeProperties.Split
                ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
                {
                    query = query.Include( includeProperty );
                }
            }


            if ( orderBy != null )
            {
                return orderBy( query );
            }
            else
            {
                return query;
            }

        }


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "" )
        {
            IQueryable<TEntity> query = dbSet;

            if ( filter != null )
            {
                query = query.Where( filter );
            }

            if ( includeProperties != null )
            {
                foreach ( var includeProperty in includeProperties.Split
                ( new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ) )
                {
                    query = query.Include( includeProperty );
                }
            }


            if ( orderBy != null )
            {
                return orderBy( query ).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID( object id )
        {
            return dbSet.Find( id );
        }

        public virtual void Insert( TEntity entity )
        {
            dbSet.Add( entity );
        }

        public virtual void Delete( object id )
        {
            TEntity entityToDelete = dbSet.Find( id );
            Delete( entityToDelete );
        }

        public virtual void Delete( TEntity entityToDelete )
        {
            if ( _context.Entry( entityToDelete ).State == EntityState.Detached )
            {
                dbSet.Attach( entityToDelete );
            }
            dbSet.Remove( entityToDelete );
        }

        public virtual void Update( TEntity entityToUpdate )
        {
            dbSet.Attach( entityToUpdate );
            _context.Entry( entityToUpdate ).State = EntityState.Modified;
        }
    }

}


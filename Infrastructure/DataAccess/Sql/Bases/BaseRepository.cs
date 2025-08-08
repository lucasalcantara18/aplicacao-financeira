using Domain.Interfaces;
using Domain.Interfaces.Base;
using Infrastructure.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Sql.Bases
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DatabaseContext _context;

        protected BaseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public virtual Task<TEntity?> SingleAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return _context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao encontrar entidade", e);
            }   
        }
        public virtual Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            try
            {
                return _context.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao adicionar entidade", e);
            }
        }
    }
}
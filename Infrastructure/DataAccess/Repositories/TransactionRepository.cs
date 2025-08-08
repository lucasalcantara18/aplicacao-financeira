using Domain.Entidades;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.Sql;
using Infrastructure.DataAccess.Sql.Bases;

namespace Infrastructure.DataAccess.Repositories
{
    public class TransactionRepository : BaseRepository<ClientTransaction>, ITransactionRepository
    {
        public TransactionRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
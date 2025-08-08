using Domain.Entidades;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.Sql;
using Infrastructure.DataAccess.Sql.Bases;

namespace Infrastructure.DataAccess.Repositories
{
    public class ContaRepository : BaseRepository<Conta>, IContaRepository
    {
        public ContaRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
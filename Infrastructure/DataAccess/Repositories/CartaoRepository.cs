using Domain.Interfaces.Repositories;
using Domain.Entidades;
using Infrastructure.DataAccess.Sql;
using Infrastructure.DataAccess.Sql.Bases;

namespace Infrastructure.DataAccess.Repositories
{
    public class CartaoRepository : BaseRepository<Cartao>, ICartaoRepository
    {
        public CartaoRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
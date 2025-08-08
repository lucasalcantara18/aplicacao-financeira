using Domain.Entidades;
using Domain.Interfaces.Repositories;
using Infrastructure.DataAccess.Sql;
using Infrastructure.DataAccess.Sql.Bases;

namespace Infrastructure.DataAccess.Repositories
{
    public class PessoaRepository : BaseRepository<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(DatabaseContext context) : base(context)
        {
        }
    }
}
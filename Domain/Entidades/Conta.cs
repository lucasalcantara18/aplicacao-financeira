using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Conta
    {
        public string Id { get; set; }
        public string Branch { get; set; }
        public string Account { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PessoaId { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual List<Cartao> Cartoes { get; set; } = [];
        public virtual List<ClientTransaction> Transactions { get; set; } = [];

        public Conta(string branch, string account, string pessoaId)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Branch = branch;
            Account = account;
            PessoaId = pessoaId;
        }

        public Conta()
        {
        }
    }
}

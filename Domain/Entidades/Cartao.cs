using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Cartao
    {
        public string Id { get; set; }
        public string Type { get; set; }       // Ex: "virtual", "físico", etc.
        public string Number { get; set; }     // Ex: "6978" — sufixo do cartão
        public string Cvv { get; set; }        // Código de segurança
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ContaId { get; set; }
        public virtual Conta Conta { get; set; }

        public Cartao(string type, string number, string cvv, string contaId)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Type = type;
            Number = number;
            Cvv = cvv;
            ContaId = contaId;
        }

        public Cartao()
        {
        }

    }
}

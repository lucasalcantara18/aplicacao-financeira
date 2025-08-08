using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class ClientTransaction
    {
        public string Id { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ContaId { get; set; }
        public bool IsReverted { get; set; }
        public string Internal { get; set; }
        public virtual Conta Conta { get; set; }

        public ClientTransaction(decimal value, string description, string contaId)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Value = value;
            Description = description;
            ContaId = contaId;
            IsReverted = false;
            Internal = string.Empty;
        }

        public void AddInternal(string originTransactionId) => Internal = originTransactionId;

        public ClientTransaction()
        {
        }

    }
}

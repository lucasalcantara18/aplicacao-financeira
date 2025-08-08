using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entidades
{
    public class Pessoa
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Documento { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Password { get; set; }
        public virtual List<Conta> Contas { get; set; } = new List<Conta>();

        public Pessoa(string name, string password, string document)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Name = name;
            Password = password;
            Documento = document;
        }

        public Pessoa()
        {
        }
    }
}

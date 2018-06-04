using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Bibliotech.Models
{
    [Serializable]
    public class PagamentoMulta
    {
        public virtual Guid? Id { get; set; }

        public virtual decimal? Valor { get; set; }

        public virtual DateTime? Data { get; set; }

        public virtual Emprestimo Emprestimo { get; set; }

        public class PagamentoMultaMap : ClassMap<PagamentoMulta>
        {
            public PagamentoMultaMap()
            {
                Id(x => x.Id).GeneratedBy.GuidNative();
                Map(x => x.Valor).Scale(2).Not.Nullable();
                Map(x => x.Data).Not.Nullable();

                References<Emprestimo>(x => x.Emprestimo).Not.Nullable();
            }
        }
    }
}
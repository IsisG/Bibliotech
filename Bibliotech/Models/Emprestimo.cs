using FluentNHibernate.Mapping;
using System;

namespace Bibliotech.Models
{
    [Serializable]
    public class Emprestimo
    {
        public virtual Guid? Id { get; set; }

        public virtual DateTime? DataInicio { get; set; }

        public virtual DateTime? DataFimPrevisao { get; set; }

        public virtual DateTime? DataFim { get; set; }

        public virtual int? QuantidadeRenovacoes { get; set; }

        public virtual Exemplar Exemplar { get; set; }

        public virtual Usuario Usuario { get; set; }

        public virtual byte[] Version { get; set; }

        public class EmprestimoMap : ClassMap<Emprestimo>
        {
            public EmprestimoMap()
            {
                Id(x => x.Id).GeneratedBy.GuidNative();
                Map(x => x.QuantidadeRenovacoes).Length(3);
                Map(x => x.DataInicio).Not.Nullable();
                Map(x => x.DataFimPrevisao).Not.Nullable();
                Map(x => x.DataFim);

                References<Exemplar>(x => x.Exemplar).Not.Nullable();
                References<Usuario>(x => x.Usuario).Not.Nullable();

                Version(x => x.Version)
                    .Nullable()
                    .CustomSqlType("timestamp")
                    .Generated.Always();
            }

        }

    }
}
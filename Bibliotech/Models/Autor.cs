using FluentNHibernate.Mapping;
using System;

namespace Bibliotech.Models
{
    [Serializable]
    public class Autor
    {
        public virtual Guid? Id { get; set; }

        public virtual string Nome { get; set; }

        public virtual DateTime? DataNascimento { get; set; }

        public virtual string Telefone { get; set; }

        public virtual byte[] Version { get; set; }

        public class AutorMap : ClassMap<Autor>
        {
            public AutorMap()
            {
                Id(x => x.Id).GeneratedBy.GuidNative();
                Map(x => x.Nome).Length(200).Not.Nullable();
                Map(x => x.Telefone).Length(15);
                Map(x => x.DataNascimento);

                
                Version(x => x.Version)
                    .Nullable()
                    .CustomSqlType("timestamp")
                    .Generated.Always();

            }

        }

    }
}
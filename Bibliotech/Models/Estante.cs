using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bibliotech.Models
{
    [Serializable]
    public class Estante
    {
        public virtual Guid? Id { get; set; }

        [Display(Name = "Descrição")]
        public virtual string Descricao { get; set; }

        [XmlIgnore]
        public virtual IList<Prateleira> Prateleiras { get; set;  }
        
        public virtual byte[] Version { get; set; }
    }

    public class EstanteMap : ClassMap<Estante>
    {
        public EstanteMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.Descricao).Length(200).Not.Nullable();

            HasMany<Prateleira>(x => x.Prateleiras).Cascade.All().Inverse().LazyLoad();

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }
    }
}
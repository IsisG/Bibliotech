using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Bibliotech.Models
{
    [Serializable]
    public class Livro
    {
        public virtual Guid? Id { get; set; }

        [Display(Name = "Título")]
        public virtual string Titulo { get; set; }

        [Display(Name = "ISBN")]
        public virtual string Isbn { get; set; }

        [Display(Name = "Ano de Publicação")]
        public virtual int? AnoPublicao { get; set; }

        [Display(Name = "Edição")]
        public virtual int? Edicao { get; set; }

        [Display(Name = "Número de Páginas")]
        public virtual int? NumeroPaginas { get; set; }

        public virtual string Assunto { get; set; }

        public virtual string NomeFoto { get; set; }

        public virtual byte[] Version { get; set; }

        [XmlIgnore]
        public virtual IList<Exemplar> Exemplares { get; set; }
    }

    public class LivroMap : ClassMap<Livro>
    {
        public LivroMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.Titulo).Length(200).Not.Nullable();
            Map(x => x.Isbn).Length(15).Not.Nullable();
            Map(x => x.AnoPublicao);
            Map(x => x.Edicao);
            Map(x => x.NumeroPaginas);
            Map(x => x.Assunto).Length(200);
            Map(x => x.NomeFoto).Length(300);

            HasMany<Exemplar>(x => x.Exemplares).Cascade.All().Inverse().LazyLoad();

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }

    }
}
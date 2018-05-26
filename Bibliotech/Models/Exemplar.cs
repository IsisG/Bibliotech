using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Exemplar
    {
        public virtual Guid? Id { get; set; }

        [Display(Name = "Código")]
        public virtual string Codigo { get; set; }

        public virtual StatusExemplar? Status { get; set; }

        [Display(Name = "Uso Exclusivo na Biblioteca")]
        public virtual bool? ExclusivoBiblioteca { get; set; }

        public virtual Livro Livro { get; set; }
    }

    public enum StatusExemplar
    {
        [Description("Disponível")]
        Disponivel = 1,
        Emprestado = 2,
        Reservado = 3
    }

    public class ExemplarMap : ClassMap<Exemplar>
    {
        public ExemplarMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.Codigo).Length(30).Not.Nullable();
            Map(x => x.Status).CustomType<StatusExemplar>();
            Map(x => x.ExclusivoBiblioteca).Not.Nullable();

            References<Livro>(x => x.Livro).Not.Nullable();
        }

    }
}
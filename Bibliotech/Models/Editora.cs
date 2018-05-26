using FluentNHibernate.Mapping;
using System;

namespace Bibliotech.Models
{
    [Serializable]
    public class Editora
{
    public virtual Guid? Id { get; set; }

    public virtual string Nome { get; set; }

    public virtual byte[] Version { get; set; }
    }
public class EditoraMap : ClassMap<Editora>
{
    public EditoraMap()
    {
        Id(x => x.Id).GeneratedBy.GuidNative();
        Map(x => x.Nome).Length(200).Not.Nullable();

         Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
    }

}
}
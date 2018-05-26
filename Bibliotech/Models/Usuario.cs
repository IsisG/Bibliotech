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
    public class Usuario
    {
        public virtual Guid? Id { get; set; }

        public virtual string Nome { get; set; }

        [Display(Name = "Data de Nascimento")]
        public virtual DateTime? DataNascimento { get; set; }

        public virtual string Telefone { get; set; }

        public virtual string Login { get; set; }

        public virtual string Email { get; set; }

        public virtual string Senha { get; set; }

        public virtual Perfil? Perfil { get; set; }

        public virtual DateTime? DataAlteracaoSenha { get; set; }

        public virtual string NomeFoto { get; set; }

        public virtual byte[] Version { get; set; }
    }

    public enum Perfil
    {
        [Description("Padrão")]
        Padrao = 1,
        [Description("Funcionário")]
        Funcionario = 2,
        Administrador = 3
    }

    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();
            Map(x => x.Nome).Length(200).Not.Nullable();
            Map(x => x.Telefone).Length(15);
            Map(x => x.DataNascimento);
            Map(x => x.Login).Length(200).Not.Nullable();
            Map(x => x.Email).Length(200).Not.Nullable();
            Map(x => x.Senha).Not.Nullable();
            Map(x => x.Perfil).CustomType<Perfil>();
            Map(x => x.DataAlteracaoSenha);
            Map(x => x.NomeFoto);

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }

    }
}
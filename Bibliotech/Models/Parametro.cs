using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bibliotech.Models
{
    [Serializable]
    public class Parametro
    {
        public virtual Guid? Id { get; set; }

        [Display(Name = "Valor da Multa por Dia")]
        public virtual decimal? ValorMultaAtraso { get; set; }

        [Display(Name = "Prazo para Alteração de Senha")]
        public virtual int? DiasAlteracaoSenha { get; set; }

        [Display(Name = "Prazo de Devolução")]
        public virtual int? DiasPrazoDevolucao { get; set; }

        [Display(Name = "Prazo de Reserva")]
        public virtual int? DiasPrazoReserva { get; set; }

        [Display(Name = "Limite de Empréstimos")]
        public virtual int? QuantidadeMaximaEmprestimo { get; set; }

        [Display(Name = "Email Remetente")]
        public virtual string EmailRemetente { get; set; }

        public virtual string Senha { get; set; }

        public virtual byte[] Version { get; set; }



    }

    public class ParametroMap : ClassMap<Parametro>
    {
        public ParametroMap()
        {
            Id(x => x.Id).GeneratedBy.GuidNative();

            Map(x => x.ValorMultaAtraso).Scale(2);
            Map(x => x.DiasAlteracaoSenha);
            Map(x => x.DiasPrazoDevolucao);
            Map(x => x.DiasPrazoReserva);
            Map(x => x.QuantidadeMaximaEmprestimo);
            Map(x => x.EmailRemetente).Length(200);
            Map(x => x.Senha);

            Version(x => x.Version)
                .Nullable()
                .CustomSqlType("timestamp")
                .Generated.Always();
        }

    }
}
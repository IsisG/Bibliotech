
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK18331AE4DB13902C]') AND parent_object_id = OBJECT_ID('[Emprestimo]'))
alter table [Emprestimo]  drop constraint FK18331AE4DB13902C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK18331AE42D5FC5DB]') AND parent_object_id = OBJECT_ID('[Emprestimo]'))
alter table [Emprestimo]  drop constraint FK18331AE42D5FC5DB


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKEBBEC2C52AF9FE37]') AND parent_object_id = OBJECT_ID('[Exemplar]'))
alter table [Exemplar]  drop constraint FKEBBEC2C52AF9FE37


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF4E80CA1E9022747]') AND parent_object_id = OBJECT_ID('[Prateleira]'))
alter table [Prateleira]  drop constraint FKF4E80CA1E9022747


    if exists (select * from dbo.sysobjects where id = object_id(N'[Autor]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Autor]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Editora]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Editora]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Emprestimo]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Emprestimo]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Estante]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Estante]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Exemplar]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Exemplar]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Livro]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Livro]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Parametro]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Parametro]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Prateleira]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Prateleira]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Usuario]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Usuario]

    create table [Autor] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Nome NVARCHAR(200) not null,
       Telefone NVARCHAR(15) null,
       DataNascimento DATETIME null,
       primary key (Id)
    )

    create table [Editora] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Nome NVARCHAR(200) not null,
       primary key (Id)
    )

    create table [Emprestimo] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       QuantidadeRenovacoes INT null,
       DataInicio DATETIME not null,
       DataFimPrevisao DATETIME not null,
       DataFim DATETIME null,
       Exemplar_id UNIQUEIDENTIFIER not null,
       Usuario_id UNIQUEIDENTIFIER not null,
       primary key (Id)
    )

    create table [Estante] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Descricao NVARCHAR(200) not null,
       primary key (Id)
    )

    create table [Exemplar] (
        Id UNIQUEIDENTIFIER not null,
       Codigo NVARCHAR(30) not null,
       Status INT null,
       ExclusivoBiblioteca BIT not null,
       Livro_id UNIQUEIDENTIFIER not null,
       primary key (Id)
    )

    create table [Livro] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Titulo NVARCHAR(200) not null,
       Isbn NVARCHAR(15) not null,
       AnoPublicao INT null,
       Edicao INT null,
       NumeroPaginas INT null,
       Assunto NVARCHAR(200) null,
       NomeFoto NVARCHAR(300) null,
       primary key (Id)
    )

    create table [Parametro] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       ValorMultaAtraso DECIMAL(19, 2) null,
       DiasAlteracaoSenha INT null,
       DiasPrazoDevolucao INT null,
       DiasPrazoReserva INT null,
       QuantidadeMaximaEmprestimo INT null,
       EmailRemetente NVARCHAR(200) null,
       Senha NVARCHAR(255) null,
       primary key (Id)
    )

    create table [Prateleira] (
        Id UNIQUEIDENTIFIER not null,
       Descricao NVARCHAR(200) not null,
       Estante_id UNIQUEIDENTIFIER not null,
       primary key (Id)
    )

    create table [Usuario] (
        Id UNIQUEIDENTIFIER not null,
       Version timestamp null,
       Nome NVARCHAR(200) not null,
       Telefone NVARCHAR(15) null,
       DataNascimento DATETIME null,
       Login NVARCHAR(200) not null,
       Email NVARCHAR(200) not null,
       Senha NVARCHAR(255) not null,
       Perfil INT null,
       DataAlteracaoSenha DATETIME null,
       NomeFoto NVARCHAR(255) null,
       primary key (Id)
    )

    alter table [Emprestimo] 
        add constraint FK18331AE4DB13902C 
        foreign key (Exemplar_id) 
        references [Exemplar]

    alter table [Emprestimo] 
        add constraint FK18331AE42D5FC5DB 
        foreign key (Usuario_id) 
        references [Usuario]

    alter table [Exemplar] 
        add constraint FKEBBEC2C52AF9FE37 
        foreign key (Livro_id) 
        references [Livro]

    alter table [Prateleira] 
        add constraint FKF4E80CA1E9022747 
        foreign key (Estante_id) 
        references [Estante]

namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        FileData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permisos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 50),
                        Descripcion = c.String(maxLength: 150),
                        Clave = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreLogin = c.String(),
                        Contraseña = c.String(),
                        Token = c.String(),
                        UltimoSignal = c.DateTime(),
                        FechaInicioSesion = c.DateTime(),
                        Nombre = c.String(maxLength: 200),
                        Estado = c.Int(nullable: false),
                        RegistrationTokenFirebase = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Extensiones_Evento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DescripcionDespachador = c.String(),
                        IdSupervisor = c.Int(nullable: false),
                        DescripcionSupervisor = c.String(),
                        Estado = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        SegundaCategoria_Id = c.Int(),
                        Evento_Id = c.Int(),
                        Zona_Id = c.Int(),
                        Despachador_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.SegundaCategoria_Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Zonas", t => t.Zona_Id)
                .ForeignKey("dbo.Usuarios", t => t.Despachador_Id)
                .Index(t => t.SegundaCategoria_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.Zona_Id)
                .Index(t => t.Despachador_Id);
            
            CreateTable(
                "dbo.AsignacionesRecursos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HoraArribo = c.DateTime(),
                        ActualmenteAsignado = c.Boolean(nullable: false),
                        Extension_Id = c.Int(),
                        Recurso_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.Extension_Id)
                .ForeignKey("dbo.Recursos", t => t.Recurso_Id, cascadeDelete: true)
                .Index(t => t.Extension_Id)
                .Index(t => t.Recurso_Id);
            
            CreateTable(
                "dbo.AsignacionRecursoDescripcion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        agregadaOffline = c.Boolean(nullable: false),
                        AsignacionRecurso_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AsignacionesRecursos", t => t.AsignacionRecurso_Id)
                .Index(t => t.AsignacionRecurso_Id);
            
            CreateTable(
                "dbo.Recursos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 60),
                        Estado = c.Int(nullable: false),
                        EstadoAsignacion = c.Int(nullable: false),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Grupos_Recursos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Audios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        AudioData_Id = c.Int(),
                        Evento_Id = c.Int(),
                        ExtensionEvento_Id = c.Int(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationFiles", t => t.AudioData_Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.ExtensionEvento_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.AudioData_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.ExtensionEvento_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Eventos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreInformante = c.String(maxLength: 200),
                        TelefonoEvento = c.String(maxLength: 50),
                        Estado = c.Int(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        Calle = c.String(maxLength: 150),
                        Esquina = c.String(maxLength: 150),
                        Numero = c.String(maxLength: 10),
                        Latitud = c.Double(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Descripcion = c.String(),
                        EnProceso = c.Boolean(nullable: false),
                        Categoria_Id = c.Int(),
                        Departamento_Id = c.Int(),
                        Sector_Id = c.Int(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.Categoria_Id)
                .ForeignKey("dbo.Departamentos", t => t.Departamento_Id)
                .ForeignKey("dbo.Sectores", t => t.Sector_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Categoria_Id)
                .Index(t => t.Departamento_Id)
                .Index(t => t.Sector_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(nullable: false, maxLength: 10),
                        Clave = c.String(nullable: false, maxLength: 150),
                        Prioridad = c.Int(nullable: false),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departamentos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Imagenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        Evento_Id = c.Int(),
                        ExtensionEvento_Id = c.Int(),
                        ImagenData_Id = c.Int(),
                        ImagenThumbnail_Id = c.Int(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.ExtensionEvento_Id)
                .ForeignKey("dbo.ApplicationFiles", t => t.ImagenData_Id)
                .ForeignKey("dbo.ApplicationFiles", t => t.ImagenThumbnail_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.ExtensionEvento_Id)
                .Index(t => t.ImagenData_Id)
                .Index(t => t.ImagenThumbnail_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Origen_Eventos",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IdOrigen = c.Int(nullable: false),
                        TipoOrigen = c.String(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Eventos", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Sectores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 50),
                        Zona_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zonas", t => t.Zona_Id)
                .Index(t => t.Zona_Id);
            
            CreateTable(
                "dbo.Zonas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 200),
                        UnidadEjecutora_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unidades_Ejecutoras", t => t.UnidadEjecutora_Id, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.UnidadEjecutora_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Unidades_Ejecutoras",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        Evento_Id = c.Int(),
                        ExtensionEvento_Id = c.Int(),
                        Usuario_Id = c.Int(),
                        VideoData_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.ExtensionEvento_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .ForeignKey("dbo.ApplicationFiles", t => t.VideoData_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.ExtensionEvento_Id)
                .Index(t => t.Usuario_Id)
                .Index(t => t.VideoData_Id);
            
            CreateTable(
                "dbo.GeoUbicaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Latitud = c.Double(nullable: false),
                        ExtensionEvento_Id = c.Int(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.ExtensionEvento_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.ExtensionEvento_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Terminal = c.String(maxLength: 50),
                        Modulo = c.String(maxLength: 50),
                        Entidad = c.String(maxLength: 50),
                        idEntidad = c.Int(nullable: false),
                        Accion = c.String(maxLength: 50),
                        Codigo = c.Int(nullable: false),
                        EsError = c.Boolean(nullable: false),
                        Detalles = c.String(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.LogNotification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Usuario = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Terminal = c.String(maxLength: 50),
                        Modulo = c.String(maxLength: 50),
                        Entidad = c.String(maxLength: 50),
                        idEntidad = c.Int(nullable: false),
                        Accion = c.String(maxLength: 50),
                        Codigo = c.Int(nullable: false),
                        EsError = c.Boolean(nullable: false),
                        Detalles = c.String(),
                        Topic = c.String(),
                        CodigoNotificacion = c.String(),
                        PKEventoAfectado = c.String(),
                        responseFireBase = c.String(),
                        LogNotificationPrevio_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LogNotification", t => t.LogNotificationPrevio_Id)
                .Index(t => t.LogNotificationPrevio_Id);
            
            CreateTable(
                "dbo.PermisoRols",
                c => new
                    {
                        Permiso_Id = c.Int(nullable: false),
                        Rol_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permiso_Id, t.Rol_Id })
                .ForeignKey("dbo.Permisos", t => t.Permiso_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRoles", t => t.Rol_Id, cascadeDelete: true)
                .Index(t => t.Permiso_Id)
                .Index(t => t.Rol_Id);
            
            CreateTable(
                "dbo.UsuarioRols",
                c => new
                    {
                        Usuario_Id = c.Int(nullable: false),
                        Rol_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Usuario_Id, t.Rol_Id })
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRoles", t => t.Rol_Id, cascadeDelete: true)
                .Index(t => t.Usuario_Id)
                .Index(t => t.Rol_Id);
            
            CreateTable(
                "dbo.GrupoRecursoRecursoes",
                c => new
                    {
                        GrupoRecurso_Id = c.Int(nullable: false),
                        Recurso_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GrupoRecurso_Id, t.Recurso_Id })
                .ForeignKey("dbo.Grupos_Recursos", t => t.GrupoRecurso_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recursos", t => t.Recurso_Id, cascadeDelete: true)
                .Index(t => t.GrupoRecurso_Id)
                .Index(t => t.Recurso_Id);
            
            CreateTable(
                "dbo.GrupoRecursoUsuarios",
                c => new
                    {
                        GrupoRecurso_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.GrupoRecurso_Id, t.Usuario_Id })
                .ForeignKey("dbo.Grupos_Recursos", t => t.GrupoRecurso_Id, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .Index(t => t.GrupoRecurso_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.UnidadEjecutoraUsuarios",
                c => new
                    {
                        UnidadEjecutora_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UnidadEjecutora_Id, t.Usuario_Id })
                .ForeignKey("dbo.Unidades_Ejecutoras", t => t.UnidadEjecutora_Id, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .Index(t => t.UnidadEjecutora_Id)
                .Index(t => t.Usuario_Id);
            
        }
        
        public override void Down()
        {
            
        }
    }
}

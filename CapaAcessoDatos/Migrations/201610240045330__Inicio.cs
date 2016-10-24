namespace Emsys.DataAccesLayer.Core
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _Inicio : DbMigration
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
                        Descripcion = c.String(),
                        FechaArribo = c.DateTime(nullable: false),
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
                "dbo.GeoUbicaciones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        Longitud = c.Double(nullable: false),
                        Latitud = c.Double(nullable: false),
                        Usuario_Id = c.Int(),
                        Evento_Id = c.Int(),
                        Extension_Evento_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.Extension_Evento_Id)
                .Index(t => t.Usuario_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.Extension_Evento_Id);
            
            CreateTable(
                "dbo.Imagenes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEnvio = c.DateTime(nullable: false),
                        Evento_Id = c.Int(),
                        ExtensionEvento_Id = c.Int(),
                        ImagenData_Id = c.Int(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Eventos", t => t.Evento_Id)
                .ForeignKey("dbo.Extensiones_Evento", t => t.ExtensionEvento_Id)
                .ForeignKey("dbo.ApplicationFiles", t => t.ImagenData_Id)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id)
                .Index(t => t.Evento_Id)
                .Index(t => t.ExtensionEvento_Id)
                .Index(t => t.ImagenData_Id)
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
                "dbo.RecursoExtension_Evento",
                c => new
                    {
                        Recurso_Id = c.Int(nullable: false),
                        Extension_Evento_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Recurso_Id, t.Extension_Evento_Id })
                .ForeignKey("dbo.Recursos", t => t.Recurso_Id, cascadeDelete: true)
                .ForeignKey("dbo.Extensiones_Evento", t => t.Extension_Evento_Id, cascadeDelete: true)
                .Index(t => t.Recurso_Id)
                .Index(t => t.Extension_Evento_Id);
            
            CreateTable(
                "dbo.Grupo_RecursoRecurso",
                c => new
                    {
                        Grupo_Recurso_Id = c.Int(nullable: false),
                        Recurso_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Grupo_Recurso_Id, t.Recurso_Id })
                .ForeignKey("dbo.Grupos_Recursos", t => t.Grupo_Recurso_Id, cascadeDelete: true)
                .ForeignKey("dbo.Recursos", t => t.Recurso_Id, cascadeDelete: true)
                .Index(t => t.Grupo_Recurso_Id)
                .Index(t => t.Recurso_Id);
            
            CreateTable(
                "dbo.Grupo_RecursoUsuario",
                c => new
                    {
                        Grupo_Recurso_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Grupo_Recurso_Id, t.Usuario_Id })
                .ForeignKey("dbo.Grupos_Recursos", t => t.Grupo_Recurso_Id, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .Index(t => t.Grupo_Recurso_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Unidad_EjecutoraUsuario",
                c => new
                    {
                        Unidad_Ejecutora_Id = c.Int(nullable: false),
                        Usuario_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Unidad_Ejecutora_Id, t.Usuario_Id })
                .ForeignKey("dbo.Unidades_Ejecutoras", t => t.Unidad_Ejecutora_Id, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.Usuario_Id, cascadeDelete: true)
                .Index(t => t.Unidad_Ejecutora_Id)
                .Index(t => t.Usuario_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Zonas", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Logs", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.GeoUbicaciones", "Extension_Evento_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.Extensiones_Evento", "Despachador_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Audios", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Audios", "ExtensionEvento_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.Videos", "VideoData_Id", "dbo.ApplicationFiles");
            DropForeignKey("dbo.Videos", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Videos", "ExtensionEvento_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.Videos", "Evento_Id", "dbo.Eventos");
            DropForeignKey("dbo.Eventos", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Zonas", "UnidadEjecutora_Id", "dbo.Unidades_Ejecutoras");
            DropForeignKey("dbo.Unidad_EjecutoraUsuario", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Unidad_EjecutoraUsuario", "Unidad_Ejecutora_Id", "dbo.Unidades_Ejecutoras");
            DropForeignKey("dbo.Sectores", "Zona_Id", "dbo.Zonas");
            DropForeignKey("dbo.Extensiones_Evento", "Zona_Id", "dbo.Zonas");
            DropForeignKey("dbo.Eventos", "Sector_Id", "dbo.Sectores");
            DropForeignKey("dbo.Origen_Eventos", "Id", "dbo.Eventos");
            DropForeignKey("dbo.Imagenes", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Imagenes", "ImagenData_Id", "dbo.ApplicationFiles");
            DropForeignKey("dbo.Imagenes", "ExtensionEvento_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.Imagenes", "Evento_Id", "dbo.Eventos");
            DropForeignKey("dbo.GeoUbicaciones", "Evento_Id", "dbo.Eventos");
            DropForeignKey("dbo.GeoUbicaciones", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Extensiones_Evento", "Evento_Id", "dbo.Eventos");
            DropForeignKey("dbo.Eventos", "Departamento_Id", "dbo.Departamentos");
            DropForeignKey("dbo.Extensiones_Evento", "SegundaCategoria_Id", "dbo.Categorias");
            DropForeignKey("dbo.Eventos", "Categoria_Id", "dbo.Categorias");
            DropForeignKey("dbo.Audios", "Evento_Id", "dbo.Eventos");
            DropForeignKey("dbo.Audios", "AudioData_Id", "dbo.ApplicationFiles");
            DropForeignKey("dbo.AsignacionesRecursos", "Recurso_Id", "dbo.Recursos");
            DropForeignKey("dbo.Recursos", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Grupo_RecursoUsuario", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Grupo_RecursoUsuario", "Grupo_Recurso_Id", "dbo.Grupos_Recursos");
            DropForeignKey("dbo.Grupo_RecursoRecurso", "Recurso_Id", "dbo.Recursos");
            DropForeignKey("dbo.Grupo_RecursoRecurso", "Grupo_Recurso_Id", "dbo.Grupos_Recursos");
            DropForeignKey("dbo.RecursoExtension_Evento", "Extension_Evento_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.RecursoExtension_Evento", "Recurso_Id", "dbo.Recursos");
            DropForeignKey("dbo.AsignacionesRecursos", "Extension_Id", "dbo.Extensiones_Evento");
            DropForeignKey("dbo.AsignacionRecursoDescripcion", "AsignacionRecurso_Id", "dbo.AsignacionesRecursos");
            DropForeignKey("dbo.UsuarioRols", "Rol_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.UsuarioRols", "Usuario_Id", "dbo.Usuarios");
            DropForeignKey("dbo.PermisoRols", "Rol_Id", "dbo.ApplicationRoles");
            DropForeignKey("dbo.PermisoRols", "Permiso_Id", "dbo.Permisos");
            DropIndex("dbo.Unidad_EjecutoraUsuario", new[] { "Usuario_Id" });
            DropIndex("dbo.Unidad_EjecutoraUsuario", new[] { "Unidad_Ejecutora_Id" });
            DropIndex("dbo.Grupo_RecursoUsuario", new[] { "Usuario_Id" });
            DropIndex("dbo.Grupo_RecursoUsuario", new[] { "Grupo_Recurso_Id" });
            DropIndex("dbo.Grupo_RecursoRecurso", new[] { "Recurso_Id" });
            DropIndex("dbo.Grupo_RecursoRecurso", new[] { "Grupo_Recurso_Id" });
            DropIndex("dbo.RecursoExtension_Evento", new[] { "Extension_Evento_Id" });
            DropIndex("dbo.RecursoExtension_Evento", new[] { "Recurso_Id" });
            DropIndex("dbo.UsuarioRols", new[] { "Rol_Id" });
            DropIndex("dbo.UsuarioRols", new[] { "Usuario_Id" });
            DropIndex("dbo.PermisoRols", new[] { "Rol_Id" });
            DropIndex("dbo.PermisoRols", new[] { "Permiso_Id" });
            DropIndex("dbo.Logs", new[] { "Usuario_Id" });
            DropIndex("dbo.Videos", new[] { "VideoData_Id" });
            DropIndex("dbo.Videos", new[] { "Usuario_Id" });
            DropIndex("dbo.Videos", new[] { "ExtensionEvento_Id" });
            DropIndex("dbo.Videos", new[] { "Evento_Id" });
            DropIndex("dbo.Zonas", new[] { "Usuario_Id" });
            DropIndex("dbo.Zonas", new[] { "UnidadEjecutora_Id" });
            DropIndex("dbo.Sectores", new[] { "Zona_Id" });
            DropIndex("dbo.Origen_Eventos", new[] { "Id" });
            DropIndex("dbo.Imagenes", new[] { "Usuario_Id" });
            DropIndex("dbo.Imagenes", new[] { "ImagenData_Id" });
            DropIndex("dbo.Imagenes", new[] { "ExtensionEvento_Id" });
            DropIndex("dbo.Imagenes", new[] { "Evento_Id" });
            DropIndex("dbo.GeoUbicaciones", new[] { "Extension_Evento_Id" });
            DropIndex("dbo.GeoUbicaciones", new[] { "Evento_Id" });
            DropIndex("dbo.GeoUbicaciones", new[] { "Usuario_Id" });
            DropIndex("dbo.Eventos", new[] { "Usuario_Id" });
            DropIndex("dbo.Eventos", new[] { "Sector_Id" });
            DropIndex("dbo.Eventos", new[] { "Departamento_Id" });
            DropIndex("dbo.Eventos", new[] { "Categoria_Id" });
            DropIndex("dbo.Audios", new[] { "Usuario_Id" });
            DropIndex("dbo.Audios", new[] { "ExtensionEvento_Id" });
            DropIndex("dbo.Audios", new[] { "Evento_Id" });
            DropIndex("dbo.Audios", new[] { "AudioData_Id" });
            DropIndex("dbo.Recursos", new[] { "Usuario_Id" });
            DropIndex("dbo.AsignacionRecursoDescripcion", new[] { "AsignacionRecurso_Id" });
            DropIndex("dbo.AsignacionesRecursos", new[] { "Recurso_Id" });
            DropIndex("dbo.AsignacionesRecursos", new[] { "Extension_Id" });
            DropIndex("dbo.Extensiones_Evento", new[] { "Despachador_Id" });
            DropIndex("dbo.Extensiones_Evento", new[] { "Zona_Id" });
            DropIndex("dbo.Extensiones_Evento", new[] { "Evento_Id" });
            DropIndex("dbo.Extensiones_Evento", new[] { "SegundaCategoria_Id" });
            DropTable("dbo.Unidad_EjecutoraUsuario");
            DropTable("dbo.Grupo_RecursoUsuario");
            DropTable("dbo.Grupo_RecursoRecurso");
            DropTable("dbo.RecursoExtension_Evento");
            DropTable("dbo.UsuarioRols");
            DropTable("dbo.PermisoRols");
            DropTable("dbo.Logs");
            DropTable("dbo.Videos");
            DropTable("dbo.Unidades_Ejecutoras");
            DropTable("dbo.Zonas");
            DropTable("dbo.Sectores");
            DropTable("dbo.Origen_Eventos");
            DropTable("dbo.Imagenes");
            DropTable("dbo.GeoUbicaciones");
            DropTable("dbo.Departamentos");
            DropTable("dbo.Categorias");
            DropTable("dbo.Eventos");
            DropTable("dbo.Audios");
            DropTable("dbo.Grupos_Recursos");
            DropTable("dbo.Recursos");
            DropTable("dbo.AsignacionRecursoDescripcion");
            DropTable("dbo.AsignacionesRecursos");
            DropTable("dbo.Extensiones_Evento");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Permisos");
            DropTable("dbo.ApplicationRoles");
            DropTable("dbo.ApplicationFiles");
        }
    }
}

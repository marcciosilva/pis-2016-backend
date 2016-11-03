 namespace DataTypeObject
{
    public class MensajesParaFE
    {
        // Mensajes.
        public const string Correcto = "Correcto";
        public const string UsuarioContraseñaInvalidos = "Usuario/Password inválido.";
        public const string TokenInvalido = "Token inválido.";
        public const string UsuarioNoAutenticado = "Usuario no autenticado.";
        public const string UsuarioTieneOperacionesNoFinalizadas = "El usuario tiene una operación no finalizada.";
        public const string ServicioExternoNoDisponible = "El servicio externo no se encuentra disponible.";
        public const string RecursoNoDisponible = "El recurso seleccionado no se encuentra disponible.";
        public const string SeleccionZonasRecursosInvalida = "Seleccion invalida de zonas y recursos.";
        public const string EventoInvalido = "El evento solicitado es invalido.";
        public const string SesionActiva = "El usuario ya cuenta con una sesion activa.";
        public const string ExtensionInvalida = "La extension de evento indicada no es valida.";
        public const string FormatoNoSoportado = "El formato del archivo enviado no es soportado.";
        public const string UsuarioNoAutorizado = "El usuario no esta autorizado a acceder al contenido solicitado.";
        public const string ImagenInvalida = "La imagen solicitada no es valida.";
        public const string VideoInvalido = "El video solicitado no es valido.";
        public const string AudioInvalido = "El audio solicitado no es valido.";
        public const string ArgumentoInvalido = "Se utilizaron argumentos invalidos.";
        public const string ZonaInvalida = "La zona indicada no es valida.";
        public const string EventoNoEnviado = "No fue posible realizar la accion deseada debido a que el evento aun no ha sido enviado.";
        public const string CategoriaInvalida = "La categoria ingresada no es valida.";
        public const string ArriboPrevio = "El recurso ya posee una hora de arribo previa en la extension seleccionada.";

        // Mensajes de error.
        public const string ErrorCerraSesion = "Ocurrio un error en el servidor al cerrar la sesion";
        public const string ErrorIniciarSesion = "Ocurrio un error en el servidor al iniciar sesion";
        public const string ErrorGetRoles = "Ocurrio un error en el servidor al obtener roles.";        
        public const string ErrorEnviarArchivo = "Ocurrio un error al enviar el archivo al servidor.";
        public const string ErrorObtenerEvento = "Ocurrio un error al intentar obtener la informacion del evento.";
        public const string ErrorListarEventos = "Ocurrio un error al intentar listar los eventos.";
        public const string ErrorActualizarDescripcion = "Ocurrio un error al intentar actualizar la descripcion.";
        public const string ErrorConsumirServicioExterno = "Ocurrio un error al intentar consumir el servicio externo.";
        public const string ErrorAdjuntarGeoUbicacion = "Ocurrio un error al intentar adjuntar la geo ubicacion.";
        public const string ErrorAdjuntarImagen = "Ocurrio un error al intentar adjuntar la imagen.";
        public const string ErrorAdjuntarVideo = "Ocurrio un error al intentar adjuntar el video.";
        public const string ErrorAdjuntarAudio = "Ocurrio un error al intentar adjuntar el audio.";
        public const string ErrorDescargarArchivo = "Ocurrio un error al intentar descargar el archivo.";
        public const string ErrorElegirRoles = "Ocurrio un error al intentar elegir los roles.";
        public const string ErrorKeepMeAlive = "Ocurrio un error al intentar indicarle actividad al servidor.";
        public const string ErrorReportarHoraArribo = "Ocurrio un error al intentar reportar la hora de arribo.";
        public const string ErrorCrearEvento = "Ocurrio un error al intentar crear un evento.";
        public const string ErrorInfoCreacionEvento = "Ocurrio un error al intentar obtener la informacion para crear un evento.";
        public const string ErrorTomarExtension = "Ocurrio un error al intentar tomar una extension.";
        public const string ErrorLiberarExtension = "Ocurrio un error al intentar liberar una extension.";
        public const string ErrorGetRecursosExtension = "Ocurrio un error al intentar obtener los recursos de una extension.";
        public const string ErrorGestionarRecursos = "Ocurrio un error al intentar gestionar recursos de una extension.";
        public const string ErrorActualizarSegundaCategoria = "Ocurrio un error al intentar actualizar la segunda categoria de una extension.";
        public const string ErrorGetZonasLibresEvento = "Ocurrio un error al intentar obtener las zonas libres de un evento.";
        public const string ErrorAbrirExtension = "Ocurrio un error al intentar abrir una extension nueva para un evento.";
        public const string ErrorCerrarExtension = "Ocurrio un error al intentar cerrar una extension de un evento.";
        public const string ErrorActualizarDescripcionDespachador = "Ocurrio un error al intentar actualizar la descripcion.";
        public const string EventoSinZonas = "El evento debe tener almenos una zona";

        // Codigos.
        public const int CorrectoCod = 0;
        public const int Correct = 0;
        public const int UsuarioContraseñaInvalidosCod = 1;
        public const int UsuarioNoAutenticadoCod = 2;
        public const int SeleccionZonasRecursosInvalidaCod = 3;        
        public const int RecursoNoDisponibleCod = 4;
        public const int UsuarioTieneOperacionesNoFinalizadasCod = 5;
        public const int SesionActivaCod = 6;
        public const int ServicioExternoNoDisponibleCod = 8;
        public const int EventoInvalidoCod = 9;
        public const int UsuarioNoAutenticadoComoRecurso = 10;
        public const int IdentificadorExtensionIncorrecto = 11;
        public const int ExtensionInvalidaCod = 12;
        public const int FormatoNoSoportadoCod = 13;
        public const int ArgumentoInvalidoCod = 14;
        public const int UsuarioNoAutorizadoCod = 15;
        public const int ZonaInvalidaCod = 16;
        public const int EventoNoEnviadoCod = 17;
        public const int CategoriaInvalidaCod = 18;
        public const int EventoSinZonasCod = 19;
        public const int ArriboPrevioCod = 20;
        public const int ImagenInvalidaCod = 101;
        public const int VideoInvalidoCod = 102;
        public const int AudioInvalidoCod = 103;

        // Codigos de error.
        public const int ErrorCod = 500;

        // Servicio externo.
        public const int ErrorConsumirServicioExternoCod = 3000;

        // Login.
        public const int ErrorIniciarSesionCod = 1001;
        public const int ErrorGetRolesCod = 1002;
        public const int ErrorElegirRolesCod = 1003;
        public const int ErrorKeepMeAliveCod = 1004;
        public const int ErrorCerrarSesionCod = 1005;
        public const int ErrorAlGenerarTokenCod = 1000;

        // Adjuntos.
        public const int ErrorAdjuntarGeoUbicacionCod = 2001;
        public const int ErrorAdjuntarImagenCod = 2002;
        public const int ErrorAdjuntarVideoCod = 2003;
        public const int ErrorAdjuntarAudioCod = 2004;
        public const int ErrorDescargarArchivoCod = 2005;
        public const int ErrorEnviarArchivoCod = 2000;  
              
        // Eventos.
        public const int ErrorListarEventosCod = 3000;
        public const int ErrorObtenerEventoCod = 3001;
        public const int ErrorActualizarDescripcionCod = 3002;
        public const int ErrorReportarHoraArriboCod = 3003;
        public const int ErrorCrearEventoCod = 3004;
        public const int ErrorInfoCreacionEventoCod = 3005;
        public const int ErrorTomarExtensionCod = 3006;
        public const int ErrorLiberarExtensionCod = 3007;
        public const int ErrorGetRecursosExtensionCod = 3008;
        public const int ErrorGestionarRecursosCod = 3009;
        public const int ErrorActualizarSegundaCategoriaCod = 3010;
        public const int ErrorGetZonasLibresEventoCod = 3011;
        public const int ErrorAbrirExtensionCod = 3012;
        public const int ErrorCerrarExtensionCod = 3013;
        public const int ErrorActualizarDescripcionDespachadorCod = 3014;

        // Codigos Logs.
        public const int LogNotificaciones = 901;
        public const int LogCapturarCambioEventoCod = 902;
        public const int LogAccionesCod = 903;

        public const int LogNotificacionesErrorReenvio = 904;
        public const int LogNotificacionesErrorGenerico = 905;
        public const int LogNotificacionesCierreEnvio = 906;
        public const int LogNotificacionesDessuscripcionUsuarioTopic = 911;
        public const int LogNotificacionesDessuscripcionUsuarioTopicError = 914;
        public const int LogNotificacionesDessuscripcionUsuarioTopicErrorGenericoRequest = 914;

        // Desconectar inactivos
        public const int LogDesconectarUsuarioCod = 904;
    }
}

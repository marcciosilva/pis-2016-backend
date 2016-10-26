using DataTypeObject;
using System.Collections.Generic;

namespace Emsys.LogicLayer
{
    public interface IMetodos
    {
        /// <summary>
        /// Interfaz Autenticar usuario implentado por la capa de servicios.
        /// </summary>
        /// <param name="userName">Nombre de usuario que se desea autenticar.</param>
        /// <param name="password">Contraseña no encriptada de usuario que se desea autenticar.</param>
        /// <returns>Retorna la respuesta segun el documento de interfaz.</returns>
        DtoAutenticacion autenticarUsuario(string userName, string password);

        /// <summary>
        /// Obtiene el rol del usuario dado un token.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <returns>Retorna los roles del usuario segun el token.</returns>
        DtoRol getRolUsuario(string token);

        /// <summary>
        /// Retorna si el usuario tiene los permisos necesarios seguin las etiquetas.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="etiquetas">Permisos que se desean validar.</param>
        /// <returns>Retorna si el usuario tiene los permisos o no.</returns>
        bool autorizarUsuario(string token, string[] etiquetas);

        /// <summary>
        /// Asigna el rol al usuario por el token.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="rol">Rol al que se desea loguear un usuario.</param>
        /// <returns>Devulve si puede asignarse el rol al usuario o no.</returns>
        bool loguearUsuario(string token, DtoRol rol);

        /// <summary>
        /// Retorna una lista con todos los eventos visibles para el usuario.
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <returns>Lista de DtoEvento con su informacion, incluyendo las extensiones del mismo</returns>
        ICollection<DtoEvento> listarEventos(string token);

        /// <summary>
        /// Cierra la sesion del usuario. Eliminando el token y la fecha de inicio de sesion de la base de datos.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <returns>Retorna si puedo realizar la operacion de cerrar sesion.</returns>
        bool cerrarSesion(string token);
        
        /// <summary>
        /// Metodo para agregar al log una nueva accion.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="terminal">Identificacion de la terminal del usuario que realizo el resquest.</param>
        /// <param name="modulo">Identificacion del modulo/proyecto que realizo la operacion de agregar un log.</param>
        /// <param name="Entidad">Identificacion de la entidad que realiza la operacion de agregar un log.</param>
        /// <param name="idEntidad">Idnetificador de la entidad que resulto afectada en la accion al agregar un log.</param>
        /// <param name="accion">Nombre del metodo o funcion que ejecuto una accion y guardo un log.</param>
        /// <param name="detalles">Informacion adicional.</param>
        /// <param name="codigo">Codigo unico definido en Mensajes que es unico para poder referenciar rapido el luagr donde se genero el log.</param>
        void AgregarLog(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        /// <summary>
        /// Metodo para agregar al log  de error.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="terminal">Identificacion de la terminal del usuario que realizo el resquest.</param>
        /// <param name="modulo">Identificacion del modulo/proyecto que realizo la operacion de agregar un log.</param>
        /// <param name="Entidad">Identificacion de la entidad que realiza la operacion de agregar un log.</param>
        /// <param name="idEntidad">Idnetificador de la entidad que resulto afectada en la accion al agregar un log.</param>
        /// <param name="accion">Nombre del metodo o funcion que ejecuto una accion y guardo un log.</param>
        /// <param name="detalles">Informacion adicional.</param>
        /// <param name="codigo">Codigo unico definido en Mensajes que es unico para poder referenciar rapido el luagr donde se genero el log.</param>
        void AgregarLogError(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        /// <summary>
        /// Metodo para agregar al log una nueva accion.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="terminal">Identificacion de la terminal del usuario que realizo el resquest.</param>
        /// <param name="modulo">Identificacion del modulo/proyecto que realizo la operacion de agregar un log.</param>
        /// <param name="Entidad">Identificacion de la entidad que realiza la operacion de agregar un log.</param>
        /// <param name="idEntidad">Idnetificador de la entidad que resulto afectada en la accion al agregar un log.</param>
        /// <param name="accion">Nombre del metodo o funcion que ejecuto una accion y guardo un log.</param>
        /// <param name="detalles">Informacion adicional.</param>
        /// <param name="codigo">Codigo unico definido en Mensajes que es unico para poder referenciar rapido el luagr donde se genero el log.</param>
        void AgregarLogNotification(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        /// <summary>
        /// Metodo para agregar al log  de error.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <param name="terminal">Identificacion de la terminal del usuario que realizo el resquest.</param>
        /// <param name="modulo">Identificacion del modulo/proyecto que realizo la operacion de agregar un log.</param>
        /// <param name="Entidad">Identificacion de la entidad que realiza la operacion de agregar un log.</param>
        /// <param name="idEntidad">Idnetificador de la entidad que resulto afectada en la accion al agregar un log.</param>
        /// <param name="accion">Nombre del metodo o funcion que ejecuto una accion y guardo un log.</param>
        /// <param name="detalles">Informacion adicional.</param>
        /// <param name="codigo">Codigo unico definido en Mensajes que es unico para poder referenciar rapido el luagr donde se genero el log.</param>
        void AgregarLogErrorNotification(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        /// <summary>
        /// Metodo que dado un evento y un identificador de usuario devuelve la informacion del evento.
        /// </summary>
        /// <param name="token">Identificador de usuario.</param>
        /// <param name="idEvento">Identificador del evento.</param>
        /// <returns>Devuelve la informacion del evento segun el documento de interfaz.</returns>
        DtoEvento verInfoEvento(string token, int idEvento);

        /// <summary>
        /// Adjunto la geoubicacion a un usuario.
        /// </summary>
        /// <param name="token">Identificador unico de usuario.</param>
        /// <param name="ubicacion">Objeto de ubicacion.</param>
        /// <returns>Retorna su pudo realizar la persistencia de la  geoubicacion o no.</returns>
        bool adjuntarGeoUbicacion(string token, DtoGeoUbicacion ubicacion);
        

        /// <summary>
        /// Devuelve el nombre y datos de el archivo de imagen indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita la imagen</param>
        /// <param name="idAdjunto">Id de la imagen solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getImageData(string token, int idAdjunto);

        /// <summary>
        /// Devuelve el nombre y datos de el archivo de video indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita el video</param>
        /// <param name="idAdjunto">Id del video solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getVideoData(string token, int idAdjunto);

        /// <summary>
        /// Devuelve el nombre y datos de el archivo de audio indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita el audio</param>
        /// <param name="idAdjunto">Id del audio solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getAudioData(string token, int idAdjunto);

        /// <summary>
        /// Adjunta un archivo de imagen a una extension
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="imagenData">Bytes de la imagen</param>
        /// <param name="extArchivo">Extension de la imagen (ej: ".jpg")</param>
        /// <param name="idExtension">Extension a la cual se desea agregar la imagen</param>
        /// <returns></returns>
        bool adjuntarImagen(string token, byte[] imagenData, string extArchivo, int idExtension);

        /// <summary>
        /// Adjunta un archivo de imagen a una extension
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="imagenData">Bytes de la imagen</param>
        /// <param name="extArchivo">Extension de la imagen (ej: ".jpg")</param>
        /// <param name="idExtension">Extension a la cual se desea agregar la imagen</param>
        /// <returns></returns>
        bool adjuntarVideo(string token, byte[] videoData, string extArchivo, int idExtension);

        /// <summary>
        /// Adjunta un archivo de imagen a una extension
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="imagenData">Bytes de la imagen</param>
        /// <param name="extArchivo">Extension de la imagen (ej: ".jpg")</param>
        /// <param name="idExtension">Extension a la cual se desea agregar la imagen</param>
        /// <returns></returns>
        bool adjuntarAudio(string token, byte[] audioData, string extArchivo, int idExtension);

        /// <summary>
        /// Indica al servidor que el usuario esta conectado.
        /// </summary>
        /// <param name="token">token del usuario</param>
        /// <returns>Si se realizo correctamente</returns>
        bool keepMeAlive(string token);        

        /// <summary>
        /// Interfaz Actualizar descripcion implentado por la capa de servicios.
        /// </summary>
        /// <param name="descParam">Contiene identificador de la extension y la descripcion que se quiere agregar.</param>
        /// <param name="token">token del usuario que desea actualizar descripcion</param>
        /// <returns>Retorna la respuesta segun el documento de interfaz.</returns>
        bool ActualizarDescripcionRecurso(DtoActualizarDescripcionParametro descParam, string token);
        
       /// <summary>
       /// Desconecta a los usuarios inactivos (que no han enviado keep alive) por mas de maxTime minutos.
       /// </summary>
       /// <param name="maxTime">Tiempo en minutos minimo para considerar inactivo a un usuario</param>
        void desconectarAusentes(int maxTime);

        /// <summary>
        /// Indica al servidor que un recurso ha arribado a un evento.
        /// </summary>
        /// <param name="token">Token del usuario recurso</param>
        /// <param name="idExtension">Id de la extension del evento que atiende</param>
        /// <returns></returns>
        bool reportarHoraArribo(string token, int idExtension);

    }
}

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
        DtoAutenticacion autenticarUsuario(string userName, string password, string token);

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
        /// Devuelve el nombre y datos del thumbnail del archivo de imagen indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita la imagen</param>
        /// <param name="idAdjunto">Id de la imagen solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getImageThumbnail(string token, int idAdjunto);

        /// <summary>
        /// Devuelve el nombre y datos de el archivo de video indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita el video</param>
        /// <param name="idAdjunto">Id del video solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getVideoData(string token, int idAdjunto);

        /// <summary>
        /// Devuelve el nombre y datos de el thumbnail del archivo de video indicado.
        /// </summary>
        /// <param name="token">Token del usuario que solicita el video</param>
        /// <param name="idAdjunto">Id del video solicitada</param>
        /// <returns>DtoApplicationFile con los bytes y nombre del archivo</returns>
        DtoApplicationFile getVideoThumbnail(string token, int idAdjunto);

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
        bool adjuntarImagen(string token, DtoApplicationFile imgN);

        /// <summary>
        /// Adjunta un archivo de imagen a una extension
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="imagenData">Bytes de la imagen</param>
        /// <param name="extArchivo">Extension de la imagen (ej: ".jpg")</param>
        /// <param name="idExtension">Extension a la cual se desea agregar la imagen</param>
        /// <returns></returns>
        bool adjuntarVideo(string token, DtoApplicationFile vidN);

        /// <summary>
        /// Adjunta un archivo de imagen a una extension
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="imagenData">Bytes de la imagen</param>
        /// <param name="extArchivo">Extension de la imagen (ej: ".jpg")</param>
        /// <param name="idExtension">Extension a la cual se desea agregar la imagen</param>
        /// <returns></returns>
        bool adjuntarAudio(string token, DtoApplicationFile audN);

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
        bool ActualizarDescripcionRecurso(DtoActualizarDescripcion descParam, string token);
        
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

        /// <summary>
        /// Devuelve la informacin de zonas, sectores, categorias y departamentos necesaria para crear un evento.
        /// </summary>
        /// <returns></returns>
        DtoInfoCreacionEvento getInfoCreacionEvento(string token);

        /// <summary>
        /// Crea un evento en el servidor.
        /// </summary>
        /// <param name="token">Token del usuario que crea el evento</param>
        /// <param name="ev">DtoEvento con la info del evento a crear</param>
        /// <returns>Si el evento se creo correctamente</returns>
        bool crearEvento(string token, DtoEvento ev);

        /// <summary>
        /// Permite a un despachador despachar una extension.
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="idExtension">Id de la extension a despachar</param>
        /// <returns></returns>
        bool tomarExtension(string token, int idExtension);

        /// <summary>
        /// Permite a un despachador liberar una extension que se encuentra despachando.
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="idExtension">Id de la extension a liberar</param>
        /// <returns></returns>
        bool liberarExtension(string token, int idExtension);

        /// <summary>
        /// Permite a un despachador de una extension obtener los recursos asignados a la extension y los recursos disponibles.
        /// </summary>
        /// <param name="token">Token del usuario</param>
        /// <param name="idExtension">Id de la extension a consultar recursos</param>
        /// <returns></returns>
        DtoRecursosExtension getRecursosExtension(string token, int idExtension);

        /// <summary>
        /// Permite a un despachador gestionar los recursos de una extesnion.
        /// </summary>
        /// <param name="token">Token del despachador</param>
        /// <param name="recursos">El dto cuenta con el id de la extension a gestionar</param> 
        /// <param>En la lista "recursosAsignados" se indica los recursos a agregar y en la lista "recursosNoAsignados" se listan los recursos a eliminar de la extension</param>
        /// <returns>Si se realizo correctamente</returns>
        bool gestionarRecursos(string token, DtoRecursosExtension recursos);

        /// <summary>
        /// Permite a un despachador actualizar la segunda categoria de una extension.
        /// </summary>
        /// <param name="token">Token del despachador</param>
        /// <param name="idExtension">Id de la extension a actualizar (debe estar despachandola)</param>
        /// <param name="idCategoria">Id de la categoria nueva (-1 indica eliminar segudna categoria)</param>
        /// <returns>Si se realizo correctamente</returns>
        bool actualizarSegundaCategoria(string token, int idExtension, int idCategoria);

        /// <summary>
        /// Permite a un despachador cerrar una extension de un evento enviado.
        /// </summary>
        /// <param name="token">Token del despachador</param>
        /// <param name="idExtension">Id de la extension a cerrar</param>
        /// <returns>Resultado</returns>
        bool cerrarExtension(string token, int idExtension);

        /// <summary>
        /// Retorna las zonas para las cuales es posible agregar una extension a un evento (usuario debe despachar ext. de ese evento).
        /// </summary>
        /// <param name="token">Token del despachador que solicita la informacion</param>
        /// <param name="idExtension">Extension del evento que despacha</param>
        /// <returns>Lista de DtoZona de las zonas posibles</returns>
        ICollection<DtoZona> getZonasLibresEvento(string token, int idExtension);

        /// <summary>
        /// Permite a un despachador abrir una extension nueva para un evento que despacha.
        /// </summary>
        /// <param name="token">Token del despachador</param>
        /// <param name="idExtension">Extension que despacha</param>
        /// <param name="idZona">Id de la zona para la cual desea abrir una extension</param>
        /// <returns>Si se realizo correctamente</returns>
        bool abrirExtension(string token, int idExtension, int idZona);

        /// <summary>
        /// Permite a un despachador actualizar la descripcion de una extension que despacha.
        /// </summary>
        /// <param name="token">Token del despachador</param>
        /// <param name="descr">Dto con el id de la extension y la descripcion a agregar</param>
        /// <returns>Resultado</returns>
        bool actualizarDescripcionDespachador(string token, DtoActualizarDescripcion descr);

    }
}

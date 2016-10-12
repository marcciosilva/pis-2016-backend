using DataTypeObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="token"></param>
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
        /// Dado un usuario se listan los eventos asociados al mismo.
        /// </summary>
        /// <param name="token">Idnetificador unico del usuario.</param>
        /// <returns>Retorna una coleccion de eventos segun el documento de interfaz.</returns>
        ICollection<DtoEvento> listarEventos(string token);

        /// <summary>
        /// Cierra la sesion del usuario. Eliminando el token y la fecha de inicio de sesion de la base de datos.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <returns>Retorna si puedo realizar la operacion de cerrar sesion.</returns>
        bool cerrarSesion(string token);

        /// <summary>
        /// Obtiene el nombre de usuario a partir de un identificador de usuario.
        /// </summary>
        /// <param name="token">Identificador unico del usuario.</param>
        /// <returns>Retorna el nombre del usuario.</returns>
        string getNombreUsuario(string token);

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
        /// <param name="codigo"></param>
        void AgregarLogError(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);
        
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
    }
}

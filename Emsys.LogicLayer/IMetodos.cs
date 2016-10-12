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

        DtoAutenticacion autenticarUsuario(string userName, string password);

        DtoRol getRolUsuario(string token);

        bool autorizarUsuario(string token, string[] etiquetas);

        bool loguearUsuario(string token, DtoRol rol);

        ICollection<DtoItemListar> listarEventos(string token);

        bool cerrarSesion(string token);

        string getNombreUsuario(string token);

        void AgregarLog(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);

        void AgregarLogError(string token, string terminal, string modulo, string Entidad, int idEntidad, string accion, string detalles, int codigo);
        
        DtoEvento verInfoEvento(string token, int idEvento);

        bool adjuntarGeoUbicacion(string token, DtoGeoUbicacion ubicacion);

        int agregarFileData(byte[] data, string extension);

        DtoApplicationFile getImageData(string token, int idAdjunto);

        DtoApplicationFile getVideoData(string token, int idAdjunto);

        DtoApplicationFile getAudioData(string token, int idAdjunto);

        bool adjuntarImagen(string token, DtoImagen imagen);

        bool adjuntarVideo(string token, DtoVideo video);

        bool adjuntarAudio(string token, DtoAudio audio);

        //string getDataImagen(string token, int idImagen);

    }
}

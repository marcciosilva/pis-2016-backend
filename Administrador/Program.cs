using Microsoft.AspNet.Identity;
using Emsys.DataAccesLayer.Core;
using Emsys.DataAccesLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Administrador
{
    class Program
    {
        //static void MostrarOpciones()
        //        {
        //            Console.WriteLine("Opcion 0: Salir");
        //            Console.WriteLine("Opcion 1: Mostrar opciones");
        //            Console.WriteLine("Opcion 2: Registrar Usuario");
        //            Console.WriteLine("Opcion 3: Crear Unidad Ejecutoora");
        //            Console.WriteLine("Opcion 4: Crear Zona");
        //            Console.WriteLine("Opcion 5: Crear Sector");
        //            Console.WriteLine("Opcion 6: Crear Categoria");
        //            Console.WriteLine("Opcion 7: Crear Evento");
        //            Console.WriteLine("Opcion 8: Crear Recurso");
        //            Console.WriteLine("Opcion 9: Asignar Recurso");
        //            Console.WriteLine("Opcion 10: Iniciar a usuario como recurso");
        //            Console.WriteLine("Opcion 11: Agregar Claim a usuario");
        //            Console.WriteLine("Opcion 12: Agregar Rol");
        //            Console.WriteLine("Opcion 13: Agregar Permiso");
        //            Console.WriteLine("Opcion 14: Asignar permiso a Rol");
        //            Console.WriteLine("Opcion 15: Asignar Rol a un usuario");
        //            Console.WriteLine("Opcion 16: Crear Grupo Recurso");
        //            Console.WriteLine("Opcion 17: Asignar grupo recurso a un usuario");
        //            Console.WriteLine("Opcion 18: Asignar unidad ejecutora a usuario");
        //            Console.WriteLine("");
        //        }

        //        static int elegirOpcion()
        //        {
        //            bool valido = false;
        //            int opcion = 0;
        //            while (!valido)
        //            {
        //                try
        //                {
        //                    opcion = Int32.Parse(Console.ReadLine());
        //                    if (opcion >= 0 && opcion < 19)
        //                    {
        //                        valido = true;
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine("No seas boludo... ingresa bien..");
        //                }
        //            }
        //            return opcion;
        //        }


        //        static void RegistrarUsuario()
        //        {
        //            using (var manager = new EmsysUserManager())
        //            {
        //                Console.WriteLine("Nombre (Real):");
        //                string nombre = Console.ReadLine();
        //                Console.WriteLine("Nombre de usuario:");
        //                string nombreU = Console.ReadLine();
        //                Console.WriteLine("Password:");
        //                string pass = Console.ReadLine();


        //                var user = new ApplicationUser() { UserName = nombreU , Nombre = nombre};
        //                IdentityResult result = manager.Create(user, pass);
        //                if (result.Succeeded)
        //                {
        //                    Console.WriteLine("Exito!");
        //                }
        //                else
        //                {
        //                    Console.WriteLine(result.Errors.FirstOrDefault());
        //                }
        //            }
        //        }

        //        static void CrearSector()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre del sector:");
        //                    string nombre = Console.ReadLine();

        //                    Console.WriteLine("Id de la zona a la que pertenece:");
        //                    int id = Int32.Parse(Console.ReadLine());

        //                    context.Sectores.Add(new Sector() { Nombre = nombre, Zona = context.Zonas.Find(id) });                    
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearZona()
        //        {
        //            using (var context = new EmsysContext())
        //            {                
        //                try
        //                {
        //                    Console.WriteLine("Nombre de la zona:");
        //                    string nombre = Console.ReadLine();

        //                    Console.WriteLine("Id de la Unidad Ejecutora a la que pertenece:");
        //                    int id = Int32.Parse(Console.ReadLine());

        //                    context.Zonas.Add(new Zona() { Nombre= nombre, Unidad_Ejecutora = context.Unidades_Ejecutoras.Find(id)});                   
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearUnidadEjecutora()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                Console.WriteLine("Nombre de la unidad ejecutora:");
        //                string nombre = Console.ReadLine();
        //                try
        //                {
        //                    context.Unidades_Ejecutoras.Add(new Unidad_Ejecutora() { Nombre = nombre });                    
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearCategoria()
        //        {
        //            using (var context = new EmsysContext())
        //            {        
        //                try
        //                {
        //                    Console.WriteLine("Codigo de la categoria:");
        //                    string codigo = Console.ReadLine();

        //                    Console.WriteLine("Descripcion de la categoria:");
        //                    string clave = Console.ReadLine();

        //                    Console.WriteLine("Prioridad de la categoria (0-> Alta, 1-> Media, 2-> Baja): ");
        //                    int prioridad = Int32.Parse(Console.ReadLine());

        //                    context.Categorias.Add(new Categoria() { Codigo = codigo, Clave = clave, Prioridad = (NombrePrioridad)prioridad, Activo = true });                    
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearEvento()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Id de la Categoria:");
        //                    int categoria = Int32.Parse(Console.ReadLine());

        //                    Console.WriteLine("Estado (Creado->0, Enviado->1):");
        //                    int estado = Int32.Parse(Console.ReadLine());

        //                    Console.WriteLine("Nombre de usuario del creador:");
        //                    string creador = Console.ReadLine();

        //                    Console.WriteLine("Id del sector de origen:");
        //                    int sector = Int32.Parse(Console.ReadLine());

        //                    Console.WriteLine("En proceso (0 , 1):");
        //                    int enProceso = Int32.Parse(Console.ReadLine());                    

        //                    Evento ev = new Evento() { Categoria = context.Categorias.Find(categoria),
        //                                Estado = (EstadoEvento)estado, Usuario= context.Users.FirstOrDefault(u=> u.UserName == creador),
        //                                Sector = context.Sectores.Find(sector), EnProceso= enProceso!=0 , TimeStamp= DateTime.Now, FechaCreacion= DateTime.Now};

        //                    List<int> zonas = new List<int>();
        //                    bool seguir = true;
        //                    while (seguir)
        //                    {
        //                        Console.WriteLine("Id de una zona del evento:");
        //                        zonas.Add(Int32.Parse(Console.ReadLine()));
        //                        Console.WriteLine("1-> Agregar mas zonas, 0-> No agregar mas zonas");
        //                        seguir = Int32.Parse(Console.ReadLine()) == 1;
        //                    }
        //                    foreach (int z in zonas)
        //                    {
        //                        context.Extensiones_Evento.Add(new Extension_Evento() { Evento = ev, Zona = context.Zonas.Find(z), Estado = EstadoExtension.FaltaDespachar, TimeStamp = DateTime.Now });
        //                    }
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearRecurso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Codigo:");
        //                    string codigo = Console.ReadLine();

        //                    Recurso rec = new Recurso() { Codigo = codigo, Estado = EstadoRecurso.Disponible, EstadoAsignacion = EstadoAsignacionRecurso.Libre , Grupos_Recursos = new List<Grupo_Recurso>()};
        //                    context.Recursos.Add(rec);

        //                    List<int> grupos = new List<int>();
        //                    bool seguir = true;
        //                    while (seguir)
        //                    {
        //                        Console.WriteLine("Id de un grupo recurso del recurso:");
        //                        grupos.Add(Int32.Parse(Console.ReadLine()));
        //                        Console.WriteLine("1-> Agregar mas grupos, 0-> No agregar mas gripos");
        //                        seguir = Int32.Parse(Console.ReadLine()) == 1;
        //                    }     
        //                    foreach (int g in grupos)
        //                    {
        //                        rec.Grupos_Recursos.Add(context.Grupos_Recursos.Find(g));
        //                    }

        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AsignarRecurso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Codigo del recurso a asignar:");
        //                    string codigo = Console.ReadLine();

        //                    Console.WriteLine("Id de la extension a la cual asignar el recurso:");
        //                    int extension = Int32.Parse(Console.ReadLine());

        //                    context.Recursos.FirstOrDefault(r => r.Codigo == codigo).Extensiones_Eventos.Add(context.Extensiones_Evento.Find(extension));
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AgregarClaim()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de usuario:");
        //                    string usuario = Console.ReadLine();

        //                    Console.WriteLine("ClaimType:");
        //                    string type = Console.ReadLine();

        //                    Console.WriteLine("ClaimValue:");
        //                    string value = Console.ReadLine();

        //                    context.Users.FirstOrDefault(u=> u.UserName== usuario).Claims.Add(new IdentityUserClaim() { ClaimType= type, ClaimValue= value});
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void IniciarUsuarioPorRecurso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de usuario:");
        //                    string usuario = Console.ReadLine();

        //                    Console.WriteLine("Id del recurso:");
        //                    int recurso = Int32.Parse(Console.ReadLine());

        //                    context.Recursos.Find(recurso).Usuario = context.Users.FirstOrDefault(us => us.UserName == usuario);
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void CrearGrupoRecurso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de grupo:");
        //                    string nombre = Console.ReadLine();

        //                    context.Grupos_Recursos.Add(new Grupo_Recurso() { Nombre = nombre });
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AgregarRol()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de rol:");
        //                    string rol = Console.ReadLine();

        //                    context.ApplicationRoles.Add(new ApplicationRole() { Nombre = rol });
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AgregarPermiso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Clave del permiso:");
        //                    string clave = Console.ReadLine();

        //                    Console.WriteLine("Descripcion del permiso:");
        //                    string descripcion = Console.ReadLine();

        //                    context.Permisos.Add(new Permiso() { Clave = clave, Descripcion = descripcion });
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AsignarPermiso()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre del permiso:");
        //                    string permiso = Console.ReadLine();

        //                    Console.WriteLine("Nombre del rol:");
        //                    string rol = Console.ReadLine();

        //                    context.ApplicationRoles.FirstOrDefault(r=> r.Nombre == rol).Permisos.Add(context.Permisos.FirstOrDefault(per => per.Clave == permiso));
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }



        //        static void AsignarRol()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de usuario:");
        //                    string usuario = Console.ReadLine();

        //                    Console.WriteLine("Nombre del rol:");
        //                    string rol = Console.ReadLine();

        //                    context.Users.FirstOrDefault(u => u.UserName == usuario).ApplicationRoles.Add(context.ApplicationRoles.FirstOrDefault(r => r.Nombre == rol));
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AsignarGR()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de usuario:");
        //                    string usuario = Console.ReadLine();

        //                    Console.WriteLine("Id del grupo recurso:");
        //                    int gr = Int32.Parse(Console.ReadLine());

        //                    context.Users.FirstOrDefault(u => u.UserName == usuario).Grupos_Recursos.Add(context.Grupos_Recursos.Find(gr));
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        //        static void AsignarUE()
        //        {
        //            using (var context = new EmsysContext())
        //            {
        //                try
        //                {
        //                    Console.WriteLine("Nombre de usuario:");
        //                    string usuario = Console.ReadLine();

        //                    Console.WriteLine("Id de la unidad ejecutora:");
        //                    int ue = Int32.Parse(Console.ReadLine());

        //                    context.Users.FirstOrDefault(u => u.UserName == usuario).Unidades_Ejecutoras.Add(context.Unidades_Ejecutoras.Find(ue));
        //                    context.SaveChanges();
        //                    Console.WriteLine("Exito!");
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e.ToString());
        //                }
        //            }
        //        }

        static void Main(string[] args)
        {
        }

        //        static void Main(string[] args)
        //        {
        //            bool salir = false;
        //            int opcion;

        //            MostrarOpciones();
        //            while (!salir)
        //            {
        //                Console.WriteLine("Ingrese el numero de la opcione deseada:");
        //                opcion = elegirOpcion();

        //                switch (opcion)
        //                {
        //                    case 0:
        //                        salir = true;
        //                        break;
        //                    case 1:
        //                        MostrarOpciones();
        //                        break;
        //                    case 2:
        //                        RegistrarUsuario();
        //                        break;
        //                    case 3:
        //                        CrearUnidadEjecutora();
        //                        break;
        //                    case 4:
        //                        CrearZona();
        //                        break;
        //                    case 5:
        //                        CrearSector();
        //                        break;
        //                    case 6:
        //                        CrearCategoria();
        //                        break;
        //                    case 7:
        //                        CrearEvento();
        //                        break;
        //                    case 8:
        //                        CrearRecurso();
        //                        break;
        //                    case 9:
        //                        AsignarRecurso();
        //                        break;
        //                    case 10:
        //                        IniciarUsuarioPorRecurso();
        //                        break;
        //                    case 11:
        //                        AgregarClaim();
        //                        break;
        //                    case 12:
        //                        AgregarRol();
        //                        break;
        //                    case 13:
        //                        AgregarPermiso();
        //                        break;
        //                    case 14:
        //                        AsignarPermiso();
        //                        break;
        //                    case 15:
        //                        AsignarRol();
        //                        break;
        //                    case 16:
        //                        CrearGrupoRecurso();
        //                        break;
        //                    case 17:
        //                        AsignarGR();
        //                        break;
        //                    case 18:
        //                        AsignarUE();
        //                        break;
        //                } 

        //  }
        //}
    }
}

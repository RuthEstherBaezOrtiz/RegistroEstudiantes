using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using RegistroEstudiantes.Models;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace RegistroEstudiantes.Controllers
{
    public class EstudiantesController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();

        private static List<Estudiantes> olista = new List<Estudiantes>();

        public SqlTransaction SqlTrans { get; private set; }


        // GET: Estudiantes
        public ActionResult Inicio()
        {
            olista = new List<Estudiantes>();

            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Estudiantes", oconexion);
                cmd.CommandType = CommandType.Text;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Estudiantes nuevoEstudiante = new Estudiantes();

                        nuevoEstudiante.IdEstudiantes = Convert.ToInt32(dr["IdEstudiantes"]);
                        nuevoEstudiante.Nombres = dr["Nombres"].ToString();
                        nuevoEstudiante.Apellidos = dr["Apellidos"].ToString();
                        nuevoEstudiante.Fecha = dr["Fecha"].ToString();
                        nuevoEstudiante.Curso = dr["Curso"].ToString();
                        nuevoEstudiante.Estado = Convert.ToBoolean(dr["Estado"]);

                        olista.Add(nuevoEstudiante);
                    }
                }
            }

            return View(olista);
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Editar(int? IdEstudiantes)
        {
            if (IdEstudiantes == null)
                return RedirectToAction("Inicio", "Estudiantes");

            Estudiantes oestudiantes = olista.Where(c => c.IdEstudiantes == IdEstudiantes).FirstOrDefault();

            return View(oestudiantes);
        }


        [HttpGet]
        public ActionResult Eliminar(int? IdEstudiantes)
        {
            if (IdEstudiantes == null)
                return RedirectToAction("Inicio", "Estudiantes");

            Estudiantes oestudiantes = olista.Where(c => c.IdEstudiantes == IdEstudiantes).FirstOrDefault();

            return View(oestudiantes);
        }


        [HttpPost]
        public ActionResult Registrar(Estudiantes oestudiantes)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
               try
                {
                    SqlCommand cmd = new SqlCommand("sp_Registrar", oconexion);
                    cmd.Parameters.AddWithValue("@Nombres", oestudiantes.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", oestudiantes.Apellidos);
                    cmd.Parameters.AddWithValue("@Fecha", oestudiantes.Fecha);
                    cmd.Parameters.AddWithValue("@Curso", oestudiantes.Curso);
                    cmd.Parameters.AddWithValue("@Estado", oestudiantes.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                }
               catch (Exception)
               {
                    //throw;
                }
            }
            return RedirectToAction("Inicio", "Estudiantes");
        }

        [HttpPost]
        public ActionResult Editar(Estudiantes oestudiantes)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand("sp_Editar", oconexion);
                    cmd.Parameters.AddWithValue("IdEstudiantes", oestudiantes.IdEstudiantes);
                    cmd.Parameters.AddWithValue("Nombres", oestudiantes.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", oestudiantes.Apellidos);
                    cmd.Parameters.AddWithValue("Fecha", oestudiantes.Fecha);
                    cmd.Parameters.AddWithValue("Curso", oestudiantes.Curso);
                    cmd.Parameters.AddWithValue("Estado", oestudiantes.Estado);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }

                catch (Exception)
                {
                    //throw;
                }
                
                
                return RedirectToAction("Inicio", "Estudiantes");
            }
        }

        [HttpPost]
        public ActionResult Eliminar(string IdEstudiantes)
        {
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("sp_Eliminar", oconexion);
                cmd.Parameters.AddWithValue("IdEstudiantes", IdEstudiantes);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Inicio", "Estudiantes");
        }
    }
}
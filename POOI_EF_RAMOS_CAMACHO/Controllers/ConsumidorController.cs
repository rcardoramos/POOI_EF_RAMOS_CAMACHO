using POOI_EF_RAMOS_CAMACHO.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace POOI_EF_RAMOS_CAMACHO.Controllers
{
    public class ConsumidorController : Controller
    {
        private string cadena =
            ConfigurationManager
            .ConnectionStrings["ExamenFinal"]
            .ConnectionString;

        // INDEX - LISTAR
        public ActionResult Index()
        {
            List<Consumidor> lista = new List<Consumidor>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_listar_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Consumidor consumidor = new Consumidor
                    {
                        dnicont = dr["dnicont"].ToString(),
                        nomcont = dr["nomcont"].ToString(),
                        apecont = dr["apecont"].ToString(),
                        emailcont = dr["emailcont"].ToString(),
                        idpais = dr["idpais"].ToString(),
                        nombrepais = dr["nombrepais"].ToString()
                    };

                    lista.Add(consumidor);
                }
            }

            return View(lista);
        }

        // CREATE - GET
        [HttpGet]
        public ActionResult Create()
        {
            CargarPaises();

            return View(new Consumidor());
        }

        // CREATE - POST
        [HttpPost]
        public ActionResult Create(Consumidor consumidor)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_agrega_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@dnicont",
                    consumidor.dnicont
                );

                cmd.Parameters.AddWithValue(
                    "@nomcont",
                    consumidor.nomcont
                );

                cmd.Parameters.AddWithValue(
                    "@apecont",
                    consumidor.apecont
                );

                cmd.Parameters.AddWithValue(
                    "@emailcont",
                    consumidor.emailcont
                );

                cmd.Parameters.AddWithValue(
                    "@idpais",
                    consumidor.idpais
                );

                SqlParameter parametroMensaje =
                    new SqlParameter(
                        "@mensaje",
                        SqlDbType.VarChar,
                        200
                    );

                parametroMensaje.Direction =
                    ParameterDirection.Output;

                cmd.Parameters.Add(parametroMensaje);

                cn.Open();

                cmd.ExecuteNonQuery();

                mensaje =
                    parametroMensaje.Value.ToString();
            }

            ViewBag.Mensaje = mensaje;

            CargarPaises(consumidor.idpais);

            return View(consumidor);
        }

        // DELETE
        public ActionResult Delete(string dni)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_eliminar_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@dnicont",
                    dni
                );

                SqlParameter parametroMensaje =
                    new SqlParameter(
                        "@mensaje",
                        SqlDbType.VarChar,
                        200
                    );

                parametroMensaje.Direction =
                    ParameterDirection.Output;

                cmd.Parameters.Add(parametroMensaje);

                cn.Open();

                cmd.ExecuteNonQuery();

                mensaje =
                    parametroMensaje.Value.ToString();
            }

            TempData["Mensaje"] = mensaje;

            return RedirectToAction("Index");
        }

        // DETAILS
        public ActionResult Details(string dni)
        {
            Consumidor consumidor = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_buscar_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@dnicont",
                    dni
                );

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    consumidor = new Consumidor
                    {
                        dnicont = dr["dnicont"].ToString(),
                        nomcont = dr["nomcont"].ToString(),
                        apecont = dr["apecont"].ToString(),
                        emailcont = dr["emailcont"].ToString(),
                        idpais = dr["idpais"].ToString(),
                        nombrepais = dr["nombrepais"].ToString()
                    };
                }
            }

            if (consumidor == null)
            {
                return HttpNotFound();
            }

            return View(consumidor);
        }

        // UPDATE - GET
        [HttpGet]
        public ActionResult Update(string dni)
        {
            Consumidor consumidor = null;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_buscar_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@dnicont",
                    dni
                );

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    consumidor = new Consumidor
                    {
                        dnicont = dr["dnicont"].ToString(),
                        nomcont = dr["nomcont"].ToString(),
                        apecont = dr["apecont"].ToString(),
                        emailcont = dr["emailcont"].ToString(),
                        idpais = dr["idpais"].ToString()
                    };
                }
            }

            if (consumidor == null)
            {
                return HttpNotFound();
            }

            CargarPaises(consumidor.idpais);

            return View(consumidor);
        }

        // UPDATE - POST
        [HttpPost]
        public ActionResult Update(Consumidor consumidor)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd =
                    new SqlCommand("usp_actualiza_consumidor", cn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@dnicont",
                    consumidor.dnicont
                );

                cmd.Parameters.AddWithValue(
                    "@nomcont",
                    consumidor.nomcont
                );

                cmd.Parameters.AddWithValue(
                    "@apecont",
                    consumidor.apecont
                );

                cmd.Parameters.AddWithValue(
                    "@emailcont",
                    consumidor.emailcont
                );

                cmd.Parameters.AddWithValue(
                    "@idpais",
                    consumidor.idpais
                );

                SqlParameter parametroMensaje =
                    new SqlParameter(
                        "@mensaje",
                        SqlDbType.VarChar,
                        200
                    );

                parametroMensaje.Direction =
                    ParameterDirection.Output;

                cmd.Parameters.Add(parametroMensaje);

                cn.Open();

                cmd.ExecuteNonQuery();

                mensaje =
                    parametroMensaje.Value.ToString();
            }

            ViewBag.Mensaje = mensaje;

            CargarPaises(consumidor.idpais);

            return View(consumidor);
        }

        // LISTA DE PAÍSES
        private void CargarPaises(string seleccionado = "")
        {
            List<SelectListItem> paises =
                new List<SelectListItem>();

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT idpais, nombrepais FROM tb_paises",
                    cn
                );

                cn.Open();

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    paises.Add(
                        new SelectListItem
                        {
                            Value = dr["idpais"].ToString(),
                            Text = dr["nombrepais"].ToString(),
                            Selected =
                                dr["idpais"].ToString() == seleccionado
                        }
                    );
                }
            }

            ViewBag.Paises = paises;
        }
    }


}
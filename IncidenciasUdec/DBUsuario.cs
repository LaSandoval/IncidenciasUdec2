using IncidenciasUdec.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IncidenciasUdec
{
    public class DBUsuario
    {
        public static bool Confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (var dbContext = new REPORTE_UDECEntities())
                {
                    var connectionString = dbContext.Database.Connection as SqlConnection;
                    string query = "UPDATE USUARIO SET USER_CONFIRMADO = 1 WHERE TOKEN = @token";

                    using (SqlCommand cmd = new SqlCommand(query, connectionString))
                    {
                        cmd.Parameters.AddWithValue("@token", token);
                        cmd.CommandType = System.Data.CommandType.Text;

                        connectionString.Open();

                        int nroFilarAfectadas = cmd.ExecuteNonQuery();
                        if (nroFilarAfectadas > 0)
                        {
                            respuesta = true;
                        }

                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool ActualizarContraseña(int actualiza, string pass, string token)
        {
            bool actualizo=false;
            try
            {
                
                using (REPORTE_UDECEntities db = new REPORTE_UDECEntities())
                {
                    var cadenaConexion = db.Database.Connection as SqlConnection;
                    var query = "UPDATE USUARIO SET RESTABLECER_PASS = @actualiza, PASSWORD = @pass WHERE TOKEN = @token";
                    SqlCommand cmd = new SqlCommand(query, cadenaConexion);
                    cmd.Parameters.AddWithValue("@actualiza", actualiza);
                    cmd.Parameters.AddWithValue("@pass", pass);
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.CommandType = System.Data.CommandType.Text;

                    cadenaConexion.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas>0)
                    {
                        actualizo = true;   
                    }

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return actualizo;
        }

        public static USUARIO ObtenerUsuario(string correo)
        {
            USUARIO uSUARIO = null;
            try
            {
                using (REPORTE_UDECEntities db = new REPORTE_UDECEntities())
                {
                    var cadenaConexion = db.Database.Connection as SqlConnection;

                    var query = "SELECT * FROM USUARIO WHERE EMAIL = @correo";
                    SqlCommand cmd = new SqlCommand(query, cadenaConexion);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@correo", correo);

                    cadenaConexion.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                    
                        while (reader.Read())
                        {
                            uSUARIO = new USUARIO();
                            uSUARIO.EMAIL = reader["EMAIL"].ToString();
                            uSUARIO.PASSWORD = reader["PASSWORD"].ToString();
                            uSUARIO.RESTABLECER_PASS = (bool)reader["RESTABLECER_PASS"];
                            uSUARIO.USER_CONFIRMADO = (bool)reader["USER_CONFIRMADO"];
                            uSUARIO.TOKEN = reader["TOKEN"].ToString();
                            uSUARIO.NOMBRE = reader["NOMBRE"].ToString();
                            
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return uSUARIO;
        } 
    }
}
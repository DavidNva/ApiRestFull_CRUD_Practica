using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ApiRestFull_CRUD.Models;
using Microsoft.AspNetCore.Cors;
namespace ApiRestFull_CRUD.Controllers
{
    //[EnableCors("ReglasCors")] //para activar la regla de permitir cualquier origen
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("lista")]
        public ActionResult lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using(var conexion  = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    //var cmd = new SqlCommand("sp_lista_productos", conexion);
                    var cmd = new SqlCommand("select * from PRODUCTO", conexion);
                    cmd.CommandType  = CommandType.Text;

                    using (var rd = cmd.ExecuteReader()){
                        while (rd.Read()) {
                            lista.Add(new Producto()
                            {
                                IdProducto =Convert.ToInt32( rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"]),
                            });
                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
              
            }
        }


        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public ActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new List<Producto>();

            Producto  producto   = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    //var cmd = new SqlCommand("sp_lista_productos", conexion);
                    var cmd = new SqlCommand("select * from PRODUCTO", conexion);
                    cmd.CommandType = CommandType.Text;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto()
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"]),
                            });
                        }
                    }
                }
                producto = lista.Where(item => item.IdProducto == idProducto).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto });

            }
        }

        [HttpPost]
        [Route("Guardar")]
        public ActionResult Guardar([FromBody] Producto objetoProducto)
        {
            Producto producto = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objetoProducto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objetoProducto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objetoProducto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objetoProducto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objetoProducto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok guardado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpPut]
        [Route("Editar")]
        public ActionResult Editar([FromBody] Producto objetoProducto)
        {
            Producto producto = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", objetoProducto.IdProducto == 0? DBNull.Value:objetoProducto.IdProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objetoProducto.CodigoBarra is null ? DBNull.Value: objetoProducto.CodigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objetoProducto.Nombre is null ? DBNull.Value : objetoProducto.Nombre);
                    cmd.Parameters.AddWithValue("marca", objetoProducto.Marca is null ? DBNull.Value : objetoProducto.Marca);
                    cmd.Parameters.AddWithValue("categoria", objetoProducto.Categoria is null ? DBNull.Value : objetoProducto.Categoria);
                    cmd.Parameters.AddWithValue("precio", objetoProducto.Precio == 0 ? DBNull.Value : objetoProducto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok editado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public ActionResult Eliminar(int idProducto)
        {
            Producto producto = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", idProducto);
                    
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }

    }
}

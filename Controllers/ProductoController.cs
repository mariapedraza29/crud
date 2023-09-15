
using crud.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProductoController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("cadenaSQL");

        }
        [HttpGet]
        [Route("Lista")]
        [Authorize]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                idProducto = Convert.ToInt32(rd["IdProducto"]),
                                codigoBarra = rd["CodigoBarra"].ToString(),
                                nombre = rd["Nombre"].ToString(),
                                marca = rd["Marca"].ToString(),
                                categoria = rd["Categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }
                    }
                }
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });
                }
            catch (Exception error) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = lista });
            }
        }
        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto oproducto = new Producto();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_lista_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                idProducto = Convert.ToInt32(rd["IdProducto"]),
                                codigoBarra = rd["CodigoBarra"].ToString(),
                                nombre = rd["Nombre"].ToString(),
                                marca = rd["Marca"].ToString(),
                                categoria = rd["categoria"].ToString(),
                                precio = Convert.ToDecimal(rd["Precio"])
                            });
                        }
                    }
                    oproducto = lista.Where(item => item.idProducto == idProducto).FirstOrDefault();
                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = oproducto });
                }
            } catch (Exception error) { 
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }
        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_producto", conexion);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.codigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                using(var conexion =new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_producto", conexion);
                    cmd.Parameters.AddWithValue("idProducto", objeto.idProducto == 0 ? DBNull.Value : objeto.idProducto);
                    cmd.Parameters.AddWithValue("codigoBarra", objeto.codigoBarra is null ? DBNull.Value : objeto.codigoBarra);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre is null ? DBNull.Value : objeto.nombre);
                    cmd.Parameters.AddWithValue("marca", objeto.marca is null ? DBNull.Value : objeto.marca);
                    cmd.Parameters.AddWithValue("categoria", objeto.categoria is null ? DBNull.Value: objeto.categoria);
                    cmd.Parameters.AddWithValue("precio", objeto.precio ==0 ? DBNull.Value:objeto.precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje=error.Message});
            }
        }
        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
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
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }   
}
 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020GL602.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020GL602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {

        private readonly blogContext _blogContexto;


        public usuariosController(blogContext blogContext)
        {
            _blogContexto= blogContext;
        }


        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get()
        {
            List<usuarios> listaUsuarios = (from u in _blogContexto.usuarios
                                            select u).ToList();

            if (listaUsuarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listaUsuarios);
        }

        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult GetbyId(int id)
        {
            usuarios? usuario = (from u in _blogContexto.usuarios
                                 where u.usuarioId == id
                                 select u).FirstOrDefault();

            if(usuario == null)
            {
                return NotFound();

            }

            return Ok(usuario);
        }

        [HttpGet]
        [Route("find/{nombre}/{apellido}")]

        public IActionResult GetByNombre(string nombre, string apellido) {

            //Retorna una lista porque pueden haber varios usuarios con el mismo nombre y apellido

            List<usuarios> listaUsuario = (from u in _blogContexto.usuarios
                                           where u.nombre.Contains(nombre) && u.apellido.Contains(apellido)
                                           select u).ToList();

            if (listaUsuario == null)
            {
                return NotFound();

            }

            return Ok(listaUsuario);
        }


        [HttpGet]
        [Route("find/{rol}")]

        public IActionResult GetByRol(int rol)
        {

            usuarios? usuario = (from u in _blogContexto.usuarios
                                 where u.rolId == rol
                                 select u).FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();

            }

            return Ok(usuario);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult addUsuario([FromBody] usuarios usuario)
        {
            try
            {
                _blogContexto.usuarios.Add(usuario);
                _blogContexto.SaveChanges();
                return Ok(usuario);

            }catch(Exception ex){
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("Actualizas/{id}")]

        public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioNuevo)
        {

            usuarios? usuario = (from u in _blogContexto.usuarios
                                 where u.usuarioId == id
                                 select u).FirstOrDefault();

            if (usuario == null) return NotFound();

            usuario.rolId = usuarioNuevo.rolId;
            usuario.nombreUsuario = usuarioNuevo.nombreUsuario;
            usuario.clave = usuarioNuevo.clave;
            usuario.nombre = usuarioNuevo.nombre;
            usuario.apellido = usuarioNuevo.apellido;

            _blogContexto.Entry(usuario).State = EntityState.Modified;
            _blogContexto.SaveChanges();
            return Ok(usuarioNuevo);
            
        }

        [HttpDelete]
        [Route("Delete/{id}")]

        public IActionResult eliminarUsuario(int id)
        {
            usuarios? usuario = (from u in _blogContexto.usuarios
                                 where u.usuarioId == id
                                 select u).FirstOrDefault();
            
            if (usuario == null) return NotFound();

            _blogContexto.usuarios.Attach(usuario);
            _blogContexto.usuarios.Remove(usuario);
            _blogContexto.SaveChanges();

            return Ok(usuario);
        }
    }
}

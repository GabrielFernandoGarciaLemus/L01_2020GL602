using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020GL602.Models;

namespace L01_2020GL602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        private readonly blogContext _blogContexto;


        public comentariosController(blogContext blogContext)
        {
            _blogContexto = blogContext;
        }

        [HttpGet]
        [Route("GetAll")]

        public IActionResult GetAll()
        {
            List<comentarios> listaComentarios = (from c in _blogContexto.comentarios
                                                      select c).ToList();

            if (listaComentarios.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listaComentarios);
        }

        [HttpGet]
        [Route("GetById")]

        public IActionResult GetById(int id)
        {
            comentarios? comentario = (from c  in _blogContexto.comentarios
                                       where c.cometarioId == id
                                       select c).FirstOrDefault();

            if (comentario == null)
            {
                return NotFound();
            }

            return Ok(comentario);
        }

        [HttpGet]
        [Route("find/{usuario}")]

        public IActionResult GetByUsuario(int usuario)
        {
            //Retorna una lista porque pueden haber varios comentarios del mismo usuario

            List<comentarios> listaUsuarios= (from c in _blogContexto.comentarios
                                       where c.usuarioId == usuario
                                       select c).ToList();

            if (listaUsuarios == null)
            {
                return NotFound();
            }

            return Ok(listaUsuarios);
        }

        [HttpPost]
        [Route("Add")]

        public IActionResult addComentario([FromBody] comentarios comentario)
        {
            try
            {
                _blogContexto.comentarios.Add(comentario);
                _blogContexto.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        
        public IActionResult updateComentario(int id, [FromBody] comentarios nuevoComentario)
        {
            comentarios? comentario = (from c in _blogContexto.comentarios
                                       where c.cometarioId == id
                                       select c).FirstOrDefault();

            if (comentario == null) return NotFound();

            comentario.publicacionId = nuevoComentario.publicacionId;
            comentario.comentario = nuevoComentario.comentario;
            comentario.usuarioId = nuevoComentario.usuarioId;

            _blogContexto.Entry(comentario).State = EntityState.Modified;
            _blogContexto.SaveChanges();
            return Ok(nuevoComentario);
            
        }

        [HttpDelete]
        [Route("delete/{id}")]

        public IActionResult eliminarComentario(int id)
        {

            comentarios? comentario = (from c in _blogContexto.comentarios
                                       where c.cometarioId == id
                                       select c).FirstOrDefault();

            if (comentario == null) return NotFound();

            _blogContexto.comentarios.Attach(comentario);
            _blogContexto.comentarios.Remove(comentario);
            _blogContexto.SaveChanges();

            return Ok(comentario);
        }
    }
}

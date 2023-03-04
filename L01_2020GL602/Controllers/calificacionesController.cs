using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using L01_2020GL602.Models;

namespace L01_2020GL602.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class calificacionesController : ControllerBase
    {

        private readonly blogContext _blogContexto;


        public calificacionesController(blogContext blogContext)
        {
            _blogContexto = blogContext;
        }

        [HttpGet]
        [Route("GetAll")]
        
        public IActionResult GetAll()
        {
            List<calificaciones> listaCalificacion = (from c in _blogContexto.calificaciones
                                                      select c).ToList();

            if (listaCalificacion.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listaCalificacion);
        }

        [HttpGet]
        [Route("GetById")]

        public IActionResult GetById(int id)
        {
            calificaciones? calificacion = (from c in _blogContexto.calificaciones
                                            where c.calificacionId == id
                                            select c).FirstOrDefault();

            if (calificacion == null)
            {
                return NotFound();
            }

            return Ok(calificacion);
        }


        [HttpGet]
        [Route("Find/{publicacion}")]

        public IActionResult BuscarPublicacion(int publicacion)
        {
            //Retorna una lista porque pueden haber varias calificaciones de una publicacion

            List<calificaciones> listaCalificacion = (from c in _blogContexto.calificaciones
                                                      where c.publicacionId == publicacion
                                                      select c).ToList();

            if (listaCalificacion == null)
            {
                return NotFound();
            }

            return Ok(listaCalificacion);
        }

        [HttpPost]
        [Route("Add")]  
        public IActionResult addCalificacion([FromBody] calificaciones calificacion)
        {
            try
            {
                _blogContexto.calificaciones.Add(calificacion);
                _blogContexto.SaveChanges();
                return Ok(calificacion);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("update/{id}")]

        public IActionResult actualizarCalificacion(int id, [FromBody] calificaciones nuevaCalificacion)
        {
            calificaciones? calificacion = (from c in _blogContexto.calificaciones
                                            where c.calificacionId == id
                                            select c).FirstOrDefault();

            if (calificacion == null) return NotFound();

            calificacion.publicacionId = nuevaCalificacion.publicacionId;
            calificacion.usuarioId = nuevaCalificacion.usuarioId;
            calificacion.calificacion = nuevaCalificacion.calificacion;

            _blogContexto.Entry(calificacion).State = EntityState.Modified;
            _blogContexto.SaveChanges();
            return Ok(nuevaCalificacion);

            
        }

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult eliminar(int id)
        {
            calificaciones? calificacion = (from c in _blogContexto.calificaciones
                                            where c.calificacionId == id
                                            select c).FirstOrDefault();

            if (calificacion == null) return NotFound();

            _blogContexto.calificaciones.Attach(calificacion);
            _blogContexto.calificaciones.Remove(calificacion);
            _blogContexto.SaveChanges();

            return Ok(calificacion);
        }

    }
}

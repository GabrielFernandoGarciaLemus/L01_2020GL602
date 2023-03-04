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
    }
}

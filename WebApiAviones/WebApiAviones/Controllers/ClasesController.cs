using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAviones.Entidades;

namespace WebApiAviones.Controllers
{
    [ApiController]
    [Route("clases")]
    public class ClasesController : ControllerBase
    {
        private readonly AplicationDbContext dbContext;
        private readonly ILogger<ClasesController> log;

        public ClasesController(AplicationDbContext context, ILogger<ClasesController> log)
        {
            this.dbContext = context;
            this.log = log;
        }

        [HttpGet]
        [HttpGet("/listadoClase")]
        public async Task<ActionResult<List<Clase>>> GetAll()
        {
            log.LogInformation("Obteniendo listado de clases");
            return await dbContext.Clases.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Clase>> GetById(int id)
        {
            log.LogInformation("EL ID ES: " + id);
            return await dbContext.Clases.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Clase clase)
        {
            var existeAlumno = await dbContext.Aviones.AnyAsync(x => x.Id == clase.AvionId);

            if (!existeAlumno)
            {
                return BadRequest($"No existe el alumno con el id: {clase.AvionId} ");
            }

            dbContext.Add(clase);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Clase clase, int id)
        {
            var exist = await dbContext.Clases.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("La clase especificada no existe. ");
            }

            if (clase.Id != id)
            {
                return BadRequest("El id de la clase no coincide con el establecido en la url. ");
            }

            dbContext.Update(clase);
            await dbContext.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Clases.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("El Recurso no fue encontrado.");
            }

            dbContext.Remove(new Clase { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}

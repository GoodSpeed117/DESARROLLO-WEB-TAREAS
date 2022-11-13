using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApiAviones.Entidades;
using WebApiAviones.Filtros;
using WebApiAviones.Services;

namespace WebApiAviones.Controllers
{

    [ApiController]
    [Route("api/aviones")]
    public class AvionesController : ControllerBase
    {
        private readonly AplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<AvionesController> logger;


        public AvionesController(AplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<AvionesController> logger)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            //throw new NotImplementedException();
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                AvionesControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                AvionesControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                AvionesControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }

        [HttpGet]
        [HttpGet("listado")]
        [HttpGet("/listado")]
        public async Task<ActionResult<List<Avion>>> Get()
        {
            logger.LogInformation("Se obtiene el listado deaviones");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Aviones.Include(x => x.clases).ToListAsync();
        }



        [HttpGet("primero")]
        public async Task<ActionResult<Avion>> PrimerAvion([FromHeader]int valor, [FromQuery] string avion, [FromForm]int avionid)
        {
            return await dbContext.Aviones.FirstOrDefaultAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Avion>> Get(int id) 
        {
            var avion = await dbContext.Aviones.FirstOrDefaultAsync(a => a.Id == id);
            if (avion == null)
            {
                return NotFound();
            }
            return avion;
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<Avion>> Get([FromRoute] string name)
        {
            var avion = await dbContext.Aviones.FirstOrDefaultAsync(a => a.Name.Contains(name));
            if (avion == null)
            {
                return NotFound();
            }
            return avion;
        }


        [HttpPost]
        public async Task<ActionResult> Post(Avion avion)
        {
            var existeAvioneMismoNombre = await dbContext.Aviones.AnyAsync(x => x.Name == avion.Name);
            if (existeAvioneMismoNombre)
            {
                return BadRequest("Ya existe un Avion con el mismo nombre");
            }


            dbContext.Add(avion);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]

        public async Task<ActionResult> Put(Avion avion, int id)
        {
            if(avion.Id != id)
            {
                return BadRequest("El id del avion no coincide con el establecido en la url");
            }

            dbContext.Update(avion);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Aviones.AnyAsync(x => x.Id == id);
            if(!exist)
            {
                return NotFound("El recuerso no fue encontrado");
            }

            dbContext.Remove(new Avion { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

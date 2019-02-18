
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radix.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Radix.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly EventoContext _context;

        public EventoController(EventoContext context)
        {
            _context = context;

            if (_context.Eventos.Count() == 0)
            {
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                string tag = "brasil.sudeste.sensor01";
                string valor = "23";
                _context.Eventos.Add(new Evento { Timestamp = unixTimestamp, Tag = tag, Valor = valor });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var eventos = await _context.Eventos.ToListAsync();
            List<EventoTag> agrupados = ReturnEventoTag(eventos);

            var regioes =
                (from agr in agrupados
                 group agr by agr.Regiao into g
                 select new { valor = g.Key, count = g.Count() }).ToList();

            var sensores =
                (from agr in agrupados
                 group agr by agr.Sensor into g
                 select new { valor = g.Key, count = g.Count() }).ToList();

            decimal chartValor = 0;
            var chart =
                from charts in eventos
                where decimal.TryParse(charts.Valor, out chartValor)
                group charts by charts.Tag into g
                select new { tag = g.Key, total = g.Sum(t => decimal.Parse(t.Valor)) };
                
            return Ok(new
            {
                eventos = await _context.Eventos.ToListAsync(),
                agrupados = sensores.Union(regioes).OrderBy(t => t.valor).ToList(),
                charts = chart
            });
        }

        private static List<EventoTag> ReturnEventoTag(List<Evento> eventos)
        {
            var agrupados = new List<EventoTag>();

            foreach (var item in eventos)
            {
                var split = item.Tag.Split('.');
                var agrupado = new EventoTag();
                agrupado.Pais = split[0];
                agrupado.Regiao = $"{split[0]}.{split[1]}";
                agrupado.Sensor = $"{split[0]}.{split[1]}.{split[2]}";
                agrupados.Add(agrupado);
            }

            return agrupados;
        }

        // GET: api/Evento/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(long id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound();

            return evento;
        }

        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            _context.Eventos.Add(evento);
            SetStatusEvento(evento);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        private static void SetStatusEvento(Evento evento)
        {
            if (string.IsNullOrWhiteSpace(evento.Valor))
                evento.Status = "Erro";
            else
                evento.Status = "Processado";
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutEvento(long id, Evento evento)
        {
            if (id != evento.Id)
                return BadRequest();

            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvento(long id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                return NotFound();

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

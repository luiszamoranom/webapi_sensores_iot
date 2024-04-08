using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemperaturaController : Controller
{
    private readonly MysqlContext _mysqlContext;

    public TemperaturaController(MysqlContext mysqlContext)
    {
        _mysqlContext = mysqlContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Temperatura>>> getAllTemperaturas()
    {
        var temperaturas = await _mysqlContext.temperatura
            .OrderByDescending(t => t.createdAt)
            .ToListAsync();
        return Ok(temperaturas);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<Temperatura>> getTemperaturaById(int id)
    {
        var temperatura = await _mysqlContext.temperatura.FindAsync(id);
        if (temperatura is null)
        {
            return NotFound("Temperatura no encontrada con ese id");
        }
        return Ok(temperatura);
    }

    [HttpPost]
    public async Task<ActionResult<Temperatura>> addTemperatura(Temperatura temperatura)
    {
        var existeTemperatura = await _mysqlContext.temperatura.FindAsync(temperatura.id);
        if (existeTemperatura != null)
        {
            return Conflict("Ya existe una temperatura con ese id");
        }

        _mysqlContext.temperatura.Add(temperatura);
        await _mysqlContext.SaveChangesAsync();

        return Ok(temperatura);
    }

    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioTemperatura()
    {
        var temperaturas = await _mysqlContext.temperatura.ToListAsync();
        if (!temperaturas.Any())
        {
            return NotFound("No hay registros de temperatura disponibles.");
        }

        var promedio = await _mysqlContext.temperatura.AverageAsync(t => t.valor);
        return Ok(promedio);
    }

    [HttpGet("mas_reciente")]
    public async Task<ActionResult<Temperatura>> getTemperaturaMasReciente()
    {
        var ultimaTemperatura = await _mysqlContext.temperatura
            .OrderByDescending(t => t.createdAt)
            .FirstOrDefaultAsync();

        if (ultimaTemperatura == null)
        {
            return NotFound("No se encontraron registros de temperatura.");
        }

        return Ok(ultimaTemperatura);
    }

    [HttpGet("minima")]
    public async Task<ActionResult<double>> getTemperaturaMinima()
    {
        var temperaturas = await _mysqlContext.temperatura.ToListAsync();
        if (!temperaturas.Any())
        {
            return NotFound("No hay registros de temperatura disponibles.");
        }

        var temperaturaMinima = await _mysqlContext.temperatura.MinAsync(h => h.valor);
        return Ok(temperaturaMinima);
    }

    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getTemperaturaMaxima()
    {
        var temperaturas = await _mysqlContext.temperatura.ToListAsync();
        if (!temperaturas.Any())
        {
            return NotFound("No hay registros de temperatura disponibles.");
        }

        var temperaturaMaxima = await _mysqlContext.temperatura.MaxAsync(h => h.valor);
        return Ok(temperaturaMaxima);
    }
}

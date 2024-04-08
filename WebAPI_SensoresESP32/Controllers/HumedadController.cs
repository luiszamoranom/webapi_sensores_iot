using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_SensoresESP32.Context;
using WebAPI_SensoresESP32.Entities;

namespace WebAPI_SensoresESP32.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HumedadController : Controller
{
    private readonly MysqlContext _mysqlContext;

    public HumedadController(MysqlContext mysqlContext)
    {
        _mysqlContext = mysqlContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<Humedad>>> getAllHumedades()
    {
        var humedades = await _mysqlContext.humedad
            .OrderByDescending(t => t.createdAt)
            .ToListAsync();

        return Ok(humedades);
    }

    [HttpGet("id/{id}")]
    public async Task<ActionResult<Humedad>> getHumedadById(int id)
    {
        var humedad = await _mysqlContext.humedad.FindAsync(id);
        if (humedad is null)
        {
            return NotFound("Humedad no encontrada con ese id");
        }
        return Ok(humedad);
    }

    [HttpPost]
    public async Task<ActionResult<Humedad>> addHumedad(Humedad humedad)
    {
        var existeHumedad = await _mysqlContext.humedad.FindAsync(humedad.id);
        if (existeHumedad != null)
        {
            return Conflict("Ya existe una humedad con ese id");
        }

        _mysqlContext.humedad.Add(humedad);
        await _mysqlContext.SaveChangesAsync();

        return Ok(humedad);
    }

    [HttpGet("promedio")]
    public async Task<ActionResult<double>> getPromedioHumedad()
    {
        var humedades = await _mysqlContext.humedad.ToListAsync();
        if (!humedades.Any())
        {
            return NotFound("No hay registros de humedad disponibles.");
        }

        var promedio = await _mysqlContext.humedad.AverageAsync(t => t.valor);
        return Ok(promedio);
    }

    [HttpGet("mas_reciente")]
    public async Task<ActionResult<Humedad>> getHumedadMasReciente()
    {
        var ultimaHumedad = await _mysqlContext.humedad
            .OrderByDescending(t => t.createdAt)
            .FirstOrDefaultAsync();

        if (ultimaHumedad == null)
        {
            return NotFound("No se encontraron registros de humedad.");
        }

        return Ok(ultimaHumedad);
    }

    [HttpGet("minima")]
    public async Task<ActionResult<double>> getHumedadMinima()
    {
        var humedades = await _mysqlContext.humedad.ToListAsync();
        if (!humedades.Any())
        {
            return NotFound("No hay registros de humedad disponibles.");
        }

        var humedadMinima = await _mysqlContext.humedad.MinAsync(h => h.valor);
        return Ok(humedadMinima);
    }

    [HttpGet("maxima")]
    public async Task<ActionResult<double>> getHumedadMaxima()
    {
        var humedades = await _mysqlContext.humedad.ToListAsync();
        if (!humedades.Any())
        {
            return NotFound("No hay registros de humedad disponibles.");
        }

        var humedadMaxima = await _mysqlContext.humedad.MaxAsync(h => h.valor);
        return Ok(humedadMaxima);
    }
}

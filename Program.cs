using chairs_dotnet7_api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("chairlist"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var chairs = app.MapGroup("api/chair");


//TODO: ASIGNACION DE RUTAS A LOS ENDPOINTS
chairs.MapGet("/", GetChairs);
chairs.MapPost("/",AddChair);
chairs.MapGet("/All/",GetAllChairs);
chairs.MapGet("/Name/{name}",GetChairByName);
chairs.MapPut("/Act/{id}",UpdateChair);
chairs.MapPut("/stock/{id}/{stock}",UpStock);

chairs.MapDelete("/del/{id}",DeleteChair);

app.Run();

//TODO: ENDPOINTS SOLICITADOS
static IResult GetChairs(DataContext db)
{
    return TypedResults.Ok();
}

static IResult AddChair(Chair ch, DataContext db)
{
    //var chair = db.Chairs.Where(s => s.Nombre == ch.Nombre);
    //if(chair != null)
    //{
       // return TypedResults.BadRequest(chair);
    //}
    //Con lo de arriba no funciona, pero asi si
    db.Chairs.Add(ch);
    db.SaveChanges();
    return TypedResults.Created("Silla creada con exito");
}

static IResult GetAllChairs(DataContext db)
{
    return TypedResults.Ok(db.Chairs.ToArray());
} 

static IResult GetChairByName(string name, DataContext db)
{
    var chair = db.Chairs.Where(s => s.Nombre == name);
    if(chair == null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(chair);
}

static IResult UpdateChair(int id, Chair ch,  DataContext db)
{
    var chair = db.Chairs.Find(id);
    if(chair == null)
    {
        return TypedResults.NotFound();
    }
    chair.Altura = ch.Altura;
    chair.Anchura = ch.Anchura;
    chair.Color = ch.Color;
    chair.Material = ch.Material;
    chair.Nombre = ch.Nombre;
    chair.Precio = ch.Precio;
    chair.Profundidad = ch.Profundidad;

    db.Chairs.Add(chair);
    db.SaveChanges();
    return TypedResults.NoContent();
}

static IResult UpStock(int id, int stock, DataContext db)
{
    var chair = db.Chairs.Find(id);
    if(chair == null)
    {
        return TypedResults.NotFound();
    }
    chair.Stock = chair.Stock + stock;

    db.Chairs.Add(chair);

    db.SaveChanges();

    return TypedResults.Ok();
}

static IResult Purchase(Chair ch, DataContext db)
{
    
    return TypedResults.Ok();
}

static IResult DeleteChair(int id, DataContext db)
{
    var chair = db.Chairs.Find(id);
    if(chair == null)
    {
        return TypedResults.NotFound();
    }
    db.Chairs.Remove(chair);
    db.SaveChanges();
    return TypedResults.NoContent();
}

using Lab4.Models;
using Lab4.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Lab4.Controllers;

[Route("api/[controller]")]
public class LibraryController : ODataController
{
    private readonly MyDbContext _context;

    public LibraryController(MyDbContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_context.Books);
    }

    [HttpGet("authors")]
    [EnableQuery]
    public IQueryable<AuthorDTO> GetAuthors()
    {
        return _context.Authors
            .Where(a => a.BirthDate.HasValue)
            .Select(a => new AuthorDTO
            {
                AuthorId = a.AuthorId,
                Name = a.Name,
                Country = a.Country,
                BirthYear = a.BirthDate.Value.Year
            });
    }

}


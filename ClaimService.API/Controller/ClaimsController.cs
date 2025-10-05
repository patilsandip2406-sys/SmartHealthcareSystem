using ClaimService.API.Commands;
using ClaimService.API.Services;
using Infrastructure.Library.DbContexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Library.Models;

namespace ClaimService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly AppDbContext _context;
    private readonly IClaimServices _claimServices;
    public ClaimsController(IMediator mediator, AppDbContext context, IClaimServices claimServices)
    {
        _mediator = mediator;
        _context = context;
        _claimServices = claimServices;
    }

    // GET: api/claims
    [HttpGet]
    public IActionResult GetClaims()
    {
        //var claims = _context.Claims
        //            .Include(c => c.Patient)   // eager load the related Patient
        //            .ToList(); ;

        var claims = _context.Claims
                .Join(_context.Patients,
                  claim => claim.PatientId,
                  patient => patient.Id,
                  (claim, patient) => new
                  {
                      claim.Id,
                      claim.Amount,
                      claim.Diagnosis,
                      claim.Procedure,
                      claim.Status,
                      patient.Name
                  }).ToList();

        return Ok(claims);
    }

    // GET: api/claims/{id}
    [HttpGet("{id}")]
    public IActionResult GetClaim(int id)
    {
        var claim = _context.Claims.Find(id);
        if (claim == null) return NotFound();
        return Ok(claim);
    }

    // POST: api/claims
    [HttpPost]
    public async Task<IActionResult> CreateClaim([FromBody] CreateClaimCommand command)
    {
        var claim = await _mediator.Send(command);
        return Ok(claim);
    }

    // PUT: api/claims/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateClaim(int id, Claim updatedClaim)
    {
        var claim = _context.Claims.Find(id);
        if (claim == null) return NotFound();

        claim.Amount = updatedClaim.Amount;
        claim.Status = updatedClaim.Status;
        _context.SaveChanges();

        return Ok(claim);
    }

    // DELETE: api/claims/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteClaim(int id)
    {
        var claim = _context.Claims.Find(id);
        if (claim == null) return NotFound();

        _context.Claims.Remove(claim);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetClaimSummary(int id)
    {       
        var result = await _claimServices.ClaimSummaryAsync(id);

        return Ok(result);
    }
}

using ClaimService.API.Commands;
using Infrastructure.Library.DbContexts;
using MediatR;
using Shared.Library.Models;

namespace ClaimService.API.Handlers;

public class CreateClaimHandler : IRequestHandler<CreateClaimCommand, Claim>
{
    private readonly AppDbContext _context;

    public CreateClaimHandler(AppDbContext context) => _context = context;

    public async Task<Claim> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var claim = new Claim { PatientId = request.PatientId, Amount = request.Amount };
        _context.Claims.Add(claim);
        await _context.SaveChangesAsync(cancellationToken);
        return claim;
    }
}

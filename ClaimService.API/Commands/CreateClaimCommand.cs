using MediatR;
using Shared.Library.Models;

namespace ClaimService.API.Commands
{
    public record CreateClaimCommand(int PatientId, decimal Amount) : IRequest<Claim>;
    
}

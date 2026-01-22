using Cinema.Domain.Entities;
using Cinema.WinForms.DTO;

namespace Cinema.WinForms.Services
{
    public interface ITicketService
    {
        Task<Ticket> PurchaseTicketAsync(PurchaseTicketRequest request);
        Task<bool> CancelTicketAsync(int ticketId);
    }
}

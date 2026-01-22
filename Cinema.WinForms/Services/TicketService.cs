using Cinema.Domain.Entities;
using Cinema.Infrastructure.Data;
using Cinema.WinForms.DTO;
using Cinema.WinForms.Exceptions;

namespace Cinema.WinForms.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Ticket> PurchaseTicketAsync(PurchaseTicketRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var session = await _unitOfWork.Sessions.FirstOrDefaultAsync(session => session.SessionId == request.SessionId);
                if (session == null)
                    throw new NotFoundException($"Сеанс с ID {request.SessionId} не найден");

                var tiket = await _unitOfWork.Tickets
                    .FirstOrDefaultAsync(tiket => tiket.SessionId == request.SessionId 
                    && tiket.RowNumber == request.RowNumber 
                    && tiket.SeatNumber == request.SeatNumber);

                if (tiket != null && !tiket.IsAvailable)
                    throw new BusinessException("Место уже занято");

                // Создаем билет
                var ticket = new Ticket
                {
                    SessionId = request.SessionId,
                    Price = request.Price,
                    RowNumber = request.RowNumber,
                    SeatNumber = request.SeatNumber,
                    IsAvailable = request.IsAvailable,
                    SeatType = request.SeatType
                };

                await _unitOfWork.Tickets.AddToContextAsync(ticket);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return ticket;

            }
            catch (Exception ex)
            {
                // Откатываем при любой ошибке
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> CancelTicketAsync(int ticketId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var ticket = await _unitOfWork.Tickets.FirstOrDefaultAsync(it => it.TicketId == ticketId);
                if (ticket == null)
                    throw new NotFoundException("Билет не найден");

                _unitOfWork.Tickets.Remove(ticket);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

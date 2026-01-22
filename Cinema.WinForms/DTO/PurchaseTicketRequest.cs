using System.ComponentModel.DataAnnotations;

namespace Cinema.WinForms.DTO
{
    /// <summary>
    /// DTO - Покупки билетов на фильм
    /// </summary>
    public class PurchaseTicketRequest
    {
        [Required(ErrorMessage = "Идентификатор сеанса обязателен")]
        public int SessionId { get; set; }
        [Required(ErrorMessage = "Идентификатор билета обязателен")]
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Номер ряда обязателен")]
        [Range(1, 10, ErrorMessage = "Номер ряда должен быть от 1 до 10")]
        public int RowNumber { get; set; }

        [Required(ErrorMessage = "Номер места обязателен")]
        [Range(1, 15, ErrorMessage = "Номер места должен быть от 1 до 15")]
        public int SeatNumber { get; set; }

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0, 10000, ErrorMessage = "Цена должна быть от 0 до 10000")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Доступность билета обязательна")]
        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "Тип места обязательна")]
        [Range(0, 3, ErrorMessage = "Тип места должен быть от 0 до 3")]
        public int SeatType { get; set; }
    }
}

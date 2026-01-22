namespace Cinema.WinForms.Entities
{
    public class Seat
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsSelected { get; set; }
        public decimal Price { get; set; }
        public SeatType SeatType { get; set; } = SeatType.Standard;

        public static Seat Create(int row, int number, decimal price, SeatType seatType, bool isAvailable = true)
        {
            return new Seat
            {
                Row = row,
                Number = number,
                Price = price,
                SeatType = seatType,
                IsAvailable = isAvailable
            };
        }

        public static Seat Create(int row, int number, decimal price, int seatType, bool isAvailable = true)
        {
            return new Seat
            {
                Row = row,
                Number = number,
                Price = price,
                SeatType = (SeatType)seatType,
                IsAvailable = isAvailable
            };
        }
    }

    public enum SeatType
    {
        Standard,      // Обычное
        Premium,       // Премиум
        VIP,           // VIP
        Handicapped    // Для инвалидов
    }
}

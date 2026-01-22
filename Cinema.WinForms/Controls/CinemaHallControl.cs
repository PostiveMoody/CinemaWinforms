using Cinema.Infrastructure.Data;
using Cinema.WinForms.Entities;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cinema.WinForms.Controls
{
    public class CinemaHallControl : Panel
    {
        private readonly IUnitOfWork _unitOfWork;

        private List<Seat> _seats = new List<Seat>();
        private Size _seatSize = new Size(30, 30);
        private int _spacing = 5;
        private int _rows = 10;
        private int _seatsPerRow = 15;

        public event EventHandler<SeatSelectedEventArgs> SeatSelected;

        public CinemaHallControl()
        {
            this.DoubleBuffered = true; // Убираем мерцание
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScroll = true;
        }

        public async Task InitializeSeatsAsync(List<Domain.Entities.Ticket> tickets)
        {
            GenerateHall();

            var seats = tickets
                .Select(tiket => Seat.Create(tiket.RowNumber, tiket.SeatNumber, tiket.Price, tiket.SeatType, tiket.IsAvailable));

            foreach(var seat in seats)
            {
                var seatRemove =_seats.Where(it => it.Row == seat.Row && it.Number == seat.Number).FirstOrDefault();
                if (seatRemove != null)
                {
                    _seats.Remove(seatRemove);
                    _seats.Add(seat);
                }
            }

            UpdateSize();
            this.Invalidate();
        }

        private void GenerateHall()
        {
            _seats.Clear();
            for (int row = 1; row <= _rows; row++)
            {
                for (int number = 1; number <= _seatsPerRow; number++)
                {
                    // По умолчанию все места свободны 
                    var seat = new Seat
                    {
                        Row = row,
                        Number = number,
                        Price = CalculatePrice(row,number),
                        IsAvailable = true,
                        IsSelected = false
                    };

                    _seats.Add(seat);

                    // Центральные места делаем VIP
                    if (number >= 6 && number <= 10 && row >= 3 && row <= 8)
                        seat.SeatType = SeatType.VIP;
                }
            }
        }

        private decimal CalculatePrice(int row, int seatNum)
        {
            decimal basePrice = 300;

            // VIP места дороже
            if (seatNum >= 6 && seatNum <= 10 && row >= 3 && row <= 8)
                return basePrice * 2;

            // Первые ряды дешевле
            if (row <= 3)
                return basePrice * 0.8m;

            return basePrice;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DrawScreen(g);
            DrawSeats(g);
            DrawLegend(g);
        }

        private void DrawScreen(Graphics g)
        {
            int screenWidth = _seatsPerRow * (_seatSize.Width + _spacing);
            int screenHeight = 30;
            int screenX = (_spacing * 2);
            int screenY = _spacing;

            using (var brush = new LinearGradientBrush(
                new Rectangle(screenX, screenY, screenWidth, screenHeight),
                Color.Silver, Color.DimGray, 0f))
            {
                g.FillRectangle(brush, screenX, screenY, screenWidth, screenHeight);
                g.DrawRectangle(Pens.DarkGray, screenX, screenY, screenWidth, screenHeight);
            }

            using (var font = new Font("Arial", 10, FontStyle.Bold))
            {
                g.DrawString("ЭКРАН", font, Brushes.White,
                    screenX + screenWidth / 2 - 25, screenY + 8);
            }
        }

        private void DrawSeats(Graphics g)
        {
            int startY = 60; // Отступ от экрана

            foreach (var seat in _seats)
            {
                int x = _spacing + (seat.Number - 1) * (_seatSize.Width + _spacing);
                int y = startY + (seat.Row - 1) * (_seatSize.Height + _spacing);

                var seatRect = new Rectangle(x, y, _seatSize.Width, _seatSize.Height);

                // Выбор цвета в зависимости от статуса
                Color seatColor = GetSeatColor(seat);

                // Рисуем место
                using (var brush = new SolidBrush(seatColor))
                {
                    g.FillRectangle(brush, seatRect);
                }

                // Обводка
                g.DrawRectangle(Pens.Black, seatRect);

                // Номер места (только если место достаточно большое)
                if (_seatSize.Width > 20)
                {
                    using (var font = new Font("Arial", 8))
                    {
                        string text = seat.Number.ToString();
                        var textSize = g.MeasureString(text, font);
                        g.DrawString(text, font, Brushes.Black,
                            x + (_seatSize.Width - textSize.Width) / 2,
                            y + (_seatSize.Height - textSize.Height) / 2);
                    }
                }

                // Индикатор выбранного места
                if (seat.IsSelected)
                {
                    using (var pen = new Pen(Color.Yellow, 3))
                    {
                        g.DrawRectangle(pen, x - 1, y - 1,
                            _seatSize.Width + 2, _seatSize.Height + 2);
                    }
                }
            }
        }

        private Color GetSeatColor(Seat seat)
        {
            if (!seat.IsAvailable)
                return Color.Red; // Занято

            if (seat.IsSelected)
                return Color.Green; // Выбрано пользователем

            // Цвет по типу места
            return seat.SeatType switch
            {
                SeatType.Standard => Color.LightBlue,
                SeatType.Premium => Color.Gold,
                SeatType.VIP => Color.Purple,
                SeatType.Handicapped => Color.LightGreen,
                _ => Color.LightGray
            };
        }

        private void DrawLegend(Graphics g)
        {
            int legendX = 10;
            int legendY = this.Height - 120;

            var legendItems = new[]
            {
            ("Свободно (обычное)", Color.LightBlue),
            ("Свободно (премиум)", Color.Gold),
            ("Свободно (VIP)", Color.Purple),
            ("Для инвалидов", Color.LightGreen),
            ("Занято", Color.Red),
            ("Выбрано", Color.Green)
        };

            using (var font = new Font("Arial", 9))
            {
                for (int i = 0; i < legendItems.Length; i++)
                {
                    var (text, color) = legendItems[i];
                    int y = legendY + i * 20;

                    // Квадратик легенды
                    g.FillRectangle(new SolidBrush(color), legendX, y, 15, 15);
                    g.DrawRectangle(Pens.Black, legendX, y, 15, 15);

                    // Текст
                    g.DrawString(text, font, Brushes.Black, legendX + 20, y);
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var clickedSeat = GetSeatAtPoint(e.Location);
            if (clickedSeat != null && clickedSeat.IsAvailable)
            {
                clickedSeat.IsSelected = !clickedSeat.IsSelected;
                this.Invalidate();

                SeatSelected?.Invoke(this,
                    new SeatSelectedEventArgs(clickedSeat));
            }
        }

        private Seat GetSeatAtPoint(Point point)
        {
            int startY = 60;

            foreach (var seat in _seats)
            {
                int x = _spacing + (seat.Number - 1) * (_seatSize.Width + _spacing);
                int y = startY + (seat.Row - 1) * (_seatSize.Height + _spacing);

                var seatRect = new Rectangle(x, y, _seatSize.Width, _seatSize.Height);

                if (seatRect.Contains(point))
                    return seat;
            }

            return null;
        }

        private void UpdateSize()
        {
            int width = _seatsPerRow * (_seatSize.Width + _spacing) + (_spacing * 3);
            int height = _rows * (_seatSize.Height + _spacing) + 180; // + место для экрана и легенды

            this.AutoScrollMinSize = new Size(width, height);
        }

        public List<Seat> GetSelectedSeats()
        {
            return _seats.Where(s => s.IsSelected).ToList();
        }

        public decimal GetTotalPrice()
        {
            return _seats.Where(s => s.IsSelected).Sum(s => s.Price);
        }
    }

    public class SeatSelectedEventArgs : EventArgs
    {
        public Seat Seat { get; }

        public SeatSelectedEventArgs(Seat seat)
        {
            Seat = seat;
        }
    }
}

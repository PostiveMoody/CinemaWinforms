using Cinema.Domain.Entities;
using Cinema.Infrastructure.Data;
using Cinema.WinForms.Controls;
using Cinema.WinForms.Entities;
using Cinema.WinForms.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Cinema.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;

        private const string _WarningTitle = "Предупреждение";
        private const string _WarningMessageTemplate = "Не найден сеанс для фильма: {0}";
        private bool _isCanPurchase = false;

        public MainForm(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            InitializeComponent();
            SetupUI();
        }

        private async void SetupUI()
        {
            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var sessions = await unitOfWork.Sessions
                    .Sessions()
                    .OrderBy(s => s.DateTime)
                    .ToListAsync();
            

            sessionComboBox.DataSource = sessions;
            sessionComboBox.DisplayMember = "DateTime";
            sessionComboBox.SelectedIndex = sessions.Count > 0 ? 0 : -1;

            var movies = await unitOfWork.Movies
                .Movies()
                .OrderBy(m => m.Title)
                .ToListAsync();

            movieComboBox.DataSource = movies;
            movieComboBox.DisplayMember = "Title";
            movieComboBox.SelectedIndex = movies.Count > 0 ? 0 : -1;

            var selectedSession = sessionComboBox.SelectedItem as Session;
            if (selectedSession != null)
            {
                var tickets = await unitOfWork.Tickets
                    .Tickets()
                    .Where(t => t.SessionId == selectedSession.SessionId)
                    .ToListAsync();

                await cinemaHallControl.InitializeSeatsAsync(tickets);
            }
            else
            {
                await cinemaHallControl.InitializeSeatsAsync(new List<Ticket>());
            }

            cinemaHallControl.SeatSelected += CinemaHall_SeatSelected;
        }

        private void CinemaHall_SeatSelected(object sender, SeatSelectedEventArgs e)
        {
            UpdateSelectionInfo();
        }

        private void UpdateSelectionInfo()
        {
            selectedSeatsList.Items.Clear();

            var selectedSeats = cinemaHallControl.GetSelectedSeats();
            foreach (var seat in selectedSeats)
            {
                string type = seat.SeatType switch
                {
                    SeatType.Standard => "Стандарт",
                    SeatType.Premium => "Премиум",
                    SeatType.VIP => "VIP",
                    SeatType.Handicapped => "Для инвалидов",
                    _ => "Неизвестно"
                };

                selectedSeatsList.Items.Add(
                    $"Ряд {seat.Row}, Место {seat.Number} - {type} ({seat.Price:C})");
            }

            decimal total = cinemaHallControl.GetTotalPrice();
            totalLabel.Text = $"Итого: {total:C}";

            bookButton.Enabled = selectedSeats.Any();
            bookButton.BackColor = selectedSeats.Any() ? Color.Green : Color.Gray;
        }

        private async void bookButton_Click(object sender, EventArgs e)
        {
            if (!_isCanPurchase)
            {
                MessageBox.Show("Не найден сеанс для фильма", _WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var selectedSeats = cinemaHallControl.GetSelectedSeats();

            if (!selectedSeats.Any())
            {
                MessageBox.Show("Выберите хотя бы одно место!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string movie = (movieComboBox.SelectedItem as Movie).Title;
            string session = (sessionComboBox.SelectedItem as Session).DateTime.ToString();

            string message = $"Фильм: {movie}\n" +
                            $"Сеанс: {session}\n" +
                            $"Выбрано мест: {selectedSeats.Count}\n" +
                            $"Сумма: {cinemaHallControl.GetTotalPrice():C}\n\n" +
                            "Подтвердить бронирование?";

            var result = MessageBox.Show(message, "Подтверждение бронирования",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Бронирования билета
                await PurchaseTicketAsync(selectedSeats, sessionComboBox.SelectedItem as Session);
                // Здесь обычно сохранение в БД
                MessageBox.Show("Бронирование успешно завершено!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ResetButton_Click(sender, e);
            }
        }

        public async Task PurchaseTicketAsync(List<Seat> seats, Session session)
        {
            if (seats == null || seats.Count == 0 || session == null)
                return;

            var tasks = seats.Select(seat => Task.Run(async () =>
            {
                using var scope = _serviceProvider.CreateScope();
                var ticketService = scope.ServiceProvider.GetRequiredService<ITicketService>();
                var request = new Cinema.WinForms.DTO.PurchaseTicketRequest
                {
                    SessionId = session.SessionId,
                    TicketId = 0,
                    RowNumber = seat.Row,
                    SeatNumber = seat.Number,
                    Price = seat.Price,
                    IsAvailable = false,
                    SeatType = (int)seat.SeatType
                }
                ;
                return await ticketService.PurchaseTicketAsync(request);
            })).ToArray();

            // Ожидаем завершения всех операций покупки
            try
            {
                var purchasedTickets = await Task.WhenAll(tasks);

                foreach (var seat in seats)
                {
                    seat.IsAvailable = false;
                    seat.IsSelected = false;
                }

                cinemaHallControl.Invalidate();
                UpdateSelectionInfo();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            cinemaHallControl.GetSelectedSeats().ForEach(s => s.IsSelected = false);
            cinemaHallControl.Invalidate();
            UpdateSelectionInfo();
        }

        private async void movieComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSeatsList.Items.Clear();
            _isCanPurchase = true;

            using var scope = _serviceProvider.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var selectedMovie = movieComboBox.SelectedItem as Movie;
            if (selectedMovie == null)
                return;

            // Загружаем сеансы для выбранного фильма
            var sessions = await unitOfWork.Sessions
                .Sessions()
                .Where(s => s.MovieId == selectedMovie.MovieId)
                .OrderBy(s => s.DateTime)
                .ToListAsync();

            // Гарантируем, что sessionComboBox.SelectedItem инициализирован
            if (sessions.Count > 0)
            {
                sessionComboBox.DataSource = sessions;
                sessionComboBox.DisplayMember = "DateTime";
                sessionComboBox.SelectedIndex = 0;
            }
            else
            {
                _isCanPurchase = false;
                sessionComboBox.DataSource = null;
                MessageBox.Show(string.Format(_WarningMessageTemplate, selectedMovie.Title), _WarningTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedSession = sessionComboBox.SelectedItem as Session;
            if (selectedSession == null)
            {
                await cinemaHallControl.InitializeSeatsAsync(new List<Ticket>());
                return;
            }

            // Загружаем билеты для выбранного сеанса
            var tickets = await unitOfWork.Tickets
                .Tickets()
                .Where(ticket => ticket.SessionId == selectedSession.SessionId)
                .ToListAsync();

            await cinemaHallControl.InitializeSeatsAsync(tickets);
        }
    }
}

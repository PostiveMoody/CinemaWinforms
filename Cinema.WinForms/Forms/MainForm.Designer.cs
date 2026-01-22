namespace Cinema.WinForms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            panel1 = new Panel();
            sessionLabel = new Label();
            movieLabel = new Label();
            movieComboBox = new ComboBox();
            movieBindingSource = new BindingSource(components);
            sessionComboBox = new ComboBox();
            hallSplitContainer = new SplitContainer();
            cinemaHallControl = new Cinema.WinForms.Controls.CinemaHallControl();
            bookButton = new Button();
            totalLabel = new Label();
            selectedSeatsList = new ListBox();
            selectedSeatsLabel = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)movieBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hallSplitContainer).BeginInit();
            hallSplitContainer.Panel1.SuspendLayout();
            hallSplitContainer.Panel2.SuspendLayout();
            hallSplitContainer.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightGray;
            panel1.Controls.Add(sessionLabel);
            panel1.Controls.Add(movieLabel);
            panel1.Controls.Add(movieComboBox);
            panel1.Controls.Add(sessionComboBox);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1237, 60);
            panel1.TabIndex = 0;
            // 
            // sessionLabel
            // 
            sessionLabel.AutoSize = true;
            sessionLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sessionLabel.Location = new Point(191, 9);
            sessionLabel.Name = "sessionLabel";
            sessionLabel.Size = new Size(43, 15);
            sessionLabel.TabIndex = 3;
            sessionLabel.Text = "Сеанс:";
            // 
            // movieLabel
            // 
            movieLabel.AutoSize = true;
            movieLabel.BackColor = Color.Transparent;
            movieLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            movieLabel.Location = new Point(12, 9);
            movieLabel.Name = "movieLabel";
            movieLabel.Size = new Size(51, 15);
            movieLabel.TabIndex = 2;
            movieLabel.Text = "Фильм:";
            // 
            // movieComboBox
            // 
            movieComboBox.DataSource = movieBindingSource;
            movieComboBox.FormattingEnabled = true;
            movieComboBox.Location = new Point(12, 34);
            movieComboBox.Name = "movieComboBox";
            movieComboBox.Size = new Size(121, 23);
            movieComboBox.TabIndex = 1;
            movieComboBox.SelectedIndexChanged += movieComboBox_SelectedIndexChanged;
            // 
            // movieBindingSource
            // 
            movieBindingSource.DataSource = typeof(Domain.Entities.Movie);
            // 
            // sessionComboBox
            // 
            sessionComboBox.FormattingEnabled = true;
            sessionComboBox.Location = new Point(191, 34);
            sessionComboBox.Name = "sessionComboBox";
            sessionComboBox.Size = new Size(121, 23);
            sessionComboBox.TabIndex = 0;
            //sessionComboBox.SelectedIndexChanged += sessionComboBox_SelectedIndexChanged;
            // 
            // hallSplitContainer
            // 
            hallSplitContainer.Dock = DockStyle.Fill;
            hallSplitContainer.Location = new Point(0, 60);
            hallSplitContainer.Name = "hallSplitContainer";
            // 
            // hallSplitContainer.Panel1
            // 
            hallSplitContainer.Panel1.Controls.Add(cinemaHallControl);
            // 
            // hallSplitContainer.Panel2
            // 
            hallSplitContainer.Panel2.Controls.Add(bookButton);
            hallSplitContainer.Panel2.Controls.Add(totalLabel);
            hallSplitContainer.Panel2.Controls.Add(selectedSeatsList);
            hallSplitContainer.Panel2.Controls.Add(selectedSeatsLabel);
            hallSplitContainer.Size = new Size(1237, 628);
            hallSplitContainer.SplitterDistance = 873;
            hallSplitContainer.TabIndex = 1;
            // 
            // cinemaHallControl
            // 
            cinemaHallControl.AutoScroll = true;
            cinemaHallControl.AutoScrollMinSize = new Size(540, 530);
            cinemaHallControl.BorderStyle = BorderStyle.FixedSingle;
            cinemaHallControl.Location = new Point(3, 3);
            cinemaHallControl.Name = "cinemaHallControl";
            cinemaHallControl.Size = new Size(867, 613);
            cinemaHallControl.TabIndex = 0;
            // 
            // bookButton
            // 
            bookButton.BackColor = Color.Gray;
            bookButton.Font = new Font("Arial", 11F, FontStyle.Bold);
            bookButton.ForeColor = Color.White;
            bookButton.Location = new Point(14, 266);
            bookButton.Name = "bookButton";
            bookButton.Size = new Size(334, 40);
            bookButton.TabIndex = 4;
            bookButton.Text = "Забронировать";
            bookButton.UseVisualStyleBackColor = false;
            bookButton.Click += bookButton_Click;
            // 
            // totalLabel
            // 
            totalLabel.AutoSize = true;
            totalLabel.Location = new Point(20, 239);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(0, 15);
            totalLabel.TabIndex = 3;
            // 
            // selectedSeatsList
            // 
            selectedSeatsList.FormattingEnabled = true;
            selectedSeatsList.Location = new Point(14, 32);
            selectedSeatsList.Name = "selectedSeatsList";
            selectedSeatsList.Size = new Size(334, 199);
            selectedSeatsList.TabIndex = 2;
            // 
            // selectedSeatsLabel
            // 
            selectedSeatsLabel.AutoSize = true;
            selectedSeatsLabel.Location = new Point(14, 14);
            selectedSeatsLabel.Name = "selectedSeatsLabel";
            selectedSeatsLabel.Size = new Size(110, 15);
            selectedSeatsLabel.TabIndex = 1;
            selectedSeatsLabel.Text = "Выбранные места:";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1237, 688);
            Controls.Add(hallSplitContainer);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "Кинотеатр";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)movieBindingSource).EndInit();
            hallSplitContainer.Panel1.ResumeLayout(false);
            hallSplitContainer.Panel2.ResumeLayout(false);
            hallSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)hallSplitContainer).EndInit();
            hallSplitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox movieComboBox;
        private ComboBox sessionComboBox;
        private Label sessionLabel;
        private Label movieLabel;
        private BindingSource movieBindingSource;
        private SplitContainer hallSplitContainer;
        private Controls.CinemaHallControl cinemaHallControl;
        private Label selectedSeatsLabel;
        private ListBox selectedSeatsList;
        private Label totalLabel;
        private Button bookButton;
    }
}

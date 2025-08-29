namespace QLKTX_App.GUI.ChildForm_NhanVien
{
    partial class FormHoaDonHopDong
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvListHD = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnTiep = new FontAwesome.Sharp.IconButton();
            this.btnXuatHD = new FontAwesome.Sharp.IconButton();
            this.btnTaoHD = new FontAwesome.Sharp.IconButton();
            this.btnXoaHD = new FontAwesome.Sharp.IconButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dtpNgayLap = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.nmuSoThang = new System.Windows.Forms.NumericUpDown();
            this.txtGhiChu = new System.Windows.Forms.TextBox();
            this.txtTongTien = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTienTheChan = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTienPhong = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.txtMSSV = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cboPhong = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListHD)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmuSoThang)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvListHD);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.MaximumSize = new System.Drawing.Size(0, 510);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(1047, 510);
            this.panel1.TabIndex = 29;
            // 
            // dgvListHD
            // 
            this.dgvListHD.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(19)))), ((int)(((byte)(73)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(19)))), ((int)(((byte)(102)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListHD.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListHD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListHD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dgvListHD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvListHD.EnableHeadersVisualStyles = false;
            this.dgvListHD.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvListHD.Location = new System.Drawing.Point(10, 150);
            this.dgvListHD.Margin = new System.Windows.Forms.Padding(10);
            this.dgvListHD.MultiSelect = false;
            this.dgvListHD.Name = "dgvListHD";
            this.dgvListHD.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListHD.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvListHD.RowHeadersWidth = 51;
            this.dgvListHD.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvListHD.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvListHD.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dgvListHD.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvListHD.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dgvListHD.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.Control;
            this.dgvListHD.RowTemplate.Height = 35;
            this.dgvListHD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListHD.Size = new System.Drawing.Size(1027, 280);
            this.dgvListHD.TabIndex = 25;
            this.dgvListHD.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListHD_CellClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnTiep);
            this.panel3.Controls.Add(this.btnXuatHD);
            this.panel3.Controls.Add(this.btnTaoHD);
            this.panel3.Controls.Add(this.btnXoaHD);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 430);
            this.panel3.MaximumSize = new System.Drawing.Size(0, 70);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1027, 70);
            this.panel3.TabIndex = 26;
            // 
            // btnTiep
            // 
            this.btnTiep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTiep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTiep.FlatAppearance.BorderSize = 0;
            this.btnTiep.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTiep.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiep.ForeColor = System.Drawing.Color.Black;
            this.btnTiep.IconChar = FontAwesome.Sharp.IconChar.ArrowRight;
            this.btnTiep.IconColor = System.Drawing.Color.Red;
            this.btnTiep.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTiep.IconSize = 25;
            this.btnTiep.Location = new System.Drawing.Point(722, 13);
            this.btnTiep.MaximumSize = new System.Drawing.Size(275, 45);
            this.btnTiep.Name = "btnTiep";
            this.btnTiep.Padding = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.btnTiep.Size = new System.Drawing.Size(143, 45);
            this.btnTiep.TabIndex = 2;
            this.btnTiep.Text = "Tiếp";
            this.btnTiep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTiep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTiep.UseVisualStyleBackColor = false;
            this.btnTiep.Click += new System.EventHandler(this.btnTiep_Click);
            // 
            // btnXuatHD
            // 
            this.btnXuatHD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXuatHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnXuatHD.FlatAppearance.BorderSize = 0;
            this.btnXuatHD.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnXuatHD.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatHD.ForeColor = System.Drawing.Color.Black;
            this.btnXuatHD.IconChar = FontAwesome.Sharp.IconChar.FilePdf;
            this.btnXuatHD.IconColor = System.Drawing.Color.ForestGreen;
            this.btnXuatHD.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnXuatHD.IconSize = 25;
            this.btnXuatHD.Location = new System.Drawing.Point(871, 13);
            this.btnXuatHD.MaximumSize = new System.Drawing.Size(275, 45);
            this.btnXuatHD.Name = "btnXuatHD";
            this.btnXuatHD.Padding = new System.Windows.Forms.Padding(3);
            this.btnXuatHD.Size = new System.Drawing.Size(143, 45);
            this.btnXuatHD.TabIndex = 3;
            this.btnXuatHD.Text = "Xuất Hóa Đơn";
            this.btnXuatHD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnXuatHD.UseVisualStyleBackColor = false;
            this.btnXuatHD.Click += new System.EventHandler(this.btnXuatHD_Click);
            // 
            // btnTaoHD
            // 
            this.btnTaoHD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTaoHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTaoHD.FlatAppearance.BorderSize = 0;
            this.btnTaoHD.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTaoHD.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTaoHD.ForeColor = System.Drawing.Color.Black;
            this.btnTaoHD.IconChar = FontAwesome.Sharp.IconChar.MoneyBillAlt;
            this.btnTaoHD.IconColor = System.Drawing.Color.ForestGreen;
            this.btnTaoHD.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnTaoHD.IconSize = 25;
            this.btnTaoHD.Location = new System.Drawing.Point(425, 13);
            this.btnTaoHD.MaximumSize = new System.Drawing.Size(275, 45);
            this.btnTaoHD.Name = "btnTaoHD";
            this.btnTaoHD.Padding = new System.Windows.Forms.Padding(3);
            this.btnTaoHD.Size = new System.Drawing.Size(143, 45);
            this.btnTaoHD.TabIndex = 0;
            this.btnTaoHD.Text = "Tạo Hóa Đơn";
            this.btnTaoHD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTaoHD.UseVisualStyleBackColor = false;
            this.btnTaoHD.Click += new System.EventHandler(this.btnTaoHD_Click);
            // 
            // btnXoaHD
            // 
            this.btnXoaHD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaHD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnXoaHD.FlatAppearance.BorderSize = 0;
            this.btnXoaHD.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnXoaHD.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXoaHD.ForeColor = System.Drawing.Color.Black;
            this.btnXoaHD.IconChar = FontAwesome.Sharp.IconChar.CircleXmark;
            this.btnXoaHD.IconColor = System.Drawing.Color.Red;
            this.btnXoaHD.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnXoaHD.IconSize = 25;
            this.btnXoaHD.Location = new System.Drawing.Point(574, 13);
            this.btnXoaHD.MaximumSize = new System.Drawing.Size(275, 45);
            this.btnXoaHD.Name = "btnXoaHD";
            this.btnXoaHD.Padding = new System.Windows.Forms.Padding(15, 3, 3, 3);
            this.btnXoaHD.Size = new System.Drawing.Size(142, 45);
            this.btnXoaHD.TabIndex = 1;
            this.btnXoaHD.Text = "Xóa";
            this.btnXoaHD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXoaHD.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnXoaHD.UseVisualStyleBackColor = false;
            this.btnXoaHD.Click += new System.EventHandler(this.btnXoaHD_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dtpNgayLap);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.nmuSoThang);
            this.panel7.Controls.Add(this.txtGhiChu);
            this.panel7.Controls.Add(this.txtTongTien);
            this.panel7.Controls.Add(this.label8);
            this.panel7.Controls.Add(this.txtTienTheChan);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.txtTienPhong);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.txtHoTen);
            this.panel7.Controls.Add(this.txtMSSV);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.label9);
            this.panel7.Controls.Add(this.cboPhong);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(10, 10);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1027, 140);
            this.panel7.TabIndex = 22;
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.CalendarFont = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayLap.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayLap.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayLap.Location = new System.Drawing.Point(800, 89);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.Size = new System.Drawing.Size(149, 30);
            this.dtpNgayLap.TabIndex = 8;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(684, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 23);
            this.label10.TabIndex = 19;
            this.label10.Text = "Ngày lập:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nmuSoThang
            // 
            this.nmuSoThang.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmuSoThang.Location = new System.Drawing.Point(465, 15);
            this.nmuSoThang.Name = "nmuSoThang";
            this.nmuSoThang.Size = new System.Drawing.Size(186, 30);
            this.nmuSoThang.TabIndex = 3;
            // 
            // txtGhiChu
            // 
            this.txtGhiChu.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGhiChu.Location = new System.Drawing.Point(800, 50);
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.Size = new System.Drawing.Size(214, 30);
            this.txtGhiChu.TabIndex = 7;
            // 
            // txtTongTien
            // 
            this.txtTongTien.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTongTien.Location = new System.Drawing.Point(800, 14);
            this.txtTongTien.Name = "txtTongTien";
            this.txtTongTien.Size = new System.Drawing.Size(214, 30);
            this.txtTongTien.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(684, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 23);
            this.label8.TabIndex = 11;
            this.label8.Text = "Ghi chú:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTienTheChan
            // 
            this.txtTienTheChan.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTienTheChan.Location = new System.Drawing.Point(465, 90);
            this.txtTienTheChan.Name = "txtTienTheChan";
            this.txtTienTheChan.Size = new System.Drawing.Size(186, 30);
            this.txtTienTheChan.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(684, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 23);
            this.label7.TabIndex = 11;
            this.label7.Text = "Tổng tiền:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTienPhong
            // 
            this.txtTienPhong.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTienPhong.Location = new System.Drawing.Point(465, 54);
            this.txtTienPhong.Name = "txtTienPhong";
            this.txtTienPhong.Size = new System.Drawing.Size(186, 30);
            this.txtTienPhong.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(333, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Tiền thế chân:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(333, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tiền phòng:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHoTen
            // 
            this.txtHoTen.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHoTen.Location = new System.Drawing.Point(124, 90);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(177, 30);
            this.txtHoTen.TabIndex = 2;
            this.txtHoTen.Leave += new System.EventHandler(this.txtMSSV_Leave);
            // 
            // txtMSSV
            // 
            this.txtMSSV.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMSSV.Location = new System.Drawing.Point(124, 54);
            this.txtMSSV.Name = "txtMSSV";
            this.txtMSSV.Size = new System.Drawing.Size(177, 30);
            this.txtMSSV.TabIndex = 1;
            this.txtMSSV.Leave += new System.EventHandler(this.txtMSSV_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(333, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 23);
            this.label4.TabIndex = 11;
            this.label4.Text = "Số tháng:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(18, 93);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 23);
            this.label9.TabIndex = 11;
            this.label9.Text = "Họ Tên:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPhong
            // 
            this.cboPhong.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPhong.FormattingEnabled = true;
            this.cboPhong.Location = new System.Drawing.Point(124, 17);
            this.cboPhong.Name = "cboPhong";
            this.cboPhong.Size = new System.Drawing.Size(177, 31);
            this.cboPhong.TabIndex = 0;
            this.cboPhong.SelectedIndexChanged += new System.EventHandler(this.cboPhong_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "MSSV:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Phòng:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.MaximumSize = new System.Drawing.Size(0, 60);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(10);
            this.label1.Size = new System.Drawing.Size(1047, 60);
            this.label1.TabIndex = 28;
            this.label1.Text = "Hóa Đơn Hợp Đồng";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormHoaDonHopDong
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1047, 583);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(1065, 630);
            this.Name = "FormHoaDonHopDong";
            this.Text = "FormHoaDonHopDong";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListHD)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmuSoThang)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.DateTimePicker dtpNgayLap;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nmuSoThang;
        private System.Windows.Forms.TextBox txtTongTien;
        private System.Windows.Forms.TextBox txtTienTheChan;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTienPhong;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMSSV;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboPhong;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvListHD;
        private System.Windows.Forms.Panel panel3;
        private FontAwesome.Sharp.IconButton btnXuatHD;
        private FontAwesome.Sharp.IconButton btnTaoHD;
        private FontAwesome.Sharp.IconButton btnXoaHD;
        private System.Windows.Forms.TextBox txtGhiChu;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.Label label9;
        private FontAwesome.Sharp.IconButton btnTiep;
    }
}
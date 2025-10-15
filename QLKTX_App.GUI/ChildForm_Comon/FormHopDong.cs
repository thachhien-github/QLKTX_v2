using QLKTX_App.BLL;
using QLKTX_App.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLKTX_App.GUI.ChildForm_Comon
{
    public partial class FormHopDong : Form
    {
        private readonly HopDongBLL _hdBLL = new HopDongBLL();

        public FormHopDong()
        {
            InitializeComponent();
        }

        private void FormHopDong_Load(object sender, EventArgs e)
        {
            cboTrangThai.Items.AddRange(new string[] { "", "Còn hạn", "Sắp hết hạn", "Hết hạn" });
            LoadHopDong();
        }

        private void LoadHopDong()
        {
            dgvListHopDong.DataSource = _hdBLL.GetAll();
            dgvListHopDong.ClearSelection();

            if (dgvListHopDong.Columns.Count > 0)
            {
                // Đổi tên cột
                dgvListHopDong.Columns["MSSV"].HeaderText = "MSSV";
                dgvListHopDong.Columns["HoTen"].HeaderText = "Họ tên";
                dgvListHopDong.Columns["MaPhong"].HeaderText = "Phòng";
                dgvListHopDong.Columns["LoaiPhong"].HeaderText = "Loại phòng";
                dgvListHopDong.Columns["GiaPhong"].HeaderText = "Giá phòng (VND)";
                dgvListHopDong.Columns["NgayPhanBo"].HeaderText = "Ngày phân bổ";
                dgvListHopDong.Columns["SoThang"].HeaderText = "Số tháng";
                dgvListHopDong.Columns["NgayHetHan"].HeaderText = "Ngày hết hạn";
                dgvListHopDong.Columns["MienTienPhong"].HeaderText = "Miễn phí";
                dgvListHopDong.Columns["SoDotThu"].HeaderText = "Số đợt thu";
                dgvListHopDong.Columns["GhiChu"].HeaderText = "Ghi chú";

                // Format tiền
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0";
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Format ngày
                dgvListHopDong.Columns["NgayPhanBo"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListHopDong.Columns["NgayHetHan"].DefaultCellStyle.Format = "dd/MM/yyyy";
                dgvListHopDong.Columns["NgayPhanBo"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvListHopDong.Columns["NgayHetHan"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvListHopDong.Columns["SoThang"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvListHopDong.Columns["SoDotThu"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


                // Font & căn giữa header
                dgvListHopDong.DefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListHopDong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                dgvListHopDong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
        }


        private void FormatGiaPhong()
        {
            if (dgvListHopDong.Columns.Contains("GiaPhong"))
            {
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Format = "N0"; // số nguyên với dấu phân tách hàng nghìn
                dgvListHopDong.Columns["GiaPhong"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string mssv = txtMSSV.Text.Trim();
            string phong = cboPhong.Text.Trim();
            string trangThai = cboTrangThai.Text.Trim();

            dgvListHopDong.DataSource = _hdBLL.Search(mssv, phong, trangThai);
        }

        private void btnGiaHan_Click(object sender, EventArgs e)
        {
            if (dgvListHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần gia hạn!");
                return;
            }

            string mssv = dgvListHopDong.CurrentRow.Cells["MSSV"].Value.ToString();

            // hỏi số tháng muốn gia hạn
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập số tháng muốn gia hạn:", "Gia hạn hợp đồng", "3");
            if (!int.TryParse(input, out int soThang) || soThang <= 0)
            {
                MessageBox.Show("Số tháng không hợp lệ!");
                return;
            }

            // xác nhận
            if (MessageBox.Show($"Bạn có chắc muốn gia hạn hợp đồng của sinh viên {mssv} thêm {soThang} tháng?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (_hdBLL.GiaHan(mssv, soThang))
            {
                MessageBox.Show("Gia hạn thành công!");
                // reload dữ liệu với filter hiện tại
                btnTimKiem_Click(null, null);
            }
            else
            {
                MessageBox.Show("Gia hạn thất bại!");
            }
        }

        private void btnXuatHopDong_Click(object sender, EventArgs e)
        {
            // Kiểm tra chọn hàng
            if (dgvListHopDong.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn hợp đồng để xuất PDF!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dgvListHopDong.CurrentRow;
                // Lấy dữ liệu an toàn từ DataGridViewRow
                string mssv = row.Cells["MSSV"]?.Value?.ToString() ?? "";
                string hoTen = row.Cells["HoTen"]?.Value?.ToString() ?? "";
                string maPhong = row.Cells["MaPhong"]?.Value?.ToString() ?? "";
                string ghiChu = row.Cells["GhiChu"]?.Value?.ToString() ?? "";

                // Parse giá phòng
                decimal tienPhong = 0;
                if (row.Cells["GiaPhong"]?.Value != null)
                    decimal.TryParse(row.Cells["GiaPhong"].Value.ToString(), out tienPhong);

                // Thông tin ngày (nếu có)
                DateTime? ngayPhanBo = null;
                DateTime tmp;
                if (row.Cells["NgayPhanBo"]?.Value != null && DateTime.TryParse(row.Cells["NgayPhanBo"].Value.ToString(), out tmp))
                    ngayPhanBo = tmp;

                DateTime? ngayHetHan = null;
                if (row.Cells["NgayHetHan"]?.Value != null && DateTime.TryParse(row.Cells["NgayHetHan"].Value.ToString(), out tmp))
                    ngayHetHan = tmp;

                // Tạo model chi tiết hợp đồng
                var ct = new HopDongExporter.ChiTietHopDong
                {
                    MSSV = mssv,
                    HoTen = hoTen,
                    MaPhong = maPhong,
                    TienPhong = tienPhong,
                    TienTheChan = 300000,
                    GhiChu = ghiChu,
                    NguoiLap = Environment.UserName,
                    NgayPhanBo = ngayPhanBo,
                    NgayHetHan = ngayHetHan
                };

                // Số hợp đồng: bạn có thể đổi format nếu muốn
                string soHopDong = $"HD-{ct.MSSV}-{ct.MaPhong}-{DateTime.Now:yyyyMMddHHmmss}";
                DateTime ngayLap = DateTime.Now;

                using (SaveFileDialog sfd = new SaveFileDialog()
                {
                    Filter = "PDF file (*.pdf)|*.pdf",
                    FileName = $"HopDong_{ct.MSSV}_{ct.MaPhong}_{DateTime.Now:yyyyMMdd}.pdf"
                })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    // Gọi exporter đã gộp vào HopDongExporter
                    HopDongExporter.ExportHopDongPdf(soHopDong, ngayLap, ct, sfd.FileName);

                    MessageBox.Show("Xuất hợp đồng PDF thành công!", "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Hỏi mở file
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = sfd.FileName,
                        UseShellExecute = true
                    });
  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất hợp đồng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

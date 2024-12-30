using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL;
namespace KT
{
    public partial class Form1 : Form
    {
        private SinhvienBUS sinhvienBUS;
        private Model1 model1 ;
        private Sinhvien currentSinhvien;
        private bool isAdding;
        public Form1()
        {
            InitializeComponent();
            model1 = new Model1();
            sinhvienBUS = new SinhvienBUS();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadComboBoxLop();
            ResetForm();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
        }

        private void add_Click(object sender, EventArgs e)
        {
            try
            {
                // Khởi tạo đối tượng Sinhvien mới từ dữ liệu trong form
                Sinhvien newSinhvien = new Sinhvien
                {
                    MaSV = txtms.Text.Trim(),
                    HotenSV = txtten.Text.Trim(),
                    Ngaysinh = dateNgaySinh.Value,
                    Malop = cblophoc.SelectedValue.ToString()
                };

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(newSinhvien.MaSV) || string.IsNullOrEmpty(newSinhvien.HotenSV) || string.IsNullOrEmpty(newSinhvien.Malop))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Gọi lớp BLL để thêm sinh viên mới
                sinhvienBUS.AddSinhvien(newSinhvien);

                // Thông báo thành công
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset form và load lại dữ liệu
                ResetForm();
                LoadData();
            }
            catch (Exception ex)
            {
                // Thông báo lỗi
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void edit_Click(object sender, EventArgs e)
        {
            if (currentSinhvien != null)
            {
                txtms.Text = currentSinhvien.MaSV;
                txtten.Text = currentSinhvien.HotenSV;
                dateNgaySinh.Value = (DateTime)currentSinhvien.Ngaysinh;
                cblophoc.SelectedValue = currentSinhvien.Malop;

                // Kích hoạt nút Lưu và K.Lưu
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                isAdding = false;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
            }
        }

        private void del_Click(object sender, EventArgs e)
        {
            if (currentSinhvien != null)
            {
                sinhvienBUS.DeleteSinhvien(currentSinhvien.MaSV);
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                ResetForm();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isAdding)
            {
                Sinhvien newSinhvien = new Sinhvien
                {
                    MaSV = txtms.Text.Trim(),
                    HotenSV = txtten.Text.Trim(),
                    Ngaysinh = dateNgaySinh.Value,
                    Malop = cblophoc.SelectedValue.ToString()
                };
                sinhvienBUS.AddSinhvien(newSinhvien);
            }
            else if (currentSinhvien != null)
            {
                currentSinhvien.HotenSV = txtten.Text.Trim();
                currentSinhvien.Ngaysinh = dateNgaySinh.Value;
                currentSinhvien.Malop = cblophoc.SelectedValue.ToString();
                sinhvienBUS.UpdateSinhvien(currentSinhvien);
            }

            MessageBox.Show("Lưu thành công!");
            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            LoadData();
            ResetForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            ResetForm();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?",
                             "Xác nhận thoát",
                             MessageBoxButtons.YesNo,
                             MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void find_Click(object sender, EventArgs e)
        {
            string search = txtfind.Text.Trim();
            var result = model1.Sinhviens
                .Where(sv => sv.MaSV.Contains(search) || sv.HotenSV.Contains(search))
                .Select(sv => new
                {
                    sv.MaSV,
                    sv.HotenSV,
                    sv.Ngaysinh,
                    sv.Malop,
                    TenLop = sv.Lop.Tenlop
                })
                .ToList();

            dataGridView1.DataSource = result;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maSV = dataGridView1.Rows[e.RowIndex].Cells["MaSV"].Value.ToString();
                currentSinhvien = model1.Sinhviens.FirstOrDefault(sv => sv.MaSV == maSV);

                if (currentSinhvien != null)
                {
                    txtms.Text = currentSinhvien.MaSV;
                    txtten.Text = currentSinhvien.HotenSV;
                    dateNgaySinh.Value = (DateTime)currentSinhvien.Ngaysinh;
                    cblophoc.SelectedValue = currentSinhvien.Malop;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void LoadData()
        {
            var sinhvienList = sinhvienBUS.GetAllSinhvien()
                .Select(sv => new
                {
                    sv.MaSV,
                    sv.HotenSV,
                    sv.Ngaysinh,
                    sv.Malop,
                    TenLop = sv.Lop.Tenlop
                })
                .ToList();
            dataGridView1.DataSource = sinhvienList;
        }


        private void LoadComboBoxLop()
        {
            var lopList = model1.Lops.ToList();
            cblophoc.DataSource = lopList;
            cblophoc.DisplayMember = "TenLop";
            cblophoc.ValueMember = "MaLop";
        }

        private void ResetForm()
        {
            txtms.Clear();
            txtten.Clear();
            dateNgaySinh.Value = DateTime.Now;
            cblophoc.SelectedIndex = -1;
            currentSinhvien = null;
            isAdding = false;
        }
    }
}

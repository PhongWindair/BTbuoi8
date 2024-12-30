using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BUS
{
    public class SinhvienBUS
    {
        private readonly Model1 context;

        public SinhvienBUS()
        {
            context = new Model1();
        }

        // Lấy danh sách tất cả sinh viên
        public List<Sinhvien> GetAllSinhvien()
        {
            return context.Sinhviens.ToList();
        }

        // Lấy sinh viên theo mã
        public Sinhvien GetSinhvienById(string maSV)
        {
            return context.Sinhviens.FirstOrDefault(sv => sv.MaSV == maSV);
        }

        // Thêm sinh viên mới
        public void AddSinhvien(Sinhvien sinhvien)
        {
            context.Sinhviens.Add(sinhvien);
            context.SaveChanges();
        }

        // Cập nhật thông tin sinh viên
        public void UpdateSinhvien(Sinhvien sinhvien)
        {
            var existing = context.Sinhviens.FirstOrDefault(sv => sv.MaSV == sinhvien.MaSV);
            if (existing != null)
            {
                existing.HotenSV = sinhvien.HotenSV;
                existing.Ngaysinh = sinhvien.Ngaysinh;
                existing.Malop = sinhvien.Malop;
                context.SaveChanges();
            }
        }

        // Xóa sinh viên
        public void DeleteSinhvien(string maSV)
        {
            var sinhvien = context.Sinhviens.FirstOrDefault(sv => sv.MaSV == maSV);
            if (sinhvien != null)
            {
                context.Sinhviens.Remove(sinhvien);
                context.SaveChanges();
            }
        }
    }
}

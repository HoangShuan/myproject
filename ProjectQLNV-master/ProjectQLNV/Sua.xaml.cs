using ProjectQLNV.DuLieu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProjectQLNV
{
    /// <summary>
    /// Interaction logic for Sua.xaml
    /// </summary>
    public partial class Sua : Window
    {
        QlnhanVienContext db = new QlnhanVienContext();
        private bool isBtnTimClicked = false;
        private bool txtLuongClicked = false;

        public Sua()
        {
            InitializeComponent();
            txtLuong.GotFocus += txtLuong_GotFocus;
            txtLuong.LostFocus += txtLuong_LostFocus;

            txtLuong.TextChanged += txtLuong_TextChanged;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            List<string> ngoaiNgu = new List<string>();
            
            if (isBtnTimClicked == true)
            {
                var querySua = from NhanVien in db.NhanViens
                               where NhanVien.MaNv == txtMaNV.Text
                               select NhanVien;
                NhanVien nvSua = querySua.First();
                nvSua.HoTen = txtHoTen.Text;
                nvSua.NgaySinh = dpkNgaySinh.SelectedDate;
                nvSua.GioiTinh = radNam.IsChecked == true ? "Nam" : "Nữ";
                if (ckbAnh.IsChecked == true)
                {
                    ngoaiNgu.Add("Anh");
                }
                if (ckbTrung.IsChecked == true)
                {
                    ngoaiNgu.Add("Trung");
                }
                if (ckbDuc.IsChecked == true)
                {
                    ngoaiNgu.Add("Đức");
                }
                nvSua.NgoaiNgu = " ";
                nvSua.NgoaiNgu = string.Join(", ", ngoaiNgu);

                nvSua.Luong = decimal.Parse(txtLuong.Text);
                nvSua.MaPb = cboPhongBan.SelectedValue.ToString();

                db.SaveChanges();
                HienThi();
            }
            else
            {
                MessageBox.Show("Thông tin nhân viên không tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMaNV.Focus();
            }
        }

        private void btnTim_Click(object sender, RoutedEventArgs e)
        {
            var queryTim = from NhanVien in db.NhanViens
                           where NhanVien.MaNv == txtMaNV.Text
                           select NhanVien;
            if (queryTim.Count() > 0)
            {
                NhanVien nvTim = queryTim.First();
                thongTinNV(nvTim);
                isBtnTimClicked = true;
            }
            else
            {
                MessageBox.Show("Mã nhân viên không tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                txtMaNV.Focus();
            }
        }

        private void HienThi()
        {
            var queryHT = from NhanVien in db.NhanViens
                          join PhongBan in db.PhongBans on NhanVien.MaPb equals PhongBan.MaPb
                          where NhanVien.MaNv == txtMaNV.Text
                          select new
                          {
                              NhanVien.MaNv,
                              NhanVien.HoTen,
                              NhanVien.NgaySinh,
                              NhanVien.GioiTinh,
                              NhanVien.NgoaiNgu,
                              NhanVien.Luong,
                              PhongBan.TenPb
                          };
            dtgTTNV.ItemsSource = queryHT.ToList();
        }

        private void txtLuong_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLuongClicked = true;
        }

        private void txtLuong_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLuongClicked = false;
        }

        private void txtLuong_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal luong;
            if (txtLuong.Text.Length >= 3 && txtLuongClicked)
            {
                int selectionStart = txtLuong.SelectionStart;
                int lengthBefore = txtLuong.Text.Length;

                if (decimal.TryParse(txtLuong.Text, out luong))
                {
                    txtLuong.Text = luong.ToString("#,##0");

                    int lengthAfter = txtLuong.Text.Length;
                    txtLuong.SelectionStart = selectionStart + (lengthAfter - lengthBefore);
                }
            }
            else if (txtLuong.Text.Length >= 3 && !txtLuongClicked)
            {
                if (decimal.TryParse(txtLuong.Text, out luong))
                {
                    txtLuong.Text = luong.ToString("#,##0");
                }
            }
        }

        private void thongTinNV(NhanVien nhanVien)
        {
            var queryPB = from PhongBan in db.PhongBans
                          select PhongBan;
            cboPhongBan.ItemsSource = queryPB.ToList();
            cboPhongBan.DisplayMemberPath = "TenPb";
            cboPhongBan.SelectedValuePath = "MaPb";
            txtHoTen.Text = nhanVien.HoTen;
            dpkNgaySinh.SelectedDate = nhanVien.NgaySinh;
            radNam.IsChecked = nhanVien.GioiTinh.Trim() == "Nam";
            radNu.IsChecked = nhanVien.GioiTinh.Trim() == "Nữ";

            ckbAnh.IsChecked = nhanVien.NgoaiNgu.Contains("Anh");
            ckbTrung.IsChecked = nhanVien.NgoaiNgu.Contains("Trung");
            ckbDuc.IsChecked = nhanVien.NgoaiNgu.Contains("Đức");

            txtLuong.Text = nhanVien.Luong?.ToString();

            cboPhongBan.SelectedValue = nhanVien.MaPb;
        }

    }
}

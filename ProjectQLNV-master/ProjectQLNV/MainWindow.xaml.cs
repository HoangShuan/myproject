using ProjectQLNV.DuLieu;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectQLNV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        QlnhanVienContext db = new QlnhanVienContext();
        List<string> ngoaiNgu = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var queryPB = from PhongBan in db.PhongBans
                          select PhongBan;
            cboPhongBan.ItemsSource = queryPB.ToList();
            cboPhongBan.DisplayMemberPath = "TenPb";
            cboPhongBan.SelectedValuePath = "MaPb";
            cboPhongBan.SelectedIndex = 0;
            HienThi();
        }

        private void HienThi()
        {
            var queryHT = from NhanVien in db.NhanViens
                          join PhongBan in db.PhongBans on NhanVien.MaPb equals PhongBan.MaPb
                          orderby NhanVien.MaNv ascending
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
            dtgDanhSach.ItemsSource = queryHT.ToList();
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (checkInput() == true)
            {
                NhanVien nvMoi = new NhanVien();
                nvMoi.MaNv = txtMaNV.Text;
                nvMoi.HoTen = txtHoTen.Text;
                nvMoi.NgaySinh = dpkNgaySinh.SelectedDate;
                nvMoi.GioiTinh = radNam.IsChecked == true ? "Nam" : "Nữ";
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
                nvMoi.NgoaiNgu = " ";
                nvMoi.NgoaiNgu = string.Join(", ", ngoaiNgu);

                nvMoi.Luong = decimal.Parse(txtLuong.Text);
                nvMoi.MaPb = cboPhongBan.SelectedValue.ToString();

                db.NhanViens.Add(nvMoi);
                db.SaveChanges();
                HienThi();
                txtMaNV.Clear();
                txtHoTen.Clear();
                dpkNgaySinh.SelectedDate = DateTime.Parse("1/1/2024");
                radNam.IsChecked = true;
                ckbAnh.IsChecked = true;
                txtLuong.Clear();
                cboPhongBan.SelectedIndex = 0;
            }
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            Sua sua = new Sua();
            sua.ShowDialog();

            HienThi();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            var queryXoa = from NhanVien in db.NhanViens
                           where NhanVien.MaNv == txtMaNV.Text
                           select NhanVien;
            if(queryXoa.Count() > 0 )
            {
                NhanVien nvXoa = queryXoa.First();
                var xacNhan = MessageBox.Show($"Bạn có chắc chắn xóa nhân viên {nvXoa.HoTen} không!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(xacNhan == MessageBoxResult.Yes)
                {
                    db.NhanViens.Remove(nvXoa);
                    db.SaveChanges();
                    HienThi();
                    txtMaNV.Clear();
                }    
            }   
            else
            {
                MessageBox.Show("Mã nhân viên không tồn tại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                txtMaNV.Focus();
            }    
        }

        private void btnTim_Click(object sender, RoutedEventArgs e)
        {
            var queryTim = from PhongBan in db.PhongBans
                           join NhanVien in db.NhanViens on PhongBan.MaPb equals NhanVien.MaPb
                           where NhanVien.GioiTinh == "Nữ"
                           group new { PhongBan, NhanVien } by new
                           {
                               PhongBan.MaPb,
                               PhongBan.TenPb
                           }
                           into grouped
                           select new
                           {
                               grouped.Key.MaPb,
                               grouped.Key.TenPb,
                               SoNVNu = grouped.Count()
                           };
            Tim tim = new Tim();
            tim.dtgThongTinPB.ItemsSource = queryTim.ToList();
            tim.Show();
        }

        private void txtLuong_TextChanged(object sender, TextChangedEventArgs e)
        {
            int selectionStart = txtLuong.SelectionStart;
            int lengthBefore = txtLuong.Text.Length;

            if (txtLuong.Text.Length >= 3)
            {
                decimal luong;
                if (decimal.TryParse(txtLuong.Text, out luong))
                {
                    txtLuong.Text = luong.ToString("#,##0");
                }

                int lengthAfter = txtLuong.Text.Length;
                txtLuong.SelectionStart = selectionStart + (lengthAfter - lengthBefore);
            }
        }

        private bool checkInput()
        {
            if (txtMaNV.Text == "")
            {
                MessageBox.Show("Bạn phải nhập mã nhân viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMaNV.Focus();
                return false;
            }
            if (db.NhanViens.Any(nv => nv.MaNv == txtMaNV.Text))
            {
                MessageBox.Show("Mã nhân viên đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtMaNV.Focus();
                return false;
            }
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Bạn phải nhập họ tên nhân viên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtHoTen.Focus();
                return false;
            }

            DateTime tuoi = (DateTime)dpkNgaySinh.SelectedDate;
            if(DateTime.Now.Year - tuoi.Year < 18)
            {
                MessageBox.Show("Tuổi phải từ 18 trở lên!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                dpkNgaySinh.Focus();
                return false;
            }    

            try
            {
                if (decimal.Parse(txtLuong.Text) <= 0)
                {
                    MessageBox.Show("Lương phải là số nguyên > 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtLuong.SelectAll();
                    txtLuong.Focus();
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lương phải là số nguyên > 0!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                txtLuong.SelectAll();
                txtLuong.Focus();
                return false;
            }
            return true;
        }
    }
}

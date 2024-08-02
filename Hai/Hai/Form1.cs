using HuynhLeHoangPhuc_bt5.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace HuynhLeHoangPhuc_bt5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            {
                try
                {
                    StudentContextDB context = new StudentContextDB();
                    List<Faculty> listFalcultys = context.Faculties.ToList(); //lấy các khoa
                    List<Student> listStudent = context.Students.ToList(); //lấy sinh viên
                    FillFalcultyCombobox(listFalcultys);
                    BindGrid(listStudent);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void FillFalcultyCombobox(List<Faculty> listFalcultys)
        {
            comboBox1.DataSource = listFalcultys;
            comboBox1.DisplayMember = "FacultyName";
            comboBox1.ValueMember = "FacultyID";
        }
        //Hàm binding gridView từ list sinh viên
        private void BindGrid(List<Student> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.StudentID;
                dataGridView1.Rows[index].Cells[1].Value = item.FullName;
                dataGridView1.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dataGridView1.Rows[index].Cells[3].Value = item.AverageScore;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            StudentContextDB context = new StudentContextDB();
            List<Faculty> listFalcultys = context.Faculties.ToList();
            Student student = new Student();
            student.StudentID = txtID.Text;
            student.FullName = txtHoTen.Text;
            student.Faculty = listFalcultys[comboBox1.SelectedIndex];
            student.AverageScore = Convert.ToDouble(txtDiemTB.Text);
            int index = dataGridView1.Rows.Add();
            dataGridView1.Rows[index].Cells[0].Value = student.StudentID;
            dataGridView1.Rows[index].Cells[1].Value = student.FullName;
            dataGridView1.Rows[index].Cells[2].Value = listFalcultys[comboBox1.SelectedIndex].FacultyName;
            dataGridView1.Rows[index].Cells[3].Value = student.AverageScore;
            context.Students.Add(student);
            context.SaveChanges();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        int number = -1;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];

            if (e.ColumnIndex == 0) 
            {
                number = e.RowIndex;
                txtID.Text = row.Cells[0].Value.ToString();
                txtHoTen.Text = row.Cells[1].Value.ToString();
                comboBox1.Text = row.Cells[2].Value.ToString();
                txtDiemTB.Text = row.Cells[3].Value.ToString();

            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(number);

            StudentContextDB context = new StudentContextDB();
            List<Student> listStudent = context.Students.ToList();
            for (int i = 0; i < listStudent.Count; i++) 
            {
                if(i == number) 
                {
                    context.Students.Remove(context.Students.Remove(listStudent[i]));
                }
            }
            context.SaveChanges();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (number < 0) // Ensure that a row is selected
                {
                    MessageBox.Show("Vui lòng chọn sinh viên để sửa.");
                    return;
                }

                using (StudentContextDB context = new StudentContextDB())
                {
                    string studentId = txtID.Text;
                    Student student = context.Students.FirstOrDefault(s => s.StudentID == studentId);

                    if (student == null)
                    {
                        MessageBox.Show("Sinh viên không tồn tại.");
                        return;
                    }

                    // Get selected Faculty ID from comboBox
                    int selectedFacultyId = (int)comboBox1.SelectedValue;
                    Faculty selectedFaculty = context.Faculties.FirstOrDefault(f => f.FacultyID == selectedFacultyId);

                    if (selectedFaculty == null)
                    {
                        MessageBox.Show("Khoa không tồn tại.");
                        return;
                    }

                    // Update student information
                    student.FullName = txtHoTen.Text;
                    student.Faculty = selectedFaculty;
                    student.AverageScore = Convert.ToDouble(txtDiemTB.Text);

                    // Save changes to the database
                    context.SaveChanges();

                    // Update DataGridView
                    DataGridViewRow row = dataGridView1.Rows[number];
                    row.Cells[0].Value = student.StudentID;
                    row.Cells[1].Value = student.FullName;
                    row.Cells[2].Value = student.Faculty.FacultyName;
                    row.Cells[3].Value = student.AverageScore;

                    MessageBox.Show("Thông tin sinh viên đã được cập nhật.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}

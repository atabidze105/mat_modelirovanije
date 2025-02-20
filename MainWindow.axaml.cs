using Avalonia.Controls;
using mat_modelirovanije.Models;
using Microsoft.EntityFrameworkCore;
using Northwoods.Go;
using Northwoods.Go.Layouts;
using Northwoods.Go.Models;
using Northwoods.Go.PanelLayouts;
using System;
using System.Collections.Generic;
using System.Linq;
using static mat_modelirovanije.Helper;
using Panel = Northwoods.Go.Panel;
using TextBlock = Northwoods.Go.TextBlock;

public class MyModel : TreeModel<NodeData, string, object> {}
public class NodeData : MyModel.NodeData
{
    public string Name { get; set; }
}

namespace mat_modelirovanije
{
    public partial class MainWindow : Window
    {
        private List<Department> _Department = DBContext.Departments.Include(x => x.MainDeps).Include(x => x.LowerDeps).Include(x => x.Employees).Include(x => x.DepartmentDirectorNavigation).ToList();
        private List<Employee> _Employees = DBContext.Employees.Include(x => x.Job).Include(x => x.Department).ToList();
        private List<Employee> _SelectedEmployees = new();
        private Diagram _myDiagram;
        public MainWindow()
        {
            InitializeComponent();

            lbox_employee.ItemsSource = _Employees.OrderBy(x => x.Lastname).ToList();
            _myDiagram = diagramControl.Diagram;

            Setup();
        }

        private void Setup()
        {
            _myDiagram.UndoManager.IsEnabled = false;
            _myDiagram.Layout = new TreeLayout { Angle = 90, LayerSpacing = 50 };

            var DisplayEmployee = new Action<InputEvent, GraphObject>((e, obj) => {
                _SelectedEmployees.Clear();
                Panel panel = obj as Panel; ;
                int depId = Convert.ToInt32(panel.Name);

                Department selectedDepartment = _Department.Where(x => x.Id == depId).First();

                LowerDepartmentEmployeesSelection(selectedDepartment);

                lbox_employee.ItemsSource = null;
                lbox_employee.ItemsSource = _SelectedEmployees.OrderBy(x => x.Lastname).ToList();
            });

            _myDiagram.NodeTemplate = new Node("Vertical")
            {
                
            }.Add(new Panel()
            {
                Background = "#e1f4c8",
                Width = 120,
                DoubleClick = DisplayEmployee

            }.Bind("Name", "Key").Add(new TextBlock("Default Text")
            {
                Wrap = Wrap.DesiredSize,
                Margin = 12,
                Stroke = "black"
            }.Bind("Text", "Name")));

            

            List<NodeData> nodes = new List<NodeData>();

            foreach (Department department in _Department)
            {
                nodes.Add(new NodeData() { Name = department.Name, Key = $"{department.Id}", Parent = $"{department.IdMainpepartment}" });
            }

            _myDiagram.Model = new MyModel
            {
                NodeDataSource = nodes
            };
        }

        private void LowerDepartmentEmployeesSelection(Department department)
        {
            _SelectedEmployees.AddRange(_Employees.Where(x => x.DepartmentId == department.Id));
            if (department.LowerDeps.Count > 0)
            {
                foreach(Department dep in department.LowerDeps)
                {
                    LowerDepartmentEmployeesSelection(dep);
                }
            }
        }

        private void RedEmployee_windowOpen(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            Employee employee = lbox_employee.SelectedItem as Employee;
            RedWindow redWindow = new RedWindow(employee);
            redWindow.Show();
            Close();
        }

        private void AddNewEmployee(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            RedWindow redWindow = new RedWindow();
            redWindow.Show();
            Close();
        }
    }
}
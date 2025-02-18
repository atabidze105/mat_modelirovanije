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

public class MyModel : TreeModel<NodeData, string, object> {
}
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
        private Diagram _myDiagram;
        public MainWindow()
        {
            InitializeComponent();

            lbox_employee.ItemsSource = _Employees.ToList();
            _myDiagram = diagramControl.Diagram;

            

            Setup();
        }

        private void Setup()
        {
            _myDiagram.UndoManager.IsEnabled = false;
            _myDiagram.Layout = new TreeLayout { Angle = 90, LayerSpacing = 50 };

            var DisplayEmployee = new Action<InputEvent, GraphObject>((e, obj) => {
                while (true) ;
            });

            _myDiagram.NodeTemplate = new Node("Vertical")
            {
                
            }.Add(new Panel()
            {
                Background = "#e1f4c8",
                Width = 120,
                DoubleClick = DisplayEmployee

            }.Add(new TextBlock("Default Text")
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

        //private void Setup()
        //{
        //    _myDiagram.UndoManager.IsEnabled = false;
        //    _myDiagram.Layout = new TreeLayout { Angle = 90, LayerSpacing = 50 };

        //    _myDiagram.NodeTemplate = new Node("Vertical")
        //    {
        //        Background = "#e1f4c8",
        //        Width = 120
        //    }.Add(new TextBlock("Default Text")
        //    {
        //        Wrap = Wrap.DesiredSize,
        //        Margin = 12,
        //        Stroke = "black"
        //    }.Bind("Text", "Name"));

        //    List<NodeData> nodes = new List<NodeData>();

        //    foreach (Department department in _Department)
        //    {
        //        nodes.Add(new NodeData() { Name = department.Name, Key = $"{department.Id}", Parent = $"{department.IdMainpepartment}" });
        //    }

        //    _myDiagram.Model = new MyModel
        //    {
        //        NodeDataSource = nodes
        //    };
        //}
    }
}
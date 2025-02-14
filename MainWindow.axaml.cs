using Avalonia.Controls;
using mat_modelirovanije.Models;
using Microsoft.EntityFrameworkCore;
using Northwoods.Go;
using Northwoods.Go.Models;
using System.Collections.Generic;
using System.Linq;
using static mat_modelirovanije.Helper;
using TextBlock = Northwoods.Go.TextBlock;
public class MyModel : Model<NodeData, string, object> { }
public class NodeData : MyModel.NodeData { }

namespace mat_modelirovanije
{
    public partial class MainWindow : Window
    {
        private List<Department> _Department = DBContext.Departments.Include(x => x.MainDeps).Include(x => x.LowerDeps).Include(x => x.Employees).Include(x => x.DepartmentDirectorNavigation).ToList();
        private Diagram _myDiagram;
        public MainWindow()
        {
            InitializeComponent();
            _myDiagram = diagramControl.Diagram;
            _myDiagram.NodeTemplate = new Node()
            {
                Background= "#e1f4c8"
            }.Add(new TextBlock("Default Text")
            { 
                Margin = 12, Stroke = "black" 
            }.Bind("Text", "Name"));

            Setup();
        }

        private void Setup()
        {
            // diagram properties
            _myDiagram.UndoManager.IsEnabled = true;  // enable undo & redo 

            _myDiagram.Model = new MyModel
            {
                // for each object in this list, the Diagram creates a Node to represent it
                NodeDataSource = new List<NodeData>
                {
                    new NodeData { Key = "Alpha" },
                    new NodeData { Key = "Beta" }
                }
            };
        }
    }
}

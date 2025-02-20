using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using mat_modelirovanije.Models;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static mat_modelirovanije.Helper;

namespace mat_modelirovanije;

public partial class RedWindow : Window
{
    private EmailAddressAttribute _emailAddressAttribute = new EmailAddressAttribute();
    private Employee _RedEmployee;
    private List<Employee> _Employees = DBContext.Employees.Include(x => x.Job).Include(x => x.Department).ToList();
    private List<Department> _Department = DBContext.Departments.Include(x => x.MainDeps).Include(x => x.LowerDeps).Include(x => x.Employees).Include(x => x.DepartmentDirectorNavigation).ToList();
    private List<Job> _Jobs = DBContext.Jobs.ToList();
    private List<Employee> _SameDepEmployees = [];
    public RedWindow()
    {
        _RedEmployee = new Employee();
        InitializeComponent();
        cbox_department.ItemsSource = _Department.ToList();
        cbox_job.ItemsSource = _Jobs.ToList();
        _SameDepEmployees.AddRange(_Employees.Where(x => x.Id != _RedEmployee.Id && x.Department == _RedEmployee.Department));
        cbox_supervisor.ItemsSource = _SameDepEmployees.ToList();
        cbox_assistant.ItemsSource = _SameDepEmployees.ToList();


        grid_red.DataContext = _RedEmployee;
    }
    public RedWindow(Employee employee)
    {
        _RedEmployee = employee;
        InitializeComponent();
        cbox_department.ItemsSource = _Department.ToList();
        cbox_job.ItemsSource = _Jobs.ToList();
        _SameDepEmployees.AddRange(_Employees.Where(x => x.Id != _RedEmployee.Id && x.Department == _RedEmployee.Department));
        cbox_supervisor.ItemsSource = _SameDepEmployees.ToList();
        cbox_assistant.ItemsSource = _SameDepEmployees.ToList();

        grid_red.DataContext = _RedEmployee;
        DateTime birthday = new DateTime(_RedEmployee.Birthday.Year, _RedEmployee.Birthday.Month, _RedEmployee.Birthday.Day);
        cdatepicker_bday.SelectedDate = birthday;
        btn_del.IsVisible = true;
    }

    private void BackToMainWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_RedEmployee.Name == "" || _RedEmployee.Lastname == "" ||
                _RedEmployee.Name == null || _RedEmployee.Lastname == null ||
                _RedEmployee.Phone.Contains('_') || !_emailAddressAttribute.IsValid(_RedEmployee.Email) ||
                _RedEmployee.Birthday == null || _RedEmployee.Department == null ||
                _RedEmployee.Job == null || _RedEmployee.Room == null || _RedEmployee.Room == "")
                return;

            _RedEmployee.Birthday = new DateOnly(cdatepicker_bday.SelectedDate.Value.Year, cdatepicker_bday.SelectedDate.Value.Month, cdatepicker_bday.SelectedDate.Value.Day);

            if (_RedEmployee.Id == 0)
                DBContext.Employees.Add( _RedEmployee );
            DBContext.SaveChanges();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
        catch { }
    }

    private void Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DBContext.Employees.Remove( _RedEmployee );
        DBContext.SaveChanges();

        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void Department_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        _SameDepEmployees.Clear();
        _SameDepEmployees.AddRange(_Employees.Where(x => x.Id != _RedEmployee.Id && x.Department == _RedEmployee.Department));
        cbox_supervisor.ItemsSource = _SameDepEmployees.ToList();
        cbox_assistant.ItemsSource = _SameDepEmployees.ToList();
    }
}
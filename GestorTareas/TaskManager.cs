using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using GestorTareas.Models;

namespace GestorTareas;

public class TaskManager
{
    private const string FilePath = "tasks.json";
    private List<WorkTask> _workTasks = new List<WorkTask>();

    public TaskManager()
    {
        LoadWorkTask();
    }

    public void AddWorkTask(string title, string description)
    {
        int newId = _workTasks.Count > 0 ? _workTasks.Max(t => t.Id) + 1 : 1;

        var workTask = new WorkTask()
        {
            Id = newId,
            Title = title,
            Description = description
        };

        _workTasks.Add(workTask);

        SaveWorkTask();
    }

    public void WorkTaskList()
    {
        if(_workTasks.Count == 0)
        {
            Console.WriteLine("No hay tareas registradas.");
            return;
        }

        foreach(var workTask in _workTasks)
        {
            string state = workTask.IsCompleted ? "Completada" : "Pendiente";
            Console.WriteLine($"{workTask.Id}. {workTask.Title} - {state}");
        }
    }

    public void CompleteWorkTask(int id)
    {
        var workTask = _workTasks.FirstOrDefault(t => t.Id == id);

        if(workTask == null)
        {
            Console.WriteLine("Tarea no encontrada");

            return;
        }

        workTask.IsCompleted = true;
        SaveWorkTask();
    }

    public void DeleteWorkTask(int id)
    {
        var workTask = _workTasks.FirstOrDefault(t => t.Id == id);
        
        if(workTask == null)
        {
            Console.WriteLine("Tarea no encontrada");
        }

        _workTasks.Remove(workTask);
        SaveWorkTask();
    }

    private void SaveWorkTask()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_workTasks, options);

        File.WriteAllText(FilePath, json);
    }

    private void LoadWorkTask()
    {
        if(File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            _workTasks = JsonSerializer.Deserialize<List<WorkTask>>(json) ?? new List<WorkTask>();
        }
    }

    public void CleanScreen()
    {
        Console.Write("Ingrese una tecla para continuar...");
        Console.ReadLine();
        Console.Clear();
    }
}

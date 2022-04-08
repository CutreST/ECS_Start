using Entities;
using Entities.Components;
using Godot;
using MySystems;
using System;

public class TestComponent : Node, IComponentNode
{
    public Entity MyEntity { get; set; }

    public void OnAwake()
    {
        
    }

    public void OnSetFree()
    {
        MyConsole.Write("Disposing the component");
    }

    public void OnStart()
    {
        MyConsole.Write("Prueba de mensaje default");
        MyConsole.WriteError("Prueba de mensaje de error");
        MyConsole.WriteWarning("Prueba de mensaje de warning");
        MyConsole.Write("Prueba de color custom", Colors.BlueViolet);
    }
    
    public void Reset()
    {
        //throw new NotImplementedException();
    }


}

using Entities;
using Entities.Components;
using Godot;
using System;

public class TestComponent : Node, IComponentNode
{
    public Entity MyEntity { get; set; }

    public void OnAwake()
    {
        
    }

    public void OnSetFree()
    {
        //throw new NotImplementedException();
    }

    public void OnStart()
    {
        GD.Print("Hola caracola, soy el Test Component");
        MyConsole.Write("Prueba de consola 1");
    }

    public void Reset()
    {
        //throw new NotImplementedException();
    }
}

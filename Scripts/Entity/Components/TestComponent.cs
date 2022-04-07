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

    public void Ontart()
    {
        //throw new NotImplementedException();
        GD.Print("Hola caracola, soy el Test Component"); 
    }

    public void Reset()
    {
        //throw new NotImplementedException();
    }
}

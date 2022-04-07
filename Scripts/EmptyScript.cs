using Godot;
using System;

public class EmptyScript : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    public override void _EnterTree()
    {
        GD.Print("Dentro tree");
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Dentro Ready");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

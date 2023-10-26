using Godot;
using System;

public partial class MenuButtonGamepadManager : Node3D
{
	public static MenuButtonGamepadManager instance = null;

	public MenuButton menuButton = null;

	public override void _EnterTree()
    {
        if(instance == null)
		{
			instance = this;
		}
		else
		{
			instance.QueueFree();
		}
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.ActionPress("ui_accept"))
		{

		}
	}
}

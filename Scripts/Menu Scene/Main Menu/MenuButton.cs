using Godot;
using System;

public partial class MenuButton : Button
{
	[Export]
	private bool setStartingActiveButton = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(setStartingActiveButton)
		{
			MenuButtonGamepadManager.instance.SetActiveButton(this);
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

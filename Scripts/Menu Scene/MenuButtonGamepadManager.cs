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

	public void SetActiveButton(MenuButton newButton)
	{
        menuButton = newButton;
        menuButton.GrabFocus();
    }
}

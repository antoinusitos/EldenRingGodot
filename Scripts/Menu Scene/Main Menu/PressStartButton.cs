using Godot;
using System;

public partial class PressStartButton : MenuButton
{
    [Export]
    private Panel TitleScreenMainMenuPanel = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Pressed()
	{
		MultiplayerManager.instance.StartNetworkAsHost();
		Visible = false;
		TitleScreenMainMenuPanel.Visible = true;
    }
}

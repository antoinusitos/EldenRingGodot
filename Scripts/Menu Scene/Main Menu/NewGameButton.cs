using Godot;
using System;

public partial class NewGameButton : MenuButton
{
	[Export]
	private Control UIRoot = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public override void _Pressed()
    {
        TitleScreenManager.instance.StartNewGame();
		UIRoot.Hide();
    }
}

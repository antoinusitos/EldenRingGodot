using Godot;
using System;

public partial class TitleScreenManager : Node
{
    public static TitleScreenManager instance = null;

	[Export]
	public MenuButton StartNewGameButton = null;

    public override void _EnterTree()
    {
        instance = this;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void StartNewGame()
	{
		WorldSaveGameManager.instance.LoadNewGame();
    }
}

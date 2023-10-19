using Godot;
using System;

public partial class WorldSaveGameManager : Node3D
{
	public static WorldSaveGameManager instance = null;

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

	public void LoadNewGame()
	{
        MasterLevel.instance.LoadLevel("res://Levels/test_scene_2.tscn");
    }
}

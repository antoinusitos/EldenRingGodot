using Godot;
using System;

public partial class PlayerManager : CharacterManager
{
	private PlayerLocomotionManager playerLocomotionManager = null;

    public override void _EnterTree()
    {
		base._EnterTree();

        playerLocomotionManager = GetNode<PlayerLocomotionManager>("./PlayerLocomotionManager");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		playerLocomotionManager.HandleAllMovement();
	}
}

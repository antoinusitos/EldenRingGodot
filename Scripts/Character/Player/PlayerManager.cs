using Godot;
using System;

public partial class PlayerManager : CharacterManager, INetworkInterface
{
    public PlayerLocomotionManager playerLocomotionManager = null;
    [Export]
    public PlayerAnimatorManager playerAnimatorManager = null;
	public MultiplayerSpawner multiplayerSpawner = null;

    public override void _EnterTree()
    {
		base._EnterTree();

        playerLocomotionManager = GetNode<PlayerLocomotionManager>("./PlayerLocomotionManager");
        playerAnimatorManager = GetNode<PlayerAnimatorManager>("./PlayerAnimatorManager");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		base._Ready();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		base._Process(delta);

		if(!IsMultiplayerAuthority())
		{
			return;
		}

		playerLocomotionManager.HandleAllMovement();
    }

	void INetworkInterface.OnNetworkSpawned()
	{
        if (!IsInsideTree())
        {
            return;
        }

        if (IsMultiplayerAuthority())
        {
			PlayerCamera.instance.player = this;
            PlayerInputManager.instance.player = this;
        }
    }

    public override void LateUpdate()
    {
        if (!IsMultiplayerAuthority())
        {
            return;
        }

        base.LateUpdate();

        PlayerCamera.instance.HandleAllCameraActions();
    }
}

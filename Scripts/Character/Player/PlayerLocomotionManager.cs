using Godot;
using System;

public partial class PlayerLocomotionManager : CharacterLocomotionManager
{
	private PlayerManager player = null;

	public float verticalMovement = 0;
	public float horizontalMovement = 0;
	public float moveAmount = 0;

	[Export]
	private float walkingSpeed = 20;
	[Export]
	private float runningSpeed = 50;

	private Vector3 moveDirection = Vector3.Zero;

    public override void _EnterTree()
    {
        base._EnterTree();

		player = GetNode<PlayerManager>("..");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleAllMovement()
	{
		HandleGroundedMovement();
	}

	private void GetVerticalAndHorizontalInputs()
	{
		verticalMovement = PlayerInputManager.instance.verticalInput;
		horizontalMovement = PlayerInputManager.instance.horizontalInput;
	}

	private void HandleGroundedMovement()
	{
		GetVerticalAndHorizontalInputs();

		// We get the forward vector of the camera
		moveDirection = PlayerCamera.instance.GlobalTransform.Basis.Z * verticalMovement;
		moveDirection += PlayerCamera.instance.GlobalTransform.Basis.X * horizontalMovement;

		moveDirection.Normalized();
		moveDirection.Y = 0;

		if(PlayerInputManager.instance.moveAmount > 0.5f)
		{
			player.Velocity = moveDirection * runningSpeed * (float)GetProcessDeltaTime();
			player.MoveAndSlide();
		}
		else if(PlayerInputManager.instance.moveAmount <= 0.5f)
		{
			player.Velocity = moveDirection * walkingSpeed * (float)GetProcessDeltaTime();
			player.MoveAndSlide();
		}
	}
}

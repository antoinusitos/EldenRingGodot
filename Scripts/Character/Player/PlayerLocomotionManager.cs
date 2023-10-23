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
	[Export]
	private float rotationSpeed = 15;

	private Vector3 moveDirection = Vector3.Zero;
	private Vector3 targetRotationDirection = Vector3.Zero;

	private Node3D parent = null;

    public override void _EnterTree()
    {
        base._EnterTree();

		player = GetNode<PlayerManager>("..");

		parent = ((Node3D)GetParent());
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
		HandleRotation();

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

	private void HandleRotation()
	{
        targetRotationDirection = Vector3.Zero;
        targetRotationDirection = -PlayerCamera.instance.cameraObject.Transform.Basis.Z * verticalMovement;
        targetRotationDirection -= PlayerCamera.instance.cameraObject.Transform.Basis.X * horizontalMovement;
		targetRotationDirection.Normalized();
		targetRotationDirection.Y = 0;

		if(targetRotationDirection == Vector3.Zero)
		{
			targetRotationDirection = parent.Rotation;
			return;
		}

		Quaternion newRotation = Transform.LookingAt(targetRotationDirection, Vector3.Up).Basis.GetRotationQuaternion();
		Quaternion targetRotation = parent.Basis.GetRotationQuaternion().Slerp(newRotation, rotationSpeed * (float)GetProcessDeltaTime());
        parent.Rotation = targetRotation.GetEuler();
    }
}

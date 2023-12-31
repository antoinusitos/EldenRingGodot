using Godot;
using System;

public partial class PlayerLocomotionManager : CharacterLocomotionManager
{
	private PlayerManager player = null;

	public float verticalMovement = 0;
	public float horizontalMovement = 0;
	public float moveAmount = 0;

	[Export]
	private float walkingSpeed = 40;
	[Export]
    private float runningSpeed = 100;
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
		base._Ready();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);

        if (player.IsMultiplayerAuthority())
        {
            player.characterNetworkManager.UpdateVerticalMovement(verticalMovement);
            player.characterNetworkManager.UpdateHorizontalMovement(horizontalMovement);
            player.characterNetworkManager.UpdateMoveAmount(moveAmount);
        }
        else
        {
            verticalMovement = player.characterNetworkManager.verticalMovement;
            horizontalMovement = player.characterNetworkManager.horizontalMovement;
            moveAmount = player.characterNetworkManager.moveAmount;

            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
    }

    public void HandleAllMovement()
	{
		HandleGroundedMovement();
		HandleRotation();
    }

	private void GetMovementValues()
	{
		verticalMovement = PlayerInputManager.instance.verticalInput;
		horizontalMovement = PlayerInputManager.instance.horizontalInput;
		moveAmount = PlayerInputManager.instance.moveAmount;
	}

	private void HandleGroundedMovement()
	{
        GetMovementValues();

		// We get the forward vector of the camera
		moveDirection = PlayerCamera.instance.GlobalTransform.Basis.Z * verticalMovement;
		moveDirection += PlayerCamera.instance.GlobalTransform.Basis.X * horizontalMovement;

		moveDirection.Normalized();
		moveDirection.Y = 0;

		Quaternion parentRotation = Quaternion.FromEuler(parent.Rotation);

		float multiplier = PlayerInputManager.instance.moveAmount > 0.5f ? 1 : 0.5f;

        player.Velocity = Utility.DivideVector3ByFloat((parentRotation.Normalized() * player.playerAnimatorManager.animationTree.GetRootMotionPosition()) * multiplier, (float)GetProcessDeltaTime()) ;
        player.MoveAndSlide();
    }

	private void HandleRotation()
	{
        targetRotationDirection = Vector3.Zero;
        targetRotationDirection = -PlayerCamera.instance.cameraObject.GlobalTransform.Basis.Z * verticalMovement;
        targetRotationDirection -= PlayerCamera.instance.cameraObject.GlobalTransform.Basis.X * horizontalMovement;
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

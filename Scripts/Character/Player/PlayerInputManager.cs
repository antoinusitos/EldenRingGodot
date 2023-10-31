using Godot;
using System;

public partial class PlayerInputManager : Node3D
{
	public static PlayerInputManager instance = null;
	public PlayerManager player = null;

	private Vector2 movementInput = Vector2.Zero;
    public float verticalInput = 0;
	public float horizontalInput = 0;
	public float moveAmount = 0;

    private Vector2 cameraInput = Vector2.Zero;
    public float cameraVerticalInput = 0;
    public float cameraHorizontalInput = 0;

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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        movementInput = Input.GetVector("MovementLeft", "MovementRight", "MovementUp", "MovementDown");
        cameraInput = Input.GetVector("CameraLeft", "CameraRight", "CameraUp", "CameraDown");

        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandlePlayerMovementInput()
	{
		verticalInput = movementInput.Y;
		horizontalInput = movementInput.X;

		moveAmount = Mathf.Clamp(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput), 0f, 1f);

		if(moveAmount <= 0.5f && moveAmount > 0)
		{
			moveAmount = 0.5f;
		}
		else if(moveAmount >= 0.5f && moveAmount <= 1)
		{
			moveAmount = 1.0f;
		}

		if(player != null)
		{
			player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
		}
	}

	private void HandleCameraMovementInput()
	{
        cameraVerticalInput = cameraInput.Y;
        cameraHorizontalInput = cameraInput.X;
    }
}

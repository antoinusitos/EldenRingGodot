using Godot;
using System;

public partial class PlayerCamera : Node3D
{
	public static PlayerCamera instance = null;

	[Export]
    public Camera3D cameraObject = null;
	public PlayerManager player = null;
	[Export]
	private Node3D cameraPivot = null;

    //Camera Settings
    private float cameraSmoothSpeed = 1.0f;
    [Export]
    private float leftAndRightRotationSpeed = 220.0f;
    [Export]
    private float upAndDownRotationSpeed = 220.0f;
	[Export]
    private float minimumPivot = -60;
    [Export]
    private float maximumPivot = 30;
	[Export]
	private float cameraCollisionRadius = 0.2f;
	[Export]
	private int collideWithLayers = 0;

    //Camera Values
    private Vector3 cameraVelocity = Vector3.Zero;
	private Vector3 cameraObjectPosition = Vector3.Zero;
	[Export]
	private float leftAndRightLookAngle = 0.0f;
    private float upAndDownLookAngle = 0.0f;
	private float CameraZPosition = 0.0f;
    private float targetCameraZPosition = 0.0f;

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
		CameraZPosition = cameraObject.Position.Z;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void HandleAllCameraActions()
	{
		if(player != null)
		{
            HandleFollowTarget();
			HandleRotations();
			HandleCollisions();
        }
	}

	private void HandleFollowTarget()
	{
		Vector3 targetCameraPosition = Utility.SmoothDamp(
			Position,
			player.Position,
			ref cameraVelocity,
			cameraSmoothSpeed * (float)GetProcessDeltaTime(), 
			Mathf.Inf, 
			(float)GetProcessDeltaTime());

		Position = targetCameraPosition;
	}

	private void HandleRotations()
	{
		leftAndRightLookAngle -= (PlayerInputManager.instance.cameraHorizontalInput * leftAndRightRotationSpeed) * (float)GetProcessDeltaTime();
		upAndDownLookAngle -= (PlayerInputManager.instance.cameraVerticalInput * upAndDownRotationSpeed) * (float)GetProcessDeltaTime();
		upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

		Vector3 cameraRotation = Vector3.Zero;
		Quaternion targetRotation = Quaternion.Identity;

        cameraRotation.Y = Mathf.DegToRad(leftAndRightLookAngle);
		targetRotation = Quaternion.FromEuler(cameraRotation);
		Rotation = targetRotation.GetEuler();

        cameraRotation = Vector3.Zero;
		cameraRotation.X = Mathf.DegToRad(upAndDownLookAngle);
		targetRotation = Quaternion.FromEuler(cameraRotation);
		cameraPivot.Rotation = targetRotation.GetEuler();
    }

	private void HandleCollisions()
	{
        targetCameraZPosition = CameraZPosition;

        Vector3 direction = cameraObject.GlobalPosition - cameraPivot.GlobalPosition;
        direction = direction.Normalized();

        var spaceState = GetWorld3D().DirectSpaceState;
        PhysicsRayQueryParameters3D physicsRayQueryParameters3D = new PhysicsRayQueryParameters3D()
        {
            From = cameraPivot.GlobalPosition,
            To = cameraPivot.GlobalPosition + direction * Mathf.Abs(targetCameraZPosition),
            Exclude = new Godot.Collections.Array<Rid> { player.GetRid() }
        };
        var result2 = spaceState.IntersectRay(physicsRayQueryParameters3D);

        if (result2.Count > 0)
        {
            Vector3 point = (Vector3)result2["position"];

            float distanceFromHitObject = cameraPivot.GlobalPosition.DistanceTo(point);
            targetCameraZPosition = (distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = cameraCollisionRadius;
        }

        cameraObjectPosition.Z = Mathf.Lerp(cameraObject.Position.Z, targetCameraZPosition, 0.2f);
        cameraObject.Position = cameraObjectPosition;
    }
}

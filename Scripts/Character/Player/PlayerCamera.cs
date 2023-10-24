using Godot;
using System;

public partial class PlayerCamera : Node3D
{
	public static PlayerCamera instance = null;

	[Export]
	public Camera3D cameraObject = null;
	public PlayerManager player = null;

	private Vector3 cameraVelocity = Vector3.Zero;
	private float cameraSmoothSpeed = 1.0f;

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
	}

	public void HandleAllCameraActions()
	{
		if(player != null)
		{
			FollowTarget();
        }
		else
		{
			GD.Print("nope");
		}
	}

	private void FollowTarget()
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
}

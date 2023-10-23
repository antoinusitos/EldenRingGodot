using Godot;
using System;

public partial class CharacterManager : CharacterBody3D
{
	private CharacterNetworkManager characterNetworkManager = null;

	public override void _EnterTree()
	{
		//MasterLevel.instance.AddNodeToDontDestroyLevel(this);
		SetMultiplayerAuthority(int.Parse(Name));

		characterNetworkManager = GetNode<CharacterNetworkManager>("CharacterNetworkManager");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		if(IsMultiplayerAuthority())
		{
			characterNetworkManager.UpdateNetworkPosition(Position);
        }
		else
		{
			Position = Utility.SmoothDamp(
				Position,
				characterNetworkManager.networkPosition,
				ref characterNetworkManager.networkPositionVelocity,
				characterNetworkManager.networkPositionSmoothTime,
				Mathf.Inf, 
				(float)delta);
        }
    }

    public override void _PhysicsProcess(double delta)
	{
		/*if(IsMultiplayerAuthority())
		{
			Vector2 vec = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down") * 100;
			Velocity = new Vector3(vec.X, vec.Y, 0);
		}
        MoveAndSlide();*/
	}
}

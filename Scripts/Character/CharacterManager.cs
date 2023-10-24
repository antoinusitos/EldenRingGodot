using Godot;
using System;

public partial class CharacterManager : CharacterBody3D
{
	private CharacterNetworkManager characterNetworkManager = null;

	public override void _EnterTree()
	{
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
		if (IsMultiplayerAuthority())
		{
			characterNetworkManager.UpdateNetworkPosition(Position);
			characterNetworkManager.UpdateNetworkRotation(Transform.Basis.GetRotationQuaternion());
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

			Rotation = Transform.Basis.GetRotationQuaternion().Slerp(characterNetworkManager.networkRotation, characterNetworkManager.networkRotationSmoothTime).GetEuler();
		}

		CallDeferred("LateUpdate");
	}

	public virtual void OnNetworkSpawned()
	{
	}

	public virtual void LateUpdate()
	{

	}
}

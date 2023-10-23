using Godot;
using System;

public partial class CharacterNetworkManager : Node3D
{
	public MultiplayerSynchronizer multiplayerSynchronizer = null;

    [Export]
    public Vector3 networkPosition = Vector3.Zero;
    public Vector3 networkPositionVelocity = Vector3.Zero;
    public float networkPositionSmoothTime = 0.1f;

    [Export]
    public Quaternion networkRotation = Quaternion.Identity;
    public float networkRotationSmoothTime = 0.1f;

    public override void _EnterTree()
    {
        base._EnterTree();

        //multiplayerSynchronizer.ReplicationConfig.AddProperty("..:position");
    }

    public override void _Ready()
    {
        base._Ready();

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void UpdateNetworkPosition(Vector3 value)
    {
        networkPosition = value;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void UpdateNetworkRotation(Quaternion value)
    {
        networkRotation = value;
    }
}

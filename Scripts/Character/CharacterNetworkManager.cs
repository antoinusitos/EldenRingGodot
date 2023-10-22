using Godot;
using System;

public partial class CharacterNetworkManager : Node3D
{
	public MultiplayerSynchronizer multiplayerSynchronizer = null;

    public override void _EnterTree()
    {
        multiplayerSynchronizer.ReplicationConfig.AddProperty("..:position");
    }
}

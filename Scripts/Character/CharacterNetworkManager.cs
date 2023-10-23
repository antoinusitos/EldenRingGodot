using Godot;
using System;

public partial class CharacterNetworkManager : Node3D
{
	public MultiplayerSynchronizer multiplayerSynchronizer = null;

    private int _testVariable = 0;

    [Export]
    public int testVariable { get { return _testVariable; } set { _testVariable = value; GD.Print("_testVariable = " + value); GetNode<SoftBody3D>("../TestBox").Visible = (value == 0 ? true : false); } }

    private bool canDo = true;
    private float lol = 0;

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

        if (!Multiplayer.IsServer() && IsMultiplayerAuthority())
        {
            if (!canDo)
            {
                return;
            }
            lol += (float)delta;

            if (lol > 3)
            {
                canDo = false;
                TestRPC(12);
            }
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void TestRPC(int aValue)
    {
        testVariable = aValue;
        if (Multiplayer.IsServer())
        {
            GD.Print("is server");
        }
        else
        {
            GD.Print("is not server");
        }
    }
}

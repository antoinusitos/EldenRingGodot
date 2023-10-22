using Godot;
using System;

public partial class MultiplayerManager : Node3D
{
    [Export]
    private int port = 135;

    [Export]
    private string address = "127.0.0.1";

    public static MultiplayerManager instance = null;

    private ENetMultiplayerPeer peer = new();

    [Export]
    private PackedScene playerPrefab = null;

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
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

    public void StartNetworkAsHost()
    {
        Error error = peer.CreateServer(port);
        if (error != Error.Ok)
        {
            GD.Print("Error Cannot Create Server");
            return;
        }

        //peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

        Multiplayer.MultiplayerPeer = peer;
        Multiplayer.PeerConnected += PeerConnected;
        PeerConnected();
        GD.Print("Server Created");
    }

    public void StartNetworkAsClient()
    {
        peer.CreateClient("localhost", port);

        //peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Joining Game");
    }

    private void PeerConnected(long id = 1)
    {
        GD.Print("Player Connected ! " + id.ToString());
        CharacterBody3D spawnedPlayer2 = (CharacterBody3D)playerPrefab.Instantiate();
        spawnedPlayer2.Position = new Vector3(GD.Randf(), 0, 0);
        spawnedPlayer2.Name = id.ToString();
        GD.Print("spawnedPlayer " + spawnedPlayer2.Position);
        CallDeferred("add_child", spawnedPlayer2);
    }

    private void PeerDisconnected(long id)
    {
        GD.Print("Player Disconnected ! " + id.ToString());
    }

    private void ConnectedToServer()
    {
        GD.Print("Connected to Server !");
    }

    private void ConnectionFailed()
    {
        GD.Print("Connection to Server Failed!");
    }
}

using Godot;
using System;

public interface INetworkInterface
{
	public void OnNetworkSpawned()
	{
		GD.Print("INetworkInterface::OnNetworkSpawn");
	}
}

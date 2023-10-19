using Godot;
using System;

public partial class MasterLevel : Node3D
{
	public static MasterLevel instance = null;

	private Node3D currentLevel = null;

    [Export]
	private Node3D startingLevel = null;

    public override void _EnterTree()
	{
		instance = this;

        if (startingLevel != null)
		{
            currentLevel = startingLevel;
		}
	}

	public void LoadLevel(string levelToLoad)
	{
        // This function will usually be called from a signal callback,
        // or some other function from the current scene.
        // Deleting the current scene at this point is
        // a bad idea, because it may still be executing code.
        // This will result in a crash or unexpected behavior.

        // The solution is to defer the load to a later time, when
        // we can be sure that no code from the current scene is running:
        CallDeferred(nameof(DeferredLoadLevel), levelToLoad);
    }

    private void DeferredLoadLevel(string levelToLoad)
    {
        currentLevel.Free();

        // Load a new scene.
        var nextScene = (PackedScene)GD.Load(levelToLoad);

        // Instance the new scene.
        currentLevel = (Node3D)nextScene.Instantiate();

        // Add it to the active scene, as child of root.
        GetTree().Root.AddChild(currentLevel);
    }

    public void AddNodeToDontDestroyLevel(Node3D childToAdd)
    {
        CallDeferred(nameof(DeferredAddNodeToDontDestroyLevel), childToAdd);
    }
    public void DeferredAddNodeToDontDestroyLevel(Node3D childToAdd)
    {
        childToAdd.Reparent(this);
    }
}

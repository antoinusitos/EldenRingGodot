using Godot;
using System;

public partial class CharacterAnimationManager : Node
{
	private CharacterManager character = null;

    private float vertical = 0.0f;
    private float horizontal = 0.0f;

    public override void _EnterTree()
    {
        base._EnterTree();

        character = GetNode<CharacterManager>("..");
    }

    public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
	{
        Vector2 animationTransition = (Vector2)character.animator.Get("parameters/BlendSpaceMovement/blend_position");
        float v = 0;
        float xValue = Utility.SmoothDamp(animationTransition.X, horizontalValue, ref v, 0.1f, Mathf.Inf, (float)GetProcessDeltaTime());
        float yValue = Utility.SmoothDamp(animationTransition.Y, verticalValue, ref v, 0.1f, Mathf.Inf, (float)GetProcessDeltaTime());

        character.animator.Set("parameters/BlendSpaceMovement/blend_position", new Vector2(xValue, yValue));
    }
}

using Godot;
using System;

public static class Utility
{
    public static float SmoothDamp(float current, float target, ref float currentVelocitY, float smoothTime, float maXSpeed, float deltaTime)
    {
        smoothTime = Mathf.Max(0.0001f, smoothTime);
        float num = 2f / smoothTime;
        float num2 = num * deltaTime;
        float num3 = 1f / (1f + num2 + 0.48f * num2 * num2 + 0.235f * num2 * num2 * num2);
        float num4 = current - target;
        float num5 = target;
        float num6 = maXSpeed * smoothTime;
        num4 = Mathf.Clamp(num4, -num6, num6);
        target = current - num4;
        float num7 = (currentVelocitY + num * num4) * deltaTime;
        currentVelocitY = (currentVelocitY - num * num7) * num3;
        float num8 = target + (num4 + num7) * num3;
        if (num5 - current > 0f == num8 > num5)
        {
            num8 = num5;
            currentVelocitY = (num8 - num5) / deltaTime;
        }
        return num8;
    }

    public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref Vector3 currentVelocitY, float smoothTime, float maXSpeed, float deltaTime)
    {
        float output_X = 0f;
        float output_Y = 0f;
        float output_Z = 0f;

        // Based on Game Programming Gems 4 Chapter 1.10
        smoothTime = Mathf.Max(0.0001F, smoothTime);
        float omega = 2F / smoothTime;

        float X = omega * deltaTime;
        float eXp = 1F / (1F + X + 0.48F * X * X + 0.235F * X * X * X);

        float change_X = current.X - target.X;
        float change_Y = current.Y - target.Y;
        float change_Z = current.Z - target.Z;
        Vector3 originalTo = target;

        // Clamp maXimum speed
        float maXChange = maXSpeed * smoothTime;

        float maXChangeSq = maXChange * maXChange;
        float sqrmag = change_X * change_X + change_Y * change_Y + change_Z * change_Z;
        if (sqrmag > maXChangeSq)
        {
            var mag = (float)Math.Sqrt(sqrmag);
            change_X = change_X / mag * maXChange;
            change_Y = change_Y / mag * maXChange;
            change_Z = change_Z / mag * maXChange;
        }

        target.X = current.X - change_X;
        target.Y = current.Y - change_Y;
        target.Z = current.Z - change_Z;

        float temp_X = (currentVelocitY.X + omega * change_X) * deltaTime;
        float temp_Y = (currentVelocitY.Y + omega * change_Y) * deltaTime;
        float temp_Z = (currentVelocitY.Z + omega * change_Z) * deltaTime;

        currentVelocitY.X = (currentVelocitY.X - omega * temp_X) * eXp;
        currentVelocitY.Y = (currentVelocitY.Y - omega * temp_Y) * eXp;
        currentVelocitY.Z = (currentVelocitY.Z - omega * temp_Z) * eXp;

        output_X = target.X + (change_X + temp_X) * eXp;
        output_Y = target.Y + (change_Y + temp_Y) * eXp;
        output_Z = target.Z + (change_Z + temp_Z) * eXp;

        // Prevent overshooting
        float origMinusCurrent_X = originalTo.X - current.X;
        float origMinusCurrent_Y = originalTo.Y - current.Y;
        float origMinusCurrent_Z = originalTo.Z - current.Z;
        float outMinusOrig_X = output_X - originalTo.X;
        float outMinusOrig_Y = output_Y - originalTo.Y;
        float outMinusOrig_Z = output_Z - originalTo.Z;

        if (origMinusCurrent_X * outMinusOrig_X + origMinusCurrent_Y * outMinusOrig_Y + origMinusCurrent_Z * outMinusOrig_Z > 0)
        {
            output_X = originalTo.X;
            output_Y = originalTo.Y;
            output_Z = originalTo.Z;

            currentVelocitY.X = (output_X - originalTo.X) / deltaTime;
            currentVelocitY.Y = (output_Y - originalTo.Y) / deltaTime;
            currentVelocitY.Z = (output_Z - originalTo.Z) / deltaTime;
        }

        return new Vector3(output_X, output_Y, output_Z);
    }

    public static Vector3 DivideVector3ByFloat(Vector3 vector, float deltaTime) { return new Vector3(vector.X / deltaTime, vector.Y / deltaTime, vector.Z / deltaTime); }
}

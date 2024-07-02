using System.Numerics;

namespace Project.Shared
{
    // Base on the freeroam-extended helper.ts DirectionVector class
    public class DirectionVector
    {
        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public DirectionVector(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Quaternion EulerToQuaternion(Vector3 rotation)
        {
            float yaw = rotation.Z * (MathF.PI / 180.0f);
            float pitch = rotation.Y * (MathF.PI / 180.0f);
            float roll = rotation.X * (MathF.PI / 180.0f);

            float x = MathF.Sin(roll / 2) * MathF.Cos(pitch / 2) * MathF.Cos(yaw / 2) - MathF.Cos(roll / 2) * MathF.Sin(pitch / 2) * MathF.Sin(yaw / 2);
            float y = MathF.Cos(roll / 2) * MathF.Sin(pitch / 2) * MathF.Cos(yaw / 2) + MathF.Sin(roll / 2) * MathF.Cos(pitch / 2) * MathF.Sin(yaw / 2);
            float z = MathF.Cos(roll / 2) * MathF.Cos(pitch / 2) * MathF.Sin(yaw / 2) - MathF.Sin(roll / 2) * MathF.Sin(pitch / 2) * MathF.Cos(yaw / 2);
            float w = MathF.Cos(roll / 2) * MathF.Cos(pitch / 2) * MathF.Cos(yaw / 2) + MathF.Sin(roll / 2) * MathF.Sin(pitch / 2) * MathF.Sin(yaw / 2);

            return new Quaternion(x, y, z, w);
        }

        public Vector3 ForwardVector()
        {
            Quaternion quaternion = EulerToQuaternion(Rotation);
            float forwardVectorX = 2 * (quaternion.X * quaternion.Y - quaternion.W * quaternion.Z);
            float forwardVectorY = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Z * quaternion.Z);
            float forwardVectorZ = 2 * (quaternion.Y * quaternion.Z + quaternion.W * quaternion.X);

            return new Vector3(forwardVectorX, forwardVectorY, forwardVectorZ);
        }

        public Vector3 RightVector()
        {
            Quaternion quaternion = EulerToQuaternion(Rotation);
            float rightVectorX = 1 - 2 * (quaternion.Y * quaternion.Y + quaternion.Z * quaternion.Z);
            float rightVectorY = 2 * (quaternion.X * quaternion.Y + quaternion.W * quaternion.Z);
            float rightVectorZ = 2 * (quaternion.X * quaternion.Z - quaternion.W * quaternion.Y);

            return new Vector3(rightVectorX, rightVectorY, rightVectorZ);
        }

        public Vector3 UpVector()
        {
            Quaternion quaternion = EulerToQuaternion(Rotation);
            float upVectorX = 2 * (quaternion.X * quaternion.Z + quaternion.W * quaternion.Y);
            float upVectorY = 2 * (quaternion.Y * quaternion.Z - quaternion.W * quaternion.X);
            float upVectorZ = 1 - 2 * (quaternion.X * quaternion.X + quaternion.Y * quaternion.Y);

            return new Vector3(upVectorX, upVectorY, upVectorZ);
        }

        public Vector3 Forward(float distance)
        {
            Vector3 forwardVector = ForwardVector();
            return new Vector3(
                Position.X + forwardVector.X * distance,
                Position.Y + forwardVector.Y * distance,
                Position.Z + forwardVector.Z * distance);
        }

        public Vector3 Right(float distance)
        {
            Vector3 rightVector = RightVector();
            return new Vector3(
                Position.X + rightVector.X * distance,
                Position.Y + rightVector.Y * distance,
                Position.Z + rightVector.Z * distance);
        }

        public Vector3 Up(float distance)
        {
            Vector3 upVector = UpVector();
            return new Vector3(
                Position.X + upVector.X * distance,
                Position.Y + upVector.Y * distance,
                Position.Z + upVector.Z * distance);
        }
    }
}

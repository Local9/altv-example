using System.Numerics;

namespace Project.Client.Core
{
    internal class Camera
    {
        private DirectionVector _directionVector;
        private Vector3 _rotation;

        public int Handle { get; private set; }
        public Vector3 UpVector => _directionVector.UpVector();
        public Vector3 RightVector => _directionVector.RightVector();
        public Vector3 ForwardVector => _directionVector.ForwardVector();

        public Vector3 Forward(float distance) => _directionVector.Forward(distance);
        public Vector3 Right(float distance) => _directionVector.Right(distance);
        public Vector3 Up(float distance) => _directionVector.Up(distance);

        public Vector3 Rotation
        {
            get => Alt.Natives.GetCamRot(Handle, 2);
            set
            {
                _rotation = value;
                _directionVector = new DirectionVector(Position, value);
                Alt.Natives.SetCamRot(Handle, value.X, value.Y, value.Z, 2);
            }
        }

        public Vector3 Position
        {
            get => Alt.Natives.GetCamCoord(Handle);
            set => Alt.Natives.SetCamCoord(Handle, value.X, value.Y, value.Z);
        }

        public float FieldOfView
        {
            get => Alt.Natives.GetCamFov(Handle);
            set => Alt.Natives.SetCamFov(Handle, value);
        }

        private Camera(int handle, Vector3 position, Vector3 rotation)
        {
            Handle = handle;
            Position = position;
            Rotation = rotation;
            _directionVector = new DirectionVector(position, rotation);
        }

        public static Camera Create(string name, Vector3 position, Vector3 rotation, float fov = 75)
        {
            int handle = Alt.Natives.CreateCameraWithParams(Alt.Hash(name), position.X, position.Y, position.Z, 0.0f, 0.0f, rotation.Z, fov, false, 2);
            return new Camera(handle, position, rotation);
        }

        public void AttachToEntity(uint handle, Vector3 offset)
        {
            Alt.Natives.AttachCamToEntity(Handle, handle, offset.X, offset.Y, offset.Z, true);
        }

        public void AttachToEntity(Entity entity, Vector3 offset)
        {
            Alt.Natives.AttachCamToEntity(Handle, entity.Id, offset.X, offset.Y, offset.Z, true);
        }

        public void Detach()
        {
            Alt.Natives.DetachCam(Handle);
        }

        public void Dispose()
        {
            Detach();
            Alt.Natives.DestroyCam(Handle, true);
        }
    }
}

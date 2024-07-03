using System.Numerics;

namespace Project.Client.Controllers.Admin
{
    internal class NoClipController : IController
    {
        private const float MIN_ROTATION_Y = -89f, MAX_ROTATION_Y = 89f, MAX_SPEED = 32f;

        private Camera? _noclipCamera;
        private uint _noclipTick;
        public float Speed { get; set; } = 1f;
        public float Fov { get; set; } = 75f;
        public float MaxFov { get; set; } = 130f;

        private readonly List<Control> _disabledControls = new List<Control>
        {
            Control.WeaponWheelUpDown,
            Control.WeaponWheelLeftRight,
            Control.WeaponWheelNext,
            Control.WeaponWheelPrev,
            Control.SelectNextWeapon,
            Control.SelectPrevWeapon,
            Control.MoveLeftRight,
            Control.MoveUpDown,
            Control.MoveUpOnly,
            Control.MoveLeftOnly,
            Control.Duck
        };

        public void OnStart()
        {
            Console.WriteLine("Started No Clip Handler");
            Alt.OnServer<bool>(AdminEvents.NOCLIP, ToggleNoclip);
        }

        public void ToggleNoclip(bool state)
        {
            if (!state)
            {
                CleanUp();

                return;
            }

            Vector3 gameplayCameraPosition = Alt.Natives.GetGameplayCamCoord();
            Vector3 gameplayCameraRotation = Alt.Natives.GetGameplayCamRot(2);

            _noclipCamera = Camera.Create("DEFAULT_SCRIPTED_CAMERA", gameplayCameraPosition, gameplayCameraRotation);
            _noclipTick = Alt.EveryTick(UpdateNoClipCamera);

            Alt.Natives.SetCamActiveWithInterp(_noclipCamera.Handle, Alt.Natives.GetRenderingCam(), 500, 0, 0);
            Alt.Natives.RenderScriptCams(true, true, 500, true, false, 0);
        }

        private void CleanUp()
        {
            _noclipCamera?.Dispose();
            _noclipCamera = null;

            Alt.ClearEveryTick(_noclipTick);
            Alt.Natives.RenderScriptCams(false, true, 500, true, false, 0);

            Position pos = Alt.FocusData.FocusOverridePosition;
            Alt.FocusData.ClearFocusOverride();

            bool groundZ = Alt.Natives.GetGroundZFor3dCoord(pos.X, pos.Y, pos.Z, ref pos.Z, false, false);
            Alt.EmitServer(AdminEvents.TELEPORT_TO_COORDS, pos.X, pos.Y, pos.Z + 1.0);
        }

        private void UpdateNoClipCamera()
        {
            try
            {
                if (_noclipCamera == null) return;

                // TODO : FOV Controls with Modifiers

                Alt.FocusData.OverrideFocusPosition(_noclipCamera.Forward(3.5f), Vector3.Zero);

                // Speed Controls
                if (Alt.Natives.IsDisabledControlJustPressed(0, (int)Control.SelectPrevWeapon))
                    Speed = Math.Min(Speed + 0.1f, MAX_SPEED);
                else if (Alt.Natives.IsDisabledControlJustPressed(0, (int)Control.SelectNextWeapon))
                    Speed = Math.Max(0.1f, Speed - 0.1f);

                float multiplier = 1f;

                if (IsDisabledControlPressed(2, Control.FrontendLs))
                {
                    multiplier = 2f;
                }
                else if (IsDisabledControlPressed(2, Control.CharacterWheel))
                {
                    multiplier = 4f;
                }
                else if (IsDisabledControlPressed(2, Control.Duck))
                {
                    multiplier = 0.25f;
                }

                Vector3 currentCameraPosition = _noclipCamera.Position;

                // todo: improve camera controls with better movement

                if (IsDisabledControlPressed(2, Control.MoveUpOnly))
                    _noclipCamera.Position = currentCameraPosition + _noclipCamera.ForwardVector * (Speed * multiplier);
                else if (IsDisabledControlPressed(2, Control.MoveUpDown))
                    _noclipCamera.Position = currentCameraPosition - _noclipCamera.ForwardVector * (Speed * multiplier);

                if (IsDisabledControlPressed(2, Control.MoveLeftOnly))
                    _noclipCamera.Position = currentCameraPosition - _noclipCamera.RightVector * (Speed * multiplier);
                else if (IsDisabledControlPressed(2, Control.MoveLeftRight))
                    _noclipCamera.Position = currentCameraPosition + _noclipCamera.RightVector * (Speed * multiplier);

                // E and Q
                if (IsDisabledControlPressed(2, Control.Context))
                    _noclipCamera.Position = currentCameraPosition + _noclipCamera.UpVector * (Speed * multiplier);
                else if (IsDisabledControlPressed(2, Control.ContextSecondary))
                    _noclipCamera.Position = currentCameraPosition - _noclipCamera.UpVector * (Speed * multiplier);

                _noclipCamera.FieldOfView = Fov;

                foreach (Control control in _disabledControls)
                    Alt.Natives.DisableControlAction(0, (int)control, true);

                ProcessCameraRotation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool IsDisabledControlPressed(int group, Control control) => Alt.Natives.IsDisabledControlPressed(group, (int)control);

        private void ProcessCameraRotation()
        {
            if (_noclipCamera == null) return;

            float rightAxisX = Alt.Natives.GetDisabledControlNormal(0, 220);
            float rightAxisY = Alt.Natives.GetDisabledControlNormal(0, 221);

            if (!(Math.Abs(rightAxisX) > 0) && !(Math.Abs(rightAxisY) > 0)) return;
            Vector3 rotation = _noclipCamera.Rotation;
            rotation.Z += rightAxisX * -10f;

            float yValue = rightAxisY * -5f;
            if (rotation.X + yValue > MIN_ROTATION_Y && rotation.X + yValue < MAX_ROTATION_Y)
                rotation.X += yValue;
            _noclipCamera.Rotation = rotation;
        }

        public void Dispose()
        {
            Console.WriteLine($"Disposing No Clip Handler");
            Alt.OffServer(AdminEvents.NOCLIP, null);

            CleanUp();
        }

        public void OnStop()
        {
            Dispose();
        }
    }
}

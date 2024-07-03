using AltV.Net.Client.Elements.Interfaces;
using System.Numerics;

namespace Project.Client.Core
{
    internal class Raycast
    {
        public static (bool, Vector3?, uint?) Detect(Position start, Position end, float radius, IEntity[] ignoredEntities)
        {
            int raycast = Alt.Natives.StartExpensiveSynchronousShapeTestLosProbe(start.X, start.Y, start.Z, end.X, end.Y, end.Z, (int)ShapeTestFlags.Everything, Alt.LocalPlayer, 0);

            bool hit = false;
            Vector3 hitPosition = new Vector3();
            Vector3 surfaceNormal = new Vector3();
            uint entity = 0;

            Alt.Natives.GetShapeTestResult(raycast, ref hit, ref hitPosition, ref surfaceNormal, ref entity);

            if (!hit) return (hit, null, null);

            return (hit, hitPosition, entity);
        }
    }
}

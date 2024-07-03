using AltV.Net;

namespace Project.Shared.Interfaces
{
    internal interface IController : IScript
    {
        void OnStart();

        void OnStop();
    }
}

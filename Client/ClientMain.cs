using System;
using System.Threading;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MyResourceNameClient;
using static CitizenFX.Core.Native.API;

namespace Pluginss.Client
{
    public class ClientMain : BaseScript
    {
        public ClientMain()
        {
           
        }

        [Tick]
        public Task OnTick()
        {
            if (jogador.inv == true)
            {
                Game.PlayerPed.Position = jogador.posjogador;
                Debug.WriteLine ("Cam: " + GetGameplayCamCoord());
                Debug.WriteLine("Per: " + Game.PlayerPed.Rotation);
                if (IsControlPressed(1, 32))
                {
                    Debug.WriteLine("Andar: " + Game.PlayerPed.ForwardVector);
                    Vector3 setar = new Vector3((jogador.posjogador.X + Game.PlayerPed.ForwardVector.X), jogador.posjogador.Y, jogador.posjogador.Z);
                    jogador.posjogador = setar;
                }

                if (IsControlPressed(1, 34))
                {
                    Game.PlayerPed.Rotation = (Game.PlayerPed.Rotation + 5);
                }
                if (IsControlPressed(1, 35))
                {
                    Game.PlayerPed.Rotation = (Game.PlayerPed.Rotation - 5);
                }
                if (IsControlPressed(1, 33))
                {
                    //Game.PlayerPed.Rotation = (Game.PlayerPed.Rotation - 5);
                }

                if (IsControlPressed(1, 44))
                {
                    Debug.WriteLine("Subir: " + Game.PlayerPed.UpVector);
                    Game.PlayerPed.CancelRagdoll();

                    Vector3 setar = new Vector3(Game.PlayerPed.Position.X, Game.PlayerPed.Position.X, (Game.PlayerPed.Position.Z + Game.PlayerPed.UpVector.Z));
                }

            }
            return Task.FromResult(0);
        }
    }
}
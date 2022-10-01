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

            if (jogador.config_inicial != true)
            {
                Debug.WriteLine("N3rdy_DEV | Iniciando Configurações...");
                
                jogador.config_inicial = true;
                Debug.WriteLine("N3rdy_DEV | Carregado!");
            }


            if (Game.PlayerPed.Health <= 0)
            {
                
            }


            if (jogador.dev == true)
            {
                Debug.WriteLine("Vida: " + Game.PlayerPed.Health + " | Colete: " + Game.PlayerPed.Armor);
            }
            


            if (jogador.inv == true)
            {

                Game.PlayerPed.Health = 100;


                Game.PlayerPed.Position = jogador.posjogador;

                Debug.WriteLine ("Cam: " + GetGameplayCamCoord());
                Debug.WriteLine("Per: " + Game.PlayerPed.Rotation);


                Vector3 campo = new Vector3(GetGameplayCamRot(Game.PlayerPed.NetworkId).X, GetGameplayCamRot(Game.PlayerPed.NetworkId).Y, GetGameplayCamRot(Game.PlayerPed.NetworkId).Z);

                Game.PlayerPed.Rotation = campo;




                if (IsControlPressed(1, 32))
                {
                    Vector3 setar = new Vector3((jogador.posjogador.X + Game.PlayerPed.ForwardVector.X), (jogador.posjogador.Y + Game.PlayerPed.ForwardVector.Y), jogador.posjogador.Z); ;
                    jogador.posjogador = setar;
                }
                if (IsControlPressed(1, 33))
                {
                    Vector3 setar = new Vector3((jogador.posjogador.X - Game.PlayerPed.ForwardVector.X), (jogador.posjogador.Y - Game.PlayerPed.ForwardVector.Y), jogador.posjogador.Z); ;
                    jogador.posjogador = setar;
                }

                if (IsControlPressed(1, 44))
                {
                    Debug.WriteLine("Subir: " + Game.PlayerPed.UpVector);
                    Vector3 setar = new Vector3(jogador.posjogador.X, jogador.posjogador.Y, (jogador.posjogador.Z + Game.PlayerPed.UpVector.Z));
                    jogador.posjogador = setar;
                }

                if (IsControlPressed(1, 20))
                {
                    Debug.WriteLine("Subir: " + Game.PlayerPed.UpVector);
                    Vector3 setar = new Vector3(jogador.posjogador.X, jogador.posjogador.Y, (jogador.posjogador.Z - Game.PlayerPed.UpVector.Z));
                    jogador.posjogador = setar;
                }


            }
            return Task.FromResult(0);
        }
    }
}
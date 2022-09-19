using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;

namespace MyResourceNameClient
{
    public class jogador : BaseScript
    {
        public static bool inv = false;
        public static bool buzina = false;
        public static bool blips = false;

        public static Vector3 posjogador = new Vector3(0,0,0);


        public jogador()
        {

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        private void OnClientResourceStart(string resourceName)
        {


            RegisterCommand("v", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Debug.WriteLine("Ok");
                var model = "";
                if (args.Count > 0)
                {
                    model = args[0].ToString();
                    Debug.WriteLine("Tem Nome");
                    var frente = Game.PlayerPed.Position.Y + 15;
                    var teste = Game.PlayerPed.Position;
                    var teste2 = Game.PlayerPed.Heading;
                    //Vector3 posicao = new Vector3(Game.PlayerPed.Position.X, frente, Game.PlayerPed.Position.Z);
                    Debug.WriteLine("" + teste);
                    Debug.WriteLine("" + teste2);
                    var hash = (uint)GetHashKey(model);
                    var carrito = await World.CreateVehicle(model, Game.PlayerPed.Position, Game.PlayerPed.Heading);
                    Game.PlayerPed.SetIntoVehicle(carrito, VehicleSeat.Driver);
                    Debug.WriteLine("Spawnado!");
                }
                else
                {
                    Debug.WriteLine("Especifique o nome do carro...");
                }
            }), false);

            RegisterCommand("paint", new Action<int, List<object>, string>(async (source, args, raw) =>
            {

                    int cor1 = Convert.ToInt32(args[0]);
                    int cor2 = Convert.ToInt32(args[1]);
                    SetVehicleColours(Game.PlayerPed.CurrentVehicle.NetworkId, cor1, cor2);

            }), false);

            RegisterCommand("debug", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Debug.WriteLine("MySql | Abrindo conexão");
                
                Debug.WriteLine("MySql | Concluido!");

            }), false);


            RegisterCommand("flash", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                
                SetRunSprintMultiplierForPlayer(Game.PlayerPed.NetworkId, Convert.ToInt32(args[0]));
            }), false);


            RegisterCommand("skin", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                uint shash = Convert.ToUInt32(args[0]);
                SetPlayerModel(Game.PlayerPed.NetworkId, shash);


            }), false);

            RegisterCommand("god", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Game.PlayerPed.Health = 100;
                Game.PlayerPed.Armor = 100;
            }), false);


            //teleport para marcação

            RegisterCommand("tpway", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
              
            }), false);


            
            //blips
            RegisterCommand("blips", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                int ped = GetPlayerPed(PlayerId());
                int gamerTagID = CreateMpGamerTag(ped, GetPlayerName(PlayerId()), false, false, "", 0);
                if (jogador.blips != true) {
                    
                    SetMpGamerTagVisibility(gamerTagID, 0, true);
                    jogador.blips = true;
                }
                else
                {
                    SetMpGamerTagVisibility(gamerTagID, 0, false);
                    jogador.blips = false;
                }
                
            }), false);
            
            
            //ragdoll
            RegisterCommand("ragdoll", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                GivePlayerRagdollControl(Game.PlayerPed.NetworkId, true);
            }), false);

            //NoCLIP
            RegisterCommand("nc", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Debug.WriteLine("" + inv);
                PlaySoundFrontend(-1, "Checkpoint_Hit", "GTAO_FM_Events_Soundset", true);
                if (inv != true)
                {
                    posjogador = Game.PlayerPed.Position;
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, true);
                    SetPoliceIgnorePlayer(Game.PlayerPed.NetworkId, true);
                    Game.PlayerPed.Ragdoll(-1);
                    Game.PlayerPed.CanRagdoll = false;
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, true);
                    SetEntityAlpha(PlayerPedId(), 0, 0);
                    //SetRunSprintMultiplierForPlayer(Game.PlayerPed.NetworkId, 5);
                    inv = true;
                }
                else
                {
                    SetEntityAlpha(PlayerPedId(), 1000, 0);
                    inv = false;
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, jogador.inv);
                }

            }), false);

            //Excluir Veiculo | DV
            RegisterCommand("dv", new Action<int, List<object>, string>(async (source, args, raw) =>
            {

                if (Game.PlayerPed.IsInVehicle() != false)
                {
                    Game.PlayerPed.CurrentVehicle.Delete();
                }
                else
                {
                    Debug.WriteLine("Não está no veiculo");
                }

            }), false);

        }
    }
}

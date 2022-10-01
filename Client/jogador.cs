using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using Mono.CSharp;
using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;
using static CitizenFX.Core.UI.Screen;
using static CitizenFX.Core.Native.API;

namespace MyResourceNameClient
{
    public class jogador : BaseScript
    {
        public static bool inv = false;
        public static bool buzina = false;
        public static bool blips = false;
        public static bool dev = false;
        public static bool driftmode = false;

        public static bool config_inicial = false;

        public static Vector3 posjogador = new Vector3(0,0,0);


        string[] admin = new string[] {"" };


        public static string[] armas_lista = new string[] { "WEAPON_PISTOL", "WEAPON_COMBATPISTOL", "WEAPON_APPISTOL", "WEAPON_PISTOL50", "WEAPON_SNSPISTOL", "WEAPON_HEAVYPISTOL", "WEAPON_VINTAGEPISTOL", "WEAPON_MARKSMANPISTOL", "WEAPON_REVOLVER", "WEAPON_MICROSMG", "WEAPON_SMG", "WEAPON_ASSAULTSMG", "WEAPON_COMBATPDW", "WEAPON_MACHINEPISTOL", "WEAPON_MINISMG", "WEAPON_ASSAULTRIFLE", "WEAPON_CARBINERIFLE", "WEAPON_ADVANCEDRIFLE", "WEAPON_SPECIALCARBINE", "WEAPON_BULLPUPRIFLE", "WEAPON_COMPACTRIFLE", "WEAPON_MG", "WEAPON_COMBATMG", "WEAPON_GUSENBERG", "WEAPON_SNIPERRIFLE", "WEAPON_HEAVYSNIPER", "WEAPON_MARKSMANRIFLE", "WEAPON_GRENADELAUNCHER", "WEAPON_RPG", "WEAPON_STINGER", "WEAPON_MINIGUN", "WEAPON_GRENADE", "WEAPON_STICKYBOMB", "WEAPON_SMOKEGRENADE", "WEAPON_BZGAS", "WEAPON_MOLOTOV", "WEAPON_FIREEXTINGUISHER", "WEAPON_PETROLCAN", "WEAPON_DIGISCANNER", "WEAPON_BALL", "WEAPON_SNSPISTOL_MK2", "WEAPON_REVOLVER_MK2", "WEAPON_DOUBLEACTION", "WEAPON_SPECIALCARBINE_MK2", "WEAPON_BULLPUPRIFLE_MK2", "WEAPON_PUMPSHOTGUN_MK2", "WEAPON_MARKSMANRIFLE_MK2", "WEAPON_ASSAULTRIFLE_MK2", "WEAPON_CARBINERIFLE_MK2", "WEAPON_COMBATMG_MK2", "WEAPON_HEAVYSNIPER_MK2", "WEAPON_SMG_MK2", "WEAPON_COMBATPDW_MK2", "WEAPON_ASSAULTSHOTGUN", "weapon_bullpupshotgun" };




        public jogador()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
        }

        public static bool permissionverify()
        {
            
            return false;
        }

        private void OnClientResourceStart(string resourceName)
        {


            RegisterCommand("tunning", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var carro_handle = Game.PlayerPed.CurrentVehicle.Handle;
                var carro_ntid = Game.PlayerPed.CurrentVehicle.NetworkId;

                Game.PlayerPed.CurrentVehicle.Explode();
                
                SetVehicleColours(carro_handle, 0, 1);
            }), false);


            RegisterCommand("reviver", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                if (Game.PlayerPed.Health <= 0)
                {
                    Game.PlayerPed.Resurrect();

                }
                else
                {
                    Game.PlayerPed.Health = 1000;
                }
            }), false);

            RegisterCommand("v", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
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
                var carro = GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
                int cor1 = Convert.ToInt32(args[0]);
                int cor2 = Convert.ToInt32(args[1]);
                SetVehicleColours(carro, cor1, cor2);

            }), false);


            RegisterCommand("debug", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                uint model = (uint)GetHashKey("mp_m_freemode_01");
                if (!HasModelLoaded(model))
                {
                    RequestModel(model);
                    while (!HasModelLoaded(model))
                    {
                        await BaseScript.Delay(0);
                    }
                }
                SetPlayerModel(Game.Player.Handle, model);
                Game.PlayerPed.Health = 1000;
                ClearPedDecorations(Game.PlayerPed.Handle);
                ClearPedFacialDecorations(Game.PlayerPed.Handle);
                SetPedDefaultComponentVariation(Game.PlayerPed.Handle);
                SetPedHairColor(Game.PlayerPed.Handle, 0, 0);
                SetPedEyeColor(Game.PlayerPed.Handle, 0);
                ClearAllPedProps(Game.PlayerPed.Handle);
            }), false);
            
            RegisterCommand("armas", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                int i = 0;
                foreach (string arma in armas_lista)
                {
                    Debug.WriteLine("Arma: " + jogador.armas_lista[i]);
                    uint hash = (uint)GetHashKey(jogador.armas_lista[i]);
                    Debug.WriteLine("Hash: " + hash);
                    GiveWeaponToPed(Game.PlayerPed.Handle, hash, 9999, false, false);
                    i++;
                }
                Debug.WriteLine("N3rdy Dev | Comando Finalizado");
            }), false);



            RegisterCommand("staff", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var player = Game.PlayerPed.NetworkId;
                string model = "u_m_m_streetart_01";
                PedHash hash = (PedHash)GetHashKey(model);
                var characterModel = new Model(hash);
                Game.Player.ChangeModel(characterModel);
            }), false);

            RegisterCommand("kill", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                Game.PlayerPed.Armor = -100;
                Game.PlayerPed.Health = -100;
            }), false);



            RegisterCommand("god", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                
                if (Game.PlayerPed.Health <= 0)
                {
                    Game.PlayerPed.Resurrect();
                    Game.PlayerPed.Armor = 1000;
                }
                else
                {
                    Game.PlayerPed.Armor = 1000;
                    Game.PlayerPed.Health = 1000;
                }

            }), false);



            RegisterCommand("dev", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
              if (dev != true)
                {
                    dev = true;
                }
                else
                {
                    dev = false;
                }
            }), false);

            RegisterCommand("seat", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                string pam = args[0].ToString();
                var carro = Game.PlayerPed.CurrentVehicle;
                switch (pam){
                    case "1":
                        Game.PlayerPed.SetIntoVehicle(carro,VehicleSeat.Driver);
                        break;
                    case "2":
                        Game.PlayerPed.SetIntoVehicle(carro, VehicleSeat.Passenger);
                        break;
                }

            }), false);



            RegisterCommand("placa", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                if (Game.PlayerPed.IsInVehicle() == true) {
                    var carro = GetVehiclePedIsIn(Game.PlayerPed.Handle, false);
                    string valor = args[0].ToString();
                    SetVehicleNumberPlateText(carro, valor);
                }
                else
                {
                    Debug.WriteLine("Você não está em um veículo!");
                }
                
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

            RegisterCommand("fixar", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                var carro = Game.PlayerPed.LastVehicle;

                if (Game.PlayerPed.IsInVehicle() != false) {
                    carro = Game.PlayerPed.CurrentVehicle;
                }
                carro.Repair();

            }), false);

            


            //NoCLIP
            RegisterCommand("nc", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                if (Game.PlayerPed.Health <= 0)
                {
                    Game.PlayerPed.Resurrect();
                }

                Debug.WriteLine("" + inv);
                PlaySoundFrontend(-1, "Checkpoint_Hit", "GTAO_FM_Events_Soundset", true);
                if (inv != true)
                {
                    posjogador = Game.PlayerPed.Position;
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, inv);
                    SetPoliceIgnorePlayer(Game.PlayerPed.NetworkId, inv);
                    Game.PlayerPed.Ragdoll(-1);
                    Game.PlayerPed.CanRagdoll = false;
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, inv);
                    SetEntityAlpha(PlayerPedId(), 0, 0);
                    inv = true;
                }
                else
                {
                    SetPoliceIgnorePlayer(Game.PlayerPed.NetworkId, false);
                    SetEntityAlpha(PlayerPedId(), 1000, 0);
                    SetPlayerInvincible(Game.PlayerPed.NetworkId, jogador.inv);
                    
                    inv = false;
                }

            }), false);

            //Excluir Veiculo | DV
            RegisterCommand("dv", new Action<int, List<object>, string>(async (source, args, raw) =>
            {
                jogador.posjogador = Game.PlayerPed.Position;
                if (Game.PlayerPed.IsInVehicle() != false)
                {
                    Game.PlayerPed.CurrentVehicle.Delete();
                }
                else
                {
                    Debug.WriteLine("Não está no veiculo");
                    
                    int carroprox = GetClosestVehicle(jogador.posjogador.X, jogador.posjogador.Y, jogador.posjogador.Z, 120f, 0, 0);
                    var deletar = Vehicle.FromNetworkId(carroprox);
                    deletar.Delete();
                }
                Debug.WriteLine("Deletado!");

            }), false);

        }
    }
}

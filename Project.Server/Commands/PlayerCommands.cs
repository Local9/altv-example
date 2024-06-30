using AltV.Net;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Project.Server.Factories;
using Project.Shared;

namespace Project.Server.Commands
{
    internal class PlayerCommands : IScript
    {
        [Command("weapons")]
        public void GetWeapons(IAltPlayer player)
        {
            foreach (WeaponModel weapon in Enum.GetValues(typeof(WeaponModel)).Cast<WeaponModel>())
            {
                player.GiveWeapon(weapon, 1000, false);
            }
        }

        [Command("weapon")]
        public void GiveWeapon(IAltPlayer player, string weaponName, int ammo = 1000)
        {
            // weapon addon fails this check...

            //if (!Enum.IsDefined(typeof(WeaponModel), Alt.Hash(weaponName)))
            //{
            //    player.SendChatMessage("{FF0000} Invalid weapon model!");
            //    return;
            //}

            if (!weaponName.StartsWith("weapon_"))
            {
                weaponName = "weapon_" + weaponName;
            }

            player.GiveWeapon(Alt.Hash(weaponName), ammo, false);
        }

        [Command("model")]
        public void ChangeModel(IAltPlayer player)
        {
            if (player.Model == Alt.Hash("mp_m_freemode_01"))
            {
                player.Model = Alt.Hash("mp_f_freemode_01");
            }
            else
            {
                player.Model = Alt.Hash("mp_m_freemode_01");
            }

            player.RefreshFace();

            player.SendChatMessage($"{{00FF00}}Your model changed");
        }

        [Command("addcomponent")]
        public void WeaponComponent(IAltPlayer player, string name)
        {
            player.AddWeaponComponent(player.CurrentWeapon, Alt.Hash(name));
        }

        [Command("removecomponent")]
        public void RemoveWeaponComponent(IAltPlayer player, string name)
        {
            player.RemoveWeaponComponent(player.CurrentWeapon, Alt.Hash(name));
        }

        [Command("noclip")]
        public void NoClip(IAltPlayer player)
        {
            Console.WriteLine($"3^NoClip command called by {player.Name}");
            player.SendChatMessage($"{{00FF00}} No clip command triggered");

            player.Emit(AdminEvents.NOCLIP, "fuck");
        }
    }
}

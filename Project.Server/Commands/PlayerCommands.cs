using AltV.Net.Enums;
using Project.Server.Factories;

namespace Project.Server.Commands
{
    internal class PlayerCommands : IScript
    {
        public void OnStart()
        {
            CommandHandlers.Add("weapons", GetWeapons);
        }

        public void GetWeapons(IAltPlayer player, string cmd, string[] args)
        {
            foreach (WeaponModel weapon in Enum.GetValues(typeof(WeaponModel)).Cast<WeaponModel>())
            {
                player.GiveWeapon(weapon, 1000, false);
            }
        }

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

        public void WeaponComponent(IAltPlayer player, string name)
        {
            player.AddWeaponComponent(player.CurrentWeapon, Alt.Hash(name));
        }

        public void RemoveWeaponComponent(IAltPlayer player, string name)
        {
            player.RemoveWeaponComponent(player.CurrentWeapon, Alt.Hash(name));
        }
    }
}

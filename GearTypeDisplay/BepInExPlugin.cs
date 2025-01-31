﻿using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace GearTypeDisplay
{
    [BepInPlugin("aedenthorn.GearTypeDisplay", "Gear Type Display", "0.1.1")]
    public partial class BepInExPlugin : BaseUnityPlugin
    {
        private static BepInExPlugin context;

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> isDebug;

        //public static ConfigEntry<int> nexusID;

        public static void Dbgl(string str = "", bool pref = true)
        {
            if (isDebug.Value)
                Debug.Log((pref ? typeof(BepInExPlugin).Namespace + " " : "") + str);
        }
        private void Awake()
        {

            context = this;
            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            isDebug = Config.Bind<bool>("General", "IsDebug", true, "Enable debug logs");
            
            //nexusID = Config.Bind<int>("General", "NexusID", 1, "Nexus mod ID for updates");

            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            Dbgl("Plugin awake");

        }


        [HarmonyPatch(typeof(UICombat), nameof(UICombat.ShowInfo))]
        static class ShowInfo_Patch
        {
            static void Postfix(UICombat __instance, Item item)
            {
                if (!modEnabled.Value || !item)
                    return;

                string old = null;

                switch (item.rarity)
                {
                    case Rarity.one:
                        old = Localization.GetContent("Common", Array.Empty<object>());
                        break;
                    case Rarity.two:
                        old = Localization.GetContent("Rare", Array.Empty<object>());
                        break;
                    case Rarity.three:
                        old = Localization.GetContent("Superior", Array.Empty<object>());
                        break;
                    case Rarity.four:
                        old = Localization.GetContent("Unique", Array.Empty<object>());
                        break;
                    case Rarity.five:
                        old = Localization.GetContent("Legendary", Array.Empty<object>());
                        break;
                    case Rarity.six:
                        old = Localization.GetContent("Ultra Legendary", Array.Empty<object>());
                        break;
                }
                if (old == null)
                    return;

                string type = null;
                switch (item.slotType)
                {
                    case SlotType.helmet:
                        type = "Helmet";
                        break;
                    case SlotType.weapon:
                        Weapon weapon = item.GetComponent<Weapon>();
                        switch (weapon.weaponType)
                        {
                            case WeaponType.onehand:
                                type = "Sword";
                                break;
                            case WeaponType.twohand:
                                type = "Sword";
                                break;
                            case WeaponType.spear:
                                type = "Spear";
                                break;
                            case WeaponType.throwingaxe:
                                type = "Axe";
                                break;
                            case WeaponType.javelin:
                                type = "Javelin";
                                break;
                            case WeaponType.twohandaxe:
                                type = "Axe";
                                break;
                            case WeaponType.onehandaxe:
                                type = "Axe";
                                break;
                            case WeaponType.bow:
                                type = "Bow";
                                break;
                            case WeaponType.dagger:
                                type = "Dagger";
                                break;
                            case WeaponType.onehandhammer:
                                type = "Mace";
                                break;
                            case WeaponType.onehandspear:
                                type = "Spear";
                                break;
                        }
                        break;
                    case SlotType.shield:
                        type = "Shield";
                        break;
                    case SlotType.armor:
                        type = "Armor";
                        break;
                    case SlotType.necklace:
                        type = "Necklace";
                        break;
                    case SlotType.ring:
                        type = "Ring";
                        break;
                    case SlotType.shoes:
                        type = "Shoes";
                        break;
                    case SlotType.misc:
                        type = "Misc";
                        break;
                    case SlotType.legging:
                        type = "Leggings";
                        break;
                    case SlotType.gloves:
                        type = "Gloves";
                        break;
                    case SlotType.none:
                        return;
                    case SlotType.bra:
                        type = "Bra";
                        break;
                    case SlotType.panties:
                        type = "Panties";
                        break;
                    case SlotType.stockings:
                        type = "Stockings";
                        break;
                    case SlotType.suspenders:
                        type = "Suspenders";
                        break;
                    case SlotType.heels:
                        type = "Heels";
                        break;
                    case SlotType.lingeriegloves:
                        type = "Lingerie Gloves";
                        break;
                    default:
                        return;
                }

                if (type != null)
                {
                    for (int i = 0; i < __instance.descriptionHolder.childCount; i++)
                    {
                        __instance.descriptionHolder.GetChild(i).GetComponent<Text>().text = __instance.descriptionHolder.GetChild(i).GetComponent<Text>().text.Replace(old, old + " " + Localization.GetContent(type, new object[0]));
                    }
                }
                //Dbgl($"show info for {item.name}, type: {type}, string: {__instance.descriptionHolder.GetChild(0).GetComponent<Text>().text}");
            }
        }
    }
}

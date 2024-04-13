using GameNetcodeStuff;
using HarmonyLib;
using NeedyCats;
using ReservedItemSlotCore;
using ReservedItemSlotCore.Data;
using ReservedItemSlotCore.Patches;
using ReservedAnimalSlot.Compatibility;
using TooManyEmotes;
using Unity.Netcode;
using UnityEngine;

namespace ReservedAnimalSlot.Patches
{
    [HarmonyPatch]
    internal class CatPatch
    {
        public static PlayerControllerB localPlayerController { get { return StartOfRound.Instance?.localPlayerController; } }
        public static ReservedPlayerData localPlayerData { get { return ReservedPlayerData.localPlayerData; } }

        [HarmonyPatch(typeof(ReservedHotbarManager), nameof(ReservedHotbarManager.CanSwapHotbars))]
        [HarmonyPostfix]
        public static void CanSwapHotbars(ref bool __result)
        {
            if (!HUDPatcher.hasReservedItemSlotsAndEnabled) return;
            if (!localPlayerData.currentItemSlotIsReserved) return;

            if (!__result)
            {
                if (localPlayerData.currentlySelectedItem as NeedyCatProp == null) return;
                if (TooManyEmotes_Patcher.Enabled && TooManyEmotes_Patcher.IsLocalPlayerPerformingCustomEmote() && !TooManyEmotes_Patcher.CanMoveWhileEmoting()) return;
                if (!(ReservedPlayerData.localPlayerData.grabbingReservedItemData != null || localPlayerController.isGrabbingObjectAnimation || localPlayerController.quickMenuManager.isMenuOpen || localPlayerController.inSpecialInteractAnimation || localPlayerData.throwingObject || localPlayerController.isTypingChat || localPlayerController.activatingItem || localPlayerController.jetpackControls || localPlayerController.disablingJetpackControls || localPlayerController.inTerminalMenu || localPlayerController.isPlayerDead || localPlayerData.timeSinceSwitchingSlots < 0.3f)) __result = false;
                __result = true;
            }
        }
    }
}

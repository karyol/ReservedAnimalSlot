using GameNetcodeStuff;
using HarmonyLib;
using NeedyCats;
using ReservedItemSlotCore.Data;
using System.Collections.Generic;
using System.Linq;

namespace ReservedAnimalSlot.Patches
{
    [HarmonyPatch]
    internal class CatPatch
    {
        public static PlayerControllerB localPlayerController { get { return StartOfRound.Instance?.localPlayerController; } }
        public static ReservedPlayerData localPlayerData { get { return ReservedPlayerData.localPlayerData; } }

        [HarmonyPatch(typeof(NeedyCatsBase), "AddNeedyCatsToAllLevels")]
        [HarmonyPostfix]
        public static void ChangeNeedyCatTwoHandedState(NeedyCatsBase __instance)
        {
            Item item = NeedyCatsBase.Assets.MainAssetBundle.LoadAsset<Item>("CatItem");
            foreach (SelectableLevel selectableLevel in StartOfRound.Instance.levels)
            {
                IEnumerable<SpawnableItemWithRarity> spawnableScrap = selectableLevel.spawnableScrap;
                spawnableScrap.Where(scrap => scrap.spawnableItem == item).First().spawnableItem.twoHanded = false;
            }
        }
    }
}

using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using UnityEngine;
using ReservedItemSlotCore.Data;
using ReservedAnimalSlot.Config;

namespace ReservedAnimalSlot;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency("FlipMods.ReservedItemSlotCore", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("Jordo.NeedyCats", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.SoftDependency)]
public class ReservedAnimalSlot : BaseUnityPlugin
{
    public static ReservedAnimalSlot Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    public static ReservedItemSlotData catSlotData;
    public static ReservedItemData catData;

    public static List<ReservedItemData> additionalItemData = new List<ReservedItemData>();

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        ConfigSettings.BindConfigSettings();
        CreateReservedItemSlots();
        CreateAdditionalReservedItemSlots();

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    void CreateReservedItemSlots()
    {
        catSlotData = ReservedItemSlotData.CreateReservedItemSlotData("cat", ConfigSettings.overrideItemSlotPriority.Value, ConfigSettings.overridePurchasePrice.Value);
        catData = catSlotData.AddItemToReservedItemSlot(new ReservedItemData("Cat", PlayerBone.RightShoulder, new Vector3(-.2f, .25f, 0f), new Vector3(0, 90, 90)));
    }

    void CreateAdditionalReservedItemSlots()
    {
        string[] additionalItemNames = ConfigSettings.ParseAdditionalItems();
        foreach (string itemName in additionalItemNames)
        {
            if (!catSlotData.ContainsItem(itemName))
            {
                LogWarning("Adding additional item to reserved item slot. Item: " + itemName);
                var itemData = new ReservedItemData(itemName);
                additionalItemData.Add(itemData);
                catSlotData.AddItemToReservedItemSlot(itemData);
            }
        }
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);
        Harmony.PatchAll();
    }

    public static void Log(string message) => Logger.LogInfo(message);
    public static void LogError(string message) => Logger.LogError(message);
    public static void LogWarning(string message) => Logger.LogWarning(message);
    public static bool IsModLoaded(string guid) => BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(guid);
}

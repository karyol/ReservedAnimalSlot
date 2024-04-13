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
[BepInDependency("Jordo.NeedyCats", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("TridentsCodes.LoudParrots", BepInDependency.DependencyFlags.SoftDependency)]
[BepInDependency("com.rune580.LethalCompanyInputUtils", BepInDependency.DependencyFlags.SoftDependency)]
public class ReservedAnimalSlot : BaseUnityPlugin
{
    public static ReservedAnimalSlot Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    public static ReservedItemSlotData animalSlotData;
    public static ReservedItemData catData;
    public static ReservedItemData parrotData;
    public static ReservedItemData goldFinchData;
    public static ReservedItemData crowData;
    public static ReservedItemData robinData;
    public static ReservedItemData cardinalData;
    public static ReservedItemData littleBirdData;
    public static ReservedItemData blueJayData;
    public static ReservedItemData sparrowData;

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
        animalSlotData = ReservedItemSlotData.CreateReservedItemSlotData("cat", ConfigSettings.overrideItemSlotPriority.Value, ConfigSettings.overridePurchasePrice.Value);
        catData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Cat", PlayerBone.RightShoulder, new Vector3(-.2f, .25f, 0f), new Vector3(0, 90, 90)));
        parrotData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Parrot", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(90, 0, 0)));
        crowData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Crow", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        robinData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Robin", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        sparrowData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Sparrow", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        goldFinchData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Gold Finch", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        cardinalData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("cardinal", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        littleBirdData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Little bird", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
        blueJayData = animalSlotData.AddItemToReservedItemSlot(new ReservedItemData("Blue bird", PlayerBone.RightShoulder, new Vector3(0f, .2f, 0f), new Vector3(0, 90, 90)));
    }

    void CreateAdditionalReservedItemSlots()
    {
        string[] additionalItemNames = ConfigSettings.ParseAdditionalItems();
        foreach (string itemName in additionalItemNames)
        {
            if (!animalSlotData.ContainsItem(itemName))
            {
                LogWarning("Adding additional item to reserved item slot. Item: " + itemName);
                var itemData = new ReservedItemData(itemName);
                additionalItemData.Add(itemData);
                animalSlotData.AddItemToReservedItemSlot(itemData);
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

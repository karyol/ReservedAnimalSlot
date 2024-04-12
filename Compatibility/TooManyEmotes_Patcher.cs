using TooManyEmotes;
using TooManyEmotes.Networking;

namespace ReservedAnimalSlot.Compatibility
{
    internal static class TooManyEmotes_Patcher
    {
        public static bool Enabled { get { return ReservedAnimalSlot.IsModLoaded("FlipMods.TooManyEmotes"); } }

        public static bool IsLocalPlayerPerformingCustomEmote()
        {
            if (EmoteControllerPlayer.emoteControllerLocal != null && EmoteControllerPlayer.emoteControllerLocal.IsPerformingCustomEmote())
                return true;
            return false;
        }

        public static bool CanMoveWhileEmoting()
        {
            if (ConfigSync.instance != null)
                return ConfigSync.instance.syncEnableMovingWhileEmoting;
            return false;
        }
    }
}

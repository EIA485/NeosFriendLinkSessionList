using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using FrooxEngine.UIX;
using BaseX;
using System.Collections.Generic;

namespace FriendLinkSessionList
{
    public class FriendLinkSessionList : NeosMod
    {
        public override string Name => "FriendLinkSessionList";
        public override string Author => "eia485";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/EIA485/NeosFriendLinkSessionList/";

        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("net.eia485.friendLinkSessionList");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(SessionControlDialog), "OnCommonUpdate")]
        private class friendLinkSessionListPatch
        {
            public static void Postfix(SessionControlDialog __instance, SyncRef<Slot> ____uiContentRoot)
            {
                if (__instance.ActiveTab.Value == SessionControlDialog.Tab.Settings) return;

                List<User> users = Pool.BorrowList<User>();
                Engine.Current.WorldManager.FocusedWorld?.GetUsers(users);

                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].UserID == null) continue;
                    Slot uislot = ____uiContentRoot.Target[(__instance.ActiveTab.Value == SessionControlDialog.Tab.Permissions) ? (6 + i) : i][0];
                    if (uislot.GetComponent<FriendLink>() != null) continue;
                    uislot.AttachComponent<FriendLink>().UserId.Value = users[i].UserID;
                    uislot.AttachComponent<Button>();
                }

            }
        }
    }
}
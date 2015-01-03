﻿using PoeHUD.Hud.Settings;

namespace PoeHUD.Hud.Loot
{
    public sealed class ItemLevelSettings : SettingsBase
    {
        public ItemLevelSettings()
        {
            Enable = true;
            TextSize = new RangeNode<int>(27, 10, 50);
        }

        public RangeNode<int> TextSize { get; set; }
    }
}
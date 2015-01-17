﻿using System;
using System.Collections.Generic;

using PoeHUD.Controllers;
using PoeHUD.Framework;
using PoeHUD.Poe.FilesInMemory;
using PoeHUD.Poe.RemoteMemoryObjects;

using SharpDX;

namespace PoeHUD.Hud.AdvancedTooltip
{
    public class ModValue
    {
        private readonly int totalTiers = 1;

        public ModValue(ItemMod mod, FsController fs, int iLvl)
        {
            string name = mod.RawName;
            Record = fs.Mods.records[name];
            AffixType = Record.AffixType;
            AffixText = String.IsNullOrEmpty(Record.UserFriendlyName) ? Record.Key : Record.UserFriendlyName;
            IsCrafted = Record.Domain == 10;
            StatValue = new[] { mod.Value1, mod.Value2, mod.Value3, mod.Value4 };
            Tier = -1;

            int subOptimalTierDistance = 0;

            List<ModsDat.ModRecord> allTiers;
            if (fs.Mods.recordsByTier.TryGetValue(Tuple.Create(Record.Group, Record.AffixType), out allTiers))
            {
                bool tierFound = false;
                totalTiers = 0;
                foreach (ModsDat.ModRecord record in allTiers)
                {
                    // still not filtering out some mods. (like a.spd from gloves projected onto rings)
                    if (record.StatNames[0] == Record.StatNames[0] && record.StatNames[1] == Record.StatNames[1]
                        && record.StatNames[2] == Record.StatNames[2] && record.StatNames[3] == Record.StatNames[3])
                    {
                        totalTiers++;
                        if (record.Equals(Record))
                        {
                            Tier = totalTiers;
                            tierFound = true;
                        }
                        if (!tierFound && record.MinLevel <= iLvl)
                        {
                            subOptimalTierDistance++;
                        }
                    }
                }
            }

            double hue = totalTiers == 1 ? 180 : 120 - Math.Min(subOptimalTierDistance, 3) * 40;
            Color = ColorUtils.ColorFromHsv(hue, totalTiers == 1 ? 0 : 1, 1);
        }

        public ModsDat.ModType AffixType { get; private set; }

        public bool IsCrafted { get; private set; }

        public String AffixText { get; private set; }

        public Color Color { get; private set; }

        public ModsDat.ModRecord Record { get; private set; }

        public int[] StatValue { get; private set; }

        public int Tier { get; private set; }

        public bool CouldHaveTiers()
        {
            return totalTiers > 1;
        }
    }
}
using AerovelenceMod.Buffs;
using AerovelenceMod.Items.Ores.PreHM.Frost;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Items.Ores.PreHM.Phantic
{
	[AutoloadEquip(EquipType.Head)]
    public class PhanticHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantic Helmet");
            Tooltip.SetDefault("10% increased melee damage\n8% increased melee swing speed\n8% increased melee critical strike chance");
        }
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<PhanticCuirass>() && legs.type == ModContent.ItemType<PhanticCuisses>() && head.type == ModContent.ItemType<PhanticHelmet>();
		}
        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons a Shiver thing to fight for you";
            if (Main.myPlayer == player.whoAmI && player.FindBuffIndex(mod.BuffType("ShiverMinion")) == -1)
            {
                player.AddBuff(mod.BuffType("ShiverMinion"), 100, false);
                for (int m = 0; m < 1; m++) { Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("ShiverMinion"), (int)(25f * player.minionDamage), player.minionKB, player.whoAmI); }
            }
        }
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10;
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }
		public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
        }

        public override void AddRecipes()  //How to craft this gun
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<FrostShard>(), 8);
            modRecipe.AddIngredient(ItemID.IceBlock, 35);
            modRecipe.AddIngredient(ItemID.HellstoneBar, 8);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
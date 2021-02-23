using AerovelenceMod.Items.Others.Crafting;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Items.Armor.HM.Starburst
{
    [AutoloadEquip(EquipType.Head)]
    public class StarburstHornedHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starburst Horned Helm");
            Tooltip.SetDefault("10% increased melee damage and swing speed\n8% increased melee critical strike chance");
        }
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StarburstChestplate>() && legs.type == ModContent.ItemType<StarburstGrieves>() && head.type == ModContent.ItemType<StarburstHornedHelm>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "11% increased melee damage\nTaking damage will release damaging shards of crystal";
            player.GetModPlayer<AeroPlayer>().BurnshockArmorBonus = true;
            player.meleeDamage += 0.11f;
        } 	
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = 10;
            item.rare = ItemRarityID.Orange;
            item.defense = 20;
        }
		public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.10f;
            player.meleeSpeed += 0.10f;
            player.meleeCrit += 8;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ModContent.ItemType<BurnshockBar>(), 8);
            modRecipe.AddIngredient(ItemID.CrystalShard, 5);
            modRecipe.AddTile(TileID.MythrilAnvil);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
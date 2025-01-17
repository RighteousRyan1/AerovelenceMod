using AerovelenceMod.Content.Items.Placeables.Blocks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Content.Items.Armor.Slate
{
    [AutoloadEquip(EquipType.Head)]
    public class SlateHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slate Helmet");
            Tooltip.SetDefault("2% increased damage");
        }
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SlateChestplate>() && legs.type == ModContent.ItemType<SlateLeggings>() && head.type == ModContent.ItemType<SlateHelmet>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Defense increased while in the cavern layer";
			if(player.ZoneRockLayerHeight)
            {
                player.statDefense += 7;
            }

        } 	
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = 10;
            item.rare = ItemRarityID.Blue;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.02f;
			player.rangedDamage += 0.02f;
			player.magicDamage += 0.02f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<SlateOre>(), 55);
            recipe.AddRecipeGroup("Wood", 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
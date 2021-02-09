using AerovelenceMod.Items.Others.Crafting;
using AerovelenceMod.Items.Placeable.Ores;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Items.Armor.PreHM.Hurm
{
    [AutoloadEquip(EquipType.Head)]
    public class thelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Slatasaae Helmet");
            Tooltip.SetDefault("2% increased damage");
        }
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<tchest>() && legs.type == ModContent.ItemType<tleggings>() && head.type == ModContent.ItemType<thelmet>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Potato";
			player.AddBuff(BuffID.Ravens, -1);
		} 	
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 22;
            item.value = 10;
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
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
            recipe.AddIngredient(ModContent.ItemType<SlateOreItem>(), 55);
            recipe.AddRecipeGroup("Wood", 20);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
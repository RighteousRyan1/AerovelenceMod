using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Content.Items.TreasureBags
{
	public class CrystalTumblerBag : ModItem
	{
		public override int BossBagNPC => mod.NPCType("CrystalTumbler");

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 36;
			item.height = 32;
			item.rare = ItemRarityID.Purple;
			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(mod.ItemType("PrismaticSoul"));
			player.QuickSpawnItem(ItemID.GoldCoin, 9);
			player.QuickSpawnItem(ItemID.HealingPotion, Main.rand.Next(4, 12));

			int drop = Main.rand.Next(8);

			player.TryGettingDevArmor();

			switch (Main.rand.Next(6))
			{
				case 0:
					player.QuickSpawnItem(mod.ItemType("CrystallineQuadshot"));
					break;
				case 1:
					player.QuickSpawnItem(mod.ItemType("PrismPiercer"));
					break;
				case 2:
					player.QuickSpawnItem(mod.ItemType("DiamondDuster"));
					break;
				case 3:
					player.QuickSpawnItem(mod.ItemType("PrismThrasher"));
					break;
				case 4:
					player.QuickSpawnItem(mod.ItemType("CavernousImpaler"));
					break;
				case 5:
					player.QuickSpawnItem(mod.ItemType("CavernMauler"));
					break;
				case 6:
					player.QuickSpawnItem(mod.ItemType("DarkCrystalStaff"));
					break;
			}
		}
	}
}
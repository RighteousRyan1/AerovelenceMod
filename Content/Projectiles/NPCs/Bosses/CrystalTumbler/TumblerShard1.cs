using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AerovelenceMod.Content.Projectiles.NPCs.Bosses.CrystalTumbler
{
	public class TumblerShard1 : ModProjectile
	{
		int t;
		public override void SetDefaults()
		{
			projectile.aiStyle = 1;
			projectile.penetrate = -1;
			projectile.width = 14;
			projectile.height = 18;
			projectile.alpha =  0;
			projectile.damage = 6;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override void AI()
		{
			t++;
			projectile.velocity *= 1.01f;
			int dust1 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Blood, projectile.velocity.X, projectile.velocity.Y, 0, Color.Blue, 1);
			Main.dust[dust1].velocity /= 2f;
			if (t > 25)
			{
				projectile.tileCollide = true;
			}
		}
	}
}
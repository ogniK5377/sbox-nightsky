using Sandbox;
using System.Linq;

namespace NightSky
{
	[Library( "nightsky" )]
	public partial class NightSkyGame : Sandbox.Game
	{
		private Entities.SkyDome _localSkyDome;
		public NightSkyGame()
		{
		}

		public override void PostLevelLoaded()
		{
			base.PostLevelLoaded();
			EnvironmentLightEntity lightEnv = All.OfType<EnvironmentLightEntity>().FirstOrDefault();

			if ( lightEnv == null ) return;
			// Lil hack to darken the map EVEN FURTHER by sucking the color out of lights
			lightEnv.Brightness = -0.5f;
			lightEnv.SkyIntensity = -0.5f;
			lightEnv.Color = Color.White;
			lightEnv.SkyColor = Color.White;
		}


		[ClientRpc]
		public void SpawnSkyDome()
		{
			_localSkyDome = new();
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			MinimalPawn ply = new();
			client.Pawn = ply;

			SpawnSkyDome( To.Single( client ) );

			ply.Respawn();
		}

		public override void DoPlayerNoclip( Client player )
		{
			if ( player.Pawn is Player basePlayer )
			{
				if ( basePlayer.DevController is NoclipController )
				{
					Log.Info( "Noclip Mode Off" );
					basePlayer.DevController = null;
				}
				else
				{
					Log.Info( "Noclip Mode On" );
					basePlayer.DevController = new NoclipController();
				}
			}
		}
	}

}

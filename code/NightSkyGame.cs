using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace NightSky
{
	[Library( "nightsky" )]
	public partial class NightSkyGame : Sandbox.Game
	{
		private TimeSince _timeSinceJoin;
		private bool _hasSkyEntity;
		private Entities.EnvNightSky _nightSky;
		public NightSkyGame()
		{
		}

		public override void PostLevelLoaded()
		{
			base.PostLevelLoaded();
			RegularLighting();

			// https://github.com/Facepunch/sbox-issues/issues/494
			List<Entity> ents = All.OfType<Entity>().Where( e => e.EngineEntityName == "sky_camera" ).ToList();
			foreach(Entity e in ents)
			{
				e.Delete();
			}
		}

		[ClientRpc]
		public void SpawnSky()
		{
			_timeSinceJoin = 0.0f;
		}

		[Event.Frame]
		public void OnFrame()
		{
			if ( Host.IsServer )
			{
				return;
			}

			// https://github.com/Facepunch/sbox-issues/issues/493
			if ( !_hasSkyEntity && _timeSinceJoin > 0.1f )
			{

				Log.Warning( "Created sky" );
				_nightSky = new();
				_hasSkyEntity = true;
			}
		}

		[ServerCmd( "nightsky_darkhack" )]
		public static void DarkHack()
		{
			EnvironmentLightEntity lightEnv = All.OfType<EnvironmentLightEntity>().FirstOrDefault();

			if ( lightEnv == null ) return;
			// Lil hack to darken the map EVEN FURTHER by sucking the color out of lights
			lightEnv.Brightness = -0.5f;
			lightEnv.SkyIntensity = -0.5f;
			lightEnv.Color = Color.White;
			lightEnv.SkyColor = Color.White;
		}

		[ServerCmd( "nightsky_regular" )]
		public static void RegularLighting()
		{
			EnvironmentLightEntity lightEnv = All.OfType<EnvironmentLightEntity>().FirstOrDefault();

			if ( lightEnv == null ) return;
			lightEnv.Brightness = 0.0f;
			lightEnv.SkyIntensity = 0.0f;
			lightEnv.Color = Color.Black;
			lightEnv.SkyColor = Color.Black;
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			MinimalPawn ply = new();
			client.Pawn = ply;

			SpawnSky( To.Single( client ) );

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

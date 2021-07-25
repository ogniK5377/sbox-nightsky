using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightSky.Entities
{
	class EnvNightSky : Entity
	{
		public SkyboxObject SkyObject { get; set; }
		private Material _skyMaterial;
		public EnvNightSky() : base()
		{
		}

		public override void Spawn()
		{
			Transmit = TransmitType.Always;
			if ( IsClient )
			{
				_skyMaterial = Material.Load( "materials/night_sky.vmat" );
				SkyObject = new SkyboxObject( _skyMaterial );
			}
		}
	}
}

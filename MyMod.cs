using System;
using Terraria;
using Terraria.ModLoader;
using ModLibsCore.Libraries.Debug;


namespace ModLibsTiles {
	/// @private
	partial class ModLibsTilesMod : Mod {
		public static ModLibsTilesMod Instance { get; private set; }



		////////////////

		public ModLibsTilesMod() {
			ModLibsTilesMod.Instance = this;
		}


		public override void Load() {
		}

		////

		public override void Unload() {
			try {
				LogLibraries.Alert( "Unloading mod..." );
			} catch { }
			
			ModLibsTilesMod.Instance = null;
		}
	}
}

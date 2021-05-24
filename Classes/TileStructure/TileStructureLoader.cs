﻿using System;
using System.IO;
using NetSerializer;
using Terraria;
using ModLibsCore.Classes.Loadable;


namespace ModLibsTiles.Classes.TileStructure {
	class TileStructureLoader : ILoadable {
		internal Serializer Serializer = new Serializer( new Type[] { typeof(TileStructure) } );



		////////////////

		void ILoadable.OnModsLoad() { }
		void ILoadable.OnModsUnload() { }
		void ILoadable.OnPostModsLoad() { }


		////////////////

		internal TileStructure Load( byte[] rawData ) {
			using( var ms = new MemoryStream(rawData) ) {
				TileStructure ret;
				this.Serializer.DeserializeDirect( ms, out ret );
				return ret;
			}
		}

		internal byte[] Save( TileStructure data ) {
			using( var ms = new MemoryStream() ) {
				this.Serializer.SerializeDirect( ms, data );
				return ms.ToArray();
			}
		}
	}
}

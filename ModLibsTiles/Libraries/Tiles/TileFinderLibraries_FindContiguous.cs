﻿using System;
using Terraria;
using ModLibsCore.Libraries.Debug;
using ModLibsTiles.Classes.Tiles.TilePattern;


namespace ModLibsTiles.Libraries.Tiles {
	/// <summary>
	/// Assorted static "helper" functions pertaining to tile finding.
	/// </summary>
	public partial class TileFinderLibraries {
		/// <summary>
		/// Finds the top left tile of a given area by a given pattern. Assumes the area is square (checks left first).
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxDistance"></param>
		/// <param name="coords"></param>
		/// <returns>Returns tile coordinates, or else `null` if not found within the given max amounts.</returns>
		public static bool FindTopLeftOfSquare(
					TilePattern pattern,
					int tileX,
					int tileY,
					int maxDistance,
					out (int TileX, int TileY) coords ) {
			if( !pattern.Check(tileX, tileY) ) {
				coords = (tileX, tileY);
				return false;
			}

			int i = 1, j = 1;
			bool foundX = false, foundY = false;
			
			do {
				if( !pattern.Check( tileX - i, tileY ) ) {
					foundX = true;
					i--;
					break;
				}
			} while( i++ < maxDistance );

			do {
				if( !pattern.Check( tileX - i, tileY - j ) ) {
					foundY = true;
					j--;
					break;
				}
			} while( j++ < maxDistance );

			coords = (TileX: tileX - i, TileY: tileY - j);
			return foundX && foundY;
		}


		////////////////

		/// <summary>
		/// Traces downwards from a given tile coordinate to the nearest floor, and then measures the contiguous width.
		/// </summary>
		/// <param name="nonFloorPattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxFallRange">Max distance to trace downwards to a floor before giving up.</param>
		/// <param name="floorX">Leftmost tile of the contiguous floor.</param>
		/// <param name="floorY">Last matching tile before hitting the floor.</param>
		/// <returns></returns>
		public static int GetFloorWidth(
					TilePattern nonFloorPattern,
					int tileX,
					int tileY,
					int maxFallRange,
					out int floorX,
					out int floorY ) {
			floorY = tileY;

			while( nonFloorPattern.Check(tileX, floorY) ) {
				floorY++;

				if( (floorY - tileY) >= maxFallRange ) {
					floorX = tileX;
					return 0;//TileFinderLibraries.GetHorizontalWidthAt( nonFloorPattern, tileX, out floorX, --floorY );
				}
			}
			floorY--;

			return TileFinderLibraries.GetHorizontalWidthAt( nonFloorPattern, tileX, out floorX, floorY );
		}

		/// <summary>
		/// Traces upwards from a given tile coordinate to the nearest ceiling, and then measures the contiguous width.
		/// </summary>
		/// <param name="nonCeilingPattern"></param>
		/// <param name="tileX"></param>
		/// <param name="tileY"></param>
		/// <param name="maxRiseRange">Max distance to trace upwards to a ceiling before giving up.</param>
		/// <param name="ceilX">Leftmost tile of the contiguous ceiling.</param>
		/// <param name="ceilY">Last matching tile before hitting the ceiling.</param>
		/// <returns></returns>
		public static int GetCeilingWidth( TilePattern nonCeilingPattern, int tileX, int tileY, int maxRiseRange, out int ceilX, out int ceilY ) {
			ceilY = tileY;

			while( nonCeilingPattern.Check( tileX, ceilY ) ) {
				ceilY--;

				if( ( tileY - ceilY ) >= maxRiseRange ) {
					ceilX = tileX;
					return 0;//TileFinderLibraries.GetHorizontalWidthAt( nonCeilingPattern, tileX, out ceilX, ++ceilY );
				}
			}
			ceilY++;

			return TileFinderLibraries.GetHorizontalWidthAt( nonCeilingPattern, tileX, out ceilX, ceilY );
		}


		////

		private static int GetHorizontalWidthAt( TilePattern pattern, int tileX, out int floorX, int floorY ) {
			int rightWidth = 1;
			while( pattern.Check(tileX + rightWidth, floorY) && !pattern.Check(tileX + rightWidth, floorY + 1) ) {
				rightWidth++;
			}

			int leftWidth = 0;
			while( pattern.Check(tileX - leftWidth, floorY) && !pattern.Check(tileX - leftWidth, floorY + 1) ) {
				leftWidth++;
			}

			floorX = tileX - leftWidth;
			return (rightWidth - 1) + leftWidth;
		}
	}
}

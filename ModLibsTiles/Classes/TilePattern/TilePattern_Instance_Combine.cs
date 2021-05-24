﻿using System.Collections.Generic;
using Terraria;
using ModLibsGeneral.Libraries.Tiles;


namespace ModLibsTiles.Classes.Tiles.TilePattern {
	/// <summary>
	/// Identifies a type of tile by its attributes.
	/// </summary>
	public partial class TilePattern {
		/// <summary>
		/// Combines 2 patterns into a new pattern. Positive filters are favored over negatives.
		/// </summary>
		/// <param name="pattern1"></param>
		/// <param name="pattern2"></param>
		/// <param name="blendLight"></param>
		/// <returns></returns>
		public static TilePattern CombinePositive( TilePattern pattern1, TilePattern pattern2, bool blendLight = false ) {
			var builder = new TilePatternBuilder();

			builder.Invert = pattern1.Invert || pattern2.Invert;

			builder.IsActive = TilePattern.CombinePositive( pattern1.IsActive, pattern2.IsActive );

			if( pattern1.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern1.IsAnyOfType );
				if( pattern2.IsAnyOfType != null ) {
					builder.IsAnyOfType.UnionWith( pattern2.IsAnyOfType );
				}
			} else if( pattern2.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern2.IsAnyOfType );
			}

			if( pattern1.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern1.IsAnyOfWallType );
				if( pattern2.IsAnyOfWallType != null ) {
					builder.IsAnyOfWallType.UnionWith( pattern2.IsAnyOfWallType );
				}
			} else if( pattern2.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern2.IsAnyOfWallType );
			}

			if( pattern1.IsNotAnyOfType != null ) {
				builder.IsNotAnyOfType = new HashSet<int>( pattern1.IsNotAnyOfType );
				if( pattern2.IsNotAnyOfType != null ) {
					builder.IsNotAnyOfType.UnionWith( pattern2.IsNotAnyOfType );
				}
			} else if( pattern2.IsNotAnyOfType != null ) {
				builder.IsNotAnyOfType = new HashSet<int>( pattern2.IsNotAnyOfType );
			}

			if( pattern1.IsNotAnyOfWallType != null ) {
				builder.IsNotAnyOfWallType = new HashSet<int>( pattern1.IsNotAnyOfWallType );
				if( pattern2.IsNotAnyOfWallType != null ) {
					builder.IsNotAnyOfWallType.UnionWith( pattern2.IsNotAnyOfWallType );
				}
			} else if( pattern2.IsNotAnyOfWallType != null ) {
				builder.IsNotAnyOfWallType = new HashSet<int>( pattern2.IsNotAnyOfWallType );
			}

			builder.HasWire1 = TilePattern.CombinePositive( pattern1.HasWire1, pattern2.HasWire1 );
			builder.HasWire2 = TilePattern.CombinePositive( pattern1.HasWire2, pattern2.HasWire2 );
			builder.HasWire3 = TilePattern.CombinePositive( pattern1.HasWire3, pattern2.HasWire3 );
			builder.HasWire4 = TilePattern.CombinePositive( pattern1.HasWire4, pattern2.HasWire4 );

			//builder.IsSolid = TilePattern.CombinePositive( pattern1.IsSolid, pattern2.IsSolid );
			builder.HasSolidProperties = TilePattern.CombinePositive( pattern1.HasSolidProperties, pattern2.HasSolidProperties );
			builder.IsPlatform = TilePattern.CombinePositive( pattern1.IsPlatform, pattern2.IsPlatform );
			builder.IsActuated = TilePattern.CombinePositive( pattern1.IsActuated, pattern2.IsActuated );
			builder.IsVanillaBombable = TilePattern.CombinePositive( pattern1.IsVanillaBombable, pattern2.IsVanillaBombable );

			builder.HasWall = TilePattern.CombinePositive( pattern1.HasWall, pattern2.HasWall );

			builder.HasWater = TilePattern.CombinePositive( pattern1.HasWater, pattern2.HasWater );
			builder.HasHoney = TilePattern.CombinePositive( pattern1.HasHoney, pattern2.HasHoney );
			builder.HasLava = TilePattern.CombinePositive( pattern1.HasLava, pattern2.HasLava );

			if( pattern1.Shape.HasValue && !pattern2.Shape.HasValue ) {
				builder.Shape = pattern1.Shape;
			} else if( !pattern1.Shape.HasValue && pattern2.Shape.HasValue ) {
				builder.Shape = pattern2.Shape;
			} else if( pattern1.Shape.HasValue && pattern2.Shape.HasValue ) {
				if( pattern1.Shape.Value == TileShapeType.TopSlope ) {
					if( pattern2.Shape.Value == TileShapeType.LeftSlope ) {
						builder.Shape = TileShapeType.TopLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.RightSlope ) {
						builder.Shape = TileShapeType.TopRightSlope;
					} else {
						builder.Shape = pattern2.Shape;
					}
				} else if( pattern1.Shape.Value == TileShapeType.BottomSlope ) {
					if( pattern2.Shape.Value == TileShapeType.LeftSlope ) {
						builder.Shape = TileShapeType.BottomLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.RightSlope ) {
						builder.Shape = TileShapeType.BottomRightSlope;
					} else {
						builder.Shape = pattern2.Shape;
					}
				} else if( pattern1.Shape.Value == TileShapeType.LeftSlope ) {
					if( pattern2.Shape.Value == TileShapeType.TopSlope ) {
						builder.Shape = TileShapeType.TopLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.BottomSlope ) {
						builder.Shape = TileShapeType.BottomLeftSlope;
					} else {
						builder.Shape = pattern2.Shape;
					}
				} else {
					if( pattern2.Shape.Value == TileShapeType.TopSlope ) {
						builder.Shape = TileShapeType.TopRightSlope;
					} else if( pattern2.Shape.Value == TileShapeType.BottomSlope ) {
						builder.Shape = TileShapeType.BottomRightSlope;
					} else {
						builder.Shape = pattern2.Shape;
					}
				}
			}

			if( pattern1.MinimumBrightness.HasValue && !pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern1.MinimumBrightness;
			} else if( !pattern1.MinimumBrightness.HasValue && pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern2.MinimumBrightness;
			} else {
				if( blendLight ) {
					builder.MinimumBrightness = ( pattern1.MinimumBrightness.Value + pattern2.MinimumBrightness.Value )
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MinimumBrightness.Value < pattern2.MinimumBrightness.Value ?
						pattern1.MinimumBrightness.Value :
						pattern2.MinimumBrightness.Value;
				}
			}
			if( pattern1.MaximumBrightness.HasValue && !pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern1.MaximumBrightness;
			} else if( !pattern1.MaximumBrightness.HasValue && pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern2.MaximumBrightness;
			} else {
				if( blendLight ) {
					builder.MaximumBrightness = ( pattern1.MaximumBrightness.Value + pattern2.MaximumBrightness.Value )
						* 0.5f;
				} else {
					builder.MaximumBrightness = pattern1.MaximumBrightness.Value < pattern2.MaximumBrightness.Value ?
						pattern1.MaximumBrightness.Value :
						pattern2.MaximumBrightness.Value;
				}
			}

			builder.IsModded = TilePattern.CombinePositive( pattern1.IsModded, pattern2.IsModded );

			if( pattern1.CustomCheck != null && pattern2.CustomCheck != null ) {
				builder.CustomCheck = ( x, y ) => pattern1.CustomCheck(x, y) || pattern2.CustomCheck(x, y);
			} else if( pattern1.CustomCheck != null ) {
				builder.CustomCheck = pattern1.CustomCheck;
			} else if( pattern2.CustomCheck != null ) {
				builder.CustomCheck = pattern2.CustomCheck;
			}
			
			return new TilePattern( builder );
		}


		/// <summary>
		/// Combines 2 patterns into a new pattern. Negative filters are favored over positives.
		/// </summary>
		/// <param name="pattern1"></param>
		/// <param name="pattern2"></param>
		/// <param name="blendLight"></param>
		/// <returns></returns>
		public static TilePattern CombineNegative( TilePattern pattern1, TilePattern pattern2, bool blendLight = false ) {
			var builder = new TilePatternBuilder();

			builder.Invert = pattern1.Invert && pattern2.Invert;

			builder.IsActive = TilePattern.CombineNegative( pattern1.IsActive, pattern2.IsActive );

			if( pattern1.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern1.IsAnyOfType );
				if( pattern2.IsAnyOfType != null ) {
					builder.IsAnyOfType.UnionWith( pattern2.IsAnyOfType );
				}
			} else if( pattern2.IsAnyOfType != null ) {
				builder.IsAnyOfType = new HashSet<int>( pattern2.IsAnyOfType );
			}

			if( pattern1.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern1.IsAnyOfWallType );
				if( pattern2.IsAnyOfWallType != null ) {
					builder.IsAnyOfWallType.UnionWith( pattern2.IsAnyOfWallType );
				}
			} else if( pattern2.IsAnyOfWallType != null ) {
				builder.IsAnyOfWallType = new HashSet<int>( pattern2.IsAnyOfWallType );
			}

			if( pattern1.IsNotAnyOfType != null ) {
				builder.IsNotAnyOfType = new HashSet<int>( pattern1.IsNotAnyOfType );
				if( pattern2.IsNotAnyOfType != null ) {
					builder.IsNotAnyOfType.UnionWith( pattern2.IsNotAnyOfType );
				}
			} else if( pattern2.IsNotAnyOfType != null ) {
				builder.IsNotAnyOfType = new HashSet<int>( pattern2.IsNotAnyOfType );
			}

			if( pattern1.IsNotAnyOfWallType != null ) {
				builder.IsNotAnyOfWallType = new HashSet<int>( pattern1.IsNotAnyOfWallType );
				if( pattern2.IsNotAnyOfWallType != null ) {
					builder.IsNotAnyOfWallType.UnionWith( pattern2.IsNotAnyOfWallType );
				}
			} else if( pattern2.IsNotAnyOfWallType != null ) {
				builder.IsNotAnyOfWallType = new HashSet<int>( pattern2.IsNotAnyOfWallType );
			}

			builder.HasWire1 = TilePattern.CombineNegative( pattern1.HasWire1, pattern2.HasWire1 );
			builder.HasWire2 = TilePattern.CombineNegative( pattern1.HasWire2, pattern2.HasWire2 );
			builder.HasWire3 = TilePattern.CombineNegative( pattern1.HasWire3, pattern2.HasWire3 );
			builder.HasWire4 = TilePattern.CombineNegative( pattern1.HasWire4, pattern2.HasWire4 );

			//builder.IsSolid = TilePattern.CombineNegative( pattern1.IsSolid, pattern2.IsSolid );
			builder.HasSolidProperties = TilePattern.CombineNegative( pattern1.HasSolidProperties, pattern2.HasSolidProperties );
			builder.IsPlatform = TilePattern.CombineNegative( pattern1.IsPlatform, pattern2.IsPlatform );
			builder.IsActuated = TilePattern.CombineNegative( pattern1.IsActuated, pattern2.IsActuated );
			builder.IsVanillaBombable = TilePattern.CombineNegative( pattern1.IsVanillaBombable, pattern2.IsVanillaBombable );

			builder.HasWall = TilePattern.CombineNegative( pattern1.HasWall, pattern2.HasWall );

			builder.HasWater = TilePattern.CombineNegative( pattern1.HasWater, pattern2.HasWater );
			builder.HasHoney = TilePattern.CombineNegative( pattern1.HasHoney, pattern2.HasHoney );
			builder.HasLava = TilePattern.CombineNegative( pattern1.HasLava, pattern2.HasLava );

			if( pattern1.Shape.HasValue && !pattern2.Shape.HasValue ) {
				builder.Shape = pattern1.Shape;
			} else if( !pattern1.Shape.HasValue && pattern2.Shape.HasValue ) {
				builder.Shape = pattern2.Shape;
			} else if( pattern1.Shape.HasValue && pattern2.Shape.HasValue ) {
				builder.Shape = pattern1.Shape;
				if( pattern1.Shape.Value == TileShapeType.TopSlope ) {
					if( pattern2.Shape.Value == TileShapeType.LeftSlope ) {
						builder.Shape = TileShapeType.TopLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.RightSlope ) {
						builder.Shape = TileShapeType.TopRightSlope;
					}
				} else if( pattern1.Shape.Value == TileShapeType.BottomSlope ) {
					if( pattern2.Shape.Value == TileShapeType.LeftSlope ) {
						builder.Shape = TileShapeType.BottomLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.RightSlope ) {
						builder.Shape = TileShapeType.BottomRightSlope;
					}
				} else if( pattern1.Shape.Value == TileShapeType.LeftSlope ) {
					if( pattern2.Shape.Value == TileShapeType.TopSlope ) {
						builder.Shape = TileShapeType.TopLeftSlope;
					} else if( pattern2.Shape.Value == TileShapeType.BottomSlope ) {
						builder.Shape = TileShapeType.BottomLeftSlope;
					}
				} else {
					if( pattern2.Shape.Value == TileShapeType.TopSlope ) {
						builder.Shape = TileShapeType.TopRightSlope;
					} else if( pattern2.Shape.Value == TileShapeType.BottomSlope ) {
						builder.Shape = TileShapeType.BottomRightSlope;
					}
				}
			}

			if( pattern1.MinimumBrightness.HasValue && !pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern1.MinimumBrightness;
			} else if( !pattern1.MinimumBrightness.HasValue && pattern2.MinimumBrightness.HasValue ) {
				builder.MinimumBrightness = pattern2.MinimumBrightness;
			} else {
				if( blendLight ) {
					builder.MinimumBrightness = (pattern1.MinimumBrightness.Value + pattern2.MinimumBrightness.Value)
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MinimumBrightness.Value < pattern2.MinimumBrightness.Value ?
						pattern2.MinimumBrightness.Value :
						pattern1.MinimumBrightness.Value;
				}
			}
			if( pattern1.MaximumBrightness.HasValue && !pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern1.MaximumBrightness;
			} else if( !pattern1.MaximumBrightness.HasValue && pattern2.MaximumBrightness.HasValue ) {
				builder.MaximumBrightness = pattern2.MaximumBrightness;
			} else {
				if( blendLight ) {
					builder.MaximumBrightness = (pattern1.MaximumBrightness.Value + pattern2.MaximumBrightness.Value)
						* 0.5f;
				} else {
					builder.MinimumBrightness = pattern1.MaximumBrightness.Value < pattern2.MaximumBrightness.Value ?
						pattern2.MaximumBrightness.Value :
						pattern1.MaximumBrightness.Value;
				}
			}

			builder.IsModded = TilePattern.CombineNegative( pattern1.IsModded, pattern2.IsModded );

			if( pattern1.CustomCheck != null && pattern2.CustomCheck != null ) {
				builder.CustomCheck = ( x, y ) => pattern1.CustomCheck(x, y) && pattern2.CustomCheck(x, y);
			} else if( pattern1.CustomCheck != null ) {
				builder.CustomCheck = pattern1.CustomCheck;
			} else if( pattern2.CustomCheck != null ) {
				builder.CustomCheck = pattern2.CustomCheck;
			}

			return new TilePattern( builder );
		}


		////

		private static bool? CombinePositive( bool? a, bool? b ) {
			if( a.HasValue && b.HasValue ) {
				return a.Value || b.Value;
			} else if( a.HasValue ) {
				return a.Value;
			} else if( b.HasValue ) {
				return b.Value;
			}
			return null;
		}

		private static bool? CombineNegative( bool? a, bool? b ) {
			if( a.HasValue && b.HasValue ) {
				return a.Value && b.Value;
			} else if( a.HasValue ) {
				return a.Value;
			} else if( b.HasValue ) {
				return b.Value;
			}
			return null;
		}
	}
}

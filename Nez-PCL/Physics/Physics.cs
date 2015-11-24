﻿using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace Nez
{
	public static class Physics
	{
		static SpatialHash _spatialHash;

		public const int AllLayers = -1;
		public static bool raycastsHitTriggers = false;


		public static void reset( int cellSize = 100 )
		{
			_spatialHash = new SpatialHash( cellSize );
		}


		/// <summary>
		/// adds the collider to the physics system
		/// </summary>
		/// <param name="collider">Collider.</param>
		public static void addCollider( Collider collider )
		{
			_spatialHash.register( collider );
		}


		/// <summary>
		/// revmoes the collider from the physics system. uses brute force if useColliderBoundsForRemovalLookup is false
		/// </summary>
		/// <param name="collider">Collider.</param>
		/// <param name="useColliderBoundsForRemovalLookup">If set to <c>true</c> use collider bounds for removal lookup.</param>
		public static void removeCollider( Collider collider, bool useColliderBoundsForRemovalLookup )
		{
			if( useColliderBoundsForRemovalLookup )
			{
				var bounds = collider.bounds;
				_spatialHash.remove( collider, ref bounds );
			}
			else
			{
				_spatialHash.remove( collider );
			}
		}


		/// <summary>
		/// removes teh collider from the phyics system. bounds should be the last bounds value that the collider had in the physics system.
		/// </summary>
		/// <param name="collider">Collider.</param>
		/// <param name="bounds">Bounds.</param>
		public static void removeCollider( Collider collider, ref Rectangle bounds )
		{
			_spatialHash.remove( collider, ref bounds );
		}


		/// <summary>
		/// updates the colliders position in the physics system. preUpdateColliderBounds should be the bounds of the collider before it
		/// was changed
		/// </summary>
		/// <param name="collider">Collider.</param>
		/// <param name="colliderBounds">Collider bounds.</param>
		public static void updateCollider( Collider collider, ref Rectangle preUpdateColliderBounds )
		{
			_spatialHash.remove( collider, ref preUpdateColliderBounds );
			_spatialHash.register( collider );
		}


		// TODO: all boxcast methods should sort nearest-to-furthest
		public static HashSet<Collider> boxcast( Rectangle bounds, int layerMask = AllLayers )
		{
			return _spatialHash.boxcast( ref bounds, null, layerMask );
		}


		/// <summary>
		/// returns a HashSet of all colliders that are intersected by collider.bounds excluding the passed-in collider (self)
		/// </summary>
		/// <returns>The neighbors excluding self.</returns>
		/// <param name="collider">Collider.</param>
		public static HashSet<Collider> boxcastExcludingSelf( Collider collider, int layerMask = AllLayers )
		{
			var bounds = collider.bounds;
			return _spatialHash.boxcast( ref bounds, collider, layerMask );
		}


		/// <summary>
		/// returns a HashSet of all colliders that are intersected by bounds excluding the passed-in collider (self).
		/// this method is useful if you want to create the swept bounds on your own for other queries
		/// </summary>
		/// <returns>The excluding self.</returns>
		/// <param name="collider">Collider.</param>
		/// <param name="bounds">Bounds.</param>
		public static HashSet<Collider> boxcastExcludingSelf( Collider collider, ref Rectangle bounds, int layerMask = AllLayers )
		{
			return _spatialHash.boxcast( ref bounds, collider, layerMask );
		}


		/// <summary>
		/// returns a HashSet of all colliders that are intersected by collider.bounds expanded to incorporate deltaX/deltaY
		/// excluding the passed-in collider (self)
		/// </summary>
		/// <returns>The neighbors excluding self.</returns>
		/// <param name="collider">Collider.</param>
		public static HashSet<Collider> boxcastExcludingSelf( Collider collider, float deltaX, float deltaY, int layerMask = AllLayers )
		{
			var sweptBounds = collider.bounds.getSweptBroadphaseBounds( deltaX, deltaY );
			return _spatialHash.boxcast( ref sweptBounds, collider, layerMask );
		}


		/// <summary>
		/// casts a ray from start to end and returns the first hit of a collider that matches layerMask
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static RaycastHit raycast( Vector2 start, Vector2 end, int layerMask = AllLayers )
		{
			return _spatialHash.raycast( start, end, layerMask );
		}

	}
}

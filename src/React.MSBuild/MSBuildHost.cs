﻿/*
 *  Copyright (c) 2014, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System;

namespace React.MSBuild
{
	/// <summary>
	/// Handles initialisation of the MSBuild environment.
	/// </summary>
	internal static class MSBuildHost
	{
		/// <summary>
		/// Hack to use Lazy{T} for thread-safe, once-off initialisation :)
		/// </summary>
		private readonly static Lazy<bool> _initializer = new Lazy<bool>(Initialize); 

		/// <summary>
		/// Ensures the environment has been initialised.
		/// </summary>
		public static bool EnsureInitialized()
		{
			return _initializer.Value;
		}

		/// <summary>
		/// Actually perform the initialisation of the environment.
		/// </summary>
		/// <returns></returns>
		private static bool Initialize()
		{
			// All "per-request" registrations should be singletons in MSBuild, since there's no
			// such thing as a "request"
			Initializer.Initialize(requestLifetimeRegistration: registration => registration.AsSingleton());

			return true;
		}
	}
}

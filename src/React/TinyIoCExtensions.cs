﻿/*
 *  Copyright (c) 2014, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System;
using React.TinyIoC;

namespace React
{
	/// <summary>
	/// ReactJS.NET extensions to TinyIoC
	/// </summary>
	public static class TinyIoCExtensions
	{
		/// <summary>
		/// Gets or sets the factory used to create per-request lifetime providers
		/// </summary>
		internal static Func<TinyIoCContainer.ITinyIoCObjectLifetimeProvider> RequestLifetimeProviderFactory { private get; set; }

		/// <summary>
		/// Registers a class in IoC that uses a singleton per "request". This is generally in the
		/// context of a web request.
		/// </summary>
		/// <param name="registerOptions">Class registration options</param>
		/// <returns>The class registration (fluent interface)</returns>
		public static TinyIoCContainer.RegisterOptions AsPerRequestSingleton(this TinyIoCContainer.RegisterOptions registerOptions)
		{
			if (RequestLifetimeProviderFactory == null)
			{
				throw new Exception(
					"RequestLifetimeProviderFactory needs to be set for per-request ReactJS.NET " +
					"assembly registrations to work. Please ensure you are calling " +
					"React.Initializer.Initialize() before using any ReactJS.NET functionality."
				);
			}

			return TinyIoCContainer.RegisterOptions.ToCustomLifetimeManager(
				registerOptions,
				RequestLifetimeProviderFactory(),
				"per request singleton"
			);
		}
	}
}

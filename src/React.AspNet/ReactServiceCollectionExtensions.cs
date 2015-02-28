﻿/*
 *  Copyright (c) 2015, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using React.TinyIoC;

namespace React.AspNet
{
	public static class ReactServiceCollectionExtensions
	{
		/// <summary>
		/// Registers all services required for ReactJS.NET
		/// </summary>
		/// <param name="services">ASP.NET services</param>
		/// <returns>The service collection (for chaining)</returns>
		public static IServiceCollection AddReact(this IServiceCollection services)
		{
			services.AddScoped<HttpContextLifetimeProvider.PerRequestRegistrations>();
			return services;
		}

		/// <summary>
		/// Initialises ReactJS.NET for this application
		/// </summary>
		/// <param name="app">ASP.NET application builder</param>
		/// <param name="configure">ReactJS.NET configuration</param>
		/// <param name="fileOptions">Options to use for serving JSX files</param>
		/// <returns>The application builder (for chaining)</returns>
		public static IApplicationBuilder UseReact(
			this IApplicationBuilder app, 
			Action<IReactSiteConfiguration> configure,
			JsxFileOptions fileOptions = null
		)
		{
			// Register IApplicationEnvironment in our dependency injection container
			// Ideally this would be in AddReact(IServiceCollection) but we can't 
			// access IApplicationEnvironment there.
			React.AssemblyRegistration.Container.Register(app.ApplicationServices.GetRequiredService<IApplicationEnvironment>());

			Initializer.Initialize(registerOptions => AsPerRequestSingleton(app.ApplicationServices, registerOptions));
			configure(ReactSiteConfiguration.Configuration);

			// Allow serving of .jsx files
			app.UseMiddleware<JsxFileMiddleware>(fileOptions ?? new JsxFileOptions());

			return app;
		}

		/// <summary>
		/// Registers a class such that every ASP.NET web request has a single instance of it.
		/// </summary>
		/// <param name="appServiceProvider">ASP.NET service provider</param>
		/// <param name="registerOptions">Registration options</param>
		/// <returns>Registration options (for chaining)</returns>
		private static TinyIoCContainer.RegisterOptions AsPerRequestSingleton(
			IServiceProvider appServiceProvider, 
			TinyIoCContainer.RegisterOptions registerOptions
		)
		{
			return TinyIoCContainer.RegisterOptions.ToCustomLifetimeManager(
				registerOptions,
				new HttpContextLifetimeProvider(appServiceProvider),
				"per request singleton"
			);
		}
	}
}
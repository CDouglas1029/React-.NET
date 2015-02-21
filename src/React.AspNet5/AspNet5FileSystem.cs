﻿/*
 *  Copyright (c) 2015, Facebook, Inc.
 *  All rights reserved.
 *
 *  This source code is licensed under the BSD-style license found in the
 *  LICENSE file in the root directory of this source tree. An additional grant 
 *  of patent rights can be found in the PATENTS file in the same directory.
 */

using System.IO;
using Microsoft.Framework.Runtime;

namespace React.AspNet5
{
	/// <summary>
	/// Handles file system functionality, such as reading files. Maps all paths from 
	/// application-relative (~/...) to full paths using ASP.NET's MapPath method.
	/// </summary>
	public class AspNet5FileSystem : FileSystemBase
	{
		private const string PREFIX = "~/";
		private readonly IApplicationEnvironment _appEnvironment;

		/// <summary>
		/// Initializes a new instance of the <see cref="AspNet5FileSystem"/> class.
		/// </summary>
		/// <param name="appEnvironment">The ASP.NET 5 application environment</param>
		public AspNet5FileSystem(IApplicationEnvironment appEnvironment)
		{
			_appEnvironment = appEnvironment;
		}

		/// <summary>
		/// Converts a path from an application relative path (~/...) to a full filesystem path
		/// </summary>
		/// <param name="relativePath">App-relative path of the file</param>
		/// <returns>Full path of the file</returns>
		public override string MapPath(string relativePath)
		{
			if (relativePath.StartsWith(PREFIX))
			{
				relativePath = relativePath.Substring(PREFIX.Length);
			}
			return Path.Combine(_appEnvironment.ApplicationBasePath, relativePath);
		}
	}
}

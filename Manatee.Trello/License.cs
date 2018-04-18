﻿#region License
// Copyright (c) Newtonsoft. All Rights Reserved.
// License: https://raw.github.com/JamesNK/Newtonsoft.Json.Schema/master/LICENSE.md
// Modified for use in Manatee.Trello
#endregion

using Manatee.Trello.Internal.Licensing;

namespace Manatee.Trello

//Please visit http://please.buy/my/library to upgrade to a commercial license.
{
	/// <summary>
	/// Manages the license used with Manatee.Trello. A license can be purchased at <see href="http://please.buy/my/library">http://please.buy/my/library</see>.
	/// </summary>
	public static class License
	{
		/// <summary>
		/// Register the specified license with Manatee.Trello. A license can be purchased at <see href="http://please.buy/my/library">http://please.buy/my/library</see>.
		/// </summary>
		/// <param name="license">The license text to register.</param>
		/// <remarks> 
		/// The recommended way to register the license key is to call <see cref="RegisterLicense"/> once during application start up. In ASP.NET web applications it can be placed in the <c>Startup.cs</c> or <c>Global.asax.cs</c>, in WPF applications it can be placed in the <c>Application.Startup</c> event, and in Console applications it can be placed in the <c>static void Main(string[] args)</c> meethod.
		/// </remarks>
		/// <example> 
		/// This sample shows how to register a Manatee.Trello license with the <see cref="RegisterLicense"/> method.
		/// <code>
		/// // replace with your license key
		/// string licenseKey = "manatee-json-license-key";
		/// License.RegisterLicense(licenseKey);
		/// </code>
		/// </example>
		public static void RegisterLicense(string license)
		{
			LicenseHelpers.RegisterLicense(license);
		}
	}
}
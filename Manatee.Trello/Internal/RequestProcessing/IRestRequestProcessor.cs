﻿/***************************************************************************************

	Copyright 2012 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		IRestRequestProcessor.cs
	Namespace:		Manatee.Trello.Internal
	Class Name:		IRestRequestProcessor
	Purpose:		Processes REST requests as they appear on the queue.

***************************************************************************************/

using System;
using Manatee.Trello.Rest;

namespace Manatee.Trello.Internal.RequestProcessing
{
	/// <summary>
	/// Processes REST requests as they appear on the queue.
	/// </summary>
	/// <remarks>
	/// This interface is only exposed for unit testing purposes.
	/// </remarks>
	public interface IRestRequestProcessor
	{
		/// <summary />
		bool IsActive { get; set; }
		/// <summary />
		string AppKey { get; }
		/// <summary />
		string UserToken { get; set; }

		/// <summary />
		void AddRequest<T>(IRestRequest request)
			where T : class;
		/// <summary />
		void ShutDown();
		/// <summary />
		void NetworkStatusChanged(object sender, EventArgs e);
	}
}
/***************************************************************************************

	Copyright 2013 Little Crab Solutions

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		InnerJsonCheckItem.cs
	Namespace:		Manatee.Trello.Internal.Json
	Class Name:		InnerJsonCheckItem
	Purpose:		Internal implementation of IJsonCheckItem.

***************************************************************************************/
using Manatee.Trello.Json;

namespace Manatee.Trello.Internal.Json
{
	internal class InnerJsonCheckItem : IdentifiableJson, IJsonCheckItem
	{
		public string State { get; set; }
		public string Name { get; set; }
		public double? Pos { get; set; }
	}
}
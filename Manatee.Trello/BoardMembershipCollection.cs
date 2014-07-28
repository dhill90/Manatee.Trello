﻿/***************************************************************************************

	Copyright 2014 Greg Dennis

	   Licensed under the Apache License, Version 2.0 (the "License");
	   you may not use this file except in compliance with the License.
	   You may obtain a copy of the License at

		 http://www.apache.org/licenses/LICENSE-2.0

	   Unless required by applicable law or agreed to in writing, software
	   distributed under the License is distributed on an "AS IS" BASIS,
	   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	   See the License for the specific language governing permissions and
	   limitations under the License.
 
	File Name:		BoardMembershipCollection.cs
	Namespace:		Manatee.Trello
	Class Name:		ReadOnlyBoardMembershipCollection, BoardMembershipCollection
	Purpose:		Collection objects for board memberships.

***************************************************************************************/

using System.Collections.Generic;
using System.Linq;
using Manatee.Trello.Exceptions;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A read-only collection of board memberships.
	/// </summary>
	public class ReadOnlyBoardMembershipCollection : ReadOnlyCollection<BoardMembership>
	{
		internal ReadOnlyBoardMembershipCollection(string ownerId)
			: base(ownerId) {}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		protected override sealed void Update()
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Read_Memberships, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = JsonRepository.Execute<List<IJsonBoardMembership>>(TrelloAuthorization.Default, endpoint);

			Items.Clear();
			Items.AddRange(newData.Select(jc => TrelloConfiguration.Cache.Find<BoardMembership>(c => c.Id == jc.Id) ?? new BoardMembership(jc, OwnerId)));
		}
	}

	/// <summary>
	/// A collection of board memberships.
	/// </summary>
	public class BoardMembershipCollection : ReadOnlyBoardMembershipCollection
	{
		internal BoardMembershipCollection(string ownerId)
			: base(ownerId) {}

		/// <summary>
		/// Adds a member to a board with specified privileges.
		/// </summary>
		/// <param name="member">The member to add.</param>
		/// <param name="membership">The membership type.</param>
		public BoardMembership Add(Member member, BoardMembershipType membership)
		{
			var error = NotNullRule<Member>.Instance.Validate(null, member);
			if (error != null)
				throw new ValidationException<Member>(member, new[] { error });

			var json = TrelloConfiguration.JsonFactory.Create<IJsonBoardMembership>();
			json.Member = member.Json;
			json.MemberType = membership;

			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Write_AddOrUpdateMember, new Dictionary<string, object> {{"_id", OwnerId}, {"_memberId", member.Id}});
			var newData = JsonRepository.Execute(TrelloAuthorization.Default, endpoint, json);

			return new BoardMembership(newData, OwnerId);
		}
		/// <summary>
		/// Removes a member from a board.
		/// </summary>
		/// <param name="member">The member to remove.</param>
		public void Remove(Member member)
		{
			var error = NotNullRule<Member>.Instance.Validate(null, member);
			if (error != null)
				throw new ValidationException<Member>(member, new[] { error });

			var json = TrelloConfiguration.JsonFactory.Create<IJsonParameter>();
			json.Value = member.Id;

			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Write_RemoveMember, new Dictionary<string, object> {{"_id", OwnerId}, {"_memberId", member.Id}});
			JsonRepository.Execute(TrelloAuthorization.Default, endpoint, json);
		}
	}
}
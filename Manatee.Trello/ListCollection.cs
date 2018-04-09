﻿using System;
using System.Threading.Tasks;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of lists.
	/// </summary>
	public class ListCollection : ReadOnlyListCollection, IListCollection
	{
		internal ListCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) { }

		/// <summary>
		/// Creates a new list.
		/// </summary>
		/// <param name="name">The name of the list to add.</param>
		/// <returns>The <see cref="IList"/> generated by Trello.</returns>
		public async Task<IList> Add(string name)
		{
			var error = NotNullOrWhiteSpaceRule.Instance.Validate(null, name);
			if (error != null)
				throw new ValidationException<string>(name, new[] { error });

			var json = TrelloConfiguration.JsonFactory.Create<IJsonList>();
			json.Name = name;
			json.Board = TrelloConfiguration.JsonFactory.Create<IJsonBoard>();
			json.Board.Id = OwnerId;

			var endpoint = EndpointFactory.Build(EntityRequestType.Board_Write_AddList);
			var newData = await JsonRepository.Execute(Auth, endpoint, json);

			return new List(newData, Auth);
		}
	}
}
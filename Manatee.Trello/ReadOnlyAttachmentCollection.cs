﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A read-only collection of attachments.
	/// </summary>
	public class ReadOnlyAttachmentCollection : ReadOnlyCollection<IAttachment>
	{
		internal ReadOnlyAttachmentCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth) {}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		public sealed override async Task Refresh()
		{
			var endpoint = EndpointFactory.Build(EntityRequestType.Card_Read_Attachments, new Dictionary<string, object> {{"_id", OwnerId}});
			var newData = await JsonRepository.Execute<List<IJsonAttachment>>(Auth, endpoint);

			Items.Clear();
			Items.AddRange(newData.Select(ja =>
				{
					var attachment = TrelloConfiguration.Cache.Find<Attachment>(a => a.Id == ja.Id) ?? new Attachment(ja, OwnerId, Auth);
					attachment.Json = ja;
					return attachment;
				}));
		}
	}
}
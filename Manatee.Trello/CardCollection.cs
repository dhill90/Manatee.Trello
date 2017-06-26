﻿using System;
using System.Collections.Generic;
using System.Linq;
using Manatee.Trello.Exceptions;
using Manatee.Trello.Internal;
using Manatee.Trello.Internal.Caching;
using Manatee.Trello.Internal.DataAccess;
using Manatee.Trello.Internal.Validation;
using Manatee.Trello.Json;

namespace Manatee.Trello
{
	/// <summary>
	/// A read-only collection of cards.
	/// </summary>
	public class ReadOnlyCardCollection : ReadOnlyCollection<Card>
	{
		private readonly EntityRequestType _updateRequestType;
		private readonly Dictionary<string, object> _requestParameters;
		private Dictionary<string, object> _additionalParameters; 

		/// <summary>
		/// Retrieves a card which matches the supplied key.
		/// </summary>
		/// <param name="key">The key to match.</param>
		/// <returns>The matching card, or null if none found.</returns>
		/// <remarks>
		/// Matches on Card.Id and Card.Name.  Comparison is case-sensitive.
		/// </remarks>
		public Card this[string key] => GetByKey(key);

		internal ReadOnlyCardCollection(Type type, Func<string> getOwnerId, TrelloAuthorization auth)
			: base(getOwnerId, auth)
		{
			_updateRequestType = type == typeof (List)
				                     ? EntityRequestType.List_Read_Cards
				                     : EntityRequestType.Board_Read_Cards;
		}
		internal ReadOnlyCardCollection(EntityRequestType requestType, Func<string> getOwnerId, TrelloAuthorization auth, Dictionary<string, object> requestParameters = null)
			: base(getOwnerId, auth)
		{
			_updateRequestType = requestType;
			_requestParameters = requestParameters ?? new Dictionary<string, object>();
		}
		internal ReadOnlyCardCollection(ReadOnlyCardCollection source, TrelloAuthorization auth)
			: base(() => source.OwnerId, auth)
		{
			_updateRequestType = source._updateRequestType;
			if (source._requestParameters != null)
				_requestParameters = new Dictionary<string, object>(source._requestParameters);
			if (source._additionalParameters != null)
				_additionalParameters = new Dictionary<string, object>(source._additionalParameters);
		}

		/// <summary>
		/// Implement to provide data to the collection.
		/// </summary>
		protected sealed override void Update()
		{
			IncorporateLimit(_additionalParameters);

			_requestParameters["_id"] = OwnerId;
			var endpoint = EndpointFactory.Build(_updateRequestType, _requestParameters);
			var newData = JsonRepository.Execute<List<IJsonCard>>(Auth, endpoint, _additionalParameters);

			Items.Clear();
			Items.AddRange(newData.Select(jc =>
				{
					var card = jc.GetFromCache<Card>(Auth);
					card.Json = jc;
					return card;
				}));
		}

		internal void SetFilter(CardFilter cardStatus)
		{
			if (_additionalParameters == null)
				_additionalParameters = new Dictionary<string, object>();
			_additionalParameters["filter"] = cardStatus.GetDescription();
		}
		internal void SetFilter(DateTime? start, DateTime? end)
		{
			if (_additionalParameters == null)
				_additionalParameters = new Dictionary<string, object>();
			if (start != null)
				_additionalParameters["since"] = start;
			if (end != null)
				_additionalParameters["before"] = end;
		}

		private Card GetByKey(string key)
		{
			return this.FirstOrDefault(c => key.In(c.Id, c.Name));
		}
	}

	/// <summary>
	/// A collection of cards.
	/// </summary>
	public class CardCollection : ReadOnlyCardCollection
	{
		internal CardCollection(Func<string> getOwnerId, TrelloAuthorization auth)
			: base(typeof (List), getOwnerId, auth) {}

		/// <summary>
		/// Creates a new card.
		/// </summary>
		/// <param name="name">A name</param>
		/// <param name="description">(optional) A description</param>
		/// <param name="position">(optional) The card's position in the list.</param>
		/// <param name="dueDate">(optional) A due date.</param>
		/// <param name="isComplete">(optional) Indicates whether the card is complete.</param>
		/// <param name="members">(optional) A collection of members to add to the card.</param>
		/// <param name="labels">(optional) A collection of labels to add to the card.</param>
		/// <returns>The <see cref="Card"/> generated by Trello.</returns>
		public Card Add(string name, string description = null, Position position = null, DateTime? dueDate = null,
		                bool? isComplete = null, IEnumerable<Member> members = null, IEnumerable<Label> labels = null)
		{
			var error = NotNullOrWhiteSpaceRule.Instance.Validate(null, name);
			if (error != null)
				throw new ValidationException<string>(name, new[] {error});

			var json = TrelloConfiguration.JsonFactory.Create<IJsonCard>();
			json.Name = name;
			json.Desc = description;
			json.Pos = position == null ? null : Position.GetJson(position);
			json.Due = dueDate;
			json.DueComplete = isComplete;
			json.IdMembers = members?.Select(m => m.Id).Join(",");
			json.IdLabels = labels?.Select(l => l.Id).Join(",");

			return CreateCard(json);
		}
		/// <summary>
		/// Creates a new card by copying a card.
		/// </summary>
		/// <param name="source">A card to copy.  Default is null.</param>
		/// <returns>The <see cref="Card"/> generated by Trello.</returns>
		public Card Add(Card source)
		{
			var error = NotNullRule<Card>.Instance.Validate(null, source);
			if (error != null)
				throw new ValidationException<Card>(source, new[] {error});

			var json = TrelloConfiguration.JsonFactory.Create<IJsonCard>();
			json.CardSource = source.Json;

			return CreateCard(json);
		}
		/// <summary>
		/// Creates a new card by importing data from a URL.
		/// </summary>
		/// <param name="name">The name of the card to add.</param>
		/// <param name="sourceUrl"></param>
		/// <returns></returns>
		public Card Add(string name, string sourceUrl)
		{
			var error = NotNullOrWhiteSpaceRule.Instance.Validate(null, name);
			if (error != null)
				throw new ValidationException<string>(name, new[] { error });
			error = UriRule.Instance.Validate(null, sourceUrl);
			if (error != null)
				throw new ValidationException<string>(sourceUrl, new[] { error });

			var json = TrelloConfiguration.JsonFactory.Create<IJsonCard>();
			json.Name = name;
			json.UrlSource = sourceUrl;

			return CreateCard(json);
		}

		private Card CreateCard(IJsonCard json)
		{
			json.List = TrelloConfiguration.JsonFactory.Create<IJsonList>();
			json.List.Id = OwnerId;

			var endpoint = EndpointFactory.Build(EntityRequestType.List_Write_AddCard);
			var newData = JsonRepository.Execute(Auth, endpoint, json);

			return new Card(newData, Auth);
		}
	}
}
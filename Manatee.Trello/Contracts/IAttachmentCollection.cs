﻿using System.Threading.Tasks;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of attachments.
	/// </summary>
	public interface IAttachmentCollection : IReadOnlyCollection<IAttachment>
	{
		/// <summary>
		/// Adds an attachment to a card using the URL of the attachment.
		/// </summary>
		/// <param name="url">A URL to the data to attach.  Must be a valid URL that begins with 'http://' or 'https://'.</param>
		/// <param name="name">An optional name for the attachment.  The linked file name will be used by default if not specified.</param>
		/// <returns>The attachment generated by Trello.</returns>
		Task<IAttachment> Add(string url, string name = null);

		/// <summary>
		/// Adds an attachment to a card by uploading data.
		/// </summary>
		/// <param name="data">The byte data of the file to attach.</param>
		/// <param name="name">A name for the attachment.</param>
		/// <returns>The attachment generated by Trello.</returns>
		Task<IAttachment> Add(byte[] data, string name);
	}
}
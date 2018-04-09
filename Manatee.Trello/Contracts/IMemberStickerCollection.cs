﻿using System.Threading.Tasks;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of <see cref="ISticker"/>s.
	/// </summary>
	public interface IMemberStickerCollection
	{
		/// <summary>
		/// Adds a <see cref="ISticker"/> to a <see cref="IMember"/>'s custom sticker set by uploading data.
		/// </summary>
		/// <param name="data">The byte data of the file to attach.</param>
		/// <param name="name">A name for the attachment.</param>
		/// <returns>The attachment generated by Trello.</returns>
		Task<ISticker> Add(byte[] data, string name);
	}
}
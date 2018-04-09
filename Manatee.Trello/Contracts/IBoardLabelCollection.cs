﻿using System.Threading.Tasks;

namespace Manatee.Trello
{
	/// <summary>
	/// A collection of labels for boards.
	/// </summary>
	public interface IBoardLabelCollection : IReadOnlyCollection<ILabel>
	{
		/// <summary>
		/// Adds a label to the collection.
		/// </summary>
		/// <param name="name">The name of the label.</param>
		/// <param name="color">The color of the label to add.</param>
		/// <returns>The <see cref="Label"/> generated by Trello.</returns>
		Task<ILabel> Add(string name, LabelColor? color);

		/// <summary>
		/// Adds a filter to the collection.
		/// </summary>
		/// <param name="labelColor">The filter value.</param>
		void Filter(LabelColor labelColor);
	}
}
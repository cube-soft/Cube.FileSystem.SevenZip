/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
/* ------------------------------------------------------------------------- */
using Cube.Collections;
using System.Collections.Generic;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveList
    ///
    /// <summary>
    /// Represents the collection of items in the provided archive.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal class ArchiveList : EnumerableBase<ArchiveItem>, IReadOnlyList<ArchiveItem>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveList
        ///
        /// <summary>
        /// Initializes a new instance of the ArchiveList class with the
        /// specified controller.
        /// </summary>
        ///
        /// <param name="controller">Controller object.</param>
        /// <param name="count">Number of items.</param>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveList(ArchiveReaderController controller, int count)
        {
            _controller = controller;
            Count = count;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Count
        ///
        /// <summary>
        /// Gets the number of items.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public int Count { get; }

        /* ----------------------------------------------------------------- */
        ///
        /// Item
        ///
        /// <summary>
        /// Gets the element at the specified index
        /// </summary>
        ///
        /// <param name="index">Index of the element to get.</param>
        ///
        /// <returns>ArchiveItem object.</returns>
        ///
        /* ----------------------------------------------------------------- */
        public ArchiveItem this[int index] => new ArchiveItem(_controller, index);

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// GetEnumerator
        ///
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        ///
        /// <returns>
        /// Enumerator that can be used to iterate through the collection.
        /// </returns>
        ///
        /* ----------------------------------------------------------------- */
        public override IEnumerator<ArchiveItem> GetEnumerator()
        {
            for (var i = 0; i < Count; ++i) yield return this[i];
        }

        #endregion

        #region Fields
        private readonly ArchiveReaderController _controller;
        #endregion
    }
}

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
using Cube.Mixin.String;
using System.Diagnostics;

namespace Cube.FileSystem.SevenZip
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchivePasswordCallback
    ///
    /// <summary>
    /// Provides functionality to query the password when extracting
    /// archive files.
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    internal abstract class ArchivePasswordCallback : ArchiveCallbackBase, ICryptoGetTextPassword
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchivePasswordCallback
        ///
        /// <summary>
        /// Initializes a new instance of the ArchivePasswordCallback class
        /// with the specified arguments.
        /// </summary>
        ///
        /// <param name="src">Path of the archive file.</param>
        /// <param name="io">I/O handler.</param>
        ///
        /* ----------------------------------------------------------------- */
        protected ArchivePasswordCallback(string src, IO io) : base(io)
        {
            Source = src;
        }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// Source
        ///
        /// <summary>
        /// Gets the path of the archive file.
        /// </summary>
        ///
        /* ----------------------------------------------------------------- */
        public string Source { get; }

        #endregion

        #region Methods

        /* ----------------------------------------------------------------- */
        ///
        /// CryptoGetTextPassword
        ///
        /// <summary>
        /// Gets the password of the provided archive.
        /// </summary>
        ///
        /// <param name="password">Password result.</param>
        ///
        /// <returns>OperationResult</returns>
        ///
        /* ----------------------------------------------------------------- */
        public int CryptoGetTextPassword(out string password)
        {
            Debug.Assert(Password != null);

            var e = Query.NewMessage(Source);
            Password.Request(e);

            var ok = !e.Cancel && e.Value.HasValue();
            Result = e.Cancel ? OperationResult.UserCancel :
                     ok       ? OperationResult.OK :
                                OperationResult.WrongPassword;
            password = ok ? e.Value : string.Empty;

            return (int)Result;
        }

        #endregion
    }
}

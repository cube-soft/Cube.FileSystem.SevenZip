﻿/* ------------------------------------------------------------------------- */
///
/// Copyright (c) 2010 CubeSoft, Inc.
/// 
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
///
///  http://www.apache.org/licenses/LICENSE-2.0
///
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
///
/* ------------------------------------------------------------------------- */
using Cube.FileSystem.Ice;

namespace Cube.FileSystem.App.Ice.Settings
{
    /* --------------------------------------------------------------------- */
    ///
    /// ArchiveSettingsViewModel
    /// 
    /// <summary>
    /// ArchiveSettings の ViewModel を表すクラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public class ArchiveSettingsViewModel : GeneralSettingsViewModel<ArchiveSettings>
    {
        #region Constructors

        /* ----------------------------------------------------------------- */
        ///
        /// ArchiveSettingsViewModel
        /// 
        /// <summary>
        /// オブジェクトを初期化します。
        /// </summary>
        /// 
        /// <param name="model">Model オブジェクト</param>
        /// 
        /* ----------------------------------------------------------------- */
        public ArchiveSettingsViewModel(ArchiveSettings model) : base(model) { }

        #endregion

        #region Properties

        /* ----------------------------------------------------------------- */
        ///
        /// UseUtf8
        /// 
        /// <summary>
        /// ファイル名を UTF-8 に変換するかどうかを示す値を取得または
        /// 設定します。
        /// </summary>
        /// 
        /* ----------------------------------------------------------------- */
        public bool UseUtf8
        {
            get { return Model.UseUtf8; }
            set { Model.UseUtf8 = value; }
        }

        #endregion
    }
}
﻿/* ------------------------------------------------------------------------- */
//
// Copyright (c) 2010 CubeSoft, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
/* ------------------------------------------------------------------------- */
using Cube.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cube.FileSystem.SevenZip.Ice
{
    /* --------------------------------------------------------------------- */
    ///
    /// PresetMenuExtension
    ///
    /// <summary>
    /// PresetMenu の拡張用クラスです。
    /// </summary>
    ///
    /* --------------------------------------------------------------------- */
    public static class PresetMenuExtension
    {
        /* --------------------------------------------------------------------- */
        ///
        /// ToContextMenuGroup
        ///
        /// <summary>
        /// PresetMenu を表す ContextMenu オブジェクト一覧を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>ContextMenu コレクション</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<ContextMenu> ToContextMenuGroup(this PresetMenu src)
        {
            var dest = new List<ContextMenu>();
            Add(src, PresetMenu.Archive, ArchiveMenu, dest);
            Add(src, PresetMenu.Extract, ExtractMenu, dest);
            Add(src, PresetMenu.Mail,    MailMenu,    dest);
            return dest;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToContextMenu
        ///
        /// <summary>
        /// PresetMenu を表す ContextMenu オブジェクトを取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>ContextMenu オブジェクト</returns>
        ///
        /// <remarks>
        /// ToContextMenu メソッドは、指定された PresetMenu オブジェクトが複数の
        /// メニューを表している場合、最初に合致したメニューに対応する
        /// ContextMenu オブジェクトを返します。全てのメニューに合致する
        /// ContextMenu オブジェクトのコレクションを取得する場合は
        /// ToContextMenuGroup メソッドを使用して下さい。
        /// </remarks>
        ///
        /* --------------------------------------------------------------------- */
        public static ContextMenu ToContextMenu(this PresetMenu src) => new ContextMenu
        {
            Name         = ToName(src),
            Arguments    = string.Join(" ", ToArguments(src)),
            IconIndex    = ToIconIndex(src),
        };

        /* --------------------------------------------------------------------- */
        ///
        /// ToName
        ///
        /// <summary>
        /// PresetMenu に対応する名前を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>名前</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static string ToName(this PresetMenu src)
        {
            if ((src & PresetMenu.ArchiveOptions) != 0) return Find(src, ArchiveNames);
            if ((src & PresetMenu.ExtractOptions) != 0) return Find(src, ExtractNames);
            if ((src & PresetMenu.MailOptions)    != 0) return Find(src, MailNames);
            if ((src & PresetMenu.Archive)        != 0) return Properties.Resources.CtxArchive;
            if ((src & PresetMenu.Extract)        != 0) return Properties.Resources.CtxExtract;
            if ((src & PresetMenu.Mail)           != 0) return Properties.Resources.CtxMail;
            return string.Empty;
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToArguments
        ///
        /// <summary>
        /// PresetMenu に対応するプログラム引数を取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>プログラム引数</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static IEnumerable<string> ToArguments(this PresetMenu src)
        {
            if ((src & PresetMenu.ArchiveOptions) != 0) return Find(src, ArchiveArguments);
            if ((src & PresetMenu.ExtractOptions) != 0) return Find(src, ExtractArguments);
            if ((src & PresetMenu.MailOptions)    != 0) return Find(src, MailArguments);
            if ((src & PresetMenu.Archive)        != 0) return Find(PresetMenu.ArchiveZip, ArchiveArguments);
            if ((src & PresetMenu.Extract)        != 0) return new[] { "/x" };
            if ((src & PresetMenu.Mail)           != 0) return Find(PresetMenu.MailZip, MailArguments);
            return new string[0];
        }

        /* --------------------------------------------------------------------- */
        ///
        /// ToIconLocation
        ///
        /// <summary>
        /// PresetMenu に対応するアイコンのインデックスを取得します。
        /// </summary>
        ///
        /// <param name="src">PresetMenu オブジェクト</param>
        ///
        /// <returns>アイコンのインデックス</returns>
        ///
        /* --------------------------------------------------------------------- */
        public static int ToIconIndex(this PresetMenu src)
        {
            var m0 = PresetMenu.Archive | PresetMenu.ArchiveOptions;
            if ((src & m0) != 0) return 1;
            var m1 = PresetMenu.Extract | PresetMenu.ExtractOptions;
            if ((src & m1) != 0) return 2;
            var m2 = PresetMenu.Mail | PresetMenu.MailOptions;
            if ((src & m2) != 0) return 1;

            return 0;
        }

        #region Implementations

        /* --------------------------------------------------------------------- */
        ///
        /// Find
        ///
        /// <summary>
        /// メニューに対応する値を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static T Find<T>(PresetMenu src, IDictionary<PresetMenu, T> cmp) =>
            cmp.FirstOrDefault(e => src.HasFlag(e.Key)).Value;

        /* --------------------------------------------------------------------- */
        ///
        /// Add
        ///
        /// <summary>
        /// PresetMenu を解析し、必要な ContextMenu オブジェクトを追加します。
        /// </summary>
        ///
        /// <param name="src">変換元オブジェクト</param>
        /// <param name="category">メニューのカテゴリ</param>
        /// <param name="cmp">変換対象となるメニュー一覧</param>
        /// <param name="dest">結果を格納するコレクション</param>
        ///
        /* --------------------------------------------------------------------- */
        private static void Add(PresetMenu src, PresetMenu category,
            IDictionary<PresetMenu, ContextMenu> cmp, ICollection<ContextMenu> dest)
        {
            if (!src.HasFlag(category)) return;

            var root = ToContextMenu(category);
            foreach (var kv in cmp)
            {
                if (src.HasFlag(kv.Key)) root.Children.Add(kv.Value);
            }
            if (root.Children.Count > 0) dest.Add(root);
        }

        #region Name

        /* --------------------------------------------------------------------- */
        ///
        /// ArchiveNames
        ///
        /// <summary>
        /// 圧縮に関連するメニューと名前の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, string> ArchiveNames { get; } =
            new Dictionary<PresetMenu, string>
            {
                { PresetMenu.ArchiveZip,         Properties.Resources.CtxZip         },
                { PresetMenu.ArchiveZipPassword, Properties.Resources.CtxZipPassword },
                { PresetMenu.ArchiveSevenZip,    Properties.Resources.CtxSevenZip    },
                { PresetMenu.ArchiveBZip2,       Properties.Resources.CtxBZip2       },
                { PresetMenu.ArchiveGZip,        Properties.Resources.CtxGZip        },
                { PresetMenu.ArchiveXz,          Properties.Resources.CtxXz          },
                { PresetMenu.ArchiveSfx,         Properties.Resources.CtxSfx         },
                { PresetMenu.ArchiveDetails,     Properties.Resources.CtxDetails     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailNames
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューと名前の対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, string> MailNames { get; } =
            new Dictionary<PresetMenu, string>
            {
                { PresetMenu.MailZip,         Properties.Resources.CtxZip         },
                { PresetMenu.MailZipPassword, Properties.Resources.CtxZipPassword },
                { PresetMenu.MailSevenZip,    Properties.Resources.CtxSevenZip    },
                { PresetMenu.MailBZip2,       Properties.Resources.CtxBZip2       },
                { PresetMenu.MailGZip,        Properties.Resources.CtxGZip        },
                { PresetMenu.MailXz,          Properties.Resources.CtxXz          },
                { PresetMenu.MailSfx,         Properties.Resources.CtxSfx         },
                { PresetMenu.MailDetails,     Properties.Resources.CtxDetails     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractNames
        ///
        /// <summary>
        /// 解凍に関連するメニューと名前の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, string> ExtractNames { get; } =
            new Dictionary<PresetMenu, string>
            {
                { PresetMenu.ExtractSource,      Properties.Resources.CtxSource      },
                { PresetMenu.ExtractDesktop,     Properties.Resources.CtxDesktop     },
                { PresetMenu.ExtractMyDocuments, Properties.Resources.CtxMyDocuments },
                { PresetMenu.ExtractRuntime,     Properties.Resources.CtxRuntime     },
            };

        #endregion

        #region Arguments

        /* --------------------------------------------------------------------- */
        ///
        /// ArchiveArguments
        ///
        /// <summary>
        /// 圧縮に関連するメニューとプログラム引数の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, IEnumerable<string>> ArchiveArguments { get; } =
            new Dictionary<PresetMenu, IEnumerable<string>>
            {
                { PresetMenu.ArchiveZip,         new[] { "/c:zip" }       },
                { PresetMenu.ArchiveZipPassword, new[] { "/c:zip", "/p" } },
                { PresetMenu.ArchiveSevenZip,    new[] { "/c:7z" }        },
                { PresetMenu.ArchiveBZip2,       new[] { "/c:bzip2" }     },
                { PresetMenu.ArchiveGZip,        new[] { "/c:gzip" }      },
                { PresetMenu.ArchiveXz,          new[] { "/c:xz" }        },
                { PresetMenu.ArchiveSfx,         new[] { "/c:exe" }       },
                { PresetMenu.ArchiveDetails,     new[] { "/c:detail" }    },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailArguments
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューとプログラム引数の対応関係
        /// 一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, IEnumerable<string>> MailArguments { get; } =
            new Dictionary<PresetMenu, IEnumerable<string>>
            {
                { PresetMenu.MailZip,         new[] { "/c:zip", "/m" }       },
                { PresetMenu.MailZipPassword, new[] { "/c:zip", "/p", "/m" } },
                { PresetMenu.MailSevenZip,    new[] { "/c:7z", "/m" }        },
                { PresetMenu.MailBZip2,       new[] { "/c:bzip2", "/m" }     },
                { PresetMenu.MailGZip,        new[] { "/c:gzip", "/m" }      },
                { PresetMenu.MailXz,          new[] { "/c:xz", "/m" }        },
                { PresetMenu.MailSfx,         new[] { "/c:exe", "/m" }       },
                { PresetMenu.MailDetails,     new[] { "/c:detail", "/m" }    },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractArguments
        ///
        /// <summary>
        /// 解凍に関連するメニューとプログラム引数の対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, IEnumerable<string>> ExtractArguments { get; } =
            new Dictionary<PresetMenu, IEnumerable<string>>
            {
                { PresetMenu.ExtractSource,      new[] { "/x", "/out:source" }      },
                { PresetMenu.ExtractDesktop,     new[] { "/x", "/out:desktop" }     },
                { PresetMenu.ExtractMyDocuments, new[] { "/x", "/out:mydocuments" } },
                { PresetMenu.ExtractRuntime,     new[] { "/x", "/out:runtime" }     },
            };

        #endregion

        #region ContextMenu

        /* --------------------------------------------------------------------- */
        ///
        /// ArchiveMenu
        ///
        /// <summary>
        /// 圧縮に関連するメニューと ContextMenu オブジェクトの対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, ContextMenu> ArchiveMenu { get; } =
            new OrderedDictionary<PresetMenu, ContextMenu>
            {
                { PresetMenu.ArchiveZip,         ToContextMenu(PresetMenu.ArchiveZip)         },
                { PresetMenu.ArchiveZipPassword, ToContextMenu(PresetMenu.ArchiveZipPassword) },
                { PresetMenu.ArchiveSevenZip,    ToContextMenu(PresetMenu.ArchiveSevenZip)    },
                { PresetMenu.ArchiveBZip2,       ToContextMenu(PresetMenu.ArchiveBZip2)       },
                { PresetMenu.ArchiveGZip,        ToContextMenu(PresetMenu.ArchiveGZip)        },
                { PresetMenu.ArchiveXz,          ToContextMenu(PresetMenu.ArchiveXz)          },
                { PresetMenu.ArchiveSfx,         ToContextMenu(PresetMenu.ArchiveSfx)         },
                { PresetMenu.ArchiveDetails,     ToContextMenu(PresetMenu.ArchiveDetails)     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// MailMenu
        ///
        /// <summary>
        /// 圧縮してメール送信に関連するメニューと ContextMenu オブジェクトの
        /// 対応関係一覧を取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, ContextMenu> MailMenu { get; } =
            new OrderedDictionary<PresetMenu, ContextMenu>
            {
                { PresetMenu.MailZip,         ToContextMenu(PresetMenu.MailZip)         },
                { PresetMenu.MailZipPassword, ToContextMenu(PresetMenu.MailZipPassword) },
                { PresetMenu.MailSevenZip,    ToContextMenu(PresetMenu.MailSevenZip)    },
                { PresetMenu.MailBZip2,       ToContextMenu(PresetMenu.MailBZip2)       },
                { PresetMenu.MailGZip,        ToContextMenu(PresetMenu.MailGZip)        },
                { PresetMenu.MailXz,          ToContextMenu(PresetMenu.MailXz)          },
                { PresetMenu.MailSfx,         ToContextMenu(PresetMenu.MailSfx)         },
                { PresetMenu.MailDetails,     ToContextMenu(PresetMenu.MailDetails)     },
            };

        /* --------------------------------------------------------------------- */
        ///
        /// ExtractMenu
        ///
        /// <summary>
        /// 解凍に関連するメニューと ContextMenu オブジェクトの対応関係一覧を
        /// 取得します。
        /// </summary>
        ///
        /* --------------------------------------------------------------------- */
        private static IDictionary<PresetMenu, ContextMenu> ExtractMenu { get; } =
            new OrderedDictionary<PresetMenu, ContextMenu>
            {
                { PresetMenu.ExtractSource,      ToContextMenu(PresetMenu.ExtractSource) },
                { PresetMenu.ExtractDesktop,     ToContextMenu(PresetMenu.ExtractDesktop) },
                { PresetMenu.ExtractMyDocuments, ToContextMenu(PresetMenu.ExtractMyDocuments) },
                { PresetMenu.ExtractRuntime,     ToContextMenu(PresetMenu.ExtractRuntime) },
            };

        #endregion

        #endregion
    }
}

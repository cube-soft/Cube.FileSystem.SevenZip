#include "ContextMenuIcon.h"

namespace Cube {
namespace FileSystem {
namespace Ice {

/* ------------------------------------------------------------------------- */
///
/// ArgbContextMenuIcon
/// 
/// <summary>
/// �I�u�W�F�N�g�����������܂��B
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::ArgbContextMenuIcon()
    : ux_(NULL), map_() {
    ux_ = LoadLibrary(_T("UXTHEME.DLL"));
}

/* ------------------------------------------------------------------------- */
///
/// ~ArgbContextMenuIcon
/// 
/// <summary>
/// �I�u�W�F�N�g��j�����܂��B
/// </summary>
///
/* ------------------------------------------------------------------------- */
ArgbContextMenuIcon::~ArgbContextMenuIcon() {
    for (auto& kv : map_) {
        if (kv.second) DeleteObject(kv.second);
    }
    map_.clear();
    if (ux_ != NULL) FreeLibrary(ux_);
    ux_ = NULL;
}

/* ------------------------------------------------------------------------- */
///
/// SetMenuIcon
/// 
/// <summary>
/// �A�C�R����ݒ肵�܂��B
/// </summary>
///
/// <param name="src">�A�C�R���p�t�@�C���̃p�X</param>
/// <param name="dest">���j���[���</param>
///
/* ------------------------------------------------------------------------- */
void ArgbContextMenuIcon::SetMenuIcon(const ArgbContextMenuIcon::TString& src, MENUITEMINFO& dest) {

}

}}}

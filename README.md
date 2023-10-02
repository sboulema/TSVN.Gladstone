# TSVN.Gladstone
A out-of-proc rewrite of [TSVN](https://github.com/sboulema/TSVN) using [VisualStudio.Extensibility](https://github.com/microsoft/VSExtensibility). 

# TSVN
Control TortoiseSVN from within Visual Studio

[![TSVN](https://github.com/sboulema/TSVN.Gladstone/actions/workflows/workflow.yml/badge.svg)](https://github.com/sboulema/TSVN.Gladstone/actions/workflows/workflow.yml)
[![Sponsor](https://img.shields.io/badge/-Sponsor-fafbfc?logo=GitHub%20Sponsors)](https://github.com/sponsors/sboulema)

# Getting started
1. Install [TortoiseSVN](http://www.tortoisesvn.net) with "command line client tools"
	`winget install --id TortoiseSVN.TortoiseSVN --interactive`
2. ~~Install the [TSVN](https://marketplace.visualstudio.com/items?itemName=SamirBoulema.TSVN) extension~~
3. Enjoy! 

## Installing
- No public build yet

# Features
All the SVN functions quickly accessible from the TSVN menu:

![Main Menu](https://raw.githubusercontent.com/sboulema/TSVN/main/Resources/Screenshots/TSVN-main.png)

When working on a single file you can find all SVN functions related to that file in the context menu:

![Context Menu](https://raw.githubusercontent.com/sboulema/TSVN/main/Resources/Screenshots/tsvn-context.png)

A customizable toolbar with all your favorite commands:

![Toolbar](https://raw.githubusercontent.com/sboulema/TSVN/main/Resources/Screenshots/tsvn-toolbar.png)

A pending changes window with the most important commands:

![Pending Changes](https://raw.githubusercontent.com/sboulema/TSVN/main/Resources/Screenshots/pendingchanges.png)

## Keyboard shortcuts

* `(S)VN (C)ommit` - Ctrl+Shift+Alt+S, C
* `(S)VN Sho(w) Changes` - Ctrl+Shift+Alt+S, W
* `(S)VN (U)pdate` - Ctrl+Shift+Alt+S, U
* `(S)VN (L)og` - Ctrl+Shift+Alt+S, L
* `(S)VN Create P(a)tch` - Ctrl+Shift+Alt+S, A
* `(S)VN Apply Patc(h)` - Ctrl+Shift+Alt+S, H
* `(S)VN (S)witch` - Ctrl+Shift+Alt+S, S
* `(S)VN (M)erge` - Ctrl+Shift+Alt+S, M
* `(S)VN (R)evert` - Ctrl+Shift+Alt+S, R
* `(S)VN Clea(n)up` - Ctrl+Shift+Alt+S, N
* `(S)VN File (B)lame` - Ctrl+Shift+Alt+S, B
* `(S)VN File (D)iff` - Ctrl+Shift+Alt+S, D
* `(S)VN Sh(e)lve` - Ctrl+Shift+Alt+S, E
* `(S)VN Unshelve` - Ctrl+Shift+Alt+S, P
* `(S)VN L(o)ck` - Ctrl+Shift+Alt+S, O
* `(S)VN Unloc(k)` - Ctrl+Shift+Alt+S, K
[简体中文](README.md) | [English](README-EN.md) | **繁體中文**

<div align="center">

<img src="Plain Craft Launcher 2/Images/icon.ico" alt="Logo" width="80" height="80">

# PCL Community Edition

[![Stars](https://img.shields.io/github/stars/RainYooSuki/PCL-CE?style=for-the-badge&logo=data:image/svg%2bxml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZlcnNpb249IjEiIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiI+PHBhdGggZD0iTTggLjI1YS43NS43NSAwIDAgMSAuNjczLjQxOGwxLjg4MiAzLjgxNSA0LjIxLjYxMmEuNzUuNzUgMCAwIDEgLjQxNiAxLjI3OWwtMy4wNDYgMi45Ny43MTkgNC4xOTJhLjc1MS43NTEgMCAwIDEtMS4wODguNzkxTDggMTIuMzQ3bC0zLjc2NiAxLjk4YS43NS43NSAwIDAgMS0xLjA4OC0uNzlsLjcyLTQuMTk0TC44MTggNi4zNzRhLjc1Ljc1IDAgMCAxIC40MTYtMS4yOGw0LjIxLS42MTFMNy4zMjcuNjY4QS43NS43NSAwIDAgMSA4IC4yNVoiIGZpbGw9IiNlYWM1NGYiLz48L3N2Zz4=&logoSize=auto&label=stars&labelColor=444444&color=eac54f)](https://github.com/RainYooSuki/PCL-CE/)
![GitHub Release](https://img.shields.io/github/v/release/RainYooSuki/PCL-CE?label=release&logo=github&style=for-the-badge)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/RainYooSuki/PCL-CE/auto-release.yml?style=for-the-badge)

[![Issues](https://img.shields.io/github/issues/RainYooSuki/PCL-CE?style=for-the-badge&label=issues&labelColor=444444&color=1F883D&logo=github)](https://github.com/RainYooSuki/PCL-CE/issues)
[![Pull requests](https://img.shields.io/github/issues-pr/RainYooSuki/PCL-CE?style=for-the-badge&label=pull%20requests&labelColor=444444&color=1F883D&logo=github)](https://github.com/RainYooSuki/PCL-CE/pulls)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RainYooSuki/PCL-CE/total?style=for-the-badge)
[![嗶哩嗶哩](https://img.shields.io/badge/貼文-bilibili-00A4DB?style=for-the-badge&labelColor=444444&logo=bilibili)](https://space.bilibili.com/3546847192811755/dynamic) <br />

[社群版下載](https://github.com/RainYooSuki/PCL-CE/releases/latest) |
[上游儲存庫](https://github.com/Meloong-Git/PCL)

[提交問題](https://github.com/RainYooSuki/PCL-CE/issues/new/choose) |
[貢獻指南](https://github.com/RainYooSuki/PCL-CE/wiki/开发指南)

</div>

## 📖 專案簡介

PCL CE（Plain Craft Launcher Community Edition）是一款面向 Minecraft Java 版的社群啟動器，基於 PCL 開放原始碼持續開發。專案在保留 PCL 輕量、易用特點的基礎上，圍繞啟動穩定性、執行效能、相容性與介面體驗進行重構與增強，並持續加入新的功能。

目前儲存庫已從舊版 PCL CE 獨立演進，並非 PCL 官方專案，也不等同於舊版 PCL CE。版本號、功能範圍與更新節奏均可能與舊版 PCL CE 或 PCL 主線不同。

歡迎透過 Issues 提交問題與建議。請注意：目前版本的問題請回報至[本儲存庫](https://github.com/RainYooSuki/PCL-CE/issues)，不要回報到舊版 PCL CE 或 PCL 官方儲存庫。

歡迎下載並體驗 PCL CE！

## 💻 支援平台

| 作業系統 | 支援情況 | 環境要求 |
|---|---|---|
| Windows 10 1809 (17763) 或更高 | ✅ 完整支援 | [.NET 10 Desktop Runtime](https://get.dot.net/10) |
| Windows 8 - Windows 10 1809 (17763) | ⚠️ 理論能跑，酌情提供社群支援 | [.NET 10 Desktop Runtime](https://get.dot.net/10) |
| Windows 7 或更低版本 | ❌ 不支援 | / |
| macOS / Linux / 其他作業系統 | ⚠️ 僅跨平台開發支援（交叉編譯）| [.NET 10 SDK](https://get.dot.net/10) |

**✅ 完整支援**：盡可能提供一切相關支援，但必須確保啟動器為最新版本。

**⚠️ 理論能跑，酌情提供社群支援**：PCL CE 應該可以在這些平台上執行，但不保證功能完全可用。你可能需要升級到完整支援的系統版本以獲得進一步社群技術支援。

**❌ 不支援**：PCL CE 在這些平台的可用性較低，甚至根本打不開。請升級作業系統以使用 PCL CE。

**⚠️ 僅跨平台開發支援（交叉編譯）**：PCL CE 的原始碼可以在 macOS 與 Linux 平台編譯，但無法直接執行。作為開發者，你可以在這些平台上進行開發，然後將編譯產物轉移到 Windows 系統測試。

**注**：
社群僅對最新版本的啟動器提供支援。
取決於部分問題的特殊性（如系統不完整），有時你仍然必須升級作業系統以繼續獲得支援。
PCL CE 始終建議使用最新版本的作業系統以獲得最佳體驗。
你仍然可以嘗試在不受支援的系統上使用 PCL CE，但可能會遇到很多額外問題。

## 🔒 授權條款

- `Plain Craft Launcher 2/` 使用 [自訂授權條款](https://github.com/RainYooSuki/PCL-CE/blob/dev/Plain%20Craft%20Launcher%202/LICENCE)
- `其餘所有目錄` 使用 [Apache License 2.0](https://github.com/RainYooSuki/PCL-CE/blob/dev/LICENSE)

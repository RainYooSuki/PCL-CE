**简体中文** | [English](README-EN.md) | [繁體中文](README-ZH_TW.md)

<div align="center">

<img src="Plain Craft Launcher 2/Images/icon.ico" alt="Logo" width="80" height="80">

# PCL Community Edition

[![Stars](https://img.shields.io/github/stars/RainYooSuki/PCL-CE?style=for-the-badge&logo=data:image/svg%2bxml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIHZlcnNpb249IjEiIHdpZHRoPSIxNiIgaGVpZ2h0PSIxNiI+PHBhdGggZD0iTTggLjI1YS43NS43NSAwIDAgMSAuNjczLjQxOGwxLjg4MiAzLjgxNSA0LjIxLjYxMmEuNzUuNzUgMCAwIDEgLjQxNiAxLjI3OWwtMy4wNDYgMi45Ny43MTkgNC4xOTJhLjc1MS43NTEgMCAwIDEtMS4wODguNzkxTDggMTIuMzQ3bC0zLjc2NiAxLjk4YS43NS43NSAwIDAgMS0xLjA4OC0uNzlsLjcyLTQuMTk0TC44MTggNi4zNzRhLjc1Ljc1IDAgMCAxIC40MTYtMS4yOGw0LjIxLS42MTFMNy4zMjcuNjY4QS43NS43NSAwIDAgMSA4IC4yNVoiIGZpbGw9IiNlYWM1NGYiLz48L3N2Zz4=&logoSize=auto&label=stars&labelColor=444444&color=eac54f)](https://github.com/RainYooSuki/PCL-CE/)
![GitHub Release](https://img.shields.io/github/v/release/RainYooSuki/PCL-CE?label=release&logo=github&style=for-the-badge)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/RainYooSuki/PCL-CE/auto-release.yml?style=for-the-badge)

[![Issues](https://img.shields.io/github/issues/RainYooSuki/PCL-CE?style=for-the-badge&label=issues&labelColor=444444&color=1F883D&logo=github)](https://github.com/RainYooSuki/PCL-CE/issues)
[![Pull requests](https://img.shields.io/github/issues-pr/RainYooSuki/PCL-CE?style=for-the-badge&label=pull%20requests&labelColor=444444&color=1F883D&logo=github)](https://github.com/RainYooSuki/PCL-CE/pulls)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/RainYooSuki/PCL-CE/total?style=for-the-badge)

[社区版下载](https://github.com/RainYooSuki/PCL-CE/releases/latest) |
[上游存储库](https://github.com/Meloong-Git/PCL)

[提交问题](https://github.com/RainYooSuki/PCL-CE/issues/new/choose) |
[贡献指南](https://github.com/RainYooSuki/PCL-CE/wiki/开发指南)

</div>

## 📖 项目简介

PCL CE（Plain Craft Launcher Community Edition）是一款面向 Minecraft Java 版的社区启动器，基于 PCL 开源代码持续开发。项目在保留 PCL 轻量、易用特点的基础上，围绕启动稳定性、运行性能、兼容性与界面体验进行重构和增强，并持续加入新的功能。

当前仓库已从旧版 PCL CE 独立演进，并非 PCL 官方项目，也不等同于旧版 PCL CE。版本号、功能范围与更新节奏均可能与旧版 PCL CE 或 PCL 主线不同。

欢迎通过 Issues 提交问题与建议。请注意：当前版本的问题请反馈至[本仓库](https://github.com/RainYooSuki/PCL-CE/issues)，不要反馈到旧版 PCL CE 或 PCL 官方仓库。

欢迎下载并体验 PCL CE！

## 💻 支持平台

| 操作系统 | 支持情况 | 环境要求 |
|---|---|---|
| Windows 10 1809 (17763) 或更高 | ✅ 完整支持 | [.NET 10 Desktop Runtime](https://get.dot.net/10) |
| Windows 8 - Windows 10 1809 (17763) | ⚠️ 理论能跑，酌情提供社区支持 | [.NET 10 Desktop Runtime](https://get.dot.net/10) |
| Windows 7 或更低版本 | ❌ 不支持 | / |
| macOS / Linux / 其他操作系统 | ⚠️ 仅跨平台开发支持（交叉编译） | [.NET 10 SDK](https://get.dot.net/10) |

**✅ 完整支持**：尽可能提供一切相关支持，但必须确保启动器为最新版本。

**⚠️ 理论能跑，酌情提供社区支持**：PCL CE 应该可以在这些平台上运行，但不保证功能完全可用。你可能需要升级到完整支持的系统版本以获得进一步社区技术支持。

**❌ 不支持**：PCL CE 在这些平台的可用性较低，甚至压根打不开。请升级操作系统以使用 PCL CE。

**⚠️ 仅跨平台开发支持（交叉编译）**：PCL CE 的源代码可以在 macOS 与 Linux 平台编译，但无法直接运行。作为开发者，你可以在这些平台上进行开发，然后将编译产物转移到 Windows 系统测试。

**注**：    
社区仅对最新版本的启动器提供支持。    
取决于部分问题的特殊性（如系统不完整），有时你仍然必须升级操作系统以继续获得支持。    
PCL CE 始终建议使用最新版本的操作系统以获得最佳体验。    
你仍然可以尝试在不受支持的系统上使用 PCL CE，但可能会遇到很多额外问题。

## 🔒 许可证

- `Plain Craft Launcher 2/` 使用 [自定义许可证](https://github.com/RainYooSuki/PCL-CE/blob/dev/Plain%20Craft%20Launcher%202/LICENCE)
- `其余所有目录` 使用 [Apache License 2.0](https://github.com/RainYooSuki/PCL-CE/blob/dev/LICENSE)

﻿/*
TXTRFileType
Copyright (C) 2021 xchellx

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using PaintDotNet;
using System;
using System.Reflection;

namespace TXTRFileType
{
    public class PluginSupportInfo : IPluginSupportInfo
    {
        private readonly Assembly assembly = typeof(PluginSupportInfo).Assembly;

        public string Author => this.assembly.GetCustomAttribute<AssemblyCompanyAttribute>().Company;

        public string Copyright => this.assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;

        public string DisplayName => this.assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        public Version Version => this.assembly.GetName().Version;

        public Uri WebsiteUri => new Uri(AssemblyInfo.WebsiteUri);
    }
}

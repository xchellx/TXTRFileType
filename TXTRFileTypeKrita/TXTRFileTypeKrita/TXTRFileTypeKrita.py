#!/usr/bin/env python3
# -*- coding: utf-8 -*-
# 
# TXTRFileType
# Copyright (C) 2021 xchellx
# 
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <http://www.gnu.org/licenses/>.

import sys
import os
from pathlib import Path

CWD: str
CWD  = None
try:
    CWD = __file__
except NameError:
    CWD = sys.argv[0]
CWD = os.fspath(Path(CWD).resolve().parent)
if (CWD not in sys.path):
    sys.path.append(CWD)

from krita import *
from TXTRFileTypeKritaDock import TXTRFileTypeKritaDock

Krita.instance().addDockWidgetFactory(DockWidgetFactory("TXTRFileTypeKritaDock_f54cc4a7-9d1f-4baa-b680-2e624ff6ba44", DockWidgetFactoryBase.DockRight, TXTRFileTypeKritaDock))

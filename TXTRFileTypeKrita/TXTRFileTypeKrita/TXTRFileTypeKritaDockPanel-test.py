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

from PyQt5.QtCore import *
from PyQt5.QtGui import *
from PyQt5.QtWidgets import *
from PyQt5 import uic

CWD: str
CWD  = None
try:
    CWD = __file__
except NameError:
    CWD = sys.argv[0]
CWD = os.fspath(Path(CWD).resolve().parent)
if (CWD not in sys.path):
    sys.path.append(CWD)

class TXTRFileTypeKritaDockPanel(QTabWidget):
    def __init__(self: 'TXTRFileTypeKritaDockPanel') -> None:
        super(TXTRFileTypeKritaDockPanel, self).__init__()
        self.load_ui()

    def load_ui(self: 'TXTRFileTypeKritaDockPanel') -> None:
        TXTRFileTypeKritaDockUI: QTabWidget
        TXTRFileTypeKritaDockUI = uic.loadUi(f'{CWD}{os.path.sep}TXTRFileTypeKritaDockPanel.ui', self)

if __name__ == "__main__":
    app = QApplication([])
    panel: TXTRFileTypeKritaDockPanel
    panel = TXTRFileTypeKritaDockPanel()
    panel.setWindowTitle('TXTRFileTypeKritaDockPanel')
    panel.show()
    sys.exit(app.exec_())

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

__all__ = ['TXTRFileTypeKritaDock']

import sys
import os
from pathlib import Path
from platform import system as get_osname, python_version_tuple as get_pyver
from traceback import format_exception
from enum import Enum, auto
from typing import Tuple

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

from libtxtrPython import libtxtr, TextureFormat, PaletteFormat, CopyPaletteSize

from krita import *

class TXTRFileTypeKritaDock(DockWidget):
    def __init__(self: 'TXTRFileTypeKritaDock') -> None:
        super(TXTRFileTypeKritaDock, self).__init__()
        self.setWindowTitle('TXTR Import/Export')
        self.initialize_variables()

        issupported: Tuple[bool, str]
        issupported = libtxtr.is_supported()
        if issupported[0]:
            if not libtxtr.is_lib_loaded():
                libtxtr.load_lib(f'{CWD}{os.path.sep}libtxtr{".dll" if get_osname() == "Windows" else ".so"}')
            self.setup_ui()
        else:
            QMessageBox.critical(self, "Unsupported", issupported[1])
            self.setEnabled(False)

    def initialize_variables(self: 'TXTRFileTypeKritaDock'):
        # UI
        self.TXTRFileTypeKritaDockPanel: QTabWidget = uic.loadUi(f'{CWD}{os.path.sep}TXTRFileTypeKritaDockPanel.ui', None)

        # UI parts
        self.importFileLineEdit: QLineEdit = self.TXTRFileTypeKritaDockPanel.findChild(QLineEdit, 'importFileLineEdit')
        self.importFileButton: QPushButton = self.TXTRFileTypeKritaDockPanel.findChild(QPushButton, 'importFileButton')
        self.importMipmapsCheckBox: QCheckBox = self.TXTRFileTypeKritaDockPanel.findChild(QCheckBox, 'importMipmapsCheckBox')
        self.importButton: QPushButton = self.TXTRFileTypeKritaDockPanel.findChild(QPushButton, 'importButton')
        self.exportFileLineEdit: QLineEdit = self.TXTRFileTypeKritaDockPanel.findChild(QLineEdit, 'exportFileLineEdit')
        self.exportFileButton: QPushButton = self.TXTRFileTypeKritaDockPanel.findChild(QPushButton, 'exportFileButton')
        self.exportTextureFormatComboBox: QComboBox = self.TXTRFileTypeKritaDockPanel.findChild(QComboBox, 'exportTextureFormatComboBox')
        self.exportPaletteFormatComboBox: QComboBox = self.TXTRFileTypeKritaDockPanel.findChild(QComboBox, 'exportPaletteFormatComboBox')
        self.exportMipmapsCheckBox: QCheckBox = self.TXTRFileTypeKritaDockPanel.findChild(QCheckBox, 'exportMipmapsCheckBox')
        self.exportMipmapWidthLimitSpinBox: QSpinBox = self.TXTRFileTypeKritaDockPanel.findChild(QSpinBox, 'exportMipmapWidthLimitSpinBox')
        self.exportMipmapHeightLimitSpinBox: QSpinBox = self.TXTRFileTypeKritaDockPanel.findChild(QSpinBox, 'exportMipmapHeightLimitSpinBox')
        self.exportCopyPaletteSizeComboBox: QComboBox = self.TXTRFileTypeKritaDockPanel.findChild(QComboBox, 'exportCopyPaletteSizeComboBox')
        self.exportButton: QPushButton = self.TXTRFileTypeKritaDockPanel.findChild(QPushButton, 'exportButton')

        # Import settings
        self.importMipmaps: bool = False
        self.textureFormat: TextureFormat = TextureFormat.I4
        self.paletteFormat: PaletteFormat = PaletteFormat.IA8

        # Export settings
        self.generateMipmaps: bool = False
        self.mipmapWidthLimit: int = 4
        self.mipmapHeightLimit: int = 4
        self.copyPaletteSize: CopyPaletteSize = CopyPaletteSize.ToWidth

    def setup_ui(self: 'TXTRFileTypeKritaDock') -> None:
        # UI
        self.setWidget(self.TXTRFileTypeKritaDockPanel)
        
        # Import UI
        self.importFileLineEdit.textChanged.connect(self.importFileLineEdit_textChanged)
        self.importFileButton.clicked.connect(self.importFileButton_clicked)
        self.importMipmapsCheckBox.stateChanged.connect(self.importMipmapsCheckBox_stateChanged)
        self.importButton.clicked.connect(self.importButton_clicked)
        
        # Export UI
        self.exportFileLineEdit.textChanged.connect(self.exportFileLineEdit_textChanged)
        self.exportFileButton.clicked.connect(self.exportFileButton_clicked)
        self.exportTextureFormatComboBox.addItems((
            libtxtr.GetEnumDescription(TextureFormat.I4),
            libtxtr.GetEnumDescription(TextureFormat.I8),
            libtxtr.GetEnumDescription(TextureFormat.IA4),
            libtxtr.GetEnumDescription(TextureFormat.IA8),
            libtxtr.GetEnumDescription(TextureFormat.CI4),
            libtxtr.GetEnumDescription(TextureFormat.CI8),
            libtxtr.GetEnumDescription(TextureFormat.CI14X2),
            libtxtr.GetEnumDescription(TextureFormat.RGB565),
            libtxtr.GetEnumDescription(TextureFormat.RGB5A3),
            libtxtr.GetEnumDescription(TextureFormat.RGBA32),
            libtxtr.GetEnumDescription(TextureFormat.CMPR)
        ))
        self.exportTextureFormatComboBox.currentIndex = TextureFormatIndex.I4.value
        self.exportTextureFormatComboBox.currentIndexChanged.connect(self.exportTextureFormatComboBox_currentIndexChanged)
        self.exportPaletteFormatComboBox.addItems((
            libtxtr.GetEnumDescription(PaletteFormat.IA8),
            libtxtr.GetEnumDescription(PaletteFormat.RGB565),
            libtxtr.GetEnumDescription(PaletteFormat.RGB5A3)
        ))
        self.exportPaletteFormatComboBox.currentIndex = PaletteFormatIndex.IA8.value
        self.exportPaletteFormatComboBox.currentIndexChanged.connect(self.exportPaletteFormatComboBox_currentIndexChanged)
        self.exportMipmapsCheckBox.stateChanged.connect(self.exportMipmapsCheckBox_stateChanged)
        self.exportMipmapWidthLimitSpinBox.valueChanged.connect(self.exportMipmapWidthLimitSpinBox_valueChanged)
        self.exportMipmapHeightLimitSpinBox.valueChanged.connect(self.exportMipmapHeightLimitSpinBox_valueChanged)
        self.exportCopyPaletteSizeComboBox.addItems((
            libtxtr.GetEnumDescription(CopyPaletteSize.ToWidth),
            libtxtr.GetEnumDescription(CopyPaletteSize.ToHeight)
        ))
        self.exportCopyPaletteSizeComboBox.currentIndex = CopyPaletteSizeIndex.ToWidth.value
        self.exportCopyPaletteSizeComboBox.currentIndexChanged.connect(self.exportCopyPaletteSizeComboBox_currentIndexChanged)
        self.exportButton.clicked.connect(self.exportButton_clicked)

    def canvasChanged(self: 'TXTRFileTypeKritaDock', canvas: Canvas) -> None:
        pass

    def importFileLineEdit_textChanged(self: 'TXTRFileTypeKritaDock', text: str) -> None:
        self.importButton.setEnabled(not (not text or not text.strip()))

    def importFileButton_clicked(self: 'TXTRFileTypeKritaDock') -> None:
        importFilePath: str
        importFilePath = QFileDialog.getOpenFileName(self, 'Import', str(Path.home()),'Metroid Prime Texture (*.TXTR);;All Files (*.*)')[0]
        if not (not importFilePath or not importFilePath.strip()):
            self.importFileLineEdit.setText(importFilePath)

    def importMipmapsCheckBox_stateChanged(self: 'TXTRFileTypeKritaDock', state: int) -> None:
        self.importMipmaps = state == Qt.CheckState.Checked

    def importButton_clicked(self: 'TXTRFileTypeKritaDock') -> None:
        importFile: str
        importFile = self.importFileLineEdit.text().strip()
        self.importButton.setText('Import')
        self.importButton.setEnabled(False)

        try:
            libtxtr.Open(importFile, self.importMipmaps, lambda p, m: self.importButton.setText(f'Import ({p}/{m})'))
            app: Krita
            app = Krita.instance()
            document: Document
            document = None

            for i in range(0, libtxtr.GetImageCount()):
                width, height = libtxtr.GetImageDimensions(i)
                if document is None:
                    # 300 dpi: 1 pixel == 1 inch
                    document = app.createDocument(width, height, Path(importFile).name[:-len(''.join(Path(importFile).suffixes))], 'RGBA', 'U8', '', 300.0)
                    # Delete default layer
                    document.rootNode().childNodes()[0].remove()

                layer: Node
                layer = document.createNode(f'Mipmap {i + 1}', 'paintlayer')
                layer.setLocked(True)
                
                layerData = QImage(layer.pixelData(0, 0, width, height), width, height, QImage.Format_RGBA8888)
                for x in range(0, width):
                    for y in range(0, height):
                        pixel: bytearray
                        pixel = libtxtr.GetPixel(i, x, y)
                        # BGRA -> RGBA
                        layerData.setPixelColor(x, y, QColor.fromRgba(qRgba(pixel[2], pixel[1], pixel[0], pixel[3])))
                layer.setPixelData(bytes(layerData.constBits().asarray(layerData.byteCount())), 0, 0, width, height)
                
                layer.setLocked(False)
                document.rootNode().addChildNode(layer, None)
                    
            if libtxtr.IsImagesLoaded():
                libtxtr.DisposeLoadedImages()
            app.activeWindow().addView(document)
            self.importButton.setText('Import')
            self.importButton.setEnabled(True)
            QMessageBox.information(self, 'Import Success', f'Successfully imported "{importFile}"')
        except Exception as e:
            if libtxtr.IsImagesLoaded():
                libtxtr.DisposeLoadedImages()
            self.importButton.setText('Import')
            self.importButton.setEnabled(True)
            QMessageBox.critical(self, 'Import Failure', f'Failed to import "{importFile}".{os.linesep}Exception:{os.linesep}{"".join(format_exception(e) if int(get_pyver()[0]) >= 3 and int(get_pyver()[1]) >= 10 else format_exception(type(e), e, e.__traceback__))}')

    def exportFileLineEdit_textChanged(self: 'TXTRFileTypeKritaDock', text: str) -> None:
        self.exportButton.setEnabled(not (not text or not text.strip()))

    def exportFileButton_clicked(self: 'TXTRFileTypeKritaDock') -> None:
        exportFilePath: str
        exportFilePath = QFileDialog.getSaveFileName(self, 'Export', str(Path.home()),'Metroid Prime Texture (*.TXTR);;All Files (*.*)')[0]
        if not (not exportFilePath or not exportFilePath.strip()):
            self.exportFileLineEdit.setText(exportFilePath)

    def exportTextureFormatComboBox_currentIndexChanged(self: 'TXTRFileTypeKritaDock', index: int) -> None:
        self.textureFormat = self.index_to_enum(TextureFormatIndex(index))

        isIndexed: bool
        isIndexed = (self.textureFormat == TextureFormat.CI4
                   or self.textureFormat == TextureFormat.CI8
                   or self.textureFormat == TextureFormat.CI14X2)

        self.exportMipmapsCheckBox.setEnabled(not isIndexed)
        self.exportMipmapWidthLimitSpinBox.setEnabled(not isIndexed)
        self.exportMipmapHeightLimitSpinBox.setEnabled(not isIndexed)
        self.exportPaletteFormatComboBox.setEnabled(isIndexed)
        self.exportCopyPaletteSizeComboBox.setEnabled(isIndexed)

    def exportPaletteFormatComboBox_currentIndexChanged(self: 'TXTRFileTypeKritaDock', index: int) -> None:
        self.paletteFormat = self.index_to_enum(PaletteFormatIndex(index))

    def exportMipmapsCheckBox_stateChanged(self: 'TXTRFileTypeKritaDock', state: int) -> None:
        self.generateMipmaps = state == Qt.CheckState.Checked

        self.exportMipmapWidthLimitSpinBox.setEnabled(self.generateMipmaps)
        self.exportMipmapHeightLimitSpinBox.setEnabled(self.generateMipmaps)

    def exportMipmapWidthLimitSpinBox_valueChanged(self: 'TXTRFileTypeKritaDock', i: int) -> None:
        self.mipmapWidthLimit = i

    def exportMipmapHeightLimitSpinBox_valueChanged(self: 'TXTRFileTypeKritaDock', i: int) -> None:
        self.mipmapWidthLimit = i

    def exportCopyPaletteSizeComboBox_currentIndexChanged(self: 'TXTRFileTypeKritaDock', index: int) -> None:
        self.copyPaletteSize = self.index_to_enum(CopyPaletteSizeIndex(index))

    def exportButton_clicked(self: 'TXTRFileTypeKritaDock') -> None:
        exportFile: str
        exportFile = self.exportFileLineEdit.text().strip()
        self.exportButton.setText('Export')
        self.exportButton.setEnabled(False)

        try:
            app: Krita
            app = Krita.instance()
            document: Document
            document = app.activeDocument()
            
            layer: Node
            layer = document.topLevelNodes()[0]
            layer.setLocked(True)
            layerData = QImage(layer.pixelData(0, 0, document.width(), document.height()), document.width(), document.height(), QImage.Format_RGBA8888)
            layer.setLocked(False)

            libtxtr.InitializeSaveImage(document.width(), document.height())
            for x in range(0, document.width()):
                for y in range(0, document.height()):
                    pixel: QColor
                    pixel = layerData.pixelColor(x, y)
                    # RGBA -> BGRA
                    libtxtr.SetPixel(x, y, bytearray((pixel.blue(), pixel.green(), pixel.red(), pixel.alpha())))

            isIndexed: bool
            isIndexed = (self.textureFormat == TextureFormat.CI4
                            or self.textureFormat == TextureFormat.CI8
                            or self.textureFormat == TextureFormat.CI14X2)
                
            libtxtr.Save(exportFile, self.textureFormat, self.paletteFormat, self.copyPaletteSize,
                            self.generateMipmaps and not isIndexed, self.mipmapWidthLimit,
                            self.mipmapHeightLimit, lambda p, m: self.exportButton.setText(f'Export ({p}/{m})'))

            if libtxtr.IsSaveImageInitialized():
                libtxtr.DisposeSaveImage()
            self.exportButton.setText('Export')
            self.exportButton.setEnabled(True)
            QMessageBox.information(self, 'Export Success', f'Successfully exported "{exportFile}"')
        except Exception as e:
            if libtxtr.IsSaveImageInitialized():
                libtxtr.DisposeSaveImage()
            self.exportButton.setText('Export')
            self.exportButton.setEnabled(True)
            QMessageBox.critical(self, 'Export Failure', f'Failed to export "{exportFile}".{os.linesep}Exception:{os.linesep}{"".join(format_exception(e) if int(get_pyver()[0]) >= 3 and int(get_pyver()[1]) >= 10 else format_exception(type(e), e, e.__traceback__))}')

    def index_to_enum(self: 'TXTRFileTypeKritaDock', index: Enum) -> Enum:
        if isinstance(index, TextureFormatIndex):
            if index == TextureFormatIndex.I4:
                return TextureFormat.I4
            elif index == TextureFormatIndex.I8:
                return TextureFormat.I8
            elif index == TextureFormatIndex.IA4:
                return TextureFormat.IA4
            elif index == TextureFormatIndex.IA8:
                return TextureFormat.IA8
            elif index == TextureFormatIndex.CI4:
                return TextureFormat.CI4
            elif index == TextureFormatIndex.CI8:
                return TextureFormat.CI8
            elif index == TextureFormatIndex.CI14X2:
                return TextureFormat.CI14X2
            elif index == TextureFormatIndex.RGB565:
                return TextureFormat.RGB565
            elif index == TextureFormatIndex.RGB5A3:
                return TextureFormat.RGB5A3
            elif index == TextureFormatIndex.RGBA32:
                return TextureFormat.RGBA32
            elif index == TextureFormatIndex.CMPR:
                return TextureFormat.CMPR
            else:
                raise ValueError('Parameter "index" is not a valid TextureFormatIndex enumeration')
        elif isinstance(index, PaletteFormatIndex):
            if index == PaletteFormatIndex.IA8:
                return PaletteFormat.IA8
            elif index == PaletteFormatIndex.RGB565:
                return PaletteFormat.RGB565
            elif index == PaletteFormatIndex.RGB5A3:
                return PaletteFormat.RGB5A3
            else:
                raise ValueError('Parameter "index" is not a valid PaletteFormatIndex enumeration')
        elif isinstance(index, CopyPaletteSizeIndex):
            if index == CopyPaletteSizeIndex.ToWidth:
                return CopyPaletteSize.ToWidth
            elif index == CopyPaletteSizeIndex.ToHeight:
                return CopyPaletteSize.ToHeight
            else:
                raise ValueError('Parameter "index" is not a valid CopyPaletteSizeIndex enumeration')
        else:
            raise ValueError('Parameter "index" is of an enum type that is not supported by this method)')

class TextureFormatIndex(Enum):
    I4     = 0
    I8     = auto()
    IA4    = auto()
    IA8    = auto()
    CI4    = auto()
    CI8    = auto()
    CI14X2 = auto()
    RGB565 = auto()
    RGB5A3 = auto()
    RGBA32 = auto()
    CMPR   = auto()

class PaletteFormatIndex(Enum):
    IA8    = 0
    RGB565 = auto()
    RGB5A3 = auto()

class CopyPaletteSizeIndex(Enum):
    ToWidth  = 0
    ToHeight = auto()

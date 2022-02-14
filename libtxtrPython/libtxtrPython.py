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

__all__ = ['libtxtr', 'TextureFormat', 'PaletteFormat', 'CopyPaletteSize']

import sys
import os
import ctypes
from pathlib import Path
from platform import system as get_osname, python_version_tuple as get_pyver, machine as get_machinearch
from typing import Callable, Tuple
from enum import Enum, EnumMeta

CWD: str
CWD  = None
try:
    CWD = __file__
except NameError:
    CWD = sys.argv[0]
CWD = os.fspath(Path(CWD).resolve().parent)
if (CWD not in sys.path):
    sys.path.append(CWD)

class libtxtr:
    """Python interface to the libtxtr library

    Loaded images and the save image are stored in a list in the library's memory.
    This allows for dynamically initializing and disposing the loaded images and save image
    from python. The consequence is it cannot be automatically collected by python so one
    must be careful to dispose the loaded images and save image at any point whenever they can.

    Even though internally the methods pass back status codes on whether success or failure occured,
    exceptions are raised instead through this interface. This is to be more thorough and understandable
    through its implementation in this interface (fit the python language).

    The libtxtr library will be loaded as a ctypes.CDLL library, so it expects cdecl calling convention.
    """

    __libtxtr = None
    __UpdateProgressDelegate = ctypes.CFUNCTYPE(ctypes.c_void_p, ctypes.c_double, ctypes.c_double)

    __STATUS_SUCCESS = 0
    __STATUS_FAILED = 1
    __STATUS_IDXOUTOFRANGE = 2
    __STATUS_NOLOADEDIMGS = 3
    __STATUS_MARSHALFAIL = 4
    __STATUS_NULLPTR = 5
    __STATUS_INVALIDARG = 6
    __STATUS_IMGALRINIT = 7
    __STATUS_IMGNOTINIT = 8

    __ENUMTYPE_TEXTUREFORMAT = 1
    __ENUMTYPE_PALETTEFORMAT = 2
    __ENUMTYPE_COPYPALETTESIZE = 3

    @classmethod
    def load_lib(cls: 'libtxtr', file_path: str) -> None:
        """Load the libtxtr library from the specified file.

        The library will be loaded as a ctypes.CDLL library (expects cdecl calling convention).
        See: libtxtr.is_lib_loaded

        Args:
            file_path (str): The path to the libtxtr library.

        Raises:
            ValueError: If file_path is None or not a str
            Exception: If the libtxtr library is already loaded
        """

        if file_path is None:
            raise ValueError('Parameter "file_path" is required')
        elif not isinstance(file_path, str):
            raise ValueError('Parameter "file_path" must be a str')
    
        if cls.__libtxtr is None:
            cls.__libtxtr = ctypes.CDLL(file_path)
            cls.__libtxtr.Open.restype = ctypes.c_int
            cls.__libtxtr.Open.argtypes = [ctypes.c_char_p, ctypes.c_bool, cls.__UpdateProgressDelegate]
            cls.__libtxtr.Save.restype = ctypes.c_int
            cls.__libtxtr.Save.argtypes = [ctypes.c_char_p, ctypes.c_uint, ctypes.c_uint, ctypes.c_uint,
                                             ctypes.c_bool, ctypes.c_int, ctypes.c_int, cls.__UpdateProgressDelegate]
            cls.__libtxtr.GetPixel.restype = ctypes.c_int
            cls.__libtxtr.GetPixel.argtypes = [ctypes.c_int, ctypes.c_int, ctypes.c_int,
                                                 ctypes.POINTER(ctypes.c_ubyte), ctypes.POINTER(ctypes.c_ubyte),
                                                 ctypes.POINTER(ctypes.c_ubyte), ctypes.POINTER(ctypes.c_ubyte)]
            cls.__libtxtr.SetPixel.restype = ctypes.c_int
            cls.__libtxtr.SetPixel.argtypes = [ctypes.c_int, ctypes.c_int, ctypes.c_ubyte, ctypes.c_ubyte,
                                                 ctypes.c_ubyte, ctypes.c_ubyte]
            cls.__libtxtr.IsImagesLoaded.restype = ctypes.c_bool;
            cls.__libtxtr.IsImagesLoaded.argtypes = []
            cls.__libtxtr.InitializeSaveImage.restype = ctypes.c_int
            cls.__libtxtr.InitializeSaveImage.argtypes = [ctypes.c_int, ctypes.c_int]
            cls.__libtxtr.IsSaveImageInitialized.restype = ctypes.c_bool;
            cls.__libtxtr.IsSaveImageInitialized.argtypes = []
            cls.__libtxtr.GetImageCount.restype = ctypes.c_int
            cls.__libtxtr.GetImageCount.argtypes = [ctypes.POINTER(ctypes.c_int)]
            cls.__libtxtr.GetImageDimensions.restype = ctypes.c_int
            cls.__libtxtr.GetImageDimensions.argtypes = [ctypes.c_int, ctypes.POINTER(ctypes.c_int),
                                                           ctypes.POINTER(ctypes.c_int)]
            cls.__libtxtr.GetLastInteropError.restype = ctypes.c_int
            cls.__libtxtr.GetLastInteropError.argtypes = [ctypes.c_char_p, ctypes.c_bool]
            cls.__libtxtr.DisposeLoadedImages.restype = ctypes.c_int
            cls.__libtxtr.DisposeLoadedImages.argtypes = []
            cls.__libtxtr.DisposeSaveImage.restype = ctypes.c_int
            cls.__libtxtr.DisposeSaveImage.argtypes = []
            cls.__libtxtr.GetEnumDescription.restype = ctypes.c_int
            cls.__libtxtr.GetEnumDescription.argtypes = [ctypes.c_int, ctypes.c_uint,
                                                           ctypes.c_char_p, ctypes.c_bool]
        else:
            raise Exception('The libtxtr library is already loaded')

    @classmethod
    def reload_lib(cls: 'libtxtr', file_path: str) -> None:
        """Reload the libtxtr library from the specified file.

        The library will be reloaded as a ctypes.CDLL (cdecl calling convention).
        Reloading loads the library again via a new instance regardless if it is
        already loaded.
        See: libtxtr.is_lib_loaded

        Args:
            file_path (str): The path to the libtxtr library.

        Raises:
            ValueError: If file_name is None or not a str
        """

        if file_path is None:
            raise ValueError('Parameter "file_path" is required')
        elif not isinstance(file_path, str):
            raise ValueError('Parameter "file_path" must be a str')
        
        cls.__libtxtr = None
        cls.load_lib(file_path)

    @classmethod
    def is_lib_loaded(cls: 'libtxtr') -> bool:
        """Check if the libtxtr library is loaded.

        Returns:
            bool: True if the libtxtr library is loaded else False
        """

        return cls.__libtxtr is not None

    @classmethod
    def is_supported(cls: 'libtxtr') -> Tuple[bool, str]:
        """Check if the OS supports the libtxtr library

        Returns:
            Tuple[bool, str]: (True, "") if supported else (False, <reason_str>)
        """

        ossupported: bool
        ossupported = get_osname() == 'Windows' or get_osname() == 'Linux'
        archsupported: bool
        archsupported = get_machinearch().endswith('64')
        pyversupported: bool
        pyversupported = int(get_pyver()[0]) >= 3 and int(get_pyver()[1]) >= 6

        issupported: bool
        issupported = ossupported and archsupported and pyversupported
        reasonStr: str
        if issupported:
            reasonStr = ""
        else:
            reasonStr = f"""Your operating system, machine architecture, and/or python version are\
    not supported.{os.linesep}Supported Operating System(s): Windows and Linux{os.linesep}Supported Machine Architecture(s): 64bit\
    {os.linesep}Supported Python Version(s): 3.6+"""

        return (issupported, reasonStr)

    @classmethod
    def Open(cls: 'libtxtr', filePath: str, readMipmaps: bool = False, progressCallback: Callable[[float, float], None] = None) -> None:
        """Reads a TXTR from a file.

        The texture will be stored as image(s) in the memory of the libtxtr library.
        It is strongly advised that you call libtxtr.DisposeLoadedImages at all possible
        points of your code's control flow or else memory leaks will ensue.
        See: libtxtr.IsImagesLoaded and libtxtr.DisposeLoadedImages

        Args:
            filePath (str): A path to a TXTR file
            readMipmaps (bool): Whether all mipmaps should be read or only the first mipmap
            progressCallback (Callable[[float, float], None]): A function where mipmap read progress will be reported to

        Raises:
            ValueError: If filePath is None/not a str, readMipmaps is None/not a bool, or progressCallback is NOT None AND is not callable
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if filePath is None:
            raise ValueError('Parameter "filePath" is required')
        elif not isinstance(filePath, str):
            raise ValueError('Parameter "filePath" must be a str')
        if readMipmaps is not None and not isinstance(readMipmaps, bool):
            raise ValueError('Parameter "readMipmaps" must be a bool')
        if progressCallback is not None and not callable(progressCallback):
            raise ValueError('Parameter "progressCallback" must be a function')
        
        if cls.__libtxtr is not None:
            filePathC: ctypes.c_char_p
            filePathC = ctypes.c_char_p(filePath.encode('ascii'))
            readMipmapsC: bool
            readMipmapsC = False
            if (readMipmaps is not None):
                readMipmapsC = readMipmaps
            progressCallbackC: cls.__UpdateProgressDelegate
            progressCallbackC = ctypes.cast(None, cls.__UpdateProgressDelegate)
            if (progressCallback is not None):
                progressCallbackC = cls.__UpdateProgressDelegate(progressCallback)
            status: int
            status = cls.__libtxtr.Open(filePathC, readMipmapsC, progressCallbackC)
            if status != cls.__STATUS_SUCCESS:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def Save(cls: 'libtxtr', filePath: str, textureFormat: 'TextureFormat', paletteFormat: 'PaletteFormat',
             copyPaletteSize: 'CopyPaletteSize', generateMipmaps: bool = False, mipmapWidthLimit: int = 4,
             mipmapHeightLimit: int = 4, progressCallback: Callable[[float, float], None] = None) -> None:
        """Writes a TXTR to a file.

        The save image in the memory of the libtxtr library will be the image that is written to the TXTR file.
        It is strongly advised that you call libtxtr.DisposeSaveImage at all possible
        points of your code's control flow or else memory leaks will ensue.
        See: libtxtr.InitializeSaveImage, libtxtr.IsSaveImageInitialized, and libtxtr.DisposeSaveImage

        Args:
            filePath (str): A path to a TXTR file
            textureFormat (libtxtr.TextureFormat): The texture format of the TXTR. See: libtxtr.TextureFormat
            textureFormat (libtxtr.PaletteFormat): The palette format of the TXTR. See: libtxtr.PaletteFormat
            textureFormat (libtxtr.CopyPaletteSize): The location to copy the palette length to in the TXTR. See: libtxtr.CopyPaletteSize
            generateMipmaps (bool): Whether mipmaps should be generated in the TXTR
            mipmapWidthLimit (int): The width limit for the TXTR mipmap generation
            mipmapHeightLimit (int): The height limit for the TXTR mipmap generation
            progressCallback (Callable[[float, float], None]): A function where mipmap read progress will be reported to

        Raises:
            ValueError: If filePath is None/not a str,
                        textureFormat is None/not a Enum/not valid,
                        paletteFormat is None/not a Enum/not valid,
                        copyPaletteSize is None/not a Enum/not valid,
                        generateMipmaps is None/not a bool,
                        mipmapWidthLimit is None/not a int,
                        mipmapHeightLimit is None/not a int,
                        or progressCallback is NOT None AND is not callable
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if filePath is None:
            raise ValueError('Parameter "filePath" is required')
        elif not isinstance(filePath, str):
            raise ValueError('Parameter "filePath" must be a str')
        if textureFormat is None:
            raise ValueError('Parameter "textureFormat" is required')
        elif not isinstance(textureFormat, TextureFormat):
            raise ValueError('Parameter "textureFormat" must be a TextureFormat')
        elif not textureFormat in TextureFormat:
            raise ValueError('Parameter "textureFormat" is not a valid TextureFormat enumeration')
        if paletteFormat is None:
            raise ValueError('Parameter "paletteFormat" is required')
        elif not isinstance(paletteFormat, PaletteFormat):
            raise ValueError('Parameter "paletteFormat" must be a PaletteFormat')
        elif not paletteFormat in PaletteFormat:
            raise ValueError('Parameter "paletteFormat" is not a valid PaletteFormat enumeration')
        if copyPaletteSize is None:
            raise ValueError('Parameter "copyPaletteSize" is required')
        elif not isinstance(copyPaletteSize, CopyPaletteSize):
            raise ValueError('Parameter "copyPaletteSize" must be a CopyPaletteSize')
        elif not copyPaletteSize in CopyPaletteSize:
            raise ValueError('Parameter "copyPaletteSize" is not a valid CopyPaletteSize enumeration')
        if generateMipmaps is not None and not isinstance(generateMipmaps, bool):
            raise ValueError('Parameter "generateMipmaps" must be a bool')
        if mipmapWidthLimit is not None and not isinstance(mipmapWidthLimit, int):
            raise ValueError('Parameter "mipmapWidthLimit" must be a bool')
        if mipmapHeightLimit is not None and not isinstance(mipmapHeightLimit, int):
            raise ValueError('Parameter "mipmapHeightLimit" must be a bool')
        if progressCallback is not None and not callable(progressCallback):
            raise ValueError('Parameter "progressCallback" must be a function')
        
        if cls.__libtxtr is not None:
            filePathC: ctypes.c_char_p
            filePathC = ctypes.c_char_p(filePath.encode('ascii'))
            progressCallbackC: cls.__UpdateProgressDelegate
            progressCallbackC = ctypes.cast(None, cls.__UpdateProgressDelegate)
            if (generateMipmaps is None):
                generateMipmaps = False
            if (mipmapWidthLimit is None):
                mipmapWidthLimit = 4
            if (mipmapHeightLimit is None):
                mipmapHeightLimit = 4
            if (progressCallback is not None):
                progressCallbackC = cls.__UpdateProgressDelegate(progressCallback)
            status: int
            status = cls.__libtxtr.Save(filePathC, textureFormat.value, paletteFormat.value,
                                      copyPaletteSize.value, generateMipmaps, mipmapWidthLimit,
                                      mipmapHeightLimit, progressCallbackC)
            if status != cls.__STATUS_SUCCESS:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def GetPixel(cls: 'libtxtr', index: int, x: int, y: int) -> bytearray:
        """Gets the RGBA color components of a pixel on a loaded image stored in the memory of the libtxtr library at the specified index

        The image in the memory of the libtxtr library at the specified index will be the image where the pixel will be caught
        It is strongly advised that you call libtxtr.DisposeLoadedImages at all possible
        points of your code's control flow or else memory leaks will ensue.
        See: libtxtr.IsImagesLoaded and libtxtr.DisposeLoadedImages

        Args:
            index (int): The index of the loaded image in the memory of the libtxtr library.
            x (int): The x coordinate of the target pixel
            y (int): The y coordinate of the target pixel

        Returns:
            bytearray: The RGBA color components of the target pixel

        Raises:
            ValueError: If index is None/not a int, x is None/not a int, or y is None/not a int
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if index is None:
            raise ValueError('Parameter "index" is required')
        elif not isinstance(index, int):
            raise ValueError('Parameter "index" must be a int')
        if x is None:
            raise ValueError('Parameter "x" is required')
        elif not isinstance(x, int):
            raise ValueError('Parameter "x" must be a int')
        if y is None:
            raise ValueError('Parameter "y" is required')
        elif not isinstance(y, int):
            raise ValueError('Parameter "y" must be a int')
    
        if cls.__libtxtr is not None:
            rC: ctypes.c_ubyte
            rC = ctypes.c_ubyte(0)
            gC: ctypes.c_ubyte
            gC = ctypes.c_ubyte(0)
            bC: ctypes.c_ubyte
            bC = ctypes.c_ubyte(0)
            aC: ctypes.c_ubyte
            aC = ctypes.c_ubyte(0)
            status = cls.__libtxtr.GetPixel(index, x, y, ctypes.byref(rC), ctypes.byref(gC),
                                          ctypes.byref(bC), ctypes.byref(aC))
            if status == cls.__STATUS_SUCCESS:
                pixel = bytearray(4)
                pixel[0] = rC.value
                pixel[1] = gC.value
                pixel[2] = bC.value
                pixel[3] = aC.value
                return pixel
            else:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def SetPixel(cls: 'libtxtr', x: int, y: int, pixel: bytearray) -> None:
        """Sets a pixel to the specified RGBA color components on the save image stored in the memory of the libtxtr library

        The save image in the memory of the libtxtr library will be the image that is to have a pixel set to.
        It is strongly advised that you call libtxtr.DisposeSaveImage at all possible
        points of your code's control flow or else memory leaks will ensue.
        See: libtxtr.InitializeSaveImage, libtxtr.IsSaveImageInitialized, and libtxtr.DisposeSaveImage

        The passed byte array must be a length of 4 in the format of [r, g, b, a]
        The passed byte array must also consist of values of bytes, so they must be within the range of 0 to 255.

        Args:
            x (int): The x coordinate of the target pixel
            y (int): The y coordinate of the target pixel
            pixel (bytearray): The RGBA color components to set on target pixel

        Raises:
            ValueError: If x is None/not a int, y is None/not a int, pixel is None/not a bytearray/not of the length 4
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if x is None:
            raise ValueError('Parameter "x" is required')
        elif not isinstance(x, int):
            raise ValueError('Parameter "x" must be a int')
        if y is None:
            raise ValueError('Parameter "y" is required')
        elif not isinstance(y, int):
            raise ValueError('Parameter "y" must be a int')
        if pixel is None:
            raise ValueError('Parameter "pixel" is required')
        elif not isinstance(pixel, bytearray):
            raise ValueError('Parameter "pixel" must be a bytearray')
        elif not len(pixel) == 4:
            raise ValueError('Parameter "pixel" must have a length of 4')
    
        if cls.__libtxtr is not None:
            status: int
            status = cls.__libtxtr.SetPixel(x, y, pixel[0], pixel[1], pixel[2], pixel[3])
            if status == cls.__STATUS_SUCCESS:
                return pixel
            else:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def IsImagesLoaded(cls: 'libtxtr') -> bool:
        """Gets whether if there are any loaded images in the memory of the libtxtr library

        Returns:
            bool: True if there are any images loaded else False

        Raises:
            Exception: If the libtxtr library is not loaded
        """

        if cls.__libtxtr is not None:
            return cls.__libtxtr.IsImagesLoaded()
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def InitializeSaveImage(cls: 'libtxtr', width: int, height: int) -> None:
        """Initializes the save image with the specified width and height into the memory of the libtxtr library.
        
        The save image will be stored as image(s) in the memory of the libtxtr library.
        It is strongly advised that you call libtxtr.DisposeSaveImage at all possible
        points of your code's control flow or else memory leaks will ensue.
        See: libtxtr.InitializeSaveImage, libtxtr.IsSaveImageInitialized, and libtxtr.DisposeSaveImage

        Args:
            width (int): The image width
            height (int): The image height

        Raises:
            ValueError: If width is None/not a int or height is None/not a int
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if width is None:
            raise ValueError('Parameter "width" is required')
        elif not isinstance(width, int):
            raise ValueError('Parameter "width" must be a int')
        if height is None:
            raise ValueError('Parameter "height" is required')
        elif not isinstance(height, int):
            raise ValueError('Parameter "height" must be a int')
    
        if cls.__libtxtr is not None:
            status: int
            status = cls.__libtxtr.InitializeSaveImage(width, height)
            if status != cls.__STATUS_SUCCESS:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def IsSaveImageInitialized(cls: 'libtxtr') -> bool:
        """Gets whether the save image is initialized in the memory of the libtxtr library

        Returns:
            bool: True if the save image is initialized else False

            Exception: If the libtxtr library is not loaded
        """

        if cls.__libtxtr is not None:
            return cls.__libtxtr.IsSaveImageInitialized()
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def GetImageCount(cls: 'libtxtr') -> int:
        """Gets the count of loaded images in the memory of the libtxtr library.

        Returns:
            int: The count of loaded images in the memory of the libtxtr library

        Raises:
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if cls.__libtxtr is not None:
            imageCountC: ctypes.c_int
            imageCountC = ctypes.c_int(0)
            status: int
            status = cls.__libtxtr.GetImageCount(ctypes.byref(imageCountC))
            if status == cls.__STATUS_SUCCESS:
                return imageCountC.value
            else:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def GetImageDimensions(cls: 'libtxtr', index: int) -> Tuple[int, int]:
        """Gets the dimensions (width and height) of a loaded image in the memory of the libtxtr library.

        Args:
            index (int): The index of the loaded image

        Returns:
            Tuple[int, int]: (image_width, image_height)

        Raises:
            ValueError: If index is None/not a int
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if index is None:
            raise ValueError('Parameter "index" is required')
        elif not isinstance(index, int):
            raise ValueError('Parameter "index" must be a int')
    
        if cls.__libtxtr is not None:
            widthC: ctypes.c_int
            widthC = ctypes.c_int(0)
            heightC: ctypes.c_int
            heightC = ctypes.c_int(0)
            status: int
            status = cls.__libtxtr.GetImageDimensions(index, ctypes.byref(widthC), ctypes.byref(heightC))
            if status == cls.__STATUS_SUCCESS:
                return (widthC.value, heightC.value)
            else:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def DisposeLoadedImages(cls: 'libtxtr') -> None:
        """Disposes the loaded images stored in the memory of the libtxtr library.

        Raises:
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if cls.__libtxtr is not None:
            status: int
            status = cls.__libtxtr.DisposeLoadedImages()
            if status != cls.__STATUS_SUCCESS:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def DisposeSaveImage(cls: 'libtxtr') -> None:
        """Disposes the save image stored in the memory of the libtxtr library.

        Raises:
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if cls.__libtxtr is not None:
            status: int
            status = cls.__libtxtr.DisposeSaveImage()
            if status != cls.__STATUS_SUCCESS:
                raise Exception(cls.__get_errstr_withstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def GetEnumDescription(cls: 'libtxtr', enum: Enum) -> str:
        """Gets the description of an enum supported by the libtxtr library.

        Args:
            enum (Enum): An enum instance

        Returns:
            str: The description of the passed enum

        Raises:
            ValueError: If enum is None/not a Enum/not supported by this library
            Exception: If the libtxtr library is not loaded or an error occurs in the libtxtr library
        """

        if enum is None:
            raise ValueError('Parameter "enum" is required')
        elif not isinstance(enum, Enum):
            raise ValueError('Parameter "enum" must be a Enum')
        
        if cls.__libtxtr is not None:
            enumTypeC: int
            enumTypeC = None
            if isinstance(enum, TextureFormat):
                enumTypeC = cls.__ENUMTYPE_TEXTUREFORMAT
            elif isinstance(enum, PaletteFormat):
                enumTypeC = cls.__ENUMTYPE_PALETTEFORMAT
            elif isinstance(enum, CopyPaletteSize):
                enumTypeC = cls.__ENUMTYPE_COPYPALETTESIZE
            else:
                raise ValueError('Parameter "enum" is of an enum type that is not supported by this method)')
            enumDescriptionC: ctypes.Array[ctypes.c_char]
            enumDescriptionC = ctypes.create_string_buffer(128)
            status: int
            status = cls.__libtxtr.GetEnumDescription(enumTypeC, enum.value, enumDescriptionC, False)
            if status == cls.__STATUS_SUCCESS:
                try:
                    return enumDescriptionC.raw.decode('ascii').strip('\x00')
                except:
                    return enumDescriptionC.value.strip('\x00')
            else:
                raise Exception(cls.__get_errstr_fromstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def __GetLastInteropError(cls: 'libtxtr') -> str:
        if cls.__libtxtr is not None:
            errMsgC: ctypes.Array[ctypes.c_char]
            errMsgC = ctypes.create_string_buffer(4096)
            status: int
            status = cls.__libtxtr.GetLastInteropError(errMsgC, False)
            if status == cls.__STATUS_SUCCESS:
                try:
                    return errMsgC.raw.decode('ascii').strip('\x00')
                except:
                    return errMsgC.value
            else:
                raise Exception(cls.__get_errstr_fromstatus(status))
        else:
            raise Exception('The libtxtr library is not loaded')

    @classmethod
    def __status_to_str(cls: 'libtxtr', status: int) -> str:
        if status == cls.__STATUS_SUCCESS:
            return 'STATUS_SUCCESS'
        elif status == cls.__STATUS_FAILED:
            return 'STATUS_FAILED'
        elif status == cls.__STATUS_IDXOUTOFRANGE:
            return 'STATUS_IDXOUTOFRANGE'
        elif status == cls.__STATUS_NOLOADEDIMGS:
            return 'STATUS_NOLOADEDIMGS'
        elif status == cls.__STATUS_MARSHALFAIL:
            return 'STATUS_MARSHALFAIL'
        elif status == cls.__STATUS_NULLPTR:
            return 'STATUS_NULLPTR'
        elif status == cls.__STATUS_INVALIDARG:
            return 'STATUS_INVALIDARG'
        elif status == cls.__STATUS_IMGALRINIT:
            return 'STATUS_IMGALRINIT'
        elif status == cls.__STATUS_IMGNOTINIT:
            return 'STATUS_IMGNOTINIT'
        else:
            raise ValueError(f'Invalid status code: {status}')

    @classmethod
    def __get_errstr_fromstatus(cls: 'libtxtr', status: int) -> str:
        return f'An error occurred{os.linesep}Status return code: {status} ({cls.__status_to_str(status)})'

    @classmethod
    def __get_errstr_withstatus(cls: 'libtxtr', status: int) -> str:
        return f'{cls.__get_errstr_fromstatus(status)}{os.linesep}Error: {cls.__GetLastInteropError()}'

class __TextureFormatMeta(EnumMeta):
    # Allow checking if an enum value is defined
    # in this enum via 'if val in MyEnum:'
    def __contains__(cls, item): 
        try:
            cls(item)
        except ValueError:
            return False
        else:
            return True

class TextureFormat(Enum, metaclass=__TextureFormatMeta):
    """
    Enum that specifies what format a GX texture is (how its pixel data is stored)
    """

    I4     = 0x0
    """
    4-bit greyscale intensity values. Two pixels per byte.
    """

    I8     = 0x1
    """
    8-bit greyscale intensity values.
    """

    IA4    = 0x2
    """
    4-bit greyscale intensity values with an additional 4-bit alpha channel.
    """

    IA8    = 0x3
    """
    8-bit greyscale intensity values with an additional 8-bit alpha channel.
    """

    CI4    = 0x4
    """
    4-bit palette indices.
    """

    CI8    = 0x5
    """
    8-bit palette indices.
    """

    CI14X2 = 0x6
    """
    14-bit palette indices (14b index).
    """

    RGB565 = 0x7
    """
    16-bit colors without alpha.
    """

    RGB5A3 = 0x8
    """
    16-bit colors with alpha.
    """

    RGBA32 = 0x9
    """
    Uncompressed 32-bit colors with alpha.
    """

    CMPR   = 0xA
    """
    DXT1 Compression
    """

class __PaletteFormatMeta(EnumMeta):
    # Allow checking if an enum value is defined
    # in this enum via 'if val in MyEnum:'
    def __contains__(cls, item): 
        try:
            cls(item)
        except ValueError:
            return False
        else:
            return True

class PaletteFormat(Enum, metaclass=__PaletteFormatMeta):
    """
    Enum that specifies what format a GX texture's palette is (how its pixel data is stored)
    """
    
    IA8    = 0x0
    """
    8-bit greyscale intensity values with an additional 8-bit alpha channel.
    """

    RGB565 = 0x1
    """
    16-bit colors without alpha.
    """

    RGB5A3 = 0x2
    """
    16-bit colors with alpha.
    """



class __CopyPaletteSizeMeta(EnumMeta):
    # Allow checking if an enum value is defined
    # in this enum via 'if val in MyEnum:'
    def __contains__(cls, item): 
        try:
            cls(item)
        except ValueError:
            return False
        else:
            return True

class CopyPaletteSize(Enum, metaclass=__CopyPaletteSizeMeta):
    """
    This enum specifies whether the palette length should be copies to the
    palette width or palette height upon texture conversion
    """

    ToWidth  = 0
    """
    Copy palette length to width.
    """
    
    ToHeight = 1
    """
    Copy palette length to height.
    """

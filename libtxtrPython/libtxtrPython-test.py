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

# It may be useful to add tests for other methods that aren't tested here.
# Also, do an internal test for __GetLastInteropError__

import sys
import os
from pathlib import Path
from platform import system as get_osname
from timeit import default_timer as time
from PIL import Image

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

start_time = 0.0
end_time = 0.0

def test_start(test_name, test_conditions, test_func, *args):
    print(f'==== Starting {test_name} test ====')
    print(f'Testing for: {test_conditions}')
    try:
        test_func(*args)
        return True
    except Exception as e:
        test_failure(e)
        return False

def test_success():
    global start_time
    global end_time
    print('==== Test success ====')
    print(f'Execution time: {format(end_time - start_time, ".1000")} ms')

def test_failure(e=None):
    global start_time
    global end_time
    if e is not None:
        end_time = time()
    print('==== Test failure ====')
    print(f'Execution time: {format(end_time - start_time, ".1000")} ms')
    if e is not None:
        print(f'Exception: {e}')
    if libtxtr.IsImagesLoaded():
        libtxtr.DisposeLoadedImages()
    if libtxtr.IsSaveImageInitialized():
        libtxtr.DisposeSaveImage()

def test_libtxtr_GetEnumDescription(*args):
    global start_time
    global end_time
    start_time = time()
    enumDescription = libtxtr.GetEnumDescription(args[0])
    end_time = time()
    print(f'enumDescription = {enumDescription}')
    if enumDescription == 'I4 - 4-Bit Greyscale Without Alpha':
        test_success()
    else:
        test_failure()

def test_libtxtr_Open(*args):
    global start_time
    global end_time
    start_time = time()
    libtxtr.Open(args[0], args[1], args[2])
    end_time = time()
    test_success()

def test_libtxtr_Open_progress(progressValue, maxValue):
    print(f'progress = {progressValue}, max = {maxValue}')

def test_libtxtr_GetImageCount(*args):
    global start_time
    global end_time
    start_time = time()
    imageCount = libtxtr.GetImageCount()
    end_time = time()
    print(f'imageCount = {imageCount}')
    if imageCount == 4:
        test_success()
    else:
        test_failure()

def test_libtxtr_GetImageDimensions(*args):
    global start_time
    global end_time
    start_time = time()
    width, height = libtxtr.GetImageDimensions(args[0])
    end_time = time()
    print(f'width = {width}, height = {height}')
    if width == 32 and height == 32:
        test_success()
    else:
        test_failure()

def test_libtxtr_GetPixel(*args):
    global start_time
    global end_time
    start_time = time()
    pixel = libtxtr.GetPixel(args[0], args[1], args[2])
    end_time = time()
    print(f'pixel[0] = {pixel[0]}, pixel[1] = {pixel[1]}, pixel[2] = {pixel[2]}, pixel[3] = {pixel[3]}')
    if pixel[0] == 187 and pixel[1] == 187 and pixel[2] == 204 and pixel[3] == 218:
        test_success()
    else:
        test_failure()

def test_libtxtr_InitializeSaveImage(*args):
    global start_time
    global end_time
    start_time = time()
    libtxtr.InitializeSaveImage(args[0], args[1])
    end_time = time()
    test_success()

def test_libtxtr_SetPixel(*args):
    global start_time
    global end_time
    start_time = time()
    pixel = bytearray(4)
    pixel[0] = args[2]
    pixel[1] = args[3]
    pixel[2] = args[4]
    pixel[3] = args[5]
    libtxtr.SetPixel(args[0], args[1], pixel)
    end_time = time()
    test_success()

def test_libtxtr_Save(*args):
    global start_time
    global end_time
    start_time = time()
    libtxtr.Save(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7])
    end_time = time()
    test_success()

def test_libtxtr_Save_progress(progressValue, maxValue):
    print(f'progress = {progressValue}, max = {maxValue}')

def main():
    issupported: bool
    issupported = libtxtr.is_supported()
    if issupported[0]:
        if libtxtr.is_lib_loaded():
            print('=== libtxtr library already loaded ===')
        else:
            try:
                __LIBTXTR_PATH = f'{CWD}{os.path.sep}libtxtr{".dll" if get_osname() == "Windows" else ".so"}'
                print('==== Loading libtxtr library ====')
                print(f'libtxtr library path = {__LIBTXTR_PATH}')
                libtxtr.load_lib(__LIBTXTR_PATH)
                print('')
            except Exception as e:
                print(f'Failed to load libtxtr library{0}Exception:{1}{2}', os.linesep, os.linesep, e)
    else:
        print(issupported[1])
        return

    if not test_start('libtxtr.GetEnumDescription', 'No exception, enumDescription = "I4 - 4-Bit Greyscale Without Alpha"',
                      test_libtxtr_GetEnumDescription, TextureFormat.I4):
        return
    print('')
    
    if not test_start('libtxtr.Open', 'No exception', test_libtxtr_Open,
                      r'D:\From Desktop\TXTRSearch\MP1Paks\NoARAM-pak\46434ed3.TXTR',
                      True, test_libtxtr_Open_progress):
        return
    print('')
    
    if not test_start('libtxtr.GetImageCount', 'No exception, imageCount = 4',
                      test_libtxtr_GetImageCount):
        return
    print('')
    
    if not test_start('libtxtr.GetImageDimensions', 'No exception, width = 32, height - 32',
                      test_libtxtr_GetImageDimensions, 0):
        return
    print('')
    
    if not test_start('libtxtr.GetPixel',
                      'No exception, pixel[0] = 187, pixel[1] = 187, pixel[2] = 204, pixel[3] = 218',
                      test_libtxtr_GetPixel, 0, 16, 4):
        return
    print('')
    
    if libtxtr.IsImagesLoaded():
        libtxtr.DisposeLoadedImages()

    image = Image.open(r'D:\From Desktop\TXTRSearch\SAVETESTS\test.png').convert('RGBA')
    pixels = image.load()
    
    if not test_start('libtxtr.InitializeSaveImage', 'No exception',
                      test_libtxtr_InitializeSaveImage, image.width, image.height):
        return
    print('')

    failed = False
    for x in range(0, image.width):
        for y in range(0, image.height):
            pixel = pixels[x, y]
            if not test_start('libtxtr.SetPixel', 'No exception', test_libtxtr_SetPixel,
                              x, y, pixel[0], pixel[1], pixel[2], pixel[3]):
                failed = True
                break;
        if failed:
            break;
    if failed:
        return

    if not test_start('libtxtr.Save', 'No exception', test_libtxtr_Save,
                      r'D:\From Desktop\TXTRSearch\SAVETESTS\test.TXTR',
                      TextureFormat.RGBA32, PaletteFormat.IA8,
                      CopyPaletteSize.ToWidth, True, 250, 188, test_libtxtr_Save_progress):
        return
    print('')

    if libtxtr.IsSaveImageInitialized():
        libtxtr.DisposeSaveImage()

if __name__ == '__main__':
    main()

﻿// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Input;

namespace ConsoleApp1.Source;

public class Keyboard : InputAdapter
{
    /// <inheritdoc />
    public override bool KeyDown( int keycode )
    {
        var flag = keycode switch
        {
            IInput.Keys.UP   => true,
            IInput.Keys.DOWN => true,
            var _            => false,
        };

        return flag;
    }

    /// <inheritdoc />
    public override bool KeyUp( int keycode )
    {
        var flag = keycode switch
        {
            IInput.Keys.UP or IInput.Keys.RIGHT  => true,
            IInput.Keys.DOWN or IInput.Keys.LEFT => true,
            var _                                => false,
        };

        return flag;
    }
}
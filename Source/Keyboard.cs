// /////////////////////////////////////////////////////////////////////////////
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

using LughSharp.Lugh.Core;

namespace ConsoleApp1.Source;

public class Keyboard : InputAdapter
{
    /// <inheritdoc />
    public override bool KeyDown( int keycode )
    {
        bool flag;

        switch ( keycode )
        {
            case IInput.Keys.UP:
                flag = true;
                break;

            case IInput.Keys.DOWN:
                flag = true;
                break;

            default:
                flag = false;
                break;
        }

        return flag;
    }

    /// <inheritdoc />
    public override bool KeyUp( int keycode )
    {
        bool flag;

        switch ( keycode )
        {
            case IInput.Keys.UP:
            case IInput.Keys.RIGHT:
                if ( App.Camera != null )
                {
                    App.Camera.Zoom -= 1f;
                    App.Camera.Update();
                }

                flag = true;
                break;

            case IInput.Keys.DOWN:
            case IInput.Keys.LEFT:
                if ( App.Camera != null )
                {
                    App.Camera.Zoom += 1f;
                    App.Camera.Update();
                }

                flag = true;
                break;

            default:
                flag = false;
                break;
        }

        return flag;
    }
}
// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
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

using Corelib.Lugh.Assets;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Utils;

namespace ConsoleApp1.Source.Tests;

public class AssetManagerTest
{
    private readonly AssetManager _assetManager;

    public AssetManagerTest()
    {
        _assetManager = new AssetManager();
    }

    public void Run()
    {
        Logger.Divider();
        Logger.Debug( "Loading assets...", true );
        Logger.Divider();
        
        _assetManager.AddToLoadqueue( "libgdx.png", typeof( Texture ) );
        _assetManager.AddToLoadqueue( "biffbaff.png", typeof( Texture ) );
        _assetManager.AddToLoadqueue( "red7logo_small.png", typeof( Texture ) );

        Logger.Debug( "All assets queued for loading.", true );
        
        _assetManager.DisplayMetrics();
        
        Task.Run( () =>
        {
            _assetManager.FinishLoading();
        } );

        Logger.Debug( "Finished!", true );
    }
}
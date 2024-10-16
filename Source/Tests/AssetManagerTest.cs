// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

using LughSharp.LibCore.Assets;
using LughSharp.LibCore.Assets.Loaders;
using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics;
using LughSharp.LibCore.Utils;

namespace ConsoleApp1.Source.Tests;

public class AssetManagerTest
{
    private readonly AssetManager _assetManager;

    public AssetManagerTest()
    {
        Logger.Checkpoint();
        
        _assetManager = new AssetManager();
    }

    public async void Run()
    {
        Logger.Checkpoint();
        Logger.Debug( "Loading assets..." );
        
        _assetManager.Load( "libgdx.png", typeof( Texture ) );
        _assetManager.Load( "biffbaff.png", typeof( Texture ) );
        _assetManager.Load( "red7logo_small.png", typeof( Texture ) );

        DisplayAssetManagerMetrics();

        await _assetManager.FinishLoadingAsync();
        
        Logger.Debug( "Finished!" );
        
        DisplayAssetManagerMetrics();
    }

    private void DisplayAssetManagerMetrics()
    {
        Logger.Divider();
        Logger.Debug( $"LoadQueue Count  : {_assetManager.LoadQueue.Count}" );
        Logger.Debug( $"TaskQueue Count  : {_assetManager.TaskQueue.Count}" );
        Logger.Debug( $"AssetTypes Count : {_assetManager.AssetTypes.Count}" );
        Logger.Debug( $"Asset Count      : {_assetManager.Assets!.Count}" );
        Logger.Divider();
    }
}
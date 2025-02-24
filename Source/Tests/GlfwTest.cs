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

using JetBrains.Annotations;

namespace ConsoleApp1.Source.Tests;

[PublicAPI]
public class GlfwTest
{
//    private static long              windowHandle;
//    private static Glfw.ErrorCallback errorCallback = GLFWErrorCallback.createPrint( System.err );
//
//    public static void main( String[] argv )
//    {
//        Glfw.glfwSetErrorCallback( errorCallback );
//
//        if ( !glfwInit() )
//        {
//            System.out.println( "Couldn't initialize Glfw" );
//            System.exit( -1 );
//        }
//
//        glfwDefaultWindowHints();
//        glfwWindowHint( GLFW_VISIBLE, GLFW_FALSE );
//
//        // fullscreen, not current resolution, fails
//        Buffer modes = glfwGetVideoModes( glfwGetPrimaryMonitor() );
//
//        for ( int i = 0; i < modes.limit(); i++ )
//        {
//            System.out.println( modes.get( i ).width() + "x" + modes.get( i ).height() );
//        }
//
//        GLFWVidMode mode = modes.get( 7 );
//        System.out.println( "Mode: " + mode.width() + "x" + mode.height() );
//        windowHandle = glfwCreateWindow( mode.width(), mode.height(), "Test", glfwGetPrimaryMonitor(), 0 );
//
//        if ( windowHandle == 0 )
//        {
//            throw new RuntimeException( "Couldn't create window" );
//        }
//
//        glfwMakeContextCurrent( windowHandle );
//        GL.createCapabilities();
//        glfwSwapInterval( 1 );
//        glfwShowWindow( windowHandle );
//
//        IntBuffer tmp  = BufferUtils.createIntBuffer( 1 );
//        IntBuffer tmp2 = BufferUtils.createIntBuffer( 1 );
//
//        int fbWidth  = 0;
//        int fbHeight = 0;
//
//        while ( !glfwWindowShouldClose( windowHandle ) )
//        {
//            glfwGetFramebufferSize( windowHandle, tmp, tmp2 );
//
//            if ( fbWidth != tmp.get( 0 ) || fbHeight != tmp2.get( 0 ) )
//            {
//                fbWidth  = tmp.get( 0 );
//                fbHeight = tmp2.get( 0 );
//                System.out.println( "Framebuffer: " + tmp.get( 0 ) + "x" + tmp2.get( 0 ) );
//
//// GL11.glViewport(0, 0, tmp.get(0) * 2, tmp2.get(0) * 2);
//            }
//
//            GL11.glClear( GL11.GL_COLOR_BUFFER_BIT );
//            GL11.glBegin( GL11.GL_TRIANGLES );
//            GL11.glVertex2f( -1f, -1f );
//            GL11.glVertex2f( 1f, -1f );
//            GL11.glVertex2f( 0, 1f );
//            GL11.glEnd();
//            glfwSwapBuffers( windowHandle );
//            glfwPollEvents();
//        }
//
//        glfwDestroyWindow( windowHandle );
//        glfwTerminate();
//    }
}
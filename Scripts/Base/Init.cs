using Godot;
using System;
using System.IO;

namespace Base
{

    /// <summary>
    /// Simple values to run on init the game. Rigth now used only for debug
    /// For example, fullscreen, centered window, etc. 
    /// OJU! This has to be in the autoload!
    /// </summary>
    public class Init : Node
    {   
        private const bool CENTER_WINDOW = true;
        private const bool CONSOLE = true;

        public override void _EnterTree()
        {
            GD.Print("Entered the init script");
            this.Loadconfig();
        }

        /// <summary>
        /// Loads the config of the game-window-whatever
        /// </summary>
        private void Loadconfig()
        { 
            // set the window centered
            this.CenterWindow();
            MyConsole.IsOn = CONSOLE;
            //when finished, dispose the node, we don't need it
            base.QueueFree();

        }

        /// <summary>
        /// Centers the window in the screen
        /// <remarks>
        /// Seems like a simple script or something not worth worrying about,
        /// but when you pass all the day debugging the game and the game window
        /// appears at the top bottom, EVERY TIME, is a mess. 
        /// </remarks>
        /// </summary>
        private void CenterWindow(){

            if(CENTER_WINDOW == false ||OS.WindowFullscreen){
                return;
            }

            GD.Print("Init Script: Center window!");
            Vector2 winSize = OS.WindowSize;
            Vector2 screenSize = OS.GetScreenSize();
            Vector2 position = screenSize / 2 - winSize / 2;            
            OS.WindowPosition = position;
        }
    }


}

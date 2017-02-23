# LiveScan3D-Hololens
This application allows for streaming point clouds to a HoloLens and rendering them. Together with our other application, LiveScan3D, it can be used for viewing 3D data captured in real time (for example for telepresence) or pre-recorded data.

You will find a short presentation of the application in the video below (click to go to YouTube):
[![YouTube link](http://img.youtube.com/vi/Wc5z9OWFTTU/0.jpg)](http://www.youtube.com/watch?v=Wc5z9OWFTTU)

## How to get it running with LiveScan3D ##
First of all you need to download LiveScan and get that running. Once this is done, open LiveScan, connect a client and open the live view window (it is the contents of that window that are streamed to Hololens). Next start the Unity application on the same machine as the server, if everything is fine it should be working. If it is not working, let me know and I will try to fix it.

If you got this far, getting it to work on Hololens should be easy, just make sure you deploy the app using XAML UWP build type instead of D3D (this is required by the touch keyboard). Once you deploy and start the app on Hololens all you need to do is input the IP number of the server and you should see the point cloud.

## How to control the scene ##
The scene can be moved, scaled and rotated around the Y axis, the modes are chosen using the following voice commands:
 * Move - the scene can be moved using the tap and hold gesture.
 * Rotate - the scene can be rotated by using the tap and hold gesture and moving the hand horizontally.
 * Zoom - the scene can be scaled using the tap and hold gesture and moving the hand horizontally.
 * Reset - resets the scene to its original position, rotation and scale.

## Citations ##
If you use this software in your research, then please use the following citation:

Kowalski, M.; Naruniec, J.; Daniluk, M.: "LiveScan3D: A Fast and Inexpensive 3D Data Acquisition System for Multiple Kinect v2 Sensors". in 3D Vision (3DV), 2015 International Conference on, Lyon, France, 2015

## Authors ##
  * Marek Kowalski <m.kowalski@ire.pw.edu.pl>, homepage: http://home.elka.pw.edu.pl/~mkowals6/
  * Jacek Naruniec <j.naruniec@ire.pw.edu.pl>, homepage: http://home.elka.pw.edu.pl/~jnarunie/

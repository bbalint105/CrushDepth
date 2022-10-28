using Microsoft.Xna.Framework;
using System;
class Camera
{
public Vector3 Position;
public int angle = 0;
public Vector3 Direction = new Vector3( 0, 0, 0 );
public Vector3 Up = Vector3.Up;
public float AspectRatio = 1;
public Matrix View => Matrix.CreateLookAt( Position, Position + Direction, Up );
public Matrix Projection => Matrix.CreatePerspectiveFieldOfView( 1, AspectRatio, 1, 4000 );
public static readonly Camera Main = new Camera( );

}

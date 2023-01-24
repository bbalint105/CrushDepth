using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace CrushDepth{
    class Crystal : RigidBody{

        BasicGeometry cube;
        Vector3 pos;
        Vector3 color1;
        Vector3 color2;

        public Crystal(int Xmax, int Xmin, int Ymax, int Ymin, int Zstart, Hitmap Ceil, Hitmap Floor)
        {
            hitmap.oncoll += (other) => OnCollision(other);
            var rand = new Random();

            color1 = new Vector3(((float)rand.NextDouble()),((float)rand.NextDouble()),((float)rand.NextDouble()));
            color2 = new Vector3(((float)rand.NextDouble()),((float)rand.NextDouble()),((float)rand.NextDouble()));

            pos.X = rand.Next(Xmin, Xmax);
            pos.Z = rand.Next(Ymin, Ymax);
            pos.Y = Zstart;

            while(true){
                Hitbox hitbox = new Hitbox();
                hitbox.coords = pos-new Vector3(10,10,10);
                hitbox.acoords = pos+new Vector3(10,10,10);
                hitmap.hitboxes.Add(hitbox);

                if(Zstart > 0){
                    if(hitmap.CollideB(Ceil)){
                        pos.Y = pos.Y - 5;
                        Hitmap.hitmaps.Remove(hitmap);
                        hitmap.hitboxes.Clear();
                    }
                    else{
                        break;
                    }
                }else{
                    if(hitmap.CollideB(Floor)){
                        pos.Y = pos.Y + 5;
                        Hitmap.hitmaps.Remove(hitmap);
                        hitmap.hitboxes.Clear();
                    }
                    else{
                        break;
                    }
                }


            }
            // Hitmap.hitmaps.Remove(hitmap);
            // hitmap.hitboxes.Clear();
            // pos = new Vector3(1,1,1);
            // Hitbox hitboxd = new Hitbox();
            // hitboxd.coords = pos-new Vector3(10,10,10);
            // hitboxd.acoords = pos+new Vector3(10,10,10);
            // hitmap.hitboxes.Add(hitboxd);
        }

        public void LoadContent(GraphicsDevice gd)
        {
            cube = BasicGeometry.CreateCube( gd);
        }

        public void Draw(){
            cube.Effect.DiffuseColor = color1;
            cube.Draw( Matrix.CreateScale( 10 ) * Matrix.CreateTranslation(pos), Camera.Main.View, Camera.Main.Projection );
            cube.Effect.DiffuseColor = color2;
            cube.Draw( (Matrix.CreateScale( 10 )*Matrix.CreateRotationX(0.785f)*Matrix.CreateRotationY(0.785f)*Matrix.CreateRotationZ(0.785f) * Matrix.CreateTranslation(pos)), Camera.Main.View, Camera.Main.Projection );
        }


        public void DrawCargo(GraphicsDevice gd,int pos){
            var rand = new Random();
            cube.Effect.DiffuseColor = color1;
            cube.Draw( Matrix.Identity*Matrix.CreateTranslation(-9,-18f,15.2f-pos*3.4f),
                        Matrix.CreateLookAt( new Vector3( 30, 0, 0 ), Vector3.Zero, Vector3.Up ),
                        Matrix.CreatePerspectiveFieldOfView( 1, gd.Viewport.AspectRatio, 1, 100 ) );
            cube.Effect.DiffuseColor = color2;
            cube.Draw( Matrix.Identity*Matrix.CreateRotationX(0.785f)*Matrix.CreateRotationY(0.785f)*Matrix.CreateRotationZ(0.785f)*Matrix.CreateTranslation(-9,-18f,15.2f-pos*3.4f),
                        Matrix.CreateLookAt( new Vector3( 30, 0, 0 ), Vector3.Zero, Vector3.Up ),
                        Matrix.CreatePerspectiveFieldOfView( 1, gd.Viewport.AspectRatio, 1, 100 ) );
            }

        void OnCollision(Hitbox other){ //nem megy :c
            Console.WriteLine("GEMGEMGEM");
        }

    }
}
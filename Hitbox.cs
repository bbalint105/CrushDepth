using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CrushDepth{
    delegate void onCollisionEvent(Hitbox hm);

    class Hitmap{

        static List<Hitmap> hitmaps = new List<Hitmap>();

        public static void CollideAll(){
            for (int i = hitmaps.Count-1; i >= 0; i--)
            {
                for (int j = hitmaps.Count-1; j >= 0; j--)
                {
                    if(j!=i)
                        hitmaps[j].Collide(hitmaps[i]);
                }
            }
        }



        public List<Hitbox> hitboxes;
        public event onCollisionEvent oncoll;

        public RigidBody parent;

        public Hitmap(RigidBody parent){
            this.parent = parent;
            hitboxes = new List<Hitbox>();
            hitmaps.Add(this);
        }

        public void Dispose(){
            hitmaps.Remove(this);
        }

        public bool Collide(Hitmap b){
            foreach (Hitbox i in hitboxes){
                foreach(Hitbox j in b.hitboxes){
                    if(i != j && i != null && j != null){
                        if(i.Intersects(j)){
                            this.oncoll?.Invoke(j);
                            b.oncoll?.Invoke(i);
                        }
                    }
                }
            }
            return false;
        }
    }


    class Hitbox{
        public Vector3 coords;
        public Vector3 acoords;


        public Hitbox() {
            coords = new Vector3();
            acoords = new Vector3();
        }


        public Vector3 min{
            get{
                return new Vector3(
                    Math.Min(coords.X,acoords.X),
                    Math.Min(coords.Y,acoords.Y),
                    Math.Min(coords.Z,acoords.Z)
                );
            }
        }
        public Vector3 max{
            get{
                return new Vector3(
                    Math.Max(coords.X,acoords.X),
                    Math.Max(coords.Y,acoords.Y),
                    Math.Max(coords.Z,acoords.Z)
                );
            }
        }

        
        public bool Intersects(Hitbox other) {
            return (
                this.min.X <= other.max.X &&
                this.max.X >= other.min.X &&
                this.min.Y <= other.max.Y &&
                this.max.Y >= other.min.Y &&
                this.min.Z <= other.max.Z &&
                this.max.Z >= other.min.Z
            );
        }

    }
}
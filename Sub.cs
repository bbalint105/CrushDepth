using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrushDepth
{
    class Sub : Body, IDrawable, ISteppable
    {
        Model model;
        BasicEffect eff;
        public bool ctrlW, ctrlS, ctrlA, ctrlD, ctrlX, ctrlY, ctrlQ, ctrlE;

        public float ballast_level;
        public Hitbox hitbox1;


        static Matrix modelOffset = Matrix.CreateTranslation(new Vector3(0, -5, -7));

        public Vector3 Position => ((verlets[0].Pos + verlets[1].Pos + verlets[2].Pos + verlets[3].Pos + verlets[4].Pos + verlets[5].Pos + verlets[6].Pos + verlets[7].Pos)) * 0.125f;
        public Vector3 Direction => (verlets[2].Pos + verlets[3].Pos) / 2 - (verlets[6].Pos + verlets[7].Pos) / 2;
        public Vector3 Right => verlets[12].Pos - verlets[13].Pos;
        public Vector3 Up => verlets[9].Pos - verlets[10].Pos;
        public Matrix WorldTransform => Matrix.CreateWorld(Position, Vector3.Normalize(Direction), Vector3.Normalize(Up));

        Matrix localTransform = Matrix.CreateScale(0.035f) * modelOffset;

        public Sub(GraphicsDevice dev, Model model, Texture2D tex)
        {
            this.model = model;

            eff = new BasicEffect(dev);
            eff.Texture = tex;
            eff.TextureEnabled = true;

            hitbox1 = new Hitbox();

            hitmap.oncoll += (other) => OnCollision(other);

            float w = 5, l = 10, h = 5;
            verlets = new Verlet[]
            {
            new Verlet( new Vector3( l, h, -w ) ),
            new Verlet( new Vector3( l, h, w ) ),
            new Verlet( new Vector3( l, -h, -w ) ),
            new Verlet( new Vector3( l, -h, w ) ),
            new Verlet( new Vector3( -l, h, -w ) ),
            new Verlet( new Vector3( -l, h, w ) ),
            new Verlet( new Vector3( -l, -h, -w ) ),
            new Verlet( new Vector3( -l, -h, w ) ),
            new Verlet( new Vector3( l+4, 0, 0 )),
            new Verlet( new Vector3( 0, h+4, 0 ) ),
            new Verlet( new Vector3( 0, -h-4, 0 ) ),
            new Verlet( new Vector3( -l-8, h, 0 ) ),
            new Verlet( new Vector3( 0, 0, w+2 ) ),
            new Verlet( new Vector3( 0, 0, -w-2 ) ),
            new Verlet( new Vector3( l/2, 0, 0 ) )
            };

            GenerateFullyConnectedBody();

            hitmap.hitboxes.Add(hitbox1);
        }

        public void Draw(Camera cam)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = (localTransform * WorldTransform);
                    effect.View = cam.View;
                    effect.Projection = cam.Projection;
                    effect.Texture = eff.Texture;
                    effect.TextureEnabled = true;
                }
            }


            model.Draw(localTransform * WorldTransform, cam.View, cam.Projection);
        }

        private void ApplyForces()
        {
            Vector3 d = Vector3.Normalize(Direction);
            Vector3 r = Vector3.Normalize(Right);
            Vector3 u = Vector3.Normalize(Up);


            Vector3 g = new Vector3(0, -9.81f, 0);
            for (int i = 0; i < verlets.Length; i++)
            {
                if (i == 2 || i == 3 || i == 6 || i == 7)
                {
                    verlets[i].Acc = 5 * g;
                }
                else
                if (i == 10)
                {
                    verlets[i].Acc = 50 * g;
                }
                else
                if (i == 14)
                {
                    verlets[i].Acc = ballast_level * g;
                }
                else
                    verlets[i].Acc = g;
            }



            for (int i = 0; i < verlets.Length; i++)
            {
                verlets[i].AddSqFriction(d, 0.5f);
                verlets[i].AddSqFriction(r, 0.5f);
                verlets[i].AddSqFriction(u, 0.5f);
            }




            if (ctrlW)
            {
                verlets[6].Acc += d * 1600;
                verlets[7].Acc += d * 1600;


                // TODAO
                verlets[11].Acc += u * 900;

            }
            if (ctrlS)
            {
                verlets[6].Acc -= d * 500;
                verlets[7].Acc -= d * 500;
            }
            if (ctrlA)
            {
                verlets[11].Acc += r * 200;

            }
            if (ctrlD)
            {
                verlets[11].Acc -= r * 200;
            }
            if (ctrlX)
            {
                ballast_level += 1;
            }
            if (ctrlY)
            {
                ballast_level -= 1;
            }
        }

        void OnCollision(Hitbox other){
            Vector3 d = Vector3.Normalize(Direction);
            Vector3 r = Vector3.Normalize(Right);
            Vector3 u = Vector3.Normalize(Up);

            Vector3 notnormal = (other.acoords + other.coords)/2;
            Verlet mini = verlets[14];
            for (int i = 0; i < verlets.Length; i++)
                {
                    if(((notnormal-verlets[i].Pos).Length()) < ((notnormal-mini.Pos).Length())){
                        mini = verlets[i];
                    }
                }
            Vector3 force = (Vector3.Normalize(mini.Pos-notnormal)*3f)*new Vector3(1,0.1f,1);
            mini.Pos += force;


            //verlets[14].Pos -= d*2;
            ApplyConstraints();
            // for (int i = 0; i < verlets.Length; i++)
            //     {
            //         verlets[i].Acc += -2000*d;
            //         verlets[i].AddSqFriction(d, 1f);
            //         verlets[i].AddSqFriction(r, 1f);
            //         verlets[i].AddSqFriction(u, 1f);
            //     }
        }

        public void Step()
        {

            hitbox1.coords = verlets[0].Pos;
            hitbox1.acoords = verlets[7].Pos;

            ApplyForces();
            for (int i = 0; i < verlets.Length; i++)
                verlets[i].Step();
            ApplyConstraints();
            if (ballast_level > 200) ballast_level = 200;
            if (ballast_level < -200) ballast_level = -200;


        }


    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CrushDepth
{
    public class CrushDepth : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        List<ISteppable> steppables = new List<ISteppable>();

        BasicGeometry cube;
        Sky sky;
        Sub sub;
        FloorCeil floor;
        FloorCeil ceiling;
        Spikerocks spikerocks;
        HUD hud;


        Point mousePrev;
        int scrollPrev;

        Vector2 angle;
        int Scale = 50;
        public CrushDepth()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            base.Initialize();

            Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += ( s, e ) =>
                {
            this.graphics.PreferredBackBufferWidth = this.Window.ClientBounds.Width;
            this.graphics.PreferredBackBufferHeight = this.Window.ClientBounds.Height;
            graphics.ApplyChanges();
                };
            Camera.Main.Position = new Vector3( 50, 20, 0 );
        }

        protected override void LoadContent()
        {

            cube = BasicGeometry.CreateCube( GraphicsDevice);
            

            sky = new Sky( GraphicsDevice, Content.Load<Texture2D>( "skyhalf" ) );
            sub = new Sub( GraphicsDevice, Content.Load<Model>( "sub" ), Content.Load<Texture2D>("red_sub_tex") );
            steppables.Add(sub);
            floor = new FloorCeil(GraphicsDevice, null, true);
            ceiling = new FloorCeil(GraphicsDevice, null, false);
            
            spikerocks = new Spikerocks( GraphicsDevice, Content.Load<Texture2D>( "spikeColor" ), Content.Load<Texture2D>( "spikeHeight" ) );

            hud = new HUD( Content.Load<SpriteFont>( "SF" ), sub );

            spriteBatch = new SpriteBatch(GraphicsDevice);
            hud.LoadContent(spriteBatch);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboard = Keyboard.GetState( );


            

            var mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed){
                Point delta = mouseState.Position - mousePrev;
                angle.Y += delta.Y*(float)(Math.PI)/1800;
                angle.X -= delta.X*(float)(Math.PI)/1800;

                if (angle.X >= Math.PI*2) angle.X=0;
                if (angle.Y >= Math.PI/3) angle.Y=(float)(Math.PI/3);
                if (angle.X <= -Math.PI*2) angle.X=0;
                if (angle.Y <= -Math.PI/6) angle.Y=-(float)(Math.PI/6);

            }
            mousePrev = mouseState.Position;

            int scrolldelta = mouseState.ScrollWheelValue - scrollPrev;
            Scale -= (scrolldelta)/30;

                if (Scale > 150) Scale=150;
                if (Scale < 60) Scale=60;

            scrollPrev = mouseState.ScrollWheelValue;



            Camera.Main.Position = new Vector3((float)(Math.Sin(angle.X)*Math.Cos(angle.Y)),(float)Math.Sin(angle.Y),(float)(Math.Cos(angle.X)*Math.Cos(angle.Y)))*Scale;
            Camera.Main.Position += sub.Position;

            Camera.Main.Direction = Vector3.Normalize(sub.Position-Camera.Main.Position);


            sub.ctrlW = keyboard.IsKeyDown( Keys.W ); //refactor
            sub.ctrlA = keyboard.IsKeyDown( Keys.A );
            sub.ctrlS = keyboard.IsKeyDown( Keys.S );
            sub.ctrlD = keyboard.IsKeyDown( Keys.D );
            sub.ctrlX = keyboard.IsKeyDown( Keys.X );
            sub.ctrlY = keyboard.IsKeyDown( Keys.Y );
            sub.ctrlQ = keyboard.IsKeyDown( Keys.Q );
            sub.ctrlE = keyboard.IsKeyDown( Keys.E );

            for(int i = steppables.Count-1; i >= 0; i--){
                steppables[i].Step();
            }
            
            
            Hitmap.CollideAll();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DeepPink);

            Camera.Main.AspectRatio = GraphicsDevice.Viewport.AspectRatio;
            //sky.Draw( Camera.Main );
            floor.Draw(Camera.Main, -500);
            ceiling.Draw(Camera.Main, 500);
            foreach( var m in new Matrix[ ]
                {
                Matrix.CreateScale( 3200, 800, 3200 )* Matrix.CreateTranslation( -1600, -499, -1600 ),
                } )


            spikerocks.Draw( m, Camera.Main );


            hud.Draw( spriteBatch );
            graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;


            sub.Draw(Camera.Main);

            cube.Effect.DiffuseColor = new Vector3(1,0,0);
            cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(1,0,0)), Camera.Main.View, Camera.Main.Projection );
            cube.Effect.DiffuseColor = new Vector3(0,1,0);
            cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(0,1,0)), Camera.Main.View, Camera.Main.Projection );
            cube.Effect.DiffuseColor = new Vector3(0,0,1);
            cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(0,0,1)), Camera.Main.View, Camera.Main.Projection );
            cube.Effect.DiffuseColor = new Vector3(1,1,1);

            // foreach(Hitbox v in spikerocks.hitmap.hitboxes){
            //     Vector3 a = v.acoords;
            //     Vector3 b = v.coords;

            //     cube.Effect.DiffuseColor = new Vector3(1,1,1);
            //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( a), Camera.Main.View, Camera.Main.Projection );
            //     cube.Effect.DiffuseColor = new Vector3(1,0,0);
            //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( b), Camera.Main.View, Camera.Main.Projection );
            // }



            // int i = 0;
            // foreach(Verlet v in sub.verlets){
            //     if (i == 6 || i == 7){
            //         cube.Effect.DiffuseColor = new Vector3(1,0,0);
            //     }
            //     else
            //         cube.Effect.DiffuseColor = new Vector3(0,0,0);
            //     cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(v.Pos.X,v.Pos.Y,v.Pos.Z)), Camera.Main.View, Camera.Main.Projection );
            //     i++;

            // }
            //     cube.Effect.DiffuseColor = new Vector3(1,0,1);
            //     cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( sub.Direction), Camera.Main.View, Camera.Main.Projection );
            //     cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( sub.Up), Camera.Main.View, Camera.Main.Projection );
            //     cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( sub.Right), Camera.Main.View, Camera.Main.Projection );



            // cube.Effect.DiffuseColor = new Vector3(0.5f,0.5f,0.5f);
            // Vector3 o = sub.hitmap.hitboxes[0].coords;
            // Vector3 p = sub.hitmap.hitboxes[0].acoords;
            // cube.Draw( Matrix.CreateScale( 3 ) * Matrix.CreateTranslation( new Vector3(o.X,o.Y,o.Z)), Camera.Main.View, Camera.Main.Projection );
            // cube.Draw( Matrix.CreateScale( 3 ) * Matrix.CreateTranslation( new Vector3(p.X,p.Y,p.Z)), Camera.Main.View, Camera.Main.Projection );

            // cube.Effect.DiffuseColor = new Vector3(1f,0.5f,0.5f);
            // o = spikerocks.hitmap.hitboxes[0].coords;
            // p = spikerocks.hitmap.hitboxes[0].acoords;
            // cube.Draw( Matrix.CreateScale( 3 ) * Matrix.CreateTranslation( new Vector3(o.X,o.Y,o.Z)), Camera.Main.View, Camera.Main.Projection );
            // cube.Draw( Matrix.CreateScale( 3 ) * Matrix.CreateTranslation( new Vector3(p.X,p.Y,p.Z)), Camera.Main.View, Camera.Main.Projection );

            


            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CrushDepth
{
    public class CrushDepth : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Boolean inMenu = true;

        List<ISteppable> steppables = new List<ISteppable>();

        BasicGeometry cube;

        List<Sky> skies = new List<Sky>();

        List<Crystal> cry = new List<Crystal>();


        Song bgm;

        Sub sub;
        FloorCeil floor;
        FloorCeil ceiling;

        Spikerocks spikerocks;
        Spikerocks spikerocksc;

        Spikerocks spikerockss;
        Spikerocks spikerockscs;

        List<SoundEffect> soundEffects = new List<SoundEffect>();
        SoundEffectInstance motor;
        SoundEffectInstance metalpipe;
        SoundEffectInstance sonar;
        SoundEffectInstance honk;
        SoundEffectInstance pickup;

        HUD hud;
        MainMenu Menu;


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

        public void LoadMenu(){
            steppables.Clear();
            skies.Clear();
            Hitmap.hitmaps.Clear();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Menu = new MainMenu(Content.Load<SpriteFont>( "SF" ));
            Menu.LoadContent(spriteBatch, Content);
        }

        protected override void LoadContent()
        {
            bgm = Content.Load<Song>("bgm");
            MediaPlayer.Volume = 0.06f;
            MediaPlayer.Play(bgm);
            MediaPlayer.IsRepeating = true;
        }
        protected void LoadGame(){

            cube = BasicGeometry.CreateCube( GraphicsDevice);
            soundEffects.Add(Content.Load<SoundEffect>("motor"));
            soundEffects.Add(Content.Load<SoundEffect>("metalpipe"));
            soundEffects.Add(Content.Load<SoundEffect>("pickup"));
            soundEffects.Add(Content.Load<SoundEffect>("honk"));
            soundEffects.Add(Content.Load<SoundEffect>("sonar"));
            motor = soundEffects[0].CreateInstance();
            motor.IsLooped = true;
            motor.Stop();
            metalpipe = soundEffects[1].CreateInstance();
            metalpipe.Volume = 0.3f;
            metalpipe.Stop();
            pickup = soundEffects[2].CreateInstance();
            pickup.Volume = 0.1f;
            pickup.Stop();
            sonar = soundEffects[4].CreateInstance();
            sonar.Volume = 0.8f;
            sonar.IsLooped = true;
            sonar.Stop();
            honk = soundEffects[3].CreateInstance();
            honk.Volume = 0.5f;
            honk.Stop();

            int startdis = 300;
            int density = 0;
            int step = 10;

            for(int i =0; i<255; i++){
                skies.Add(new Sky( GraphicsDevice, Content.Load<Texture2D>( "skyhalf" ), startdis+density));
                density = density + step;
            }
            skies.Reverse();

            sub = new Sub( GraphicsDevice, Content.Load<Model>( "sub" ), Content.Load<Texture2D>("red_sub_tex") );
            sub.cargo.Clear();
            sub.metalpipe = metalpipe;
            steppables.Add(sub);
            floor = new FloorCeil(GraphicsDevice, null, true);
            ceiling = new FloorCeil(GraphicsDevice, null, false);
            
            spikerocks = new Spikerocks( GraphicsDevice, Content.Load<Texture2D>( "spikeColor" ), Content.Load<Texture2D>( "spikeHeight" ), false );
            spikerocksc = new Spikerocks( GraphicsDevice, Content.Load<Texture2D>( "spikeColor" ), Content.Load<Texture2D>( "spikeHeightc" ), true );

            spikerockss = new Spikerocks( GraphicsDevice, Content.Load<Texture2D>( "spikeColorSonar" ), Content.Load<Texture2D>( "spikeHeight" ), false );
            spikerockscs = new Spikerocks( GraphicsDevice, Content.Load<Texture2D>( "spikeColorSonar" ), Content.Load<Texture2D>( "spikeHeightc" ), true );


            for(int i =0; i<5; i++){
                cry.Add(new Crystal(1600,-1600,1600,-1600,-498,spikerocksc.hitmap,spikerocks.hitmap));
            }
            for(int i =0; i<5; i++){
                cry.Add(new Crystal(1600,-1600,1600,-1600,300,spikerocksc.hitmap,spikerocks.hitmap));
            }
            cry.Reverse();
            foreach(Crystal c in cry){
                c.LoadContent(GraphicsDevice);
            }

            hud = new HUD( Content.Load<SpriteFont>( "SF" ), sub );
            hud.LoadContent(spriteBatch);
        }


        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState( );
            if(!inMenu){
                
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
                sub.ctrlSpace = keyboard.IsKeyDown( Keys.Space );

                if (sub.ctrlW || sub.ctrlS){
                    motor.Play();
                }else
                    motor.Pause();

                if (sub.ctrlSpace){
                    sonar.Play();
                }else
                    sonar.Pause();

                if (sub.ctrlE)
                    honk.Play();

                if(keyboard.IsKeyDown( Keys.Escape )){
                    inMenu=true;
                    motor.Stop();
                    sonar.Stop();
                }
                    

                for(int i = steppables.Count-1; i >= 0; i--){
                    steppables[i].Step();
                }
                
                Hitmap.CollideAll(sub.hitmap);

                if (sub.health<=-200){
                    motor.Stop();
                    sonar.Stop();
                    inMenu = true;
                }
                if (sub.electricity<=-200){
                    sonar.Stop();
                    sub.Sonar = false;
                }

                Crystal removable= null;
                foreach(Crystal c in cry){
                    if(c.hitmap.CollideB(sub.hitmap)){
                        Hitmap.hitmaps.Remove(c.hitmap);
                        removable = c;
                        sub.cargo.Add(c);
                        pickup.Play();
                    }
                }
                cry.Remove(removable);
            }
            else{
                LoadMenu();
                if(keyboard.IsKeyDown( Keys.Enter )){
                    inMenu = false;
                    LoadGame();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if(!inMenu){
                Camera.Main.AspectRatio = GraphicsDevice.Viewport.AspectRatio;
                
                floor.Draw(Camera.Main, -500);
                ceiling.Draw(Camera.Main, 500);

                if (!sub.Sonar){
                    foreach( var m in new Matrix[ ]
                    {
                        Matrix.CreateScale( 3200, 800, 3200 )* Matrix.CreateTranslation( -1600, -499, -1600 ),
                    } ){
                        spikerocks.Draw( m, Camera.Main );
                    }
                
                foreach( var m in new Matrix[ ]
                    {
                        Matrix.CreateScale( 3200, -800, 3200 )* Matrix.CreateTranslation( -1600, 301, -1600 ),
                    } ){
                        spikerocksc.Draw( m, Camera.Main );
                    }
                }
                else {
                    foreach( var m in new Matrix[ ]
                    {
                        Matrix.CreateScale( 3200, 800, 3200 )* Matrix.CreateTranslation( -1600, -499, -1600 ),
                    } ){
                        spikerockss.Draw( m, Camera.Main );
                    }
                
                foreach( var m in new Matrix[ ]
                    {
                        Matrix.CreateScale( 3200, -800, 3200 )* Matrix.CreateTranslation( -1600, 301, -1600 ),
                    } ){
                        spikerockscs.Draw( m, Camera.Main );
                    }
                }
                foreach (Crystal c in cry)
                {
                    c.Draw();
                }

                if (!sub.Sonar){
                    foreach (Sky sky in skies)
                    {
                        sky.Draw(Camera.Main, sub.Position);
                    }
                }

                hud.Draw( spriteBatch );
                graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (Crystal c in sub.cargo)
                {
                    c.DrawCargo(GraphicsDevice, sub.cargo.IndexOf(c));
                }

                sub.Draw(Camera.Main);

                // cube.Effect.DiffuseColor = new Vector3(1,0,0);
                // cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(1,0,0)), Camera.Main.View, Camera.Main.Projection );
                // cube.Effect.DiffuseColor = new Vector3(0,1,0);
                // cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(0,1,0)), Camera.Main.View, Camera.Main.Projection );
                // cube.Effect.DiffuseColor = new Vector3(0,0,1);
                // cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(0,0,1)), Camera.Main.View, Camera.Main.Projection );
                // cube.Effect.DiffuseColor = new Vector3(1,1,1);


                // foreach(Hitbox v in spikerocks.hitmap.hitboxes){
                //     Vector3 a = v.acoords;
                //     Vector3 b = v.coords;

                //     cube.Effect.DiffuseColor = new Vector3(1,1,1);
                //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( a), Camera.Main.View, Camera.Main.Projection );
                //     cube.Effect.DiffuseColor = new Vector3(1,0,0);
                //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( b), Camera.Main.View, Camera.Main.Projection );
                // }
                // foreach(Hitbox v in spikerocksc.hitmap.hitboxes){
                //     Vector3 a = v.acoords;
                //     Vector3 b = v.coords;

                //     cube.Effect.DiffuseColor = new Vector3(1,1,1);
                //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( a), Camera.Main.View, Camera.Main.Projection );
                //     cube.Effect.DiffuseColor = new Vector3(1,0,0);
                //     cube.Draw( Matrix.CreateScale( 5 ) * Matrix.CreateTranslation( b), Camera.Main.View, Camera.Main.Projection );
                // }

                // int i = 0;s
                // foreach(Verlet v in sub.verlets){
                //     if (i == 6 || i == 7){
                //         cube.Effect.DiffuseColor = new Vector3(1,0,0);
                //     }
                //     else
                //         cube.Effect.DiffuseColor = new Vector3(0,0,0);
                //     cube.Draw( Matrix.CreateScale( 0.5f ) * Matrix.CreateTranslation( new Vector3(v.Pos.X,v.Pos.Y,v.Pos.Z)), Camera.Main.View, Camera.Main.Projection );
                //     i++;
                // }

                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox1.acoords), Camera.Main.View, Camera.Main.Projection );
                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox1.coords), Camera.Main.View, Camera.Main.Projection );

                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox2.acoords), Camera.Main.View, Camera.Main.Projection );
                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox2.coords), Camera.Main.View, Camera.Main.Projection );

                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox3.acoords), Camera.Main.View, Camera.Main.Projection );
                // cube.Draw( Matrix.CreateScale( 2 ) * Matrix.CreateTranslation( sub.hitbox3.coords), Camera.Main.View, Camera.Main.Projection );
            }
            else{
                Menu.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrushDepth{
    class MainMenu{

        SpriteFont font;
        Texture2D Blackbar;
        Texture2D Sub;


        public MainMenu( SpriteFont font)
        {
            this.font = font;
        }

        public void LoadContent(SpriteBatch spriteBatch, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            Blackbar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Blackbar.SetData(new[] { Color.Black });
            Sub = Content.Load<Texture2D>("subicon");
        }

        public void Draw( SpriteBatch spriteBatch ){

            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;

            spriteBatch.Begin();
            spriteBatch.Draw(Blackbar, new Rectangle(w, h, w, h), Color.Chocolate);
            spriteBatch.Draw(Sub, new Vector2( w/2-Sub.Width/2, h/2-h/7f), Color.White);

            spriteBatch.DrawString( font, "Crush Depth",new Vector2( w/2-font.MeasureString("Crush Depth").X/2, h/2-h/5), Color.White );
            spriteBatch.End();

        }

    }
}



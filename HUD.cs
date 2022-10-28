using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrushDepth{
    class HUD{

        Sub sub;
        SpriteFont font;


        Texture2D Blackbar;
        Texture2D Bluebar;


        public HUD( SpriteFont font, Sub sub )
        {
            this.font = font;
            this.sub = sub;

        }

        public void LoadContent(SpriteBatch spriteBatch)
        {
            Blackbar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Blackbar.SetData(new[] { Color.Black });
            Bluebar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Bluebar.SetData(new[] { Color.RoyalBlue });
        }

        public void Draw( SpriteBatch spriteBatch ){

            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;

            float bl = -sub.ballast_level/2;

            int fuelbar_posX = w-w/6+w/24;
            int fuelbar_posY = (h-h/6)+(int)bl*h/6/100-h/12;
            int fuelbar_height = (int)(h/6-bl*h/6/100);
            int fuelbar_width = w/12;

            spriteBatch.Begin();
            spriteBatch.Draw(Blackbar, new Rectangle(w-w/6, h-h/2, w/6, h/2), Color.Chocolate);
            spriteBatch.Draw(Bluebar, new Rectangle(fuelbar_posX, fuelbar_posY, fuelbar_width, fuelbar_height), Color.RoyalBlue);

            spriteBatch.DrawString( font, "WATER",new Vector2( w-w/6+w/24, h-h/2+h/96 ), Color.White );
            spriteBatch.DrawString( font, "LEVEL",new Vector2( w-w/6+w/24, h-h/12+h/96 ), Color.White );
            spriteBatch.DrawString( font, ((int)((sub.ballast_level+200)/4)).ToString(),new Vector2( fuelbar_posX-w/48-w/96, fuelbar_posY ), Color.White );
            spriteBatch.End();

        }

    }
}



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrushDepth{
    class HUD{

        Sub sub;
        SpriteFont font;
        Texture2D Blackbar;
        Texture2D Bluebar;

        Texture2D BlackbarHP;
        Texture2D Greenbar;

        Texture2D BlackbarI;
        Texture2D BlackbarIt;

        Texture2D BlackbarE;
        Texture2D Yellowbar;

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

            BlackbarHP = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            BlackbarHP.SetData(new[] { Color.Black });
            Greenbar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Greenbar.SetData(new[] { Color.LimeGreen });

            BlackbarI = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            BlackbarI.SetData(new[] { Color.Black });

            BlackbarIt = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            BlackbarIt.SetData(new[] { Color.Black });

            BlackbarE = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            BlackbarE.SetData(new[] { Color.Black });

            Yellowbar = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            Yellowbar.SetData(new[] { Color.LightYellow });
        }

        public void Draw( SpriteBatch spriteBatch ){

            int w = spriteBatch.GraphicsDevice.Viewport.Width;
            int h = spriteBatch.GraphicsDevice.Viewport.Height;

            float bl = -sub.ballast_level/2;
            float hp = -sub.health/2;


            int fuelbar_posX = w-w/6+w/24;
            int fuelbar_posY = (h-h/6)+(int)bl*h/6/100-h/12;
            int fuelbar_height = (int)(h/6-bl*h/6/100);
            int fuelbar_width = w/12;

            int hpbar_posX = w/24;
            int hpbar_posY = (h-h/6)+(int)hp*h/6/100-h/12;
            int hpbar_height = (int)(h/6-hp*h/6/100);
            int hpbar_width = w/12;

            int ebar_posX = w/4+w/48;
            int ebar_posY = h/48;
            int ebar_w = (w-w/2-w/24)*((int)(sub.electricity+200)/4)/100;
            int ebar_h = (h/6-h/12);

            spriteBatch.Begin();
            spriteBatch.Draw(Blackbar, new Rectangle(w-w/6, h-h/2, w/6, h/2), Color.Chocolate);
            spriteBatch.Draw(Bluebar, new Rectangle(fuelbar_posX, fuelbar_posY, fuelbar_width, fuelbar_height), Color.RoyalBlue);

            spriteBatch.Draw(BlackbarHP, new Rectangle(0, h-h/2, w/6, h/2), Color.Chocolate);
            spriteBatch.Draw(Greenbar, new Rectangle(hpbar_posX, hpbar_posY, hpbar_width, hpbar_height), Color.LimeGreen);

            spriteBatch.DrawString( font, "WATER",new Vector2( w-w/6+w/24, h-h/2+h/96 ), Color.White );
            spriteBatch.DrawString( font, "LEVEL",new Vector2( w-w/6+w/24, h-h/12+h/96 ), Color.White );
            spriteBatch.DrawString( font, ((int)((sub.ballast_level+200)/4)).ToString(),new Vector2( fuelbar_posX-w/48-w/96, fuelbar_posY ), Color.White );

            spriteBatch.DrawString( font, "HULL",new Vector2( w/24, h-h/2+h/96 ), Color.White );
            spriteBatch.DrawString( font, ((int)((sub.health+200)/4)).ToString(),new Vector2( hpbar_posX-w/48-w/96, hpbar_posY ), Color.White );

            spriteBatch.Draw(BlackbarI, new Rectangle(w/4, h-h/6, w-w/2, h/6), Color.Chocolate);
            spriteBatch.Draw(BlackbarIt, new Rectangle(w/2-((int)font.MeasureString("CARGO").X+10)/2, h-h/6-(int)font.MeasureString("CARGO").Y, (int)font.MeasureString("CARGO").X+10, (int)font.MeasureString("CARGO").Y), Color.Chocolate);
            spriteBatch.DrawString( font, "CARGO",new Vector2( w/2-((int)font.MeasureString("CARGO").X)/2, h-h/6-(int)font.MeasureString("CARGO").Y ), Color.White );

            spriteBatch.Draw(BlackbarE, new Rectangle(w/4, 0, w-w/2, h/6), Color.Chocolate);
            spriteBatch.DrawString( font, "BATTERY",new Vector2( w/2-((int)font.MeasureString("BATTERY").X)/2, h/6-(int)font.MeasureString("BATTERY").Y ), Color.White );
            spriteBatch.Draw(Yellowbar, new Rectangle(w/4+w/48, h/48, ebar_w, ebar_h), Color.Yellow);

            spriteBatch.End();

        }

    }
}



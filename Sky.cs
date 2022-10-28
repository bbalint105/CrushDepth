using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrushDepth{
    class Sky
    {
    BasicGeometry model;
    public Sky( GraphicsDevice dev, Texture2D tex )
    {
    model = BasicGeometry.CreateSphere( dev, 15, 7,
    v => new VertexPositionTexture( v.Position,
    new Vector2( v.TextureCoordinate.X, 1 - v.TextureCoordinate.Y ) ) );
    model.Effect.Texture = tex;
    model.Effect.LightingEnabled = false;
    model.Effect.TextureEnabled = true;
    }
    public void Draw( Camera cam )
    {
        model.Effect.World = Matrix.CreateScale( 1000 ) * Matrix.CreateTranslation( cam.Position );
        model.Draw(model.Effect.World, cam.View, cam.Projection);
    }
    }
}

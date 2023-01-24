using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrushDepth{
    class Sky
    {
    BasicGeometry model;
    int dis;
    public Sky( GraphicsDevice dev, Texture2D tex, int dis )
    {
    model = BasicGeometry.CreateSphere( dev, 15, 7,
    v => new VertexPositionTexture( v.Position,
    new Vector2( v.TextureCoordinate.X, 1 - v.TextureCoordinate.Y ) ) );
    model.Effect.Texture = tex;
    model.Effect.LightingEnabled = false;
    model.Effect.TextureEnabled = true;
    this.dis = dis;
    }
    public void Draw( Camera cam , Vector3 pos)
    {
        model.Effect.World = Matrix.CreateScale( dis ) * Matrix.CreateTranslation( pos );
        model.Draw(model.Effect.World, cam.View, cam.Projection);
    }
    }
}

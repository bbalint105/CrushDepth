using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrushDepth{
    class FloorCeil{
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect effect;

        public FloorCeil( GraphicsDevice dev, Texture2D normalMap, bool isFloor)
            {
                vertexBuffer = new VertexBuffer( dev, VertexPosition.VertexDeclaration, 4,
                BufferUsage.WriteOnly );

                vertexBuffer.SetData( new VertexPosition[ ] {
                    new VertexPosition( new Vector3( 1, 0, 1 ) ),
                    new VertexPosition( new Vector3( -1, 0, 1 ) ),
                    new VertexPosition( new Vector3( 1, 0, -1 ) ),
                    new VertexPosition( new Vector3( -1, 0, -1 ) )
                } );

                indexBuffer = new IndexBuffer( dev, typeof( ushort ), 4, BufferUsage.WriteOnly );
                
                if(!isFloor)
                    indexBuffer.SetData( new ushort[ ] { 2, 3, 0, 1 } );
                else
                    indexBuffer.SetData( new ushort[ ] { 3, 2, 1, 0 } );
                effect = new BasicEffect( dev );
                effect.DiffuseColor = new Vector3( 0.0f, 0.0f, 0.05f );
            }
        public void Draw( Camera cam, int Y)
            {   
                var dev = vertexBuffer.GraphicsDevice;
                dev.SetVertexBuffer( vertexBuffer );
                dev.Indices = indexBuffer;
                effect.World = Matrix.CreateScale( 1000 ) * Matrix.CreateTranslation(
                new Vector3( cam.Position.X, Y, cam.Position.Z ) );
                effect.View = cam.View;
                effect.Projection = cam.Projection;
                effect.CurrentTechnique.Passes[ 0 ].Apply( );
                dev.DrawIndexedPrimitives( PrimitiveType.TriangleStrip, 0, 0, 2 );
            }

    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrushDepth{
    class Spikerocks : RigidBody{
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;
        BasicEffect effect;

        public Spikerocks( GraphicsDevice dev, Texture2D tex, Texture2D heightMapTex, Boolean isCeil ){
            int w = heightMapTex.Width;
            int h = heightMapTex.Height;
            uint[ ] hm = new uint[ w * h ];
            var vbData = new VertexPositionTexture[ hm.Length ];
            heightMapTex.GetData( hm );

            for ( int i = 0; i < hm.Length; i++ )
            {
                float height = ( hm[ i ] & 255 ) / 255.0f;


                int x = i % w;
                int y = i / w;
                var pos = new Vector3( x / (float)( w - 1 ), height, y / (float)( h - 1 ) );
                vbData[ i ] = new VertexPositionTexture( pos, new Vector2( pos.X, pos.Z ) );

                Hitbox hitbox = new Hitbox();
                hitbox.coords = new Vector3( x / (float)( w - 1 ), height, y / (float)( h - 1 ) );
                hitbox.acoords = new Vector3( (x+1) / (float)( w - 1 ), 0, (y+1) / (float)( h - 1 ));
                hitmap.hitboxes.Add(hitbox);
            }

            foreach(Hitbox hb in hitmap.hitboxes){
                if (!isCeil){
                    hb.acoords *= new Vector3(3200, 800, 3200);
                    hb.acoords += new Vector3(-1600, -499, -1600);
                    hb.coords *= new Vector3(3200, 800, 3200);
                    hb.coords += new Vector3(-1600, -499, -1600);
                }
                else{
                    hb.acoords *= new Vector3(3200, -800, 3200);
                    hb.acoords += new Vector3(-1600, 301, -1600);
                    hb.coords *= new Vector3(3200, -800, 3200);
                    hb.coords += new Vector3(-1600, 301, -1600);
                }

            }


            vertexBuffer = new VertexBuffer( dev, VertexPositionTexture.VertexDeclaration, vbData.Length,
            BufferUsage.WriteOnly );
            vertexBuffer.SetData( vbData );

            var indices = new ushort[ ( h - 1 ) * ( w * 2 + 1 ) ];
            var indicesr = new ushort[ ( h - 1 ) * ( w * 2 + 1 ) ];
            int idx = 0;
            int dir = 1;
            for ( int j = 1; j < w; j++ )
                {
                    int i = dir > 0 ? 0 : w - 1;
                    for ( ; i >= 0 && i < w; i += dir )
                        {
                            indices[ idx++ ] = (ushort)( j * w + i );
                            indices[ idx++ ] = (ushort)( ( j - 1 ) * w + i );
                        }
                    indices[ idx++ ] = (ushort)( ( j ) * w + i - dir );
                    dir = -dir;
                }
            indexBuffer = new IndexBuffer( dev, typeof( ushort ), indices.Length, BufferUsage.WriteOnly );
            
            int n = indices.Length;
            for (int i = 0; i < n; i++) {
                indicesr[n - 1 - i] = indices[i];
            }

            if(!isCeil)
                indexBuffer.SetData(indices);

            if(isCeil)
                indexBuffer.SetData(indicesr);
            
            effect = new BasicEffect( dev );
            effect.Texture = tex;
            effect.TextureEnabled = true;

        }

        public void Draw( Matrix world, Camera cam )
            {
            var dev = vertexBuffer.GraphicsDevice;
            dev.SetVertexBuffer( vertexBuffer );
            dev.Indices = indexBuffer;
            effect.World = world;
            effect.View = cam.View;
            effect.Projection = cam.Projection;
            effect.CurrentTechnique.Passes[ 0 ].Apply( );
            dev.DrawIndexedPrimitives( PrimitiveType.TriangleStrip, 0, 0, indexBuffer.IndexCount - 2 );
            }
    }
}
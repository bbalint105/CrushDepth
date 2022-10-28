namespace CrushDepth
{
    interface IDrawable{
        void Draw(Camera cam);
    }

    interface ISteppable{
        void Step();
    }
}


/*

abstract class Component{}

class Updateable : Component

class Drawable : Component

class Collider : Component


abstract class Entity{
    List<Component> components;
}


class Player : Entity{

    components = {Updateable,Drawable,Collider}

    Player(){

        component.drawable.onDraw += Draw();

    }


    public void Draw(){

    }

}


*/
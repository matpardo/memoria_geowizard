using UnityEngine;
using System.Collections;

public abstract class RoomEntity : MonoBehaviour
{
    private Vector2 pos;
    public abstract void handleCollision();
	public abstract void ask();
    
    // TODO : do Touch
    public void touch()
    {
        return;
    }

    public virtual bool destroyable()
    {
        return false;
    }

    public void setPosition(int i, int j) {
        setPosition(new Vector2(i, j));
    }

    public void setPosition(Vector2 pos)
    {
        this.pos = pos;
    }

    public Vector2 getPosition()
    {
        return pos;
    }



}


using UnityEngine;

[CreateAssetMenu]
class MouseInputProvider : InputProvider {

    private PlayerActor player;
    
    public override Vector2 MoveDirection {
        get {
            var vp = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition);
            vp = (vp - Vector2.one * 0.5f) * 2;

            if (player == null) player = GameObject.FindObjectOfType<PlayerActor>();
            
            if (player.Orientation == Orientation.Down || player.Orientation == Orientation.Up)
                vp.y = 0;
            else {
                vp.x = vp.y;
                vp.y = 0;
            }

            if (player.Orientation == Orientation.Down) vp.x *= -1;
            if (player.Orientation == Orientation.Right) vp.x *= -1;
            
            if(vp.magnitude < 0.3f) return Vector2.zero;
            
            return vp;
        }
    }

    public override bool JumpPressed {
        get { return Input.GetMouseButtonDown(0); }
    }

    public override bool PeekingPressed => false;

    public override bool InteractionPressed {
        get {
            var vp = (Vector2) Camera.main.ScreenToViewportPoint(Input.mousePosition);
            vp = (vp - Vector2.one * 0.5f) * 2;

            return Input.GetMouseButtonDown(0) && vp.magnitude < 0.3f;
        }
    }


}
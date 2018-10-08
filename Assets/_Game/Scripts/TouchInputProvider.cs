using UnityEngine;

[CreateAssetMenu]
class TouchInputProvider : InputProvider {

    private PlayerActor player;
    
    public override Vector2 MoveDirection {
        get {
            if (Input.touchCount < 1) return Vector2.zero;
            var vp = (Vector2) Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
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
        get {
            return Input.touchCount > 1;
        }
    }

    public override bool PeekingPressed => false;

    public override bool InteractionPressed {
        get {
            if (Input.touchCount < 1) return false;
            var vp = (Vector2) Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
            vp = (vp - Vector2.one * 0.5f) * 2;

            return vp.magnitude < 0.3f;
        }
    }


}
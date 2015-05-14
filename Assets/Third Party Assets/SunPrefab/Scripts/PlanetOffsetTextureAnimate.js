// Scroll main texture based on time
var scrollSpeed = 0.5;
function Update () {
    var offset = Time.time * scrollSpeed;
    GetComponent.<Renderer>().material.SetTextureOffset ("_BumpMap", Vector2(offset,offset));
    GetComponent.<Renderer>().material.SetTextureOffset ("_MainTex", Vector2(offset,offset));
    GetComponent.<Renderer>().materials[1].SetTextureOffset ("_MainTex", Vector2(-offset,-offset));
}
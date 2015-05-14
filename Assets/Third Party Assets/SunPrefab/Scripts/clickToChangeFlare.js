var flare1:Flare;
var flare2:Flare;
var lig:UnityEngine.Light;
function Update () {
    if(Input.GetKeyDown(KeyCode.Mouse0))
    {
    if(lig.flare == flare1){
     lig.flare = flare2;
    }else{
     lig.flare = flare1;
    }
   
    
    }
    }
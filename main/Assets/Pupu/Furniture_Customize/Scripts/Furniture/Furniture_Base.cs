using FurnitureSystem;
using UnityEngine;

public class Furniture_Base : MonoBehaviour
{
    [SerializeField] private string fName;
    [SerializeField] private FurnitureType fType;
    [SerializeField] private SpriteRenderer m_sprite;
    [SerializeField] private Collider2D[] colliders;
    [SerializeField] private Rigidbody2D m_rigid;
    [SerializeField] private FurnitureState fState;

    public string FName{get{return fName;}}
    public FurnitureType FType{get{return fType;}}

    private int overlapCount = 0;

    public bool IsOverlapping{get{return overlapCount != 0;}}

    public void SwitchToDragging(){
        m_sprite.sortingOrder += 1;

        m_rigid.velocity = Vector2.zero;
        m_rigid.angularVelocity = 0;
        m_rigid.isKinematic = true;
        for(int i=0; i<colliders.Length; i++){
            colliders[i].isTrigger = true;
        }

        fState = FurnitureState.Dragging;
    }
    public void SwitchToSimmode(){
        if(fType == FurnitureType.WallMountedItem) m_sprite.sortingOrder = 0; 
        else{
            var fLayer = FurnitureManager.LayerMaskToFurnitureLayer(gameObject.layer);
            switch(fLayer){
                case FurnitureLayer.Back:
                    m_sprite.sortingOrder = FurnitureManager.backOrder;
                    break;
                case FurnitureLayer.Mid:
                    m_sprite.sortingOrder = FurnitureManager.midOrder;
                    break;
                case FurnitureLayer.Front:
                    m_sprite.sortingOrder = FurnitureManager.frontOrder;
                    break;
            }
        }

        for(int i=0; i<colliders.Length; i++){
            colliders[i].isTrigger = false;
        }

        if(fType != FurnitureType.WallMountedItem && fType != FurnitureType.HangingItem){
            m_rigid.isKinematic = false;
        }

        fState = FurnitureState.Simulating;
    }
    public void SwitchToSleep(){
        m_rigid.velocity = Vector2.zero;
        m_rigid.angularVelocity = 0;
        m_rigid.Sleep();
    }
    public void SwitchToStatic(){
        m_rigid.isKinematic = true;
        m_rigid.velocity = Vector2.zero;
        m_rigid.angularVelocity = 0;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(fState == FurnitureState.Simulating) return;
        if(other.gameObject.name == gameObject.name) return; //两件家具名字重合，说明该碰撞体是玩家在移动家具时创建的一个复制品
        
        Furniture_Base furnitureBase = other.GetComponent<Furniture_Base>();
        if(furnitureBase!=null){
            if(overlapCount == 0) m_sprite.color = Color.red;
            overlapCount ++;
        }
    }
    void OnTriggerExit2D(Collider2D other){
        if(fState == FurnitureState.Simulating) return;
        if(other.gameObject.name == gameObject.name) return; //两件家具名字重合，说明该碰撞体是玩家在移动家具时创建的一个复制品
        
        Furniture_Base furnitureBase = other.GetComponent<Furniture_Base>();
        if(furnitureBase!=null){
            overlapCount --;
            if(overlapCount == 0) m_sprite.color = Color.white;
        }
    }
}

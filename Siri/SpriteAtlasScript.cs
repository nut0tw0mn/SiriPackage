using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
[ExecuteInEditMode]
public class SpriteAtlasScript : MonoBehaviour
{
    [SerializeField] private SpriteAtlas atlas;
    [SerializeField] private string sprite_name;
    // Start is called before the first frame update
    void Start()
    {
        if (!atlas)
            return;
        if (Application.isPlaying)
        {
            var spr = atlas.GetSprite(sprite_name);
            if(spr)
                GetComponent<Image>().sprite = spr;
            atlas = null;
        }
    }
#if UNITY_EDITOR_WIN
    void OnGUI()
    {
        if (Application.isPlaying)
            return;
        Validate();
    }
    private void Validate()
    {
        if (!atlas)
            return;
        if (!string.IsNullOrEmpty(sprite_name))
            return;
        var img = GetComponent<Image>();
        if (img.sprite && atlas.CanBindTo(img.sprite))
        {
            sprite_name = img.sprite.name;
        }
    }
#endif

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Spell", menuName = "Combat/Spell")]
public class Spell : ScriptableObject
{
    [SerializeField]
    private GameObject focusEffect;
    
    [SerializeField]
    private List<GameObject> effectAssets = null;

    #region Properties
    public IReadOnlyList<GameObject> EffectAssets { get; private set; }
    public GameObject FocusEffect { get; private set; }
    #endregion

    public void OnEnable()
    {
        if (effectAssets != null)
        {
            EffectAssets = effectAssets.AsReadOnly();
        }

        if (focusEffect != null)
        {
            FocusEffect = focusEffect;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicUser : MonoBehaviour
{
    #region Serialized Members
    [SerializeField]
    private List<Spell> spells = null;
    [SerializeField]
    private Transform center = null;
    #endregion

    #region Private Members
    private Spell focusedSpell = null;
    private GameObject focusedSpellObject = null;
    private Animator characterAnimation = null;
    #endregion

    #region Public members
    public IReadOnlyList<Spell> Spells { get; private set; }
    public Spell ActiveSpell { get; private set; }
    public Transform EffectCenter
    {
        get { return center; }
    }
    #endregion

    void Awake()
    {
        if (spells != null)
        {
            Spells = spells.AsReadOnly();
        }

        characterAnimation = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Public functions

    public void FocusSpell(int index)
    {
        if (center == null)
        {
            return;
        }

        focusedSpell = spells[index];
        focusedSpellObject = Instantiate(focusedSpell.FocusEffect, center);
    }

    public void CastMagic()
    {
        if (focusedSpell == null)
        {
            return;
        }

        Destroy(focusedSpellObject);

        ActiveSpell = focusedSpell;

        focusedSpell = null;

        characterAnimation.SetTrigger(ActiveSpell.name.ToLower());

    }

    public void CancelMagic()
    {
        if (focusedSpell == null)
        {
            return;
        }

        Destroy(focusedSpellObject);
    }

    #endregion
}

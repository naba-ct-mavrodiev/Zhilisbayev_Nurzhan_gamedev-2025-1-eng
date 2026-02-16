using System;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractable
{

        [SerializeField] private string displayName = "Interact";
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private UnityEvent onInteract;
        [SerializeField] private float outlineWidth = 2f;
        
        
        public string DisplayName => displayName;

        public bool CanInteract() => isEnabled;
    
        private Outline outline;

        private void Awake()
        {
            outline = gameObject.GetComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = outlineWidth;
            outline.enabled = false;
        }

        public void Interact()
        {
            onInteract?.Invoke(); 
        }

        public void OnFocusGained()
        {
            outline.enabled = true;
        }

        public void OnFocusLost()
        {
            outline.enabled = false;
        }
    }


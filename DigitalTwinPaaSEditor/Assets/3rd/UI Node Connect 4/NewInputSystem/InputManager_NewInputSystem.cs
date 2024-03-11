using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MeadowGames.UINodeConnect4
{
    public class InputManager_NewInputSystem : InputManager
    {
        public override Vector3 ScreenPointerPosition => _inputActions.Pointer.Position.ReadValue<Vector2>();

        public override UnityEvent e_OnPointerDown { get; set; } = new UnityEvent();
        public override UnityEvent e_OnDrag { get; set; } = new UnityEvent();
        public override UnityEvent e_OnPointerUp { get; set; } = new UnityEvent();
        public override UnityEvent e_OnDelete { get; set; } = new UnityEvent();
        public override UnityEvent e_OnPointerHover { get; set; } = new UnityEvent();
        public override bool PointerPress => _inputActions.Pointer.Press.IsPressed();
        public override bool Aux0KeyPress => _inputActions.SecondaryKey.Press.IsPressed();

        UIC_InputActions _inputActions;
        Vector3 _initialMousePos;
        bool _isDragging = false;

        void Start()
        {
            _inputActions.Pointer.Press.started += ctx => OnPointerDown();
            _inputActions.Pointer.Press.canceled += ctx => OnPointerUp();
            _inputActions.Delete.Press.started += ctx => OnDeleteKeyPressed();
            _inputActions.Pointer.Position.performed += ctx => OnPointerHover();
        }

        void Awake()
        {
            _inputActions = new UIC_InputActions();
            _inputActions.Enable();
        }

        void OnEnable()
        {
            UICSystemManager.AddToUpdate(OnUpdate);
        }

        void OnDisable()
        {
            UICSystemManager.RemoveFromUpdate(OnUpdate);
        }

        public override void OnUpdate()
        {
            if (_isDragging)
            {
                OnDrag();
            }
        }

        public void OnPointerDown()
        {
            _initialMousePos = ScreenPointerPosition;
            e_OnPointerDown.Invoke();
            _isDragging = true;
        }

        public void OnDrag()
        {
            if (_initialMousePos != ScreenPointerPosition)
            {
                e_OnDrag.Invoke();
            }
        }

        public void OnPointerUp()
        {
            _isDragging = false;
            e_OnPointerUp.Invoke();
        }

        public void OnDeleteKeyPressed()
        {
            e_OnDelete.Invoke();
        }

        public void OnPointerHover()
        {
            e_OnPointerHover.Invoke();
        }

        public override Vector3 GetCanvasPointerPosition(GraphManager graphManager)
        {
            if (_inputActions != null)
            {
                if (graphManager.CanvasRenderMode == RenderMode.ScreenSpaceOverlay)
                {
                    return ScreenPointerPosition;
                }
                else //if (graphManager.CanvasRenderMode != RenderMode.ScreenSpaceOverlay)
                {
                    Camera mainCamera = graphManager.mainCamera;
                    var screenPoint = ScreenPointerPosition;
                    screenPoint.z = graphManager.transform.position.z - mainCamera.transform.position.z; //distance of the plane from the camera
                    return mainCamera.ScreenToWorldPoint(screenPoint);
                }
            }
            else
                return Vector3.zero;
        }
    }
}
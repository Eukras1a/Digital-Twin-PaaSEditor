//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/3rd/UI Node Connect 4/NewInputSystem/UIC_InputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UIC_InputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UIC_InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UIC_InputActions"",
    ""maps"": [
        {
            ""name"": ""Pointer"",
            ""id"": ""a5323ab9-2c94-4df3-b896-df221775e07a"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""dd4500f5-36f9-41f5-99d3-4d1f42238cbf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cc2c8528-f349-4726-8dcb-2a4845f981cb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4040a9b5-25f2-4abb-8871-64771a50a2a5"",
                    ""path"": ""<Pointer>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c43fa315-4c54-4dc6-85cf-94376d4a0437"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SecondaryKey"",
            ""id"": ""d72b77fa-9ccb-4cf6-a8db-ac10c7635d44"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""a047eb50-5de7-4ad6-ae13-c09e41d0cd80"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bd101d7e-49e4-4a60-b242-6d2fa9e18d0a"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Delete"",
            ""id"": ""6a262e05-fa2f-47b1-8361-36445c194a55"",
            ""actions"": [
                {
                    ""name"": ""Press"",
                    ""type"": ""Button"",
                    ""id"": ""df265c24-9590-4ccb-8644-c2c5f0861020"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ec76987b-643d-426e-a4fd-3b2fe73368b7"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Pointer
        m_Pointer = asset.FindActionMap("Pointer", throwIfNotFound: true);
        m_Pointer_Press = m_Pointer.FindAction("Press", throwIfNotFound: true);
        m_Pointer_Position = m_Pointer.FindAction("Position", throwIfNotFound: true);
        // SecondaryKey
        m_SecondaryKey = asset.FindActionMap("SecondaryKey", throwIfNotFound: true);
        m_SecondaryKey_Press = m_SecondaryKey.FindAction("Press", throwIfNotFound: true);
        // Delete
        m_Delete = asset.FindActionMap("Delete", throwIfNotFound: true);
        m_Delete_Press = m_Delete.FindAction("Press", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Pointer
    private readonly InputActionMap m_Pointer;
    private List<IPointerActions> m_PointerActionsCallbackInterfaces = new List<IPointerActions>();
    private readonly InputAction m_Pointer_Press;
    private readonly InputAction m_Pointer_Position;
    public struct PointerActions
    {
        private @UIC_InputActions m_Wrapper;
        public PointerActions(@UIC_InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Press => m_Wrapper.m_Pointer_Press;
        public InputAction @Position => m_Wrapper.m_Pointer_Position;
        public InputActionMap Get() { return m_Wrapper.m_Pointer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PointerActions set) { return set.Get(); }
        public void AddCallbacks(IPointerActions instance)
        {
            if (instance == null || m_Wrapper.m_PointerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PointerActionsCallbackInterfaces.Add(instance);
            @Press.started += instance.OnPress;
            @Press.performed += instance.OnPress;
            @Press.canceled += instance.OnPress;
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
        }

        private void UnregisterCallbacks(IPointerActions instance)
        {
            @Press.started -= instance.OnPress;
            @Press.performed -= instance.OnPress;
            @Press.canceled -= instance.OnPress;
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
        }

        public void RemoveCallbacks(IPointerActions instance)
        {
            if (m_Wrapper.m_PointerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPointerActions instance)
        {
            foreach (var item in m_Wrapper.m_PointerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PointerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PointerActions @Pointer => new PointerActions(this);

    // SecondaryKey
    private readonly InputActionMap m_SecondaryKey;
    private List<ISecondaryKeyActions> m_SecondaryKeyActionsCallbackInterfaces = new List<ISecondaryKeyActions>();
    private readonly InputAction m_SecondaryKey_Press;
    public struct SecondaryKeyActions
    {
        private @UIC_InputActions m_Wrapper;
        public SecondaryKeyActions(@UIC_InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Press => m_Wrapper.m_SecondaryKey_Press;
        public InputActionMap Get() { return m_Wrapper.m_SecondaryKey; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SecondaryKeyActions set) { return set.Get(); }
        public void AddCallbacks(ISecondaryKeyActions instance)
        {
            if (instance == null || m_Wrapper.m_SecondaryKeyActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SecondaryKeyActionsCallbackInterfaces.Add(instance);
            @Press.started += instance.OnPress;
            @Press.performed += instance.OnPress;
            @Press.canceled += instance.OnPress;
        }

        private void UnregisterCallbacks(ISecondaryKeyActions instance)
        {
            @Press.started -= instance.OnPress;
            @Press.performed -= instance.OnPress;
            @Press.canceled -= instance.OnPress;
        }

        public void RemoveCallbacks(ISecondaryKeyActions instance)
        {
            if (m_Wrapper.m_SecondaryKeyActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISecondaryKeyActions instance)
        {
            foreach (var item in m_Wrapper.m_SecondaryKeyActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SecondaryKeyActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SecondaryKeyActions @SecondaryKey => new SecondaryKeyActions(this);

    // Delete
    private readonly InputActionMap m_Delete;
    private List<IDeleteActions> m_DeleteActionsCallbackInterfaces = new List<IDeleteActions>();
    private readonly InputAction m_Delete_Press;
    public struct DeleteActions
    {
        private @UIC_InputActions m_Wrapper;
        public DeleteActions(@UIC_InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Press => m_Wrapper.m_Delete_Press;
        public InputActionMap Get() { return m_Wrapper.m_Delete; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DeleteActions set) { return set.Get(); }
        public void AddCallbacks(IDeleteActions instance)
        {
            if (instance == null || m_Wrapper.m_DeleteActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DeleteActionsCallbackInterfaces.Add(instance);
            @Press.started += instance.OnPress;
            @Press.performed += instance.OnPress;
            @Press.canceled += instance.OnPress;
        }

        private void UnregisterCallbacks(IDeleteActions instance)
        {
            @Press.started -= instance.OnPress;
            @Press.performed -= instance.OnPress;
            @Press.canceled -= instance.OnPress;
        }

        public void RemoveCallbacks(IDeleteActions instance)
        {
            if (m_Wrapper.m_DeleteActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDeleteActions instance)
        {
            foreach (var item in m_Wrapper.m_DeleteActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DeleteActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DeleteActions @Delete => new DeleteActions(this);
    public interface IPointerActions
    {
        void OnPress(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
    }
    public interface ISecondaryKeyActions
    {
        void OnPress(InputAction.CallbackContext context);
    }
    public interface IDeleteActions
    {
        void OnPress(InputAction.CallbackContext context);
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Scripts/Input/GameplayInput.inputactions
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

namespace ConnectIt.Input
{
    public partial class @GameplayInput: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameplayInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameplayInput"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""835f7a64-79d7-43ae-9802-be15a7def873"",
            ""actions"": [
                {
                    ""name"": ""InteractionPosition"",
                    ""type"": ""Value"",
                    ""id"": ""a5091a1e-fd34-43fe-a25c-1e9bfe24bc7e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InteractionDelta"",
                    ""type"": ""Value"",
                    ""id"": ""36fe93a4-9920-4a88-9590-0f3940ee50a4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InteractionPress"",
                    ""type"": ""Button"",
                    ""id"": ""ad17eed2-5c66-4125-b209-5fccbfbeeafc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InteractionClick"",
                    ""type"": ""Button"",
                    ""id"": ""e6b3bd5d-838d-437b-b49a-59f06ef11544"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2ebcd605-9af8-4ad2-9e8f-16e68959b738"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""InteractionPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68ced192-7839-4033-91c6-de8fb564d614"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""InteractionPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15c36371-6da5-4e1c-8cca-5f5971601d88"",
                    ""path"": ""<Touchscreen>/primaryTouch/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""InteractionDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a69bd6d-2da8-42ea-aa1f-1a949b47046d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""InteractionDelta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3bc1252f-cdbd-4c09-8773-61b2b243e0f4"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""InteractionPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbdf1277-d8e2-469c-a812-9e8eaea72e47"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""InteractionPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85111aa1-d8c4-4c2a-b934-12daf269a71f"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch"",
                    ""action"": ""InteractionClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1f05652-71a9-41cb-9a41-4e924fdfb197"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mouse"",
                    ""action"": ""InteractionClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Mouse"",
            ""bindingGroup"": ""Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Main
            m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
            m_Main_InteractionPosition = m_Main.FindAction("InteractionPosition", throwIfNotFound: true);
            m_Main_InteractionDelta = m_Main.FindAction("InteractionDelta", throwIfNotFound: true);
            m_Main_InteractionPress = m_Main.FindAction("InteractionPress", throwIfNotFound: true);
            m_Main_InteractionClick = m_Main.FindAction("InteractionClick", throwIfNotFound: true);
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

        // Main
        private readonly InputActionMap m_Main;
        private List<IMainActions> m_MainActionsCallbackInterfaces = new List<IMainActions>();
        private readonly InputAction m_Main_InteractionPosition;
        private readonly InputAction m_Main_InteractionDelta;
        private readonly InputAction m_Main_InteractionPress;
        private readonly InputAction m_Main_InteractionClick;
        public struct MainActions
        {
            private @GameplayInput m_Wrapper;
            public MainActions(@GameplayInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @InteractionPosition => m_Wrapper.m_Main_InteractionPosition;
            public InputAction @InteractionDelta => m_Wrapper.m_Main_InteractionDelta;
            public InputAction @InteractionPress => m_Wrapper.m_Main_InteractionPress;
            public InputAction @InteractionClick => m_Wrapper.m_Main_InteractionClick;
            public InputActionMap Get() { return m_Wrapper.m_Main; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
            public void AddCallbacks(IMainActions instance)
            {
                if (instance == null || m_Wrapper.m_MainActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MainActionsCallbackInterfaces.Add(instance);
                @InteractionPosition.started += instance.OnInteractionPosition;
                @InteractionPosition.performed += instance.OnInteractionPosition;
                @InteractionPosition.canceled += instance.OnInteractionPosition;
                @InteractionDelta.started += instance.OnInteractionDelta;
                @InteractionDelta.performed += instance.OnInteractionDelta;
                @InteractionDelta.canceled += instance.OnInteractionDelta;
                @InteractionPress.started += instance.OnInteractionPress;
                @InteractionPress.performed += instance.OnInteractionPress;
                @InteractionPress.canceled += instance.OnInteractionPress;
                @InteractionClick.started += instance.OnInteractionClick;
                @InteractionClick.performed += instance.OnInteractionClick;
                @InteractionClick.canceled += instance.OnInteractionClick;
            }

            private void UnregisterCallbacks(IMainActions instance)
            {
                @InteractionPosition.started -= instance.OnInteractionPosition;
                @InteractionPosition.performed -= instance.OnInteractionPosition;
                @InteractionPosition.canceled -= instance.OnInteractionPosition;
                @InteractionDelta.started -= instance.OnInteractionDelta;
                @InteractionDelta.performed -= instance.OnInteractionDelta;
                @InteractionDelta.canceled -= instance.OnInteractionDelta;
                @InteractionPress.started -= instance.OnInteractionPress;
                @InteractionPress.performed -= instance.OnInteractionPress;
                @InteractionPress.canceled -= instance.OnInteractionPress;
                @InteractionClick.started -= instance.OnInteractionClick;
                @InteractionClick.performed -= instance.OnInteractionClick;
                @InteractionClick.canceled -= instance.OnInteractionClick;
            }

            public void RemoveCallbacks(IMainActions instance)
            {
                if (m_Wrapper.m_MainActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMainActions instance)
            {
                foreach (var item in m_Wrapper.m_MainActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MainActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MainActions @Main => new MainActions(this);
        private int m_TouchSchemeIndex = -1;
        public InputControlScheme TouchScheme
        {
            get
            {
                if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
                return asset.controlSchemes[m_TouchSchemeIndex];
            }
        }
        private int m_MouseSchemeIndex = -1;
        public InputControlScheme MouseScheme
        {
            get
            {
                if (m_MouseSchemeIndex == -1) m_MouseSchemeIndex = asset.FindControlSchemeIndex("Mouse");
                return asset.controlSchemes[m_MouseSchemeIndex];
            }
        }
        public interface IMainActions
        {
            void OnInteractionPosition(InputAction.CallbackContext context);
            void OnInteractionDelta(InputAction.CallbackContext context);
            void OnInteractionPress(InputAction.CallbackContext context);
            void OnInteractionClick(InputAction.CallbackContext context);
        }
    }
}
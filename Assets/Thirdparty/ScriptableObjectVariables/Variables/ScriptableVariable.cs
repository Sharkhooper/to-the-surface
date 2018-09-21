// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using UnityEngine;

namespace RoboRyanTron.Unite2017.Variables {
    
    [Serializable]
    public class ScriptableVariableReference<T, S> where S : ScriptableVariable<T> {
        public bool UseConstant = true;
        public T ConstantValue;
        public S Variable;

        public ScriptableVariableReference() { }

        public ScriptableVariableReference(T value) {
            UseConstant = true;
            ConstantValue = value;
        }

        public T Value { get { return UseConstant ? ConstantValue : Variable.Value; } }

        public static implicit operator T(ScriptableVariableReference<T, S> reference) { return reference.Value; }
    }
    
    [Serializable]
    public class ScriptableVariable<T> : ScriptableObject {

#if UNITY_EDITOR
        [Multiline] public string DeveloperDescription = "";
#endif
        public T Value;

        public static implicit operator T(ScriptableVariable<T> reference) { return reference.Value; }
        
        public void SetValue(T value) { Value = value; }

        public void SetValue(ScriptableVariable<T> value) { Value = value.Value; }
    }

    
    [Serializable]
    public class IntReference : ScriptableVariableReference<int, IntVariable> {}
    [Serializable]
    [CreateAssetMenu(menuName = "Variables/IntVariable", order = 50)]
    public class IntVariable : ScriptableVariable<int> {
    }


    [Serializable]
    public class FloatReference : ScriptableVariableReference<float, FloatVariable> {}
    [Serializable]
    [CreateAssetMenu(menuName = "Variables/FloatVariable", order = 50)]
    public class FloatVariable : ScriptableVariable<float> {
        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }
    }
    
    [Serializable]
    public class BoolReference : ScriptableVariableReference<bool, BoolVariable> {}
    [Serializable]
    [CreateAssetMenu(menuName = "Variables/BoolVariable", order = 50)]
    public class BoolVariable : ScriptableVariable<bool> {
    }
    
    [Serializable]
    public class StringReference : ScriptableVariableReference<string, StringVariable> {}
    [Serializable]
    [CreateAssetMenu(menuName = "Variables/StringVariable", order = 50)]
    public class StringVariable : ScriptableVariable<string> {
    }
}
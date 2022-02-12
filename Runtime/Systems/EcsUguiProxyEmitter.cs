// ----------------------------------------------------------------------------
// The MIT License
// Ugui bindings https://github.com/Leopotam/ecslite-unity-ugui
// for LeoECS Lite https://github.com/Leopotam/ecslite
// Copyright (c) 2021-2022 Leopotam <leopotam@gmail.com>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    public class EcsUguiProxyEmitter : EcsUguiEmitter {
        public enum SearchType {
            InGlobal,
            InHierarchy
        }

        [SerializeField] EcsUguiEmitter _parent;
        [SerializeField] SearchType _searchType = SearchType.InGlobal;

        public override EcsWorld GetWorld () {
            return ValidateEmitter () ? _parent.GetWorld () : default;
        }

        public override void SetNamedObject (string widgetName, GameObject go) {
            if (ValidateEmitter ()) { _parent.SetNamedObject (widgetName, go); }
        }

        public override GameObject GetNamedObject (string widgetName) {
            return ValidateEmitter () ? _parent.GetNamedObject (widgetName) : default;
        }

        bool ValidateEmitter () {
            if (_parent) { return true; }
            // parent was killed.
            if ((object) _parent != null) { return false; }

            var parent = _searchType == SearchType.InGlobal
                ? FindObjectOfType<EcsUguiEmitter> ()
                : GetComponentInParent<EcsUguiEmitter> ();
            // fix for GetComponentInParent.
            if (parent == this) { parent = null; }
#if DEBUG && !LEOECSLITE_NO_SANITIZE_CHECKS
            if (parent == null) {
                Debug.LogError ("EcsUiEmitter not found in hierarchy", this);
                return false;
            }
#endif
            _parent = parent;
            return true;
        }
    }
}
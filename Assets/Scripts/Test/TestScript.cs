using ConnectIt.Input;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ConnectIt.Test
{
    public class TestScript : MonoBehaviour
    {
        [SerializeField] private bool _skip;
        [SerializeField] private TileBase _tile1;
        [SerializeField] private TileBase _tile2;

        private MainInput _input;
        private Tilemap _tilemap;
        private Camera _camera;

        private void Start()
        {
            //TileBase[] tiles = new TileBase[] { _tile1, _tile2 };
            //foreach (TileBase tile in tiles)
            //{
            //    TileData data = new TileData();

            //}

            Func<string> func = TestFunc;
            func += TestFunc2;

            string TestFunc()
            {
                return "some string";
            }

            string TestFunc2()
            {
                return "some string2";
            }

            var d = func.GetInvocationList()[1];

            print(d.DynamicInvoke());

            //print(_tile1 == _tile2);

            _tilemap = GetComponent<Tilemap>();
            _camera = Camera.main;
            _input = new MainInput();

            _input.Enable();
            _input.Main.InteractionPosition.performed += InteractionPosition_performed;

            _tilemap.CompressBounds();
            print($"{_tilemap.cellBounds} {_tilemap.cellBounds.max} {_tilemap.cellBounds.min} {_tilemap.cellBounds.center}");
        }

        private void InteractionPosition_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_skip)
                return;

            Vector2 interactionPosition = obj.ReadValue<Vector2>();
            Vector3 worldPosition = _camera.ScreenToWorldPoint(interactionPosition);
            Vector3Int selectedCell = _tilemap.WorldToCell(worldPosition);

            print(_tilemap.GetTile(selectedCell) == null);
        }

        private void OnDestroy()
        {
            _input.Main.InteractionPosition.performed -= InteractionPosition_performed;
        }
    }
}
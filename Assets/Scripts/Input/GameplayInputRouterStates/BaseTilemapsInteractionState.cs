using ConnectIt.Model;
using UnityEngine;

namespace ConnectIt.Input.GameplayInputRouterStates
{
    public abstract class BaseTilemapsInteractionState : BaseState
    {
        protected readonly RenderCameraProvider cameraProvider;
        protected readonly Tilemaps tilemaps;

        protected Tile lastOnUpdateInteractedTile;

        public BaseTilemapsInteractionState(
            GameplayInput input,
            GameplayInputRouter router,
            RenderCameraProvider cameraProvider,
            Tilemaps tilemaps) : base(input, router)
        {
            this.cameraProvider = cameraProvider;
            this.tilemaps = tilemaps;
        }

        public override void StateExit()
        {
            if (lastOnUpdateInteractedTile != null)
                ResetLastOnUpdateInteractedTile();
        }

        protected bool TryGetTileByPositionOnScreen(out Tile tile)
        {
            Vector2 positionOnScreen = input.Main.InteractionPosition.ReadValue<Vector2>();
            Vector3 worldPosition = cameraProvider.RenderCamera.ScreenToWorldPoint(positionOnScreen);

            return tilemaps.TryGetTileAtWorldPosition(worldPosition, out tile);
        }

        protected bool TryGetObjectInfoFromTile<T>(Tile tile, out T objInfo) where T : class
        {
            objInfo = null;

            var tileUsersInfo = tile.RequestUsersInfo();
            if (tileUsersInfo == null || tileUsersInfo.Length == 0)
                return false;

            foreach (var userInfo in tileUsersInfo)
            {
                if (userInfo is T info)
                {
                    objInfo = info;
                    break;
                }
            }

            return objInfo != null;
        }

        protected void SetLastOnUpdateInteractedTile(Tile tile)
        {
            if (lastOnUpdateInteractedTile != null)
                ResetLastOnUpdateInteractedTile();

            lastOnUpdateInteractedTile = tile;
            lastOnUpdateInteractedTile.UsersChanged += OnLastInteractedTileUserChanged;
        }

        protected void ResetLastOnUpdateInteractedTile()
        {
            lastOnUpdateInteractedTile.UsersChanged -= OnLastInteractedTileUserChanged;
            lastOnUpdateInteractedTile = null;
        }

        protected void OnLastInteractedTileUserChanged(Tile tile)
        {
            ResetLastOnUpdateInteractedTile();
        }
    }
}

using UnityEngine;
using Map.Model;
using Map.View;

namespace Map.Presenter
{
    public class MapPresenter
    {
        private readonly MapModel _model;
        private readonly MapView _view;
        private readonly MarkerModel _marker;

        public MapPresenter(MapView view, MapModel model, MarkerModel marker)
        {
            _view = view;
            _model = model;
            _marker = marker;
        }

        public void Enable()
        {
            _view.Zoom += OnZoom;
            _view.MoveInDirection += OnMoveInDirection;
            _view.TranslateToPosition += OnTranslateToPosition;

            _model.PositionChanged += _view.UpdatePosition;
            _model.Scaling.FovChanged += _view.UpdateFov;

            _model.Scaling.ScaleLevelChanged += _view.UpdateScale;
            _model.Scaling.GridCellSizeChanged += _view.UpdateGridCellSize;

            _view.Enable();
            _model.RefreshViev();
            _model.Scaling.RefreshViev();
        }

        public void Disable()
        {
            _view.Zoom -= OnZoom;
            _view.TranslateToPosition -= OnTranslateToPosition;
            _view.MoveInDirection -= OnMoveInDirection;

            _model.PositionChanged -= _view.UpdatePosition;
            _model.Scaling.FovChanged -= _view.UpdateFov;

            _model.Scaling.ScaleLevelChanged -= _view.UpdateScale;
            _model.Scaling.GridCellSizeChanged -= _view.UpdateGridCellSize;

            _view.Disable();
        }

        private void OnZoom(float zoom)
        {
            _model.Scaling.Zoom(zoom);
        }

        private void OnMoveInDirection(Vector2 direction)
        {
            _model.MoveInDirection(direction);
        }

        private void OnTranslateToPosition(Vector2 position)
        {
            _model.TranslateToPosition(position);
        }
    }
}

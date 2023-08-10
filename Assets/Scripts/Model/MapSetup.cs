using Assets.Scripts.Presenter;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class MapSetup : MonoBehaviour
    {
        [SerializeField] private MapView _viev;

        [SerializeField] private DynamicMarkerData _playerMarkerData;

        private MapPresenter _presenter;
        private MapModel _model;
        private MarkerModel _markerModel;

        private bool _isEnabled = false;

        private void Awake()
        {
            if (TextQuest.Instance.GetGameWorld() == null)
            {
                Debug.Log("World is null. Subscribe to event");
                EventBus.WorldEvents.GameWorldLoaded += Init;
                return;
            }

            Init();
        }

        private void Init()
        {
            _model = new MapModel(TextQuest.Instance.GetGameWorld());
            _markerModel = new MarkerModel(_playerMarkerData);
            _presenter = new MapPresenter(_viev, _model, _markerModel);
            _viev.Init(_model.MapPainting);

            _model.RefreshViev();
            _model.Scaling.RefreshViev();
            EventBus.WorldEvents.GameWorldLoaded -= Init;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (_isEnabled)
                    Disable();
                else
                    Enable();
            }
        }

        private void Enable()
        {
            _presenter.Enable();
            _isEnabled = true;
        }

        private void Disable()
        {
            _presenter.Disable();
            _isEnabled = false;
        }
    }
}

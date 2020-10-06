using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

	public Camera mainCamera;

    private Vector2 cameraVelocity = new Vector2();

	private int _maxOffset = 5;

	private float _TSM = 0f;//time for smooth movement
	private float _defTSM = 0.5f;
	private float _cameraSpeed = 0.5f;
	private float _minZoom = 5;
	private float _maxZoom = 18;

	private void Start() => mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

    public void Init() {
        mainCamera.transform.position = new Vector3(GameManager.GetInstance().world.dimensions.x / 2,
                                                    GameManager.GetInstance().world.dimensions.y / 2, -100);
		mainCamera.orthographicSize = _maxZoom;
		SubscribeToInput();
    }

	private void LateUpdate() {
		MouseListener();
		SmoothMovement();
		Scale();
		BorderCheck();

		gameObject.transform.Translate(cameraVelocity);
	}	

	private void Scale() {
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > _minZoom) {
			mainCamera.orthographicSize -= 1f;
		} else if (Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < _maxZoom) {
			mainCamera.orthographicSize += 1f;
		}
	}

	//Check if mainCamera went out of bounds
	private void BorderCheck() {
		Vector3 leftDown = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
		Vector3 rightUp = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
		Vector3 newPos = gameObject.transform.position;

        if (leftDown.x + cameraVelocity.x < -_maxOffset && cameraVelocity.x < 0) {
            cameraVelocity.x = 0;
		} else if (rightUp.x + cameraVelocity.x > GameManager.GetInstance().world.dimensions.x + _maxOffset && cameraVelocity.x > 0) {
            cameraVelocity.x = 0;
		}

        if (leftDown.y + cameraVelocity.y < -_maxOffset && cameraVelocity.y < 0) {
            cameraVelocity.y = 0;
		} else if (rightUp.y + cameraVelocity.y > GameManager.GetInstance().world.dimensions.y + _maxOffset && cameraVelocity.y > 0) {
            cameraVelocity.y = 0;
		}
	}

	private void SmoothMovement() {
		if(_TSM <= 0) {
			cameraVelocity = Vector2.zero;
		} else {
			_TSM -= Time.deltaTime;
            cameraVelocity *= 0.9f;
		}
	}

	private Vector3 _dragOrigin;
	private Vector3 _dragDiff;
	private bool _isDragging = false;
    private void MouseListener() {
		Vector3 currMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (Input.GetMouseButton (2)) {

			_dragDiff = currMousePos - Camera.main.transform.position;
			if (!_isDragging){

				_isDragging = true;
				_dragOrigin = currMousePos;
			}
		} else {
			_isDragging = false;
		}

		if (_isDragging) {
			cameraVelocity = (Vector2) ( (_dragOrigin - _dragDiff) - Camera.main.transform.position);
			_TSM = _defTSM;
		}
    }

	private void MoveUp() {
		cameraVelocity.y = _cameraSpeed;
        _TSM = _defTSM;
	}

	private void MoveDown() {
		cameraVelocity.y = -_cameraSpeed;
        _TSM = _defTSM;
	}

	private void MoveLeft() {
		cameraVelocity.x = -_cameraSpeed;
        _TSM = _defTSM;
	}

	private void MoveRight() {
		cameraVelocity.x = _cameraSpeed;
        _TSM = _defTSM;
	}

	private void OnDestroy() => UnsubscribeFromInput();

	private void SubscribeToInput() {
		InputController input = InputController.GetInstance();
		input.OnW_Pressed += MoveUp;
		input.OnA_Pressed += MoveLeft;
		input.OnS_Pressed += MoveDown;
		input.OnD_Pressed += MoveRight;
	}

	private void UnsubscribeFromInput() {
		InputController input = InputController.GetInstance();
		input.OnW_Pressed -= MoveUp;
		input.OnA_Pressed -= MoveLeft;
		input.OnS_Pressed -= MoveDown;
		input.OnD_Pressed -= MoveRight;
	}
}
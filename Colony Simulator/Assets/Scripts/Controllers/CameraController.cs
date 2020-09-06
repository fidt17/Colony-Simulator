using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera mainCamera;

    private Vector2 cameraVelocity = new Vector2();
	private float TSM = 0f;//time for smooth movement
	private float defTSM = 0.5f;
	private float cameraSpeed = 0.5f;

	private float minZoom = 5;
	private float maxZoom = 18;
	private int maxOffset = 5;

	private void Start() {

		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

    public void Init() {
        mainCamera.transform.position = new Vector3(GameManager.Instance.world.dimensions.x / 2,
                                                    GameManager.Instance.world.dimensions.y / 2, -100);

		mainCamera.orthographicSize = maxZoom;

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
		
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > minZoom) {

			mainCamera.orthographicSize -= 1f;
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < maxZoom) {

			mainCamera.orthographicSize += 1f;
		}
	}

	//Check if mainCamera went out of bounds
	private void BorderCheck() {

		Vector3 leftDown = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
		Vector3 rightUp = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
		Vector3 newPos = gameObject.transform.position;

        if (leftDown.x + cameraVelocity.x < -maxOffset && cameraVelocity.x < 0)
            cameraVelocity.x = 0;

        if (rightUp.x + cameraVelocity.x > GameManager.Instance.world.dimensions.x + maxOffset && cameraVelocity.x > 0)
            cameraVelocity.x = 0;

        if (leftDown.y + cameraVelocity.y < -maxOffset && cameraVelocity.y < 0)
            cameraVelocity.y = 0;

        if (rightUp.y + cameraVelocity.y > GameManager.Instance.world.dimensions.y + maxOffset && cameraVelocity.y > 0)
            cameraVelocity.y = 0;
	}

	private void SmoothMovement() {

		if(TSM <= 0) {
			cameraVelocity = Vector2.zero;
		} else {

			TSM -= Time.deltaTime;
            cameraVelocity *= 0.9f;
		}
	}

	private Vector3 dragOrigin;
	private Vector3 dragDiff;
	private bool isDragging = false;

    private void MouseListener() {

		Vector3 currMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (Input.GetMouseButton (2)) {

			dragDiff = currMousePos - Camera.main.transform.position;
			if (!isDragging){

				isDragging = true;
				dragOrigin = currMousePos;
			}
		} else {
			isDragging = false;
		}

		if (isDragging) {
			cameraVelocity = (Vector2) ( (dragOrigin - dragDiff) - Camera.main.transform.position);
			TSM = defTSM;
		}
    }

	private void MoveUp() {

		cameraVelocity.y = cameraSpeed;
        TSM = defTSM;
	}

	private void MoveDown() {

		cameraVelocity.y = -cameraSpeed;
        TSM = defTSM;
	}

	private void MoveLeft() {

		cameraVelocity.x = -cameraSpeed;
        TSM = defTSM;
	}

	private void MoveRight() {

		cameraVelocity.x = cameraSpeed;
        TSM = defTSM;
	}

	private void OnDestroy() {

		UnsubscribeFromInput();
	}

	private void SubscribeToInput() {

		InputController input = InputController.Instance;

		input.OnW_Pressed += MoveUp;
		input.OnA_Pressed += MoveLeft;
		input.OnS_Pressed += MoveDown;
		input.OnD_Pressed += MoveRight;
	}

	private void UnsubscribeFromInput() {

		InputController input = InputController.Instance;

		input.OnW_Pressed -= MoveUp;
		input.OnA_Pressed -= MoveLeft;
		input.OnS_Pressed -= MoveDown;
		input.OnD_Pressed -= MoveRight;
	}
}
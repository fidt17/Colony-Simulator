using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Camera mainCamera;

    private Vector2 cameraVelocity = new Vector2();
	private float TSM = 0f;//time for smooth movement
	private float defTSM = 0.5f;
	private float cameraSpeed = 0.5f;
	private float defCameraSpeed = 0.5f;
	private float speedModificator = 20;

	//Предыдущее положение курсора.
	Vector3 prevMousePos = new Vector3(0f, 0f, 0f);

	private void Start() {

		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

    public void Init() {
        mainCamera.transform.position = new Vector3(GameManager.Instance.world.dimensions.x / 2,
                                                    GameManager.Instance.world.dimensions.y / 2, -100);
    }

	private void Update() {

		KeyboardListener();
		MouseListener();
		SmoothMovement();
		Scale();
		BorderCheck();

		gameObject.transform.Translate(cameraVelocity);
	}	

	private void Scale() {
		
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && mainCamera.orthographicSize > 5f) {

			mainCamera.orthographicSize -= 1f;
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0 && mainCamera.orthographicSize < 30f) {

			mainCamera.orthographicSize += 1f;
		}
	}

	//Check if mainCamera went out of bounds
	private void BorderCheck() {

		Vector3 leftDown = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
		Vector3 rightUp = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));
		Vector3 newPos = gameObject.transform.position;

        if (leftDown.x + cameraVelocity.x < -5 && cameraVelocity.x < 0)
            cameraVelocity.x = 0;

        if (rightUp.x + cameraVelocity.x > GameManager.Instance.world.dimensions.x + 5 && cameraVelocity.x > 0)
            cameraVelocity.x = 0;

        if (leftDown.y + cameraVelocity.y < -5 && cameraVelocity.y < 0)
            cameraVelocity.y = 0;

        if (rightUp.y + cameraVelocity.y > GameManager.Instance.world.dimensions.y + 5 && cameraVelocity.y > 0)
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

    private void MouseListener() {

        Vector3 curMousePos = Input.mousePosition;
		
		if(!Input.GetKey(KeyCode.Mouse2)) {
			prevMousePos = curMousePos;
			return;
		}

        float horDist = prevMousePos.x - curMousePos.x;
        float verDist = prevMousePos.y - curMousePos.y;

        cameraVelocity = new Vector2(horDist, verDist) / speedModificator;
		prevMousePos = curMousePos;
        TSM = defTSM;
    }

    private void KeyboardListener() {

        if(Input.GetKey(KeyCode.LeftShift))
			cameraSpeed *= 1.5f;

        if (Input.GetKey(KeyCode.W)) {
            cameraVelocity.y = cameraSpeed;
            TSM = defTSM;
        } else if (Input.GetKey(KeyCode.S)) {
            cameraVelocity.y = -cameraSpeed;
            TSM = defTSM;
        }

        if (Input.GetKey(KeyCode.A)) {
            cameraVelocity.x = -cameraSpeed;
            TSM = defTSM;
        } else if (Input.GetKey(KeyCode.D)) {
            cameraVelocity.x = cameraSpeed;
            TSM = defTSM;
        }

        cameraSpeed = defCameraSpeed;
    }
}
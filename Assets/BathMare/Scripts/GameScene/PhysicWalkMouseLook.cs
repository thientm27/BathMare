using UnityEngine;

namespace BathMare.Scripts.GameScene
{
	public class PhysicWalkMouseLook : MonoBehaviour
	{
		public enum RotationAxes
		{
			MouseXAndY,
			MouseX,
			MouseY
		}

		public Player pl;

		public static PhysicWalkMouseLook instance;

		public Transform _camPos;

		public Vector3 camPosBasePosition;

		public Transform alternateCamPos;

		public RotationAxes axes;

		public float sensitivity = 15f;

		public float minimumX = -360f;

		public float maximumX = 360f;

		public float minimumY = -60f;

		public float maximumY = 60f;

		public float rotationY;

		public float rotationX;

		public Vector2 smoothedMouse = new Vector2(0f, 0f);

		public float smoothing = 4f;

		public bool isCamera;

		public float wobbleX;

		public float wobbleY;

		public float wobbleXtarget;

		public float wobbleYtarget;

		public float wobbleXspeed = 10f;

		public float wobbleYspeed = 10f;

		private float Ydirection = 1f;

		public float inputSensitivity;

		private Quaternion startRotation;

		private void Start()
		{
			if (isCamera)
			{
				instance = this;
			}
			startRotation = base.transform.localRotation;
			if (_camPos != null)
			{
				camPosBasePosition = _camPos.transform.localPosition;
			}
			Vector3 eulerAngles = base.transform.rotation.eulerAngles;
			rotationX = eulerAngles.y;
			Vector3 eulerAngles2 = base.transform.rotation.eulerAngles;
			rotationY = eulerAngles2.x;
		}

		private void FixedUpdate()
		{
			smoothedMouse = Vector2.Lerp(smoothedMouse, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), 1f / smoothing);
			if (isCamera)
			{
				_camPos.localPosition   = Vector3.Lerp(_camPos.localPosition, camPosBasePosition, Time.fixedDeltaTime * 10f);
				base.transform.position = Vector3.Lerp(base.transform.position, _camPos.position, Time.fixedDeltaTime * 5f);
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				if (localEulerAngles.y < 180f)
				{
					Transform transform         = base.transform;
					Vector3   localEulerAngles2 = base.transform.localEulerAngles;
					Vector3   localEulerAngles3 = base.transform.localEulerAngles;
					float     x                 = localEulerAngles3.x;
					Vector3   localEulerAngles4 = base.transform.localEulerAngles;
					transform.localEulerAngles = Vector3.Lerp(localEulerAngles2, new Vector3(x, 0f, localEulerAngles4.y), Time.fixedDeltaTime * 5f);
				}
				else
				{
					Vector3 localEulerAngles5 = base.transform.localEulerAngles;
					if (localEulerAngles5.y > 180f)
					{
						Transform transform2        = base.transform;
						Vector3   localEulerAngles6 = base.transform.localEulerAngles;
						Vector3   localEulerAngles7 = base.transform.localEulerAngles;
						float     x2                = localEulerAngles7.x;
						Vector3   localEulerAngles8 = base.transform.localEulerAngles;
						transform2.localEulerAngles = Vector3.Lerp(localEulerAngles6, new Vector3(x2, 360f, localEulerAngles8.y), Time.fixedDeltaTime * 5f);
					}
				}
			}
			else if (isCamera && alternateCamPos != null)
			{
				transform.position = Vector3.Lerp(transform.position, alternateCamPos.position, Time.fixedDeltaTime * 10f);
				transform.rotation = Quaternion.Lerp(base.transform.rotation, alternateCamPos.rotation, Time.fixedDeltaTime * 10f);
			}
		
			if (axes == RotationAxes.MouseX)
			{
				if (pl.Live)
				{
					rotationX += smoothedMouse.x * inputSensitivity * Time.deltaTime;
				}
				Transform transform3        = base.transform;
				Vector3   localEulerAngles9 = base.transform.localEulerAngles;
				transform3.localEulerAngles = new Vector3(localEulerAngles9.x, rotationX, 0f);
			}
			else
			{
				if (pl.Live)
				{
					rotationY += smoothedMouse.y * Ydirection * inputSensitivity * Time.deltaTime;
				}
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				Transform transform4         = base.transform;
				float     x3                 = 0f - rotationY;
				Vector3   localEulerAngles10 = base.transform.localEulerAngles;
				transform4.localEulerAngles = new Vector3(x3, localEulerAngles10.y, (0f - smoothedMouse.x) * inputSensitivity * Time.deltaTime);
			}
			wobbleX = Mathf.Lerp(wobbleX, wobbleXtarget, Time.fixedDeltaTime * wobbleXspeed);
			wobbleY = Mathf.Lerp(wobbleY, wobbleYtarget, Time.fixedDeltaTime * wobbleYspeed);
			if (wobbleXtarget > 0f)
			{
				wobbleXtarget -= Time.fixedDeltaTime * wobbleXspeed;
			}
			if (wobbleXtarget < 0f)
			{
				wobbleXtarget += Time.fixedDeltaTime * wobbleXspeed;
			}
			if (wobbleYtarget > 0f)
			{
				wobbleYtarget -= Time.fixedDeltaTime * wobbleYspeed;
			}
			if (wobbleYtarget < 0f)
			{
				wobbleYtarget += Time.fixedDeltaTime * wobbleYspeed;
			}
			Quaternion localRotation = base.transform.localRotation;
			Vector3    eulerAngles   = localRotation.eulerAngles;
			eulerAngles.x                += wobbleY;
			localRotation.eulerAngles    =  eulerAngles;
			base.transform.localRotation =  localRotation;
		}

		public void wobble(float _x, float _y, float _speedX, float _speedY)
		{
			wobbleX       = 0f;
			wobbleY       = 0f;
			wobbleXtarget = _x;
			wobbleYtarget = _y;
			wobbleXspeed  = _speedX;
			wobbleYspeed  = _speedY;
		}
	}
}

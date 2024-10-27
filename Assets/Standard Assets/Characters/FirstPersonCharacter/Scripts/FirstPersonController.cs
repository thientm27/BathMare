using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace UnityStandardAssets.Characters.FirstPerson
{
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(AudioSource))]
	public class FirstPersonController : MonoBehaviour
	{
		[SerializeField]
		private bool m_IsWalking;

		[SerializeField]
		private float m_WalkSpeed;

		[SerializeField]
		private float m_RunSpeed;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_RunstepLenghten;

		[SerializeField]
		private float m_JumpSpeed;

		[SerializeField]
		private float m_StickToGroundForce;

		[SerializeField]
		private float m_GravityMultiplier;

		[SerializeField]
		private MouseLook m_MouseLook;

		[SerializeField]
		private bool m_UseFovKick;

		[SerializeField]
		private bool m_UseHeadBob;

		[SerializeField]
		private CurveControlledBob m_HeadBob = new CurveControlledBob();

		[SerializeField]
		private LerpControlledBob m_JumpBob = new LerpControlledBob();

		[SerializeField]
		private float m_StepInterval;

		[SerializeField]
		private AudioClip[] m_FootstepSounds;

		[SerializeField]
		private AudioClip m_JumpSound;

		[SerializeField]
		private AudioClip m_LandSound;

		private Camera m_Camera;

		private bool m_Jump;

		private float m_YRotation;

		private Vector2 m_Input;

		private Vector3 m_MoveDir = Vector3.zero;

		private CharacterController m_CharacterController;

		private CollisionFlags m_CollisionFlags;

		private bool m_PreviouslyGrounded;

		private Vector3 m_OriginalCameraPosition;

		private float m_StepCycle;

		private float m_NextStep;

		private bool m_Jumping;

		private AudioSource m_AudioSource;

		private float JumpingHeight;

		public float LandingSoundHeight;

		private bool crouched;

		public float CrouchFromSize = 1f;

		public float CrouchToSize = 0.5f;

		public float CrouchSpeed = 1f;

		private CharacterController controller;

		public GameObject MyCamera;

		public GameObject CenterPos;

		private float CrouchDegree = 1f;

		private void Awake()
		{
			controller = GetComponent<CharacterController>();
		}

		private void Start()
		{
			m_CharacterController = GetComponent<CharacterController>();
			m_Camera = Camera.main;
			m_OriginalCameraPosition = m_Camera.transform.localPosition;
			m_HeadBob.Setup(m_Camera, m_StepInterval);
			m_StepCycle = 0f;
			m_NextStep = m_StepCycle / 2f;
			m_Jumping = false;
			m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(base.transform, m_Camera.transform);
		}

		private void Update()
		{
			if (Input.GetButtonDown("Crouch"))
			{
				Crouch();
			}
			if (!m_Jump)
			{
			}
			if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
			{
				if (JumpingHeight > LandingSoundHeight)
				{
					StartCoroutine(m_JumpBob.DoBobCycle());
					PlayLandingSound();
				}
				m_MoveDir.y = 0f;
				m_Jumping = false;
			}
			if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			{
				m_MoveDir.y = 0f;
			}
			if (!m_CharacterController.isGrounded)
			{
				JumpingHeight += Time.deltaTime * 1f;
			}
			else
			{
				JumpingHeight = 0f;
			}
			m_PreviouslyGrounded = m_CharacterController.isGrounded;
		}

		private void PlayLandingSound()
		{
			m_AudioSource.clip = m_LandSound;
			m_AudioSource.Play();
			m_NextStep = m_StepCycle + 0.5f;
		}

		private void FixedUpdate()
		{
			GetInput(out float speed);
			Vector3 vector = base.transform.forward * m_Input.y + base.transform.right * m_Input.x;
			Physics.SphereCast(base.transform.position, m_CharacterController.radius, Vector3.down, out RaycastHit hitInfo, m_CharacterController.height / 2f, -1, QueryTriggerInteraction.Ignore);
			vector = Vector3.ProjectOnPlane(vector, hitInfo.normal).normalized;
			m_MoveDir.x = vector.x * speed;
			m_MoveDir.z = vector.z * speed;
			if (m_CharacterController.isGrounded)
			{
				m_MoveDir.y = 0f - m_StickToGroundForce;
				if (m_Jump)
				{
					m_MoveDir.y = m_JumpSpeed;
					PlayJumpSound();
					m_Jump = false;
					m_Jumping = true;
				}
			}
			else
			{
				m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
			}
			m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
			ProgressStepCycle(speed);
			UpdateCameraPosition(speed);
			m_MouseLook.UpdateCursorLock();
		}

		private void PlayJumpSound()
		{
			m_AudioSource.clip = m_JumpSound;
			m_AudioSource.Play();
		}

		private void ProgressStepCycle(float speed)
		{
			if (m_CharacterController.velocity.sqrMagnitude > 0f && (m_Input.x != 0f || m_Input.y != 0f))
			{
				m_StepCycle += (m_CharacterController.velocity.magnitude + speed * ((!m_IsWalking) ? m_RunstepLenghten : 1f)) * Time.fixedDeltaTime;
			}
			if (m_StepCycle > m_NextStep)
			{
				m_NextStep = m_StepCycle + m_StepInterval;
				PlayFootStepAudio();
			}
		}

		private void PlayFootStepAudio()
		{
			if (m_CharacterController.isGrounded)
			{
				int num = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
				m_AudioSource.clip = m_FootstepSounds[num];
				m_AudioSource.PlayOneShot(m_AudioSource.clip);
				m_FootstepSounds[num] = m_FootstepSounds[0];
				m_FootstepSounds[0] = m_AudioSource.clip;
			}
		}

		private void UpdateCameraPosition(float speed)
		{
			if (m_UseHeadBob)
			{
				Vector3 localPosition;
				if (m_CharacterController.velocity.magnitude > 0f && m_CharacterController.isGrounded)
				{
					m_Camera.transform.localPosition = m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude + speed * ((!m_IsWalking) ? m_RunstepLenghten : 1f));
					localPosition = m_Camera.transform.localPosition;
					Vector3 localPosition2 = m_Camera.transform.localPosition;
					float num = localPosition2.y * CrouchDegree - m_JumpBob.Offset();
					Vector3 center = controller.center;
					localPosition.y = num + center.y;
				}
				else
				{
					localPosition = m_Camera.transform.localPosition;
					float num2 = m_OriginalCameraPosition.y * CrouchDegree - m_JumpBob.Offset();
					Vector3 center2 = controller.center;
					localPosition.y = num2 + center2.y;
				}
				m_Camera.transform.localPosition = localPosition;
			}
		}

		public void Crouch()
		{
			if (!crouched)
			{
				CrouchSpeed = controller.height / CrouchFromSize;
				crouched = true;
				iTween.Stop(base.gameObject);
				iTween.ValueTo(base.gameObject, iTween.Hash("from", controller.height, "to", CrouchToSize, "time", CrouchSpeed, "onupdate", "ValueChange"));
				GameObject gameObject = base.gameObject;
				object[] obj = new object[8]
				{
					"from",
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				Vector3 center = controller.center;
				obj[1] = center.y;
				obj[2] = "to";
				obj[3] = 0f - CrouchToSize;
				obj[4] = "time";
				obj[5] = CrouchSpeed;
				obj[6] = "onupdate";
				obj[7] = "ValueChange2";
				iTween.ValueTo(gameObject, iTween.Hash(obj));
			}
			else
			{
				CrouchSpeed = (CrouchToSize + controller.height / CrouchFromSize) / controller.height;
				crouched = false;
				iTween.Stop(base.gameObject);
				iTween.ValueTo(base.gameObject, iTween.Hash("from", controller.height, "to", CrouchFromSize, "time", CrouchSpeed, "onupdate", "ValueChange"));
				GameObject gameObject2 = base.gameObject;
				object[] obj2 = new object[8]
				{
					"from",
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
				Vector3 center2 = controller.center;
				obj2[1] = center2.y;
				obj2[2] = "to";
				obj2[3] = 0;
				obj2[4] = "time";
				obj2[5] = CrouchSpeed;
				obj2[6] = "onupdate";
				obj2[7] = "ValueChange2";
				iTween.ValueTo(gameObject2, iTween.Hash(obj2));
			}
		}

		private void ValueChange(float val)
		{
			controller.height = val;
			CrouchDegree = controller.height / CrouchFromSize;
		}

		private void ValueChange2(float val)
		{
			controller.center = new Vector3(0f, val, 0f);
		}

		private void GetInput(out float speed)
		{
			float axis = CrossPlatformInputManager.GetAxis("Horizontal");
			float axis2 = CrossPlatformInputManager.GetAxis("Vertical");
			bool isWalking = m_IsWalking;
			m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
			speed = ((!m_IsWalking) ? m_RunSpeed : m_WalkSpeed);
			m_Input = new Vector2(axis, axis2);
			if (m_Input.sqrMagnitude > 1f)
			{
				m_Input.Normalize();
			}
			if (m_IsWalking != isWalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0f)
			{
				StopAllCoroutines();
			}
		}
	}
}

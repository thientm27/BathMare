using DG.Tweening;
using UnityEngine;

namespace BathMare.Scripts.GameScene
{
    public class CameraMove : MonoBehaviour
    {
        public GameObject           Player;
        public PhysicWalkMouseLook Look_X;
        public PhysicWalkMouseLook Look_Y;
        // public Blur                 blur;
        // public SSAOPro              ssao;
        // public UltimateBloom        bloom;
        // public DeluxeEyeAdaptation  adaptation;
        // public AntiAliasing         antiailias;
    
        public bool ZoomEnd;
        public bool Examining;

        private void Update()
        {
            // if (Input.GetMouseButton(1))
            // {
            //     if (!ZoomEnd && pl.NowState == Player.State.None && !Examining)
            //     {
            //         Zoom();
            //     }
            // }
            // else
            // {
            //     ZoomOut();
            // }

            // if (Input.GetMouseButtonUp(1))
            // {
            //     if (Examining)
            //     {
            //         Examining = false;
            //     }
            //     ZoomEnd = false;
            // }
        }

        public void Zoom()
        {
            Camera camera = GetComponent<Camera>();
            camera.fieldOfView = Mathf.Max(camera.fieldOfView - 1f, 40f);
            if (camera.fieldOfView <= 40f)
            {
                ZoomEnd = true;
            }
        }

        public void ZoomOut()
        {
            Camera camera = GetComponent<Camera>();
            if (camera.fieldOfView < 60f)
            {
                camera.fieldOfView += 1f;
            }
            else
            {
                ZoomEnd = false;
            }
        }

        public void Look(Vector3 targetpos, float speed)
        {
            Vector3 eulerAngles         = Quaternion.LookRotation(targetpos - transform.position).eulerAngles;
            float   yRotationDifference = eulerAngles.y - Player.transform.eulerAngles.y;

            yRotationDifference = (yRotationDifference > 180f) ? yRotationDifference - 360f : (yRotationDifference < -180f) ? yRotationDifference + 360f : yRotationDifference;

            Look_X.transform.DORotate(new Vector3(Look_X.rotationX + yRotationDifference, 0, 0), speed)
                .SetEase(Ease.InOutCubic)
                .OnUpdate(() => ValueChangeX(Look_X.rotationX));

            Look_Y.transform.DORotate(new Vector3(0, Look_Y.rotationY, 0), speed)
                .SetEase(Ease.InOutCubic)
                .OnUpdate(() => ValueChangeY(Look_Y.rotationY));
        }

        public void Look_GameOver(Vector3 targetpos, float speed)
        {
            Vector3 eulerAngles = Quaternion.LookRotation(targetpos - transform.position).eulerAngles;
            transform.DORotate(new Vector3(eulerAngles.x, eulerAngles.y, 0), speed)
                .SetEase(Ease.InOutCubic);
        }

        private void ValueChangeX(float value)
        {
            Look_X.rotationX = value;
        }

        private void ValueChangeY(float value)
        {
            Look_Y.rotationY = value;
        }

        public void Move(Vector3 targetpos, float speed)
        {
            transform.DOMove(targetpos, speed).SetEase(Ease.InOutQuad);
        }

        public void StopDotween()
        {
            DOTween.Kill(transform);
        }

        public void StartStopCamera()
        {
            Look_X.enabled = !Look_X.enabled;
            Look_Y.enabled = !Look_Y.enabled;
        }

        public void BlurDelete()
        {
        }

        private void ValueChange(int value)
        {
        }
    }
}
